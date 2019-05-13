 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using mAuth.BusinessEntities;
using mAuth.BusinessLogics;
using mAuth.DataAccess;
using Unity.Lifetime;
using Unity.Injection;
using ProductStore.Resolver;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace MAuthAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // You have to use Replace() because only one handler is supported

             var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            // Web API configuration and services
            var services = new UnityContainer();
            var connection = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            IMauthData data = new mAuth.DataAccess.Authorization(connection);

            IHttpAccess httpClient = new mAuth.Common.HttpAccessService();
            InjectionConstructor cityPassInjectionConstructor = new InjectionConstructor(httpClient);
            services.RegisterType<ICityPass, mAuth.BusinessLogics.CityPassService>(cityPassInjectionConstructor);

            

            InjectionConstructor injectionConstructor = new InjectionConstructor(data);

            services.RegisterType<IMauthBussiness, mAuth.BusinessLogics.Authorization>(injectionConstructor);


            ImAuthUserDataAccess udata = new mAuth.DataAccess.mAuthUser(connection);
            InjectionConstructor userInjector = new InjectionConstructor(udata);
            services.RegisterType<ImAuthUserBusiness, mAuth.BusinessLogics.mAuthUser>(userInjector);


            IPeopleCounterData peopleData = new mAuth.DataAccess.PeopleCounter(connection);
            InjectionConstructor peopleInjection = new InjectionConstructor(peopleData);
            services.RegisterType<IPeopleCounterBusiness, mAuth.BusinessLogics.PeopleCounter>(peopleInjection);

            
            config.DependencyResolver = new UnityResolver(services);
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IExceptionHandler), new CommonExceptionHandler());
            //config.Services.Add(typeof(IExceptionHandler), new CommonExceptionHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
            );

            log4net.Config.XmlConfigurator.Configure();
        }
            

    }
}
