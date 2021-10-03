using System;

namespace ThisNetWorks.OrchardCore.Styles.ViewModels
{
    public class StyleSchemaViewModel
    {

        public string Name { get; set; } = String.Empty;
        public bool Enable { get; set; } = true;
        public string Schema { get; set; }
        public string Description { get; set; } = String.Empty;
    }
}
