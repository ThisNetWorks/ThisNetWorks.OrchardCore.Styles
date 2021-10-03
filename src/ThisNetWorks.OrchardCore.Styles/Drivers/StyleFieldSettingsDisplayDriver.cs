using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using ThisNetWorks.OrchardCore.Styles.Models;
using ThisNetWorks.OrchardCore.Styles.Services;
using ThisNetWorks.OrchardCore.Styles.ViewModels;
using OrchardCore.Mvc.Utilities;

namespace ThisNetWorks.OrchardCore.Styles.Drivers
{
    public class StyleFieldSettingsDisplayDriver : ContentPartFieldDefinitionDisplayDriver<StyleField>
    {
        private readonly StyleSchemaManager _styleSchemaManager;
        private readonly IStringLocalizer S;

        public StyleFieldSettingsDisplayDriver(
            StyleSchemaManager styleSchemaManager,
            IStringLocalizer<StyleFieldSettingsDisplayDriver> stringLocalizer)
        {
            _styleSchemaManager = styleSchemaManager;
            S = stringLocalizer;
        }

        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
            => Initialize<StyleFieldSettingsViewModel>("StyleFieldSettings_Edit", async model =>
                 {
                     var settings = new StyleFieldSettings();
                     partFieldDefinition.PopulateSettings(settings);

                     model.StyleSchema = settings.StyleSchema;
                     model.StyleSchemaName = settings.StyleSchemaName;
                     model.Hint = settings.Hint;

                     var document = await _styleSchemaManager.GetDocumentAsync();
                     model.StyleSchemas = document.Schemas
                         .Select(x => new SelectListItem(x.Key, x.Key, String.Equals(model.StyleSchemaName, x.Key, StringComparison.OrdinalIgnoreCase)))
                         .ToList();

                     model.StyleSchemas.Insert(0, new SelectListItem(S["Custom schema"], String.Empty, String.IsNullOrEmpty(model.StyleSchemaName)));

                     if (String.IsNullOrEmpty(model.StyleSchema))
                     {
                         model.StyleSchema = "{}";
                     }
                 })
                .Location("Content");

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            var viewModel = new StyleFieldSettingsViewModel();

            if (await context.Updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                var model = new StyleFieldSettings
                {
                    StyleSchemaName = viewModel.StyleSchemaName,
                    StyleSchema = viewModel.StyleSchema,
                    Hint = viewModel.Hint
                };

                if (String.IsNullOrEmpty(model.StyleSchemaName) && String.IsNullOrEmpty(model.StyleSchema))
                {
                    context.Updater.ModelState.AddModelError(Prefix + '.' + nameof(StyleFieldSettingsViewModel.StyleSchema), S["A style schema is required."]);
                }
                else if (String.IsNullOrEmpty(model.StyleSchemaName) && !model.StyleSchema.IsJson())
                {
                    context.Updater.ModelState.AddModelError(Prefix + '.' + nameof(StyleFieldSettingsViewModel.StyleSchema), S["The style schema is written in an incorrect format."]);
                }
                else
                {
                    context.Builder.WithSettings(model);
                }
            }

            return Edit(partFieldDefinition);
        }
    }
}
