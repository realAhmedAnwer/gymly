using Microsoft.EntityFrameworkCore;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Repositories;

public class MemberRepository(GymlyDbContext context) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Member?> GetByQrTokenAsync(Guid token, CancellationToken cancellationToken = default)
    {
        return await context.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.AttendanceCardToken == token, cancellationToken);
    }

    public async Task<IEnumerable<Member>> GetAllActiveMembersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Members
            .AsNoTracking()
            .Where(m => m.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        await context.Members
            .AddAsync(member, cancellationToken);
    }

    public void Update(Member member)
    {
        context.Members
            .Update(member);
    }

    public void Delete(Member member)
    {
        context.Members
            .Remove(member);
    }
}