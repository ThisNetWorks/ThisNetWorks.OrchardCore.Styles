using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.Styles.Models;
using OrchardCore.Documents;

namespace ThisNetWorks.OrchardCore.Styles.Services
{
    public class StyleSchemaManager
    {

        private readonly IDocumentManager<StyleSchemaDocument> _documentManager;

        public StyleSchemaManager(IDocumentManager<StyleSchemaDocument> documentManager) => _documentManager = documentManager;

        /// <summary>
        /// Loads the schema document from the store for updating and that should not be cached.
        /// </summary>
        public Task<StyleSchemaDocument> LoadDocumentAsync() => _documentManager.GetOrCreateMutableAsync();

        /// <summary>
        /// Gets the schema document from the cache for sharing and that should not be updated.
        /// </summary>
        public Task<StyleSchemaDocument> GetDocumentAsync() => _documentManager.GetOrCreateImmutableAsync();


        public async Task RemoveAsync(string name)
        {
            var document = await LoadDocumentAsync();
            document.Schemas.Remove(name);
            await _documentManager.UpdateAsync(document);
        }

        public async Task UpdateAsync(string name, StyleSchema styleSchema)
        {
            var document = await LoadDocumentAsync();
            document.Schemas[name] = styleSchema;
            await _documentManager.UpdateAsync(document);
        }
    }
}
