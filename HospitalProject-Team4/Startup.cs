using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProject_Team4.Startup))]
namespace HospitalProject_Team4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
