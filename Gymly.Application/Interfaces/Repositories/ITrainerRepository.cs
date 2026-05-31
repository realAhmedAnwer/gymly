using Gymly.Domain.Entities.Users;

namespace Gymly.Application.Interfaces.Repositories;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Trainer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Trainer>> GetAllTrainersAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Trainer trainer, CancellationToken cancellationToken = default);
    void Update(Trainer trainer);
    void Delete(Trainer trainer);
}