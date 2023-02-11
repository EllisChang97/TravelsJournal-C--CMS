using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TravelsJournal.Startup))]
namespace TravelsJournal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
