using Gymly.Domain.Entities.Users;

namespace Gymly.Application.Interfaces.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id);
    Task<Member?> GetByQrTokenAsync(Guid token);
    Task<IEnumerable<Member>> GetAllActiveMembersAsync();
    Task AddAsync(Member member);
    void Update(Member member);
}
