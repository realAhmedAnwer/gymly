using Microsoft.EntityFrameworkCore;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Repositories;

public class TrainerRepository(GymlyDbContext context) : ITrainerRepository
{
    public async Task<Trainer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Trainers
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Trainer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Trainers
            .FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower(), cancellationToken);
    }

    public async Task<IEnumerable<Trainer>> GetAllTrainersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Trainers.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        await context.Trainers.AddAsync(trainer, cancellationToken);
    }

    public void Update(Trainer trainer)
    {
        context.Trainers.Update(trainer);
    }

    public void Delete(Trainer trainer)
    {
        context.Trainers.Remove(trainer);
    }

    public Task<Trainer?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Trainer>> GetAllTrainersAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Trainer trainer)
    {
        throw new NotImplementedException();
    }
}