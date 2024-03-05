using System;
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

namespace HealthCare.MVC.Controllers
{
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IAsignService _asignService;
        private readonly IMapper _mapper;

        public NotesController(INoteService noteService, IAsignService asignService, IMapper mapper)
        {
            _noteService = noteService;
            _asignService = asignService;
            _mapper = mapper;
        }

        // GET: Notes
        public async Task<IActionResult> Index(int asignId)
        {
            var healthCareContext = _noteService.Get(x => x.AsignId == asignId).Include(n => n.Asign);
            return View(await healthCareContext.ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _noteService == null || id == 0)
            {
                return NotFound();
            }

            var note = await _noteService.GetAll()
                .Include(n => n.Asign)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create(int asignId)
        {
            ViewData["AsignId"] = asignId;
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AsignId,Title,Content,Type")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsignId"] = new SelectList(_context.Asigns, "Id", "Id", note.AsignId);
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["AsignId"] = new SelectList(_context.Asigns, "Id", "Id", note.AsignId);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AsignId,Title,Content,Type,CreatedDate,UpdatedDate")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            ViewData["AsignId"] = new SelectList(_context.Asigns, "Id", "Id", note.AsignId);
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .Include(n => n.Asign)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notes == null)
            {
                return Problem("Entity set 'HealthCareContext.Notes'  is null.");
            }
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
          return (_context.Notes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
