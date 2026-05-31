using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Repositories;
using Gymly.Domain.Entities.Users;
using MediatR;

namespace Gymly.Application.Features.Trainers.Commands.CreateTrainer;

public class CreateTrainerCommandHandler(
    ITrainerRepository trainerRepository,
    IApplicationDbContext context) : IRequestHandler<CreateTrainerCommand, int>
{
    public async Task<int> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var existingTrainer = await trainerRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existingTrainer != null)
        {
            throw new InvalidOperationException("A trainer with this email address already exists.");
        }

        var trainer = new Trainer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Specialization = request.Specialization,
            HireDate = DateTime.UtcNow
        };

        await trainerRepository.AddAsync(trainer, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return trainer.Id;
    }
}