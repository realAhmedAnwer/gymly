using Gymly.Application.Features.Classes.Queries.GetClassesLookup;

namespace Gymly.Web.Models.Classes;

public class ClassesIndexViewModel
{
    public IEnumerable<ClassLookupDto> Classes { get; set; } = [];
    public int TotalClasses => Classes.Count();
}