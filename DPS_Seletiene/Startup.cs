using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DPS_Seletiene.Startup))]
namespace DPS_Seletiene
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
