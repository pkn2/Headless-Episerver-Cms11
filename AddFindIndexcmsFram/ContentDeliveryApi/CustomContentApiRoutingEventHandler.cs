using EPiServer.ContentApi.Routing;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;
using System.Web;

namespace AddFindIndexcmsFram.ContentDeliveryApi
{
    public class CustomContentApiRoutingEventHandler : RoutingEventHandler
    {
        public CustomContentApiRoutingEventHandler(IContentRouteEvents routerEvents,
            ServiceAccessor<HttpContextBase> httpContextAccessor,
            ContentApiRouteService contentApiRouteService) 
            : base(routerEvents, httpContextAccessor, contentApiRouteService)
        {
        }

        protected override string GetLanguage(SegmentContext routingContext, HttpRequestBase request)
        {
            return routingContext.Language ?? routingContext.ContentLanguage ?? ContentLanguage.PreferredCulture.Name;
        }
    }
}