using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BackendOrganizationManagement.Startup))]
namespace BackendOrganizationManagement
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
