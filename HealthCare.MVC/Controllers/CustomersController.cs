using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            foreach (var asign in asignList)
            {
                customerVM.Agents.Add(asign.Agent.FirstName + " " + asign.Agent.LastName);
            }


            var notesVM = _mapper.Map<List<NoteViewModel>>(_noteService.Get(x => x.CustomerId == id).OrderBy(x => x.UpdatedDate).ToList());

            foreach (var note in notesVM)
            {
                note.Agent = await _agentService.FindAsync(note.AgentId);
            }
            customerVM.Notes = notesVM;

            if (customerVM == null)
            {
                return NotFound();
            }

            return View(customerVM);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,PhoneNumber,Address")] CustomerCreateModel customer)
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
                return View(customer);
            }
            if (ModelState.IsValid)
            {
                await _customerService.AddAsync(_mapper.Map<Customer>(customer));
                await _customerService.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _customerService == null || id == 0)
            {
                return NotFound();
            }

            var customer = await _customerService.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FirstName,LastName,PhoneNumber,Address")] Customer customer)
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
                        return View(customer);
                    }
                    _customerService.Update(_mapper.Map<Customer>(customer));
                    await _customerService.SaveChangeAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _customerService == null)
            {
                return NotFound();
            }

            var customer = await _customerService.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_customerService == null)
            {
                return Problem("Entity set 'HealthCareContext.Customers'  is null.");
            }

            await _customerService.Remove(id);
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
