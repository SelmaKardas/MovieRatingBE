using MistralMovieRating.Repository.Entities;

namespace MistralMovieRating.Repository
{
    public class RolesRepository : Repository<Role, MovieRatingDBContext>, IRolesRepository
    {
        public RolesRepository(MovieRatingDBContext context)
            : base(context)
        {
        }
    }
}
