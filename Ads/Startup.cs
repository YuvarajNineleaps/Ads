using Owin;
using System.Web.Http;

namespace Ads
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            // Swagger
            SwaggerConfig.Register(config);

            // Authentication token
            // ConfigureOAuth(app);

            // SignalR configuration
            // ConfigureSignalR(app);

            // Register routes
            WebApiConfig.Register(config);

            // Allow cross-domain requests
            // appBuilder.UseCors(CorsOptions.AllowAll);

            appBuilder.UseWebApi(config);



        }
    }
}