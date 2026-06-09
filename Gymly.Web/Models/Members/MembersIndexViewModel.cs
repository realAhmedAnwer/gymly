using Gymly.Application.Features.Members.Queries.GetMembers;

namespace Gymly.Web.Models.Members;

public class MembersIndexViewModel
{
    public List<MemberDto> Members { get; set; } = [];
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool ShowInactive { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10;
}