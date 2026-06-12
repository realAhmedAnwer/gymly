using Gymly.Application.Features.Classes.Queries.GetClassesLookup;

namespace Gymly.Web.Models.Classes;

public class ClassesIndexViewModel
{
    public List<ClassLookupDto> Classes { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10;
}