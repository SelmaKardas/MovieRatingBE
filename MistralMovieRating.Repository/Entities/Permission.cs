using System.Collections.Generic;

namespace MistralMovieRating.Repository.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<RolePermission> PermissionRoles { get; set; }

    }
}
