using AutoMapper;
using HealthCare.MVC.Entities;
using HealthCare.MVC.Models;
using HealthCare.MVC.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.MVC.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(int asignId, string SearchString)
        {

            TempData["AsignId"] = asignId;
            if (SearchString != null)
            {
                return View(
                    _noteService.Get(
                        x => x.AsignId == asignId
                        && x.Title.ToLower().Contains(SearchString.ToLower())
                        || x.Content.ToLower().Contains(SearchString.ToLower())
                        )
                    );
            }
            else
            {
                return View(_noteService.Get(x => x.AsignId == asignId).ToList());
            }
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _noteService == null || id == 0)
            {
                return NotFound();
            }

            var note = await _noteService.GetAll()
                .Include(n => n.Asign).ThenInclude(a => a.Customer)
                .Include(n => n.Asign).ThenInclude(a => a.Agent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            TempData["AsignId"] = note.AsignId;
            return View(note);
        }

        public List<string> TypeList()
        {

            var enumValues = Enum.GetValues(typeof(NoteType));
            var typeList = new List<string>();
            foreach (var value in enumValues)
            {
                typeList.Add(value.ToString());
            }
            return typeList;
        }

        // GET: Notes/Create
        public IActionResult Create(int asignId)
        {
            ViewData["Type"] = new SelectList(TypeList());
            TempData["AsignId"] = asignId;
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
                return RedirectToAction(nameof(Index), new { asignId = note.AsignId });

            }
            ViewData["Type"] = new SelectList(TypeList(),note.Type);
            TempData["AsignId"] = note.AsignId;
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
            if(asignId == null || asignId == 0)
            {
                asignId = note.AsignId;
            }
            if (note == null)
            {
                return NotFound();
            }
            ViewData["Type"] = new SelectList(TypeList(), note.Type);
            TempData["AsignId"] = asignId;
            return View(_mapper.Map<NoteUpdateModel>(note));
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
                return RedirectToAction(nameof(Index), new { asignId = note.AsignId });
            }
            ViewData["Type"] = new SelectList(TypeList(), note.Type);
            TempData["AsignId"] = note.AsignId;
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
            TempData["AsignId"] = note.AsignId;
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
            var asignId = _noteService.Get(x => x.Id == id).FirstOrDefault().AsignId;
            await _noteService.Remove(id);
            await _noteService.SaveChangeAsync();
            return RedirectToAction(nameof(Index), new { asignId = asignId });
        }

        private bool NoteExists(int id)
        {
            return (_noteService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
