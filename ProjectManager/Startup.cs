using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Owin;
using ProjectManager.Features.AddProject;
using ProjectManager.Infrastructure;
using ProjectManager.ViewProjectsFeature;

namespace ProjectManager
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException($"{nameof(app)} was null");
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IHttpControllerActivator), new CompositionRoot());
            app.UseWebApi(config);
        }
    }

    public class CompositionRoot : IHttpControllerActivator
    {
        private readonly EventBus _eventBus;

        public CompositionRoot()
        {
            _eventBus = new EventBus();
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (controllerType == typeof(ProjectController))
            {
                return new ProjectController(_eventBus);
            }

            throw new ArgumentException("Unexpected type!", nameof(controllerType));
        }
    }
}