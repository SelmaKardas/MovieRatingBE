using MistralMovieRating.Repository.Entities;

namespace MistralMovieRating.Repository
{
    public class PermissionsRepository : Repository<Permission, MovieRatingDBContext>, IPermissionsRepository
    {
        public PermissionsRepository(MovieRatingDBContext context)
            : base(context)
        {
        }
    }
}
