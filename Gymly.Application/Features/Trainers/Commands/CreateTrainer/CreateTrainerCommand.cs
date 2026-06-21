using Gymly.Application.Common.Caching;
using Gymly.Application.Interfaces;
using Gymly.Application.Interfaces.Common;
using Gymly.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Trainers.Commands.CreateTrainer;

public record CreateTrainerCommand(
    string Name,
    string Email,
    string Phone,
    string Specialization) : IRequest<int>;

public class CreateTrainerCommandHandler(
    IApplicationDbContext context,
    ICacheService cacheService) : IRequestHandler<CreateTrainerCommand, int>
{
    public async Task<int> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await context.Trainers
            .AnyAsync(t => t.Email == normalizedEmail, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("A trainer with this email address already exists.");
        }

        var trainer = new Trainer
        {
            Name = request.Name,
            Email = normalizedEmail,
            Phone = request.Phone,
            Specialization = request.Specialization,
            HireDate = DateTime.UtcNow
        };

        context.Trainers.Add(trainer);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("A trainer with this email address already exists.");
        }

        await cacheService.RemoveAsync(CacheKeys.AllTrainers, cancellationToken);

        return trainer.Id;
    }
}
