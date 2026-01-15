namespace Exam1.Areas.Admin.ViewModels;

public class TeamMemberCreateVM
{
    public string FullName { get; set; } = null!;
    public int DesignationId { get; set; }
    public IFormFile Image { get; set; } = null!;

}
