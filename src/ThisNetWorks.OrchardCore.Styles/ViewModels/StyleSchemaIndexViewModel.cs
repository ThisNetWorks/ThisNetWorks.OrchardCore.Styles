using System.Collections.Generic;
using ThisNetWorks.OrchardCore.Styles.Models;

namespace ThisNetWorks.OrchardCore.Styles.ViewModels
{
    public class StyleSchemaIndexViewModel
    {
        public List<StyleSchemaEntry> Schemas { get; set; }
        public StyleSchemaIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }
    }

    public class StyleSchemaEntry
    {
        public StyleSchema Schema { get; set; }
    }

    public class StyleSchemaIndexOptions
    {
        public string Search { get; set; }
    }
}
