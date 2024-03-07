using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace HealthCare.MVC.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INoteService _noteService;
        private readonly IAgentService _agentService;
        private readonly IAsignService _asignService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, INoteService noteService, IAgentService agentService, IAsignService asignService, IMapper mapper)
        {
            _customerService = customerService;
            _noteService = noteService;
            _agentService = agentService;
            _asignService = asignService;
            _mapper = mapper;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string? SearchString)
        {
            TempData["Active"] = "Customers";
            if (SearchString != null)
            {
                TempData["SearchString"] = SearchString;
                return _customerService != null ?
                          View(_customerService.Get(x => x.FirstName.ToLower().Contains(SearchString.ToLower()) || x.LastName.ToLower().Contains(SearchString.ToLower())).ToList()) :
                          Problem("Entity set 'HealthCareContext.Customers'  is null.");
            }
            return _customerService != null ?
                          View(_customerService.GetAll()) :
                          Problem("Entity set 'HealthCareContext.Customers'  is null.");
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _customerService == null)
            {
                return NotFound();
            }

            var customerVM = _mapper.Map<CustomerViewModel>(await _customerService.FindAsync(id));

            var asignList = _asignService.Get(x => x.CustomerId == id).Include(x => x.Agent).ToList();
            customerVM.Agents = new List<string>();
            int check = 0;
            foreach (var asign in asignList)
            {
                customerVM.Agents.Add(asign.Agent.FirstName + " " + asign.Agent.LastName);
                if (asign.AgentId == int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value))
                {
                    check = asign.AgentId;
                }
            }


            var notesVM = _mapper.Map<List<NoteViewModel>>(_noteService.Get(x => x.CustomerId == id && x.IsDeleted == false).OrderByDescending(x => x.UpdatedDate).ToList());

            foreach (var note in notesVM)
            {
                note.Agent = await _agentService.FindAsync(note.AgentId);
            }
            customerVM.Notes = notesVM;

            if (customerVM == null)
            {
                return NotFound();
            }
            TempData["AgentId"] = check;
            TempData["Active"] = "Customers";
            return View(customerVM);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            TempData["Active"] = "Customers";
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,PhoneNumber,AddressCountry,City,CompanyName,Region," +
            "PostalCode State,County,HomePhone,HomePage,Longitude,Latitude,JobTitle,ContactGender,EmployeeSize,CapitalSize,ClassifyCode," +
            "BusinessType,Nationality,IsMarried,HaveChildren,HomeOwner,YearInBusiness,IsSelfEmployed,DynamicInfo")] CustomerCreateModel customer)
        {
            if (!IsValidPhoneNumber(customer.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "Invalid phone number format.");
            }
            if (_customerService.Get(x => x.Email == customer.Email).Any())
            {
                ModelState.AddModelError("Email", "Email already exists.");
            }
            if (_customerService.Get(x => x.PhoneNumber == customer.PhoneNumber).Any())
            {
                ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
            }
            if (customer.Email == null || customer.FirstName == null || customer.LastName == null || customer.PhoneNumber == null || customer.Address == null)
            {
                return Problem("One or more fields are empty.");
            }
            if (!IsValidEmail(customer.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }
            if (ModelState.ErrorCount > 0)
            {
                TempData["Active"] = "Customers";
                return View(customer);
            }
            if (ModelState.IsValid)
            {
                await _customerService.AddAsync(_mapper.Map<Customer>(customer));
                await _customerService.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["Active"] = "Customers";
            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _customerService == null || id == 0)
            {
                return NotFound();
            }

            var customer = await _customerService.FindAsync(id);
            if (customer == null || customer.IsDeleted == true)
            {
                return NotFound();
            }
            TempData["Active"] = "Customers";
            return View(_mapper.Map<CustomerUpdateModel>(customer));
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FirstName,LastName,PhoneNumber,Address,Country,City,CompanyName,Region," +
            "PostalCode State,County,HomePhone,HomePage,Longitude,Latitude,JobTitle,ContactGender,EmployeeSize,CapitalSize,ClassifyCode," +
            "BusinessType,Nationality,IsMarried,HaveChildren,HomeOwner,YearInBusiness,IsSelfEmployed,DynamicInfo")] CustomerUpdateModel customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!IsValidPhoneNumber(customer.PhoneNumber))
                    {
                        ModelState.AddModelError("PhoneNumber", "Invalid phone number format.");
                    }
                    if (_customerService.Get(x => x.Email == customer.Email).Any())
                    {
                        if (_customerService.Get(x => x.Email == customer.Email && x.Id == customer.Id).Count() == 0)
                        {
                            ModelState.AddModelError("Email", "Email already exists.");
                        }
                    }
                    if (_customerService.Get(x => x.PhoneNumber == customer.PhoneNumber).Any())
                    {
                        if (_customerService.Get(x => x.PhoneNumber == customer.PhoneNumber && x.Id == customer.Id).Count() == 0)
                        {
                            ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
                        }
                    }
                    if (customer.Email == null || customer.FirstName == null || customer.LastName == null || customer.PhoneNumber == null || customer.Address == null)
                    {
                        return Problem("One or more fields are empty.");
                    }
                    if (!IsValidEmail(customer.Email))
                    {
                        ModelState.AddModelError("Email", "Invalid email format.");
                    }
                    if (ModelState.ErrorCount > 0)
                    {
                        TempData["Active"] = "Customers";
                        return View(customer);
                    }

                    var customerEntity = _customerService.Get(x => x.Id == customer.Id).FirstOrDefault();
                    if (customerEntity == null)
                    {
                        return NotFound();
                    }
                    customerEntity.Email = customer.Email;
                    customerEntity.FirstName = customer.FirstName;
                    customerEntity.LastName = customer.LastName;
                    customerEntity.PhoneNumber = customer.PhoneNumber;
                    customerEntity.Address = customer.Address;

                    customerEntity.Country = customer.Country;
                    customerEntity.City = customer.City;
                    customerEntity.CompanyName = customer.CompanyName;
                    customerEntity.Region = customer.Region;
                    customerEntity.PostalCode = customer.PostalCode;
                    customerEntity.State = customer.State;
                    customerEntity.County = customer.County;
                    customerEntity.HomePhone = customer.HomePhone;
                    customerEntity.HomePage = customer.HomePage;
                    customerEntity.Longitude = customer.Longitude;
                    customerEntity.Latitude = customer.Latitude;
                    customerEntity.JobTitle = customer.JobTitle;
                    customerEntity.ContactGender = customer.ContactGender;
                    customerEntity.EmployeeSize = customer.EmployeeSize;
                    customerEntity.CapitalSize = customer.CapitalSize;
                    customerEntity.ClassifyCode = customer.ClassifyCode;
                    customerEntity.BusinessType = customer.BusinessType;
                    customerEntity.Nationality = customer.Nationality;
                    customerEntity.IsMarried = customer.IsMarried;
                    customerEntity.HaveChildren = customer.HaveChildren;
                    customerEntity.HomeOwner = customer.HomeOwner;
                    customerEntity.YearInBusiness = customer.YearInBusiness;
                    customerEntity.IsSelfEmployed = customer.IsSelfEmployed;
                    customerEntity.DynamicInfo = customer.DynamicInfo;

                    customerEntity.UpdatedDate = DateTime.Now;

                    _customerService.Update(customerEntity);
                    await _customerService.SaveChangeAsync();
                return RedirectToAction(nameof(Details), new { id = customerEntity.Id});
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            TempData["Active"] = "Customers";
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _customerService == null)
            {
                return NotFound();
            }

            var customer = await _customerService.FindAsync(id);
            if (customer == null || customer.IsDeleted == true)
            {
                return NotFound();
            }
            TempData["Active"] = "Customers";
            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_customerService == null)
            {
                return Problem("Entity set 'HealthCareContext.Customers'  is null.");
            }
            var customer = await _customerService.FindAsync(id);
            customer.IsDeleted = true;
            _customerService.Update(customer);
            await _customerService.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return (_customerService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(\([0-9]{3}\)|[0-9]{3}-)[0-9]{3}-[0-9]{4}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        static bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email format
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
