using System;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Deployment;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using ThisNetWorks.OrchardCore.Styles.Drivers;
using ThisNetWorks.OrchardCore.Styles.Models;
using ThisNetWorks.OrchardCore.Styles.Controllers;
using ThisNetWorks.OrchardCore.Styles.Services;
using ThisNetWorks.OrchardCore.Styles.ViewModels;
using OrchardCore.Recipes;
using ThisNetWorks.OrchardCore.Styles.Recipes;
using ThisNetWorks.OrchardCore.Styles.Deployment;
using OrchardCore.DisplayManagement.Handlers;

namespace ThisNetWorks.OrchardCore.Styles
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<StyleSchemaManager>()
                .AddScoped<IPermissionProvider, Permissions>()
                .AddScoped<INavigationProvider, AdminMenu>()
                ;

            services.Configure<TemplateOptions>(o =>
            {
                o.MemberAccessStrategy.Register<StyleField>();
                o.MemberAccessStrategy.Register<StyleFieldViewModel>();
            });

            services.AddContentField<StyleField>()
                .UseDisplayDriver<StyleFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, StyleFieldSettingsDisplayDriver>();

            services.AddRecipeExecutionStep<StyleSchemasStep>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var styleSchemaControllerName = typeof(StyleSchemaController).ControllerName();

            routes.MapAreaControllerRoute(
                name: "StyleSchema",
                areaName: "ThisNetWorks.OrchardCore.Styles",
                pattern: _adminOptions.AdminUrlPrefix + "/StyleSchema",
                defaults: new { controller = styleSchemaControllerName, action = nameof(StyleSchemaController.Index) }
            );
            routes.MapAreaControllerRoute(
                name: "StyleSchemaCreate",
                areaName: "ThisNetWorks.OrchardCore.Styles",
                pattern: _adminOptions.AdminUrlPrefix + "/StyleSchema/Create/{name}",
                defaults: new { controller = styleSchemaControllerName, action = nameof(StyleSchemaController.Create) }
            );
            routes.MapAreaControllerRoute(
                name: "StyleSchemaEdit",
                areaName: "ThisNetWorks.OrchardCore.Styles",
                pattern: _adminOptions.AdminUrlPrefix + "/StyleSchema/Edit/{name}",
                defaults: new { controller = styleSchemaControllerName, action = nameof(StyleSchemaController.Edit) }
            );
        }
    }

    [RequireFeatures("OrchardCore.Deployment")]
    public class ShortcodeTemplatesDeployementStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDeploymentSource, AllStyleSchemasDeploymentSource>();
            services.AddSingleton<IDeploymentStepFactory>(new DeploymentStepFactory<AllStyleSchemasDeploymentStep>());
            services.AddScoped<IDisplayDriver<DeploymentStep>, AllStyleSchemasDeploymentStepDriver>();
        }
    }
}
