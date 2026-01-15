using Exam1.Areas.Admin.ViewModels;
using Exam1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam1.Areas.Admin.Controllers;

[Area("Admin")]
public class DesignationController : Controller
{
    private readonly AppDbContext _context;

    public DesignationController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Designation> designations = await _context.Designations.ToListAsync();
        return View(designations);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(DesignationCreateVM designationCreateVM)
    {
        if (!ModelState.IsValid)
        {
            return View(designationCreateVM);
        }
        Designation designation = new();
        designation.DesignationName = designationCreateVM.DesignationName;

        _context.Designations.Add(designation);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Update(int id)
    {
        Designation? designation = _context.Designations.Find(id);
        if (designation == null) return NotFound();
        return View(designation);
    }

    [HttpPost]
    public IActionResult Update(Designation designation)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }


        Designation? baseDesignation = _context.Designations.Find(designation.Id);
        if (baseDesignation == null) return NotFound();

        baseDesignation.DesignationName = designation.DesignationName;

        _context.Designations.Update(baseDesignation);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Designation? designation = await _context.Designations.FindAsync(id);
        if (designation == null) return NotFound();
        _context.Designations.Remove(designation);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
