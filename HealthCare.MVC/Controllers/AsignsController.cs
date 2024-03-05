﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.MVC.Data;
using HealthCare.MVC.Entities;
using AutoMapper;
using HealthCare.MVC.Services.IServices;
using HealthCare.MVC.Services;
using HealthCare.MVC.Models;

namespace HealthCare.MVC.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var healthCareContext = _asignService.GetAll().Include(a => a.Agent).Include(a => a.Customer);
            return View(await healthCareContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["AgentId"] = new SelectList(_agentService.GetAll(), "Id", "Email");
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
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AgentId"] = new SelectList(_agentService.GetAll(), "Id", "Email", asign.AgentId);
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email", asign.CustomerId);
            return View(asign);
        }

        // GET: Asigns/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _asignService == null || id ==0)
            {
                return NotFound();
            }

            var asign = await _asignService.FindAsync(id);
            if (asign == null)
            {
                return NotFound();
            }
            ViewData["AgentId"] = new SelectList(_agentService.GetAll(), "Id", "Email", asign.AgentId);
            ViewData["CustomerId"] = new SelectList(_customerService.GetAll(), "Id", "Email", asign.CustomerId);
            return View(asign);
        }

        // POST: Asigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AgentId,CustomerId")] AsignUpdateModel asign)
        {
            if (id != asign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_asignService.Get(x => x.AgentId == asign.AgentId && x.CustomerId == asign.CustomerId && asign.Id == x.Id).Any())
                    {
                        ModelState.AddModelError("AgentId", "This customer is already assigned to this agent.");
                        ModelState.AddModelError("CustomerId", "This customer is already assigned to this agent.");
                    }
                    else
                    {
                        _asignService.Update(_mapper.Map<Asign>(asign));
                        await _asignService.SaveChangeAsync();
                        return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgentId"] = new SelectList(_agentService.GetAll(), "Id", "Email", asign.AgentId);
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
            await _asignService.Remove(id);

            await _asignService.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignExists(int id)
        {
          return (_asignService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}