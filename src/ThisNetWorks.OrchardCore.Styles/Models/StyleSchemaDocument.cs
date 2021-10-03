using System;
using System.Collections.Generic;
using OrchardCore.Data.Documents;

namespace ThisNetWorks.OrchardCore.Styles.Models
{
    public class StyleSchemaDocument : Document
    {
        public Dictionary<string, StyleSchema> Schemas { get; } = new Dictionary<string, StyleSchema>(StringComparer.OrdinalIgnoreCase);
    }
}
