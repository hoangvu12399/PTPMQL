
using Microsoft.AspNetCore.Mvc;
using DemoMVC.Models.Entities;
using DemoMVC.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers;

    
public class Hethongphanphoi : Controller
{
    private readonly ApplicationDbContext _context;

    public Hethongphanphoi(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.HeThongPhanPhoi.ToListAsync();
        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Hethongphanphoi hethongphanphoi)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hethongphanphoi);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(hethongphanphoi);
    }
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.HeThongPhanPhoi == null)
        {
            return NotFound();
        }
        var hethongphanphoi = await _context.HeThongPhanPhoi.FindAsync(id);
        if (hethongphanphoi == null)
        {
            return NotFound();
        }
        return View(hethongphanphoi);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("MaHTPP,TenHTPP")] HeThongPhanPhoi hethongphanphoi)
    {
        if (id != hethongphanphoi.MaHTPP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hethongphanphoi);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeThongPhanPhoiExists(hethongphanphoi.MaHTPP))
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
        return View(hethongphanphoi);
    }
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.HeThongPhanPhoi == null)
        {
            return NotFound();
        }
        var hethongphanphoi = await _context.HeThongPhanPhoi.FindAsync(id);
        if (hethongphanphoi == null)
        {
            return NotFound();
        }
        return View(hethongphanphoi);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.HeThongPhanPhoi == null)
        {
            return Problem("Entity set 'ApplicationDbContext.HeThongPhanPhoi'  is null.");
        }
        var hethongphanphoi = await _context.HeThongPhanPhoi.FindAsync(id);
        if (hethongphanphoi != null)
        {
            _context.HeThongPhanPhoi.Remove(hethongphanphoi);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HeThongPhanPhoiExists(string maHTPP)
    {
        return _context.HeThongPhanPhoi.Any(e => e.MaHTPP == maHTPP);
    }
}
