using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DMS_M306.Startup))]
namespace DMS_M306
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
