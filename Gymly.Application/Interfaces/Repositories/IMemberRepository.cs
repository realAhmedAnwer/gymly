using Gymly.Domain.Entities.Users;

namespace Gymly.Application.Interfaces.Repositories;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Member?> GetByQrTokenAsync(Guid token, CancellationToken cancellationToken = default);
    Task<IEnumerable<Member>> GetAllActiveMembersAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
    void Update(Member member);
    void Delete(Member member);
}
