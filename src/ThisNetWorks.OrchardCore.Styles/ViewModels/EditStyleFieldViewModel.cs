using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using ThisNetWorks.OrchardCore.Styles.Models;

namespace ThisNetWorks.OrchardCore.Styles.ViewModels
{
    public class EditStyleFieldViewModel
    {
        public string StyleRecord { get; set; }
        public string CompiledStyle { get; set; }

        [BindNever]
        public string Schema { get; set; }

        [BindNever]
        public StyleField Field { get; set; }

        [BindNever]
        public ContentPart Part { get; set; }

        [BindNever]
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
