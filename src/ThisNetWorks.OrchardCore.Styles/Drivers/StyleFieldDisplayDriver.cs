using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using OrchardCore.Shortcodes.Services;
using ThisNetWorks.OrchardCore.Styles.Models;
using ThisNetWorks.OrchardCore.Styles.Services;
using ThisNetWorks.OrchardCore.Styles.ViewModels;

namespace ThisNetWorks.OrchardCore.Styles.Drivers
{
    public class StyleFieldDisplayDriver : ContentFieldDisplayDriver<StyleField>
    {
        private readonly StyleSchemaManager _styleSchemaManager;
        private readonly IShortcodeService _shortcodeService;
        private readonly IStringLocalizer S;

        public StyleFieldDisplayDriver(
            StyleSchemaManager styleSchemaManager,
            IShortcodeService shortcodeService,
            IStringLocalizer<StyleFieldDisplayDriver> localizer)
        {
            _styleSchemaManager = styleSchemaManager;
            _shortcodeService = shortcodeService;
            S = localizer;
        }

        public override IDisplayResult Display(StyleField field, BuildFieldDisplayContext context)
        {
            return Initialize<StyleFieldViewModel>(GetDisplayShapeType(context), async model =>
            {
                model.CompiledStyle = await _shortcodeService.ProcessAsync(field.CompiledStyle ?? String.Empty);
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            })
            .Location("Detail", "Content")
            .Location("Summary", "Content");
        }

        public override IDisplayResult Edit(StyleField field, BuildFieldEditorContext context)
        {
            return Initialize<EditStyleFieldViewModel>(GetEditorShapeType(context), async model =>
            {
                var settings = context.PartFieldDefinition.GetSettings<StyleFieldSettings>();
                model.StyleRecord = field.StyleRecord.ToString();
                model.CompiledStyle = String.IsNullOrEmpty(field.CompiledStyle) ? String.Empty : field.CompiledStyle;
                if (!String.IsNullOrEmpty(settings.StyleSchemaName))
                {
                    var document = await _styleSchemaManager.GetDocumentAsync();
                    if (document.Schemas.TryGetValue(settings.StyleSchemaName, out var schema))
                    {
                        model.Schema = schema.Schema;
                    }
                }
                else
                {
                    model.Schema = String.IsNullOrEmpty(settings.StyleSchema) ? "{}" : settings.StyleSchema;
                }
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(StyleField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditStyleFieldViewModel();
            if (await updater.TryUpdateModelAsync(viewModel, Prefix))
            {
                try
                {
                    field.StyleRecord = JObject.Parse(viewModel.StyleRecord);
                    field.CompiledStyle = viewModel.CompiledStyle;
                }
                catch
                {
                    updater.ModelState.AddModelError(Prefix, nameof(viewModel.StyleRecord), S["Invalid JSON supplied"]);
                }
            }

            return Edit(field, context);
        }
    }
}
