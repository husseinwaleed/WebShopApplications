using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webshoproject.Startup))]
namespace webshoproject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
