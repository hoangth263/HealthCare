using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

            TempData["AsignId"] = asignId;
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
        public async Task<IActionResult> Create([Bind("AsignId,Title,Content,Type")] NoteCreateModel note)
        {
            if (ModelState.IsValid)
            {
                await _noteService.AddAsync(_mapper.Map<Note>(note));
                await _noteService.SaveChangeAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AsignId"] = new SelectList(_asignService.GetAll(), "Id", "Id", note.AsignId);
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int id, int asignId)
        {
            if (id == null || _noteService == null)
            {
                return NotFound();
            }

            var note = await _noteService.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["AsignId"] = asignId;
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AsignId,Title,Content,Type")] NoteUpdateModel note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _noteService.Update(_mapper.Map<Note>(note));
                    await _noteService.SaveChangeAsync();
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
            ViewData["AsignId"] = note.AsignId;
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _noteService == null)
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

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_noteService == null)
            {
                return Problem("Entity set 'HealthCareContext.Notes'  is null.");
            }
            await _noteService.Remove(id);
            await _noteService.SaveChangeAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return (_noteService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
