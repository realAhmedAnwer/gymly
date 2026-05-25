using Microsoft.EntityFrameworkCore;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;

namespace Gymly.Infrastructure.Repositories;

public class TrainerRepository(GymlyDbContext context) : ITrainerRepository
{
    public async Task<Trainer?> GetByIdAsync(int id)
    {
        return await context.Trainers.FindAsync(id);
    }

    public async Task<IEnumerable<Trainer>> GetAllTrainersAsync()
    {
        return await context.Trainers.ToListAsync();
    }

    public async Task AddAsync(Trainer trainer)
    {
        await context.Trainers.AddAsync(trainer);
    }

    public void Update(Trainer trainer)
    {
        context.Trainers.Update(trainer);
    }
}