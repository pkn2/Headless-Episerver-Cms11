using EPiServer.ContentApi.Cms;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace AddFindIndexcmsFram.ContentDeliveryApi
{
    //   [InitializableModule]
    //[ModuleDependency(typeof(ServiceContainerInitialization), typeof(ContentApiCmsInitialization))]
    //public class ContentApiInitialization : IConfigurableModule
    //{
    //	public void ConfigureContainer(ServiceConfigurationContext context)
    //	{
    //		/// Optional
    //		//context.Services.AddTransient<RoutingEventHandler, CustomContentApiRoutingEventHandler>();


    //		context.Services.Configure<ContentApiConfiguration>(config =>
    //		{
    //               config.Default()
    //                   .SetMinimumRoles(string.Empty)
    //                   //.SetMultiSiteFilteringEnabled(false)
    //                   .SetSiteDefinitionApiEnabled(true);
    //               //.SetRequiredRole("contentapiread")
    //               //.SetSiteDefinitionApiEnabled(true)
    //               //config.EnablePreviewFeatures = false;
    //           });


    //	}

    //       public void Initialize(InitializationEngine context)
    //       {
    //	}

    //       public void Uninitialize(InitializationEngine context)
    //       {
    //       }
    //   }

    [InitializableModule]
    [ModuleDependency(typeof(ServiceContainerInitialization), typeof(ContentApiCmsInitialization), typeof(ServiceContainerInitialization))]

    public class ContentApiInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {

            context.Services.Configure<ContentApiSearchConfiguration>(config =>
            {
                config.Default().SetMaximumSearchResults(10);
            });



            context.Services.Configure<ContentApiConfiguration>(config =>
            {
                config.Default()
                    .SetMinimumRoles(string.Empty)
                    //.SetMultiSiteFilteringEnabled(false)
                    .SetSiteDefinitionApiEnabled(true);
                //.SetRequiredRole("contentapiread")
                //.SetSiteDefinitionApiEnabled(true)
                //config.EnablePreviewFeatures = true;

            });


        }
        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}