using Exam1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam1;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<Designation> Designations { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

}
