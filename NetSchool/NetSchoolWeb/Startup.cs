using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetSchoolWeb.Startup))]
namespace NetSchoolWeb
{
    public partial class Startup
    { 
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}