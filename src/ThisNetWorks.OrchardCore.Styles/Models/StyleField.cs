using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;

namespace ThisNetWorks.OrchardCore.Styles.Models
{
    public class StyleField : ContentField
    {
        public JObject StyleRecord { get; set; } = new JObject();
        public string CompiledStyle { get; set; }
    }
}
