using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThisNetWorks.OrchardCore.Styles.ViewModels
{
    public class StyleFieldSettingsViewModel
    {
        public string StyleSchemaName { get; set; }
        public string StyleSchema { get; set; }
        public string Hint { get; set; }
        public bool Required { get; set; }

        [BindNever]
        public List<SelectListItem> StyleSchemas { get; set; }
    }
}
