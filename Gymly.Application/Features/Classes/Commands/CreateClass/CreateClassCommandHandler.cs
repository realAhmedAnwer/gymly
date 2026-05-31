using Gymly.Application.Interfaces;
using Gymly.Domain.Entities.Schedules;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Gymly.Application.Features.Classes.Commands.CreateClass;

public class CreateClassCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateClassCommand, int>
{
    public async Task<int> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        var existingClass = await context.Classes
            .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (existingClass != null)
        {
            throw new InvalidOperationException("A class with this name already exists.");
        }

        var fitnessClass = new Class
        {
            Name = request.Name,
            Description = request.Description,
            MaxCapacity = request.MaxCapacity
        };

        await context.Classes.AddAsync(fitnessClass, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return fitnessClass.Id;
    }
}