using Gymly.Domain.Entities.Users;

namespace Gymly.Application.Interfaces.Repositories;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(int id);
    Task<IEnumerable<Trainer>> GetAllTrainersAsync();
    Task AddAsync(Trainer trainer);
    void Update(Trainer trainer);
}
