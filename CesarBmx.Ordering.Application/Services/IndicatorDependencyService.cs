using System.Collections.Generic;
using System.Threading.Tasks;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Ordering.Application.Messages;
using CesarBmx.Ordering.Domain.Builders;
using CesarBmx.Ordering.Domain.Models;
using CesarBmx.Ordering.Persistence.Contexts;

namespace CesarBmx.Ordering.Application.Services
{
    public class IndicatorDependencyService
    {
        private readonly MainDbContext _mainDbContext;

        public IndicatorDependencyService(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<List<Indicator>> GetDependencies(List<string> dependencyIds)
        {
            var dependencies = new List<Indicator>();
            foreach (var dependencyId in dependencyIds)
            {
                var userId = IndicatorBuilder.BuildUserId(dependencyId);
                var indicatorId = IndicatorBuilder.BuildIndicatorId(dependencyId);

                // Get indicator
                var dependency = await _mainDbContext.Indicators.FindAsync(userId, indicatorId);

                // Throw NotFound if it does not exist
                if (dependency == null) throw new NotFoundException(string.Format(IndicatorDependencyMessage.IndicatorDependencyNotFound, dependencyId));

                // Add
                dependencies.Add(dependency);
            }

            // Return
            return dependencies;
        }
    }
}
