using MistralMovieRating.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MistralMovieRating.Repository.Entities
{
    public class Role : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        public BuiltInRoleType? BuiltInRoleType { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }

        public ICollection<UserRoles> RoleUsers { get; set; }


        [MaxLength(200)]
        public string Description { get; set; }

    }
}
