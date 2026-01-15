using System.ComponentModel.DataAnnotations;

namespace Exam1.Models;

public class TeamMember : BaseEntity
{
    public string FullName { get; set; } = null!;
    
    public string ImageUrl { get; set; } = null!;

    public int DesignationId { get; set; }
    public Designation Designation { get; set; } = null!;
}

public class Designation : BaseEntity
{
    public string DesignationName { get; set; } = null!;
}
