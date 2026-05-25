using Microsoft.EntityFrameworkCore;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Repositories;

public class MemberRepository(GymlyDbContext context) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(int id)
    {
        return await context.Members.FindAsync(id);
    }

    public async Task<Member?> GetByQrTokenAsync(Guid token)
    {
        return await context.Members.FirstOrDefaultAsync(m => m.AttendanceCardToken == token);
    }

    public async Task<IEnumerable<Member>> GetAllActiveMembersAsync()
    {
        return await context.Members.ToListAsync();
    }

    public async Task AddAsync(Member member)
    {
        await context.Members.AddAsync(member);
    }

    public void Update(Member member)
    {
        context.Members.Update(member);
    }
}