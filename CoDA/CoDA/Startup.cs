using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoDA.Startup))]
namespace CoDA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
