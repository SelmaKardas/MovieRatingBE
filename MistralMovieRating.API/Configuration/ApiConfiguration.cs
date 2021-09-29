
namespace MistralMovieRating.API.Configuration
{
    public class ApiConfiguration
    {
        public string DatabaseConnectionString { get; set; }

        public string ApiName { get; set; }

        public string ApiVersion { get; set; }

        public string IdentityServerBaseUrl { get; set; }

        public string ApiBaseUrl { get; set; }

        public bool RequireHttpsMetadata { get; set; }

        public string OidcApiName { get; set; }

        public string OriginAllowed { get; set; }

    }
}
