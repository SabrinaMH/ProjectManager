using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using ProjectManager;
using ProjectManager.API;
using ProjectManager.Infrastructure;
using ProjectManager.Persistence;

[assembly: OwinStartup(typeof(Startup))]
namespace ProjectManager
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (app == null) throw new ArgumentNullException($"{nameof(app)} was null");
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();
            config.Services.Replace(typeof(IHttpControllerActivator), new CompositionRoot());
            

            app.UseWebApi(config);

            const string rootFolder = ".";
            var fileSystem = new PhysicalFileSystem(rootFolder);
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = fileSystem,
            };
            options.DefaultFilesOptions.DefaultFileNames = new List<string> {"index.html"};
            app.UseFileServer(options);
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
            ApiController controller;
            if (controllerType == typeof(ProjectController))
            {
                var projectRepository = new ProjectRepository(_eventBus);
                controller = new ProjectController(projectRepository);
            }
            else if (controllerType == typeof(TaskController))
            {
                var taskRepository = new TaskRepository(_eventBus);
                controller = new TaskController(taskRepository);
            }
            else if (controllerType == typeof(NoteController))
            {
                var noteRepository = new NoteRepository(_eventBus);
                controller = new NoteController(noteRepository);
            }
            else
            {
                throw new ArgumentException("Unexpected type!", nameof(controllerType));
            }

            return controller;
        }
    }
}