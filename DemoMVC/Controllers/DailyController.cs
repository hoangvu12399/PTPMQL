using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models;
using DemoMVC.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

public class DailyController : Controller
{
    private readonly ApplicationDbContext _context;

    public DailyController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _context.Daily.ToListAsync();
        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Daily daily)
    {
        if (ModelState.IsValid)
        {
            _context.Add(daily);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(daily);
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Daily == null)
        {
            return NotFound();
        }
        var daily = await _context.Daily.FindAsync(id);
        if (daily == null)
        {
            return NotFound();
        }
        return View(daily);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("MaDaily,TenDaily")] Daily daily)
    {
        if (id != daily.MaDaily)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(daily);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyExists(daily.MaDaily))
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
        return View(daily);
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.Daily == null)
        {
            return NotFound();
        }
        var daily = await _context.Daily.FindAsync(id);
        if (daily == null)
        {
            return NotFound();
        }
        return View(daily);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.Daily == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Daily'  is null.");
        }
        var daily = await _context.Daily.FindAsync(id);
        if (daily != null)
        {
            _context.Daily.Remove(daily);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DailyExists(string maDaily)
    {
        return _context.Daily.Any(e => e.MaDaily == maDaily);
    }
}
