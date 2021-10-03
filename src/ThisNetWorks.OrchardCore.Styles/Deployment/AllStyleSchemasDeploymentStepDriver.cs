using OrchardCore.Deployment;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;

namespace ThisNetWorks.OrchardCore.Styles.Deployment
{
    public class AllStyleSchemasDeploymentStepDriver : DisplayDriver<DeploymentStep, AllStyleSchemasDeploymentStep>
    {
        public override IDisplayResult Display(AllStyleSchemasDeploymentStep step)
        {
            return
                Combine(
                    View("AllStyleSchemasDeploymentStep_Summary", step).Location("Summary", "Content"),
                    View("AllStyleSchemasDeploymentStep_Thumbnail", step).Location("Thumbnail", "Content")
                );
        }

        public override IDisplayResult Edit(AllStyleSchemasDeploymentStep step)
        {
            return View("AllStyleSchemassDeploymentStep_Edit", step).Location("Content");
        }
    }
}
