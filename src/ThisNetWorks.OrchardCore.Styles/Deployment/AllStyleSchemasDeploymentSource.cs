using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.Deployment;
using OrchardCore.Shortcodes.Services;
using ThisNetWorks.OrchardCore.Styles.Services;

namespace ThisNetWorks.OrchardCore.Styles.Deployment
{
    public class AllStyleSchemasDeploymentSource : IDeploymentSource
    {
        private readonly StyleSchemaManager _styleSchemaManager;

        public AllStyleSchemasDeploymentSource(StyleSchemaManager styleSchemaManager)
        {
            _styleSchemaManager = styleSchemaManager;
        }

        public async Task ProcessDeploymentStepAsync(DeploymentStep step, DeploymentPlanResult result)
        {
            var allTemplatesStep = step as AllStyleSchemasDeploymentStep;

            if (allTemplatesStep == null)
            {
                return;
            }

            var styleSchemaObjects = new JObject();
            var tyleSchemas = await _styleSchemaManager.GetDocumentAsync();

            foreach (var styleSchema in tyleSchemas.Schemas)
            {
                styleSchemaObjects[styleSchema.Key] = JObject.FromObject(styleSchema.Value);
            }

            result.Steps.Add(new JObject(
                new JProperty("name", "StyleSchemas"),
                new JProperty("StyleSchemas", styleSchemaObjects)
            ));
        }
    }
}
