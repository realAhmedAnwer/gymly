using Gymly.Application.Features.Sessions.Queries.GetSessionsList;

namespace Gymly.Web.Models.Sessions;

public class SessionsDashboardViewModel
{
    public List<SessionDto> Sessions { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? SearchTerm { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
