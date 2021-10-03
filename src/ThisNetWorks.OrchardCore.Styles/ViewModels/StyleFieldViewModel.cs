using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using ThisNetWorks.OrchardCore.Styles.Models;

namespace ThisNetWorks.OrchardCore.Styles.ViewModels
{
    public class StyleFieldViewModel
    {
        public string CompiledStyle { get; set; }
        public StyleField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
