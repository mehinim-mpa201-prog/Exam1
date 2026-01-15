using Exam1.Areas.Admin.ViewModels;
using Exam1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Exam1.Areas.Admin.Controllers;
[Area("Admin")]
public class TeamMembersController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public TeamMembersController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        List<TeamMember> teamMembers = _context.TeamMembers.Include(tm => tm.Designation).ToList();
        return View(teamMembers);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Designations = await _context.Designations.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeamMemberCreateVM teamMemberCreateVM)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Designations = await _context.Designations.ToListAsync();
            return View();
        }

        TeamMember teamMember = new TeamMember
        {
            FullName = teamMemberCreateVM.FullName,
            DesignationId = teamMemberCreateVM.DesignationId,
        };

        #region AddImage
        string path = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string imageName = Guid.NewGuid() + teamMemberCreateVM.Image.FileName;
        using FileStream stream = new(Path.Combine(path, imageName), FileMode.Create);
        teamMemberCreateVM.Image.CopyTo(stream);
        teamMember.ImageUrl = imageName;
        #endregion

        await _context.TeamMembers.AddAsync(teamMember);
        await _context.SaveChangesAsync();


        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var designations = await _context.Designations.ToListAsync();
        ViewBag.Designations = designations;

        var teamMember = await _context.TeamMembers.Include(t => t.Designation).FirstOrDefaultAsync(t => t.Id == id);

        if (teamMember is null)
        {
            return NotFound();
        }

        TeamMemberUpdateVM teamMemberUpdateVM = new()
        {
            FullName = teamMember.FullName,
            DesignationId = teamMember.DesignationId
        };
        return View(teamMemberUpdateVM);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int id, TeamMemberUpdateVM teamMemberUpdateVM)
    {
        if (!ModelState.IsValid)
        {
            var designations = await _context.Designations.ToListAsync();
            ViewBag.Designations = designations;
            return View(teamMemberUpdateVM);
        }
        var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(t => t.Id == id);
        if (teamMember is null)
        {
            return NotFound();
        }

        teamMember.FullName = teamMemberUpdateVM.FullName;
        teamMember.DesignationId = teamMemberUpdateVM.DesignationId;
        if (teamMemberUpdateVM.Image is not null)
        {
            #region DeleteOldImage
            string oldImagePath = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");

            if (System.IO.File.Exists(Path.Combine(oldImagePath, teamMember.ImageUrl)))
                System.IO.File.Delete(Path.Combine(oldImagePath, teamMember.ImageUrl));
            #endregion

            #region AddImage
            string path = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string imageName = Guid.NewGuid() + teamMemberUpdateVM.Image.FileName;
            using FileStream stream = new(Path.Combine(path, imageName), FileMode.Create);
            teamMemberUpdateVM.Image.CopyTo(stream);
            teamMember.ImageUrl = imageName;
            #endregion          
        }

        _context.TeamMembers.Update(teamMember);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        TeamMember? teamMember = await _context.TeamMembers.FindAsync(id);
        if (teamMember == null) return NotFound();
        _context.TeamMembers.Remove(teamMember);
        await _context.SaveChangesAsync();

        #region DeleteOldImage
        string oldImagePath = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");

        if (System.IO.File.Exists(Path.Combine(oldImagePath, teamMember.ImageUrl)))
            System.IO.File.Delete(Path.Combine(oldImagePath, teamMember.ImageUrl));
        #endregion


        return RedirectToAction(nameof(Index));
    }
}
