using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HealthCare.MVC.Controllers
{
    [Authorize]
    public class AgentsController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public AgentsController(IAgentService agentService, IMapper mapper)
        {
            _agentService = agentService;
            _mapper = mapper;
        }

        // GET: Agents
        public async Task<IActionResult> Index(string? SearchString)
        {

            if (SearchString != null)
            {
                TempData["SearchString"] = SearchString;
                return _agentService != null ?
                          View(_mapper.Map<List<AgentViewModel>>(_agentService.Get(x => x.FirstName.ToLower().Contains(SearchString.ToLower())
                          || x.LastName.ToLower().Contains(SearchString.ToLower()))
                          .Where(x => x.IsDeleted == false).ToList())) :

                          Problem("Entity set 'HealthCareContext.Agents'  is null.");
            }
            return _agentService != null ?
                          View(_mapper.Map<List<AgentViewModel>>(_agentService.Get(x => x.IsDeleted == false)).ToList()) :
                          Problem("Entity set 'HealthCareContext.Agents'  is null.");
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _agentService == null || id == 0)
            {
                return NotFound();
            }

            var agent = _mapper.Map<AgentDetailsModel>(_agentService.Get(x => x.Id == id).Include(x => x.Asigns).ThenInclude(a => a.Customer).FirstOrDefault());
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // GET: Agents/Create
        public IActionResult Create()
        {
            ViewData["Role"] = new SelectList(new List<string> { "User", "Admin" });
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Agents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,PhoneNumber,Address,Password,Role")] AgentCreateModel agent)
        {
            if (ModelState.IsValid)
            {
                if (!IsValidPhoneNumber(agent.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Invalid phone number format.");
                }
                if (_agentService.Get(x => x.Email == agent.Email).Any())
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                }
                if (_agentService.Get(x => x.PhoneNumber == agent.PhoneNumber).Any())
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
                }
                if (agent.Email == null || agent.FirstName == null || agent.LastName == null || agent.PhoneNumber == null || agent.Address == null || agent.Password == null)
                {
                    return Problem("One or more fields are empty.");
                }
                if (!IsValidEmail(agent.Email))
                {
                    ModelState.AddModelError("Email", "Invalid email format.");
                }
                if (agent.Role == null && agent.Role == "")
                {
                    agent.Role = "User";
                }
                if (ModelState.ErrorCount == 0)
                {
                    await _agentService.AddAsync(_mapper.Map<Agent>(agent));
                    await _agentService.SaveChangeAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Role"] = new SelectList(new List<string> { "User", "Admin" });
            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // GET: Agents/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _agentService == null || id == 0)
            {
                return NotFound();
            }

            var agent = await _agentService.FindAsync(id);
            agent.Password = null;
            if (agent == null)
            {
                return NotFound();
            }
            ViewData["Role"] = new SelectList(new List<string> { "User", "Admin" }, agent.Role);
            return View(_mapper.Map<AgentUpdateModel>(agent));
        }

        [Authorize(Roles = "Admin")]
        // POST: Agents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FirstName,LastName,PhoneNumber,Address,Password,Role")] AgentUpdateModel agent)
        {
            if (id != agent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!IsValidPhoneNumber(agent.PhoneNumber))
                    {
                        ModelState.AddModelError("PhoneNumber", "Invalid phone number format.");
                    }
                    if (_agentService.Get(x => x.Email == agent.Email).Any())
                    {
                        if (_agentService.Get(x => x.Email == agent.Email && x.Id == agent.Id).Count() == 0)
                        {
                            ModelState.AddModelError("Email", "Email already exists.");
                        }
                    }
                    if (_agentService.Get(x => x.PhoneNumber == agent.PhoneNumber).Any())
                    {
                        if (_agentService.Get(x => x.PhoneNumber == agent.PhoneNumber && x.Id == agent.Id).Count() == 0)
                        {
                            ModelState.AddModelError("PhoneNumber", "Phone number already exists.");
                        }
                    }
                    if (agent.Email == null || agent.FirstName == null || agent.LastName == null || agent.PhoneNumber == null || agent.Address == null || agent.Password == null)
                    {
                        return Problem("One or more fields are empty.");
                    }
                    if (!IsValidEmail(agent.Email))
                    {
                        ModelState.AddModelError("Email", "Invalid email format.");
                    }
                    if (agent.Role == null && agent.Role == "")
                    {
                        agent.Role = "User";
                    }
                    if (ModelState.ErrorCount == 0)
                    {
                        var agentUpdate = _agentService.Get(x => x.Id == agent.Id).FirstOrDefault();

                        agentUpdate.Email = agent.Email;
                        agentUpdate.FirstName = agent.FirstName;
                        agentUpdate.LastName = agent.LastName;
                        agentUpdate.PhoneNumber = agent.PhoneNumber;
                        agentUpdate.Address = agent.Address;
                        agentUpdate.Password = agent.Password;
                        agentUpdate.Role = agent.Role;
                        agentUpdate.UpdatedDate = DateTime.Now;

                        _agentService.Update(agentUpdate);
                        await _agentService.SaveChangeAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgentExists(agent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["Role"] = new SelectList(new List<string> { "User", "Admin" }, agent.Role);
            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _agentService == null)
            {
                return NotFound();
            }

            var agent = _mapper.Map<AgentViewModel>(await _agentService.FindAsync(id));
            if (agent == null)
            {
                return NotFound();
            }


            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_agentService == null)
            {
                return Problem("Entity set 'HealthCareContext.Agents'  is null.");
            }
            var agent = await _agentService.FindAsync(id);
            agent.IsDeleted = true;
            agent.UpdatedDate = DateTime.Now;
            _agentService.Update(agent);

            await _agentService.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgentExists(int id)
        {
            return (_agentService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
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
