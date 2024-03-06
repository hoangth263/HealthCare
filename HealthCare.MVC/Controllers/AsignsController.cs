using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthCare.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AsignsController : Controller
    {
        private readonly IAsignService _asignService;
        private readonly IAgentService _agentService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public AsignsController(IAsignService asignService, IMapper mapper, IAgentService agentService, ICustomerService customerService)
        {
            _asignService = asignService;
            _agentService = agentService;
            _customerService = customerService;
            _mapper = mapper;
        }

        // GET: Asigns
        public async Task<IActionResult> Index(string SearchString)
        {
            var healthCareContext = _asignService.GetAll().Include(a => a.Agent).Include(a => a.Customer);
            if (SearchString != null)
            {
                TempData["SearchString"] = SearchString;
                return View(healthCareContext.Where(x => x.Agent.Email.ToLower().Contains(SearchString.ToLower())
                || x.Customer.Email.ToLower().Contains(SearchString.ToLower()))
                    .ToList().OrderBy(u => u.Agent.Email == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value ? 0 : 1)
                               .ThenBy(u => u.Agent.Email));
            }
            return View(healthCareContext.ToList().OrderBy(u => u.Agent.Email == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value ? 0 : 1)
                               .ThenBy(u => u.Agent.Email));
        }

        // GET: Asigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _asignService == null || id == 0)
            {
                return NotFound();
            }

            var asign = await _asignService.GetAll()
                .Include(a => a.Agent)
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asign == null)
            {
                return NotFound();
            }

            return View(asign);
        }
        // GET: Asigns/Create
        public IActionResult Create(int id)
        {
            TempData["AgentId"] = id;
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email");
            return View();
        }

        // POST: Asigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgentId,CustomerId")] AsignCreateModel asign)
        {
            if (ModelState.IsValid)
            {
                if (_asignService.Get(x => x.AgentId == asign.AgentId && x.CustomerId == asign.CustomerId).Any())
                {
                    ModelState.AddModelError("AgentId", "This customer is already assigned to this agent.");
                    ModelState.AddModelError("CustomerId", "This customer is already assigned to this agent.");
                }
                else
                {
                    await _asignService.AddAsync(_mapper.Map<Asign>(asign));
                    await _asignService.SaveChangeAsync();
                    return RedirectToAction("Details", "Agents", new { id = asign.AgentId });
                }
            }
            ViewData["AgentId"] = new SelectList(_agentService.GetAll(), "Id", "Email", asign.AgentId);
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email", asign.CustomerId);
            return View(asign);
        }

        // GET: Asigns/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _asignService == null || id == 0)
            {
                return NotFound();
            }

            var asign = await _asignService.FindAsync(id);
            if (asign == null)
            {
                return NotFound();
            }
            TempData["AgentId"] = asign.AgentId;
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email", asign.CustomerId);
            return View(_mapper.Map<AsignUpdateModel>(asign));
        }

        // POST: Asigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AgentId,CustomerId")] AsignUpdateModel asign)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var checkList = _asignService.Get(x => x.AgentId == asign.AgentId && x.CustomerId == asign.CustomerId);
                    if (checkList.Any())
                    {
                        if (!checkList.Where(x => x.Id == id).Any())
                        {
                            ModelState.AddModelError("AgentId", "This customer is already assigned to this agent.");
                            ModelState.AddModelError("CustomerId", "This customer is already assigned to this agent.");
                        }
                    }
                    if (ModelState.ErrorCount == 0)
                    {
                        var asignUpdate = await _asignService.FindAsync(id);
                        asignUpdate.AgentId = asign.AgentId;
                        asignUpdate.CustomerId = asign.CustomerId;
                        asignUpdate.UpdatedDate = DateTime.Now;
                        _asignService.Update(asignUpdate);
                        await _asignService.SaveChangeAsync();
                        return RedirectToAction("Details", "Agents", new { id = asign.AgentId });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignExists(asign.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            TempData["AgentId"] = asign.AgentId;
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email", asign.CustomerId);
            return View(asign);
        }

        // GET: Asigns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _asignService == null)
            {
                return NotFound();
            }

            var asign = await _asignService.GetAll()
                .Include(a => a.Agent)
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asign == null)
            {
                return NotFound();
            }

            return View(asign);
        }

        // POST: Asigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_asignService == null)
            {
                return Problem("Entity set 'HealthCareContext.Asigns'  is null.");
            }
            var asign = await _asignService.FindAsync(id);
            asign.IsDeleted = true;
            asign.UpdatedDate = DateTime.Now;
            _asignService.Update(asign);

            await _asignService.SaveChangeAsync();
            return RedirectToAction("Details", "Agents", new { id = asign.AgentId });
        }

        private bool AsignExists(int id)
        {
            return (_asignService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
