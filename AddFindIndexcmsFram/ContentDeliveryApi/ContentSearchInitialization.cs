using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;


//namespace AddFindIndexcmsFram.ContentDeliveryApi
//{
//    [InitializableModule]
//    [ModuleDependency(typeof(ServiceContainerInitialization), typeof(ServiceContainerInitialization))]
//    public class ContentSearchInitialization : IConfigurableModule
//    {
//        public void ConfigureContainer(ServiceConfigurationContext context)
//        {
//            context.Services.Configure<ContentApiSearchConfiguration>(config =>
//            {
//                config.Default().SetMaximumSearchResults(1);
//            }); 
//        }

//        public void Initialize(InitializationEngine context)
//        {
//        }

//        public void Uninitialize(InitializationEngine context)
//        {
//        }
//    }
//}