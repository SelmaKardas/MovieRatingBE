using MistralMovieRating.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public class UsersRepository : Repository<User, MovieRatingDBContext>, IUsersRepository
    {

        public UsersRepository(MovieRatingDBContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<string>> GetPermissionsAsync(Guid id)
        {
            return await this.Context.RolePermissions
                    .AsNoTracking()
                    .Where(x => x.Role.RoleUsers.Any(user => user.UserId == id))
                    .Select(x => x.Permission.Name)
                    .Distinct().ToListAsync();
        }
    }
}
