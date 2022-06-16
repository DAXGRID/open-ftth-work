using Microsoft.Extensions.DependencyInjection;
using OpenFTTH.EventSourcing;
using OpenFTTH.EventSourcing.InMem;
using System;
using System.Reflection;

namespace OpenFTTH.Work.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEventStore, InMemEventStore>();

            var businessAssemblies = new Assembly[] {
                AppDomain.CurrentDomain.Load("OpenFTTH.Work.Business")
            };

            services.AddProjections(businessAssemblies);
        }
    }
}
