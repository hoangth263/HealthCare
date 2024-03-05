using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HealthCare.MVC.Controllers
{
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
                return _agentService != null ?
                          View(_mapper.Map<List<AgentViewModel>>(_agentService.Get(x => x.FirstName.ToLower().Contains(SearchString.ToLower()) || x.LastName.ToLower().Contains(SearchString.ToLower())).ToList())) :
                          Problem("Entity set 'HealthCareContext.Agents'  is null.");
            }
            return _agentService != null ?
                          View(_mapper.Map<List<AgentViewModel>>(_agentService.GetAll()).ToList()) :
                          Problem("Entity set 'HealthCareContext.Agents'  is null.");
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _agentService == null || id == 0)
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

        // GET: Agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,FirstName,LastName,PhoneNumber,Address,Password")] AgentCreateModel agent)
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
                if (ModelState.ErrorCount > 0)
                {
                    return View(agent);
                }
                await _agentService.AddAsync(_mapper.Map<Agent>(agent));
                await _agentService.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

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
            return View(agent);
        }

        // POST: Agents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,FirstName,LastName,PhoneNumber,Address,Password")] Agent agent)
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
                    if (ModelState.ErrorCount > 0)
                    {
                        return View(agent);
                    }

                    _agentService.Update(_mapper.Map<Agent>(agent));
                    await _agentService.SaveChangeAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

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

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_agentService == null)
            {
                return Problem("Entity set 'HealthCareContext.Agents'  is null.");
            }
            await _agentService.Remove(id);

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
