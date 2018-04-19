using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheLearningCenter.Startup))]
namespace TheLearningCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
