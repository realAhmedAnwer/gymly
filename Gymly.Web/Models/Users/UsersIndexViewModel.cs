using Gymly.Application.Features.Auth.Queries.GetSystemUsers;

namespace Gymly.Web.Models.Users;

public class UsersIndexViewModel
{
    public List<SystemUserDto> Users { get; set; } = [];
}