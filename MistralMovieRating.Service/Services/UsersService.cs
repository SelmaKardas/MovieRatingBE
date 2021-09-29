using MistralMovieRating.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MistralMovieRating.Service
{
    public class UsersService : IUsersService
    {
        private IUnitOfWork<MovieRatingDBContext> unitOfWork;

        public UsersService(
            IUnitOfWork<MovieRatingDBContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid id)
        {
         
            return await this.unitOfWork.GetRepository<IUsersRepository>().GetPermissionsAsync(id);
        }

    }
}
