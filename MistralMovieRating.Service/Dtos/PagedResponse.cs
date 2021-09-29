using System.Collections.Generic;

namespace MistralMovieRating.Service.Dtos
{
    public class PagedResponse<TDto>
    {
        public List<TDto> Entities { get; set; }

        public int TotalCount { get; set; }
    }
}
