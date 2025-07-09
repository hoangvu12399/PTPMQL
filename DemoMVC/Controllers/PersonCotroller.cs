using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AutoGenerateCode _autoGenerateCode;
        
        public PersonController(ApplicationDbContext context)
        {
            _context = context;
            _autoGenerateCode = new AutoGenerateCode();
        }
        public async Task<IActionResult> Index()
        {
            var lastperson = await _context.Person.OrderByDescending(p => p.PersonId).FirstOrDefaultAsync();
            if (lastperson != null)
            {
                // If there is a last person, generate the next PersonId based on the last one
                _autoGenerateCode.SetNextIndex(lastperson.PersonId);
            }
            else
            {
                // If no persons exist, start from the first index
                _autoGenerateCode.SetNextIndex("PS000");
            }
            var model = await _context.Person.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            // Generate a new PersonId using the AutoGenerateCode class
            var newPersonId = _autoGenerateCode.GenerateCode();
            ViewBag.NewPersonId = newPersonId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            var newPersonId = _autoGenerateCode.GenerateCode();
            person.PersonId = newPersonId; // Assign the generated PersonId to the person object
            // Ensure the PersonId is set before saving
            if (person.PersonId == null)
            {
                person.PersonId = newPersonId; // Assign the generated PersonId if not set
            }
            // Validate the model state
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("PersonId", "PersonId is required.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            return View(person);
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Person == null)
            {
                return NotFound();
            }
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Person == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Person'  is null.");
            }
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);    
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
    }
}
