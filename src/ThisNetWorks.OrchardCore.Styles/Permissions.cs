using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace ThisNetWorks.OrchardCore.Styles
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageStyleSchemas = new Permission("ManageStyleSchemas", "Manage Style Schemas");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageStyleSchemas }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageStyleSchemas }
                }
            };
        }
    }
}
