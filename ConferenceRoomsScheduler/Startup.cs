using ConferenceRoomsScheduler.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ConferenceRoomsScheduler.Startup))]
namespace ConferenceRoomsScheduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ApplicationDbContext AppContext = ApplicationDbContext.Create();
            Initializer Initializer = new Initializer();
            Initializer.InitializeDatabase(AppContext);
        }
    }
}
