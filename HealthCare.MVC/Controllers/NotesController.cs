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
    [Authorize]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly ICustomerService _asignService;
        private readonly IMapper _mapper;

        public NotesController(INoteService noteService, ICustomerService asignService, IMapper mapper)
        {
            _noteService = noteService;
            _asignService = asignService;
            _mapper = mapper;
        }

        // GET: Notes
        public async Task<IActionResult> Index(int asignId, string SearchString)
        {

            TempData["CustomerId"] = asignId;
            var check = _asignService.Get(x => x.Id == asignId).FirstOrDefault();
            //if(check.Agent.Email == ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value)
            //{
            //    TempData["Check"] = check;
            //}
            //else
            //{
            //    TempData["Check"] = check;
            //}
            if (SearchString != null)
            {
                TempData["SearchString"] = SearchString;
                var list = _noteService.Get(
                        x => x.CustomerId == asignId)
                        .Where(x => x.Title.ToLower().Contains(SearchString.ToLower())
                        || x.Content.Contains(SearchString.ToLower())
                        ).ToList();
                return View(list);
            }
            else
            {
                return View(_noteService.Get(x => x.CustomerId == asignId).ToList());
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
                .Include(n => n.Customer)
                //.ThenInclude(a => a.Customer)
                //.Include(n => n.Customer).ThenInclude(a => a.Agent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            TempData["CustomerId"] = note.CustomerId;
            TempData["Active"] = "Agents";
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
        public IActionResult Create(int customerId)
        {
            ViewData["Type"] = new SelectList(TypeList());
            TempData["Active"] = "Agents";
            TempData["AgentId"] = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value;
            TempData["CustomerId"] = customerId;
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgentId,CustomerId,Title,Content,Type")] NoteCreateModel note)
        {
            if (ModelState.IsValid)
            {
                await _noteService.AddAsync(_mapper.Map<Note>(note));
                await _noteService.SaveChangeAsync();
                return RedirectToAction("Details", "Customers", new { id = note.CustomerId });

            }
            ViewData["Type"] = new SelectList(TypeList(), note.Type);
            TempData["Active"] = "Agents";
            TempData["CustomerId"] = note.CustomerId;
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
            if (asignId == null || asignId == 0)
            {
                asignId = note.CustomerId;
            }
            if (note == null)
            {
                return NotFound();
            }
            ViewData["Type"] = new SelectList(TypeList(), note.Type);
            TempData["Active"] = "Agents";
            TempData["CustomerId"] = asignId;
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AgentId,CustomerId,Title,Content,Type,CreatedDate,UpdatedDate")] NoteUpdateModel note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    note.UpdatedDate = DateTime.Now;
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
                return RedirectToAction("Details", "Customers", new { id = note.CustomerId });
            }
            ViewData["Type"] = new SelectList(TypeList(), note.Type);
            TempData["CustomerId"] = note.CustomerId;
            TempData["Active"] = "Agents";
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
                .Include(n => n.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            TempData["CustomerId"] = note.CustomerId;
            TempData["Active"] = "Agents";
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
            //var asignId = _noteService.Get(x => x.Id == id).FirstOrDefault().CustomerId;
            //await _noteService.Remove(id);
            var note = await _noteService.FindAsync(id);
            note.IsDeleted = true;
            note.UpdatedDate = DateTime.Now;
            _noteService.Update(note);
            await _noteService.SaveChangeAsync();
            return RedirectToAction("Details", "Customers", new { id = note.CustomerId });
        }

        private bool NoteExists(int id)
        {
            return (_noteService.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
