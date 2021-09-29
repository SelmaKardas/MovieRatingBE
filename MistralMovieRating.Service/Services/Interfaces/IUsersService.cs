using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralMovieRating.Service
{
    public interface IUsersService
    {
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid id);

    }
}
