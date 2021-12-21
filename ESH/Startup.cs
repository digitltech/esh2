using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ESH.Startup))]
namespace ESH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
