using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Schedules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Application.Features.Classes.Commands.CreateClass;

public record CreateClassCommand(
    string Name,
    string Description,
    int MaxCapacity) : IRequest<int>;

public class CreateClassCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateClassCommand, int>
{
    public async Task<int> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim().ToLowerInvariant();

        var classExists = await context.Classes
            .AnyAsync(c => c.Name == normalizedName, cancellationToken);

        if (classExists)
        {
            throw new InvalidOperationException("A class with this name already exists.");
        }

        var fitnessClass = new Class
        {
            Name = normalizedName,
            Description = request.Description,
            MaxCapacity = request.MaxCapacity
        };

        await context.Classes.AddAsync(fitnessClass, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("A class with this name already exists.");
        }

        return fitnessClass.Id;
    }
}