using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.Recipes.Models;
using OrchardCore.Recipes.Services;
using ThisNetWorks.OrchardCore.Styles.Models;
using ThisNetWorks.OrchardCore.Styles.Services;

namespace ThisNetWorks.OrchardCore.Styles.Recipes
{
    /// <summary>
    /// This recipe step creates a set of shortcodes.
    /// </summary>
    public class StyleSchemasStep : IRecipeStepHandler
    {
        private readonly StyleSchemaManager _styleSchemaManager;

        public StyleSchemasStep(StyleSchemaManager styleSchemaManager)
        {
            _styleSchemaManager = styleSchemaManager;
        }

        public async Task ExecuteAsync(RecipeExecutionContext context)
        {
            if (!String.Equals(context.Name, "StyleSchemas", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (context.Step.Property("StyleSchemas").Value is JObject styleSchemas)
            {
                foreach (var property in styleSchemas.Properties())
                {
                    var name = property.Name;
                    var value = property.Value.ToObject<StyleSchema>();

                    await _styleSchemaManager.UpdateAsync(name, value);
                }
            }
        }
    }
}
