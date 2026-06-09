using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Schedules;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Gymly.Application.Features.Classes.Commands.CreateClass;

public class CreateClassCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateClassCommand, int>
{
    public async Task<int> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        // Normalize class name: trim + lowercase for case-insensitive uniqueness
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
        await context.SaveChangesAsync(cancellationToken);

        return fitnessClass.Id;
    }
}