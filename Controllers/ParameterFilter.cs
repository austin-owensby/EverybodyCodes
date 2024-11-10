using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EverybodyCodes.Controllers
{
    public class ParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            // Ensure that the input quest is a valid value (1 - 20)
            // This does not check if the currently selected year has that date
            if (parameter.Name.Equals("quest", StringComparison.InvariantCultureIgnoreCase))
            {
                List<int> quests = Enumerable.Range(1, Globals.LAST_PUZZLE).ToList();
                parameter.Schema.Enum = quests.Select(d => new OpenApiString(d.ToString())).ToList<IOpenApiAny>();
            }

            // Ensure that the input year is a valid value (2024 - this year)
            if (parameter.Name.Equals("year", StringComparison.InvariantCultureIgnoreCase))
            {
                DateTime now = DateTime.UtcNow.AddHours(Globals.SERVER_UTC_OFFSET);

                List<int> quests = Enumerable.Range(Globals.START_YEAR, now.Year - Globals.START_YEAR + 1).ToList();
                parameter.Schema.Enum = quests.Select(d => new OpenApiString(d.ToString())).ToList<IOpenApiAny>();
            }

            // Ensure that the input part is a valid value (2024 - this year)
            if (parameter.Name.Equals("part", StringComparison.InvariantCultureIgnoreCase))
            {
                List<int> parts = [1, 2, 3];
                parameter.Schema.Enum = parts.Select(d => new OpenApiString(d.ToString())).ToList<IOpenApiAny>();
            }
        }
    }
}