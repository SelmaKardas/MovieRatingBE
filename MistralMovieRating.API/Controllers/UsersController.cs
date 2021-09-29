using MistralMovieRating.Common;
using MistralMovieRating.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MistralMovieRating.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }


        [HttpGet("{id}/permissions")]
        public Task<IEnumerable<string>> GetUserPermissions([Required] string id)
        {
            return this.usersService.GetUserPermissionsAsync(id.ToGuid());
        }
     
    }
}
