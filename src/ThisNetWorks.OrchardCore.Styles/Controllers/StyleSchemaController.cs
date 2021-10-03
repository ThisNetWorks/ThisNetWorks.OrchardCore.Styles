using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement;
using OrchardCore.Mvc.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Routing;
using OrchardCore.Settings;
using ThisNetWorks.OrchardCore.Styles.Models;
using ThisNetWorks.OrchardCore.Styles.Services;
using ThisNetWorks.OrchardCore.Styles.ViewModels;

namespace ThisNetWorks.OrchardCore.Styles.Controllers
{
    [Admin]
    public class StyleSchemaController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly StyleSchemaManager _styleSchemaManager;
        private readonly ISiteService _siteService;
        private readonly IStringLocalizer S;
        private readonly dynamic New;

        public StyleSchemaController(
            IAuthorizationService authorizationService,
            StyleSchemaManager styleSchemaManager,
            IShapeFactory shapeFactory,
            ISiteService siteService,
            IStringLocalizer<StyleSchemaController> stringLocalizer)
        {
            _authorizationService = authorizationService;
            _styleSchemaManager = styleSchemaManager;
            New = shapeFactory;
            _siteService = siteService;
            S = stringLocalizer;
        }

        public async Task<IActionResult> Index(StyleSchemaIndexOptions options, PagerParameters pagerParameters)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var pager = new Pager(pagerParameters, siteSettings.PageSize);

            // default options
            if (options == null)
            {
                options = new StyleSchemaIndexOptions();
            }

            var document = await _styleSchemaManager.GetDocumentAsync();

            var schemaEntries = document.Schemas.Values.Select(x => new StyleSchemaEntry { Schema = x })
                .OrderBy(entry => entry.Schema.Name)
                .Skip(pager.GetStartIndex())
                .Take(pager.PageSize)
                .ToList();


            var pagerShape = (await New.Pager(pager)).TotalItemCount(schemaEntries.Count());

            var model = new StyleSchemaIndexViewModel
            {
                Schemas = schemaEntries,
                Options = options,
                Pager = pagerShape
            };

            return View(model);
        }

        [HttpPost, ActionName("Index")]
        [FormValueRequired("submit.Filter")]
        public ActionResult IndexFilterPOST(StyleSchemaIndexViewModel model)
        {
            return RedirectToAction("Index", new RouteValueDictionary {
                { "Options.Search", model.Options.Search }
            });
        }
        public async Task<IActionResult> Create(string name)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            var model = new StyleSchemaViewModel
            {
                Name = name
            };

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePost(StyleSchemaViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError(nameof(StyleSchemaViewModel.Name), S["The name is mandatory."]);
                }
                if (String.IsNullOrWhiteSpace(model.Schema) || !model.Schema.IsJson())
                {
                    ModelState.AddModelError(nameof(StyleSchemaViewModel.Schema), S["Invalid JSON configuration."]);
                }
            }

            if (ModelState.IsValid)
            {
                var document = await _styleSchemaManager.GetDocumentAsync();
                if (document.Schemas.ContainsKey(model.Name))
                {
                    ModelState.AddModelError(nameof(StyleSchemaViewModel.Name), S["A schema with the same name already exists."]);
                }
                else
                {
                    var styleSchema = new StyleSchema
                    {
                        Name = model.Name,
                        Schema = FormatJson(model.Schema)
                    };

                    await _styleSchemaManager.UpdateAsync(model.Name, styleSchema);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string name)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            var document = await _styleSchemaManager.GetDocumentAsync();

            if (!document.Schemas.ContainsKey(name))
            {
                return RedirectToAction("Create", new { name });
            }


            var schema = document.Schemas[name];

            var model = new StyleSchemaViewModel
            {
                Name = name,
                Schema = schema.Schema
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StyleSchemaViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                if (String.IsNullOrWhiteSpace(model.Name))
                {
                    ModelState.AddModelError(nameof(StyleSchemaViewModel.Name), S["The name is mandatory."]);
                }
                if (String.IsNullOrWhiteSpace(model.Schema) || !model.Schema.IsJson())
                {
                    ModelState.AddModelError(nameof(StyleSchemaViewModel.Schema), S["Invalid JSON configuration."]);
                }
            }

            if (ModelState.IsValid)
            {
                var schema = new StyleSchema
                {
                    Name = model.Name,
                    Schema = FormatJson(model.Schema)
                };

                await _styleSchemaManager.UpdateAsync(model.Name, schema);

                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageStyleSchemas))
            {
                return Forbid();
            }

            var document = await _styleSchemaManager.LoadDocumentAsync();

            if (!document.Schemas.ContainsKey(name))
            {
                return NotFound();
            }

            await _styleSchemaManager.RemoveAsync(name);

            return RedirectToAction(nameof(Index));
        }

        private string FormatJson(string json)
        {
            var jObject = JObject.Parse(json);
            return jObject.ToString(Newtonsoft.Json.Formatting.Indented);
        }
    }
}
