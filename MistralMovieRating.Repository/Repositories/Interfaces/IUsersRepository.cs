using MistralMovieRating.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralMovieRating.Repository
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<IEnumerable<string>> GetPermissionsAsync(Guid id);
    }
}
