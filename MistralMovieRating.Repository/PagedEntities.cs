using System.Collections.Generic;

namespace MistralMovieRating.Repository
{
    public class PagedEntities<TEntity>
    {
        public List<TEntity> Entities { get; set; }

        public int TotalCount { get; set; }
    }
}
