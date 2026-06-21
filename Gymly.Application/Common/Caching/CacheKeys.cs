namespace Gymly.Application.Common.Caching;

public static class CacheKeys
{
    private const string TrainerPrefix = "trainers";
    private const string ClassPrefix = "classes";
    private const string RolePrefix = "roles";
    private const string PlanPrefix = "plans";

    public static string AllTrainers = $"{TrainerPrefix}:lookup";
    public static string AllClasses = $"{ClassPrefix}:lookup";
    public static string AllRoles = $"{RolePrefix}:lookup";
    public static string AllPlans = $"{PlanPrefix}:list";

    public static string ClassesByPage(int pageNumber, int pageSize) => $"{ClassPrefix}:lookup:page:{pageNumber}:size:{pageSize}";
    public static string PlansByFilter(bool? showInactive, string? sortBy) => $"{PlanPrefix}:list:{(showInactive ?? false)}:{sortBy ?? "default"}";
}