using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace ThisNetWorks.OrchardCore.Styles
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder.Add(S["Design"], design => design
                    .Add(S["Style Schemas"], S["Style Schemas"].PrefixPosition(), widgets => widgets
                        .Permission(Permissions.ManageStyleSchemas)
                        .Action("Index", "StyleSchema", new { area = "ThisNetWorks.OrchardCore.Styles" })
                        .LocalNav()
                    ));

            return Task.CompletedTask;
        }
    }
}
