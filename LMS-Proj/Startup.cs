using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LMS_Proj.Startup))]
namespace LMS_Proj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
