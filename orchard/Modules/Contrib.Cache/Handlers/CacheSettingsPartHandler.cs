using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Contrib.Cache.Models;
using Contrib.Cache.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Data;
using Orchard.ContentManagement.Handlers;
using Orchard.Mvc.Html;

namespace Contrib.Cache.Handlers {
    public class CacheSettingsPartHandler : ContentHandler {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly ICacheService _cacheService;
        private readonly RequestContext _requestContext;

        public CacheSettingsPartHandler(
            IRepository<CacheSettingsPartRecord> repository,
            IWorkContextAccessor workContextAccessor,
            ICacheService cacheService,
            RequestContext requestContext) {
            _workContextAccessor = workContextAccessor;
            _cacheService = cacheService;
            _requestContext = requestContext;
            Filters.Add(new ActivatingFilter<CacheSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));

            // initializing default cache settings values
            OnInitializing<CacheSettingsPart>((context, part) => { part.DefaultCacheDuration = 300; });

            // evict modified routable content when updated
            OnPublished<IContent>(
                (context, part) => {
                    // list of cache keys to evict
                    var evict = new List<CacheItem>();
                    var workContext = _workContextAccessor.GetContext();

                    Action<IContent> findAndEvict = p => {
                        foreach (var cacheItem in _cacheService.GetCacheItems()) {
                            var urlHelper = new UrlHelper(_requestContext);
                            if (String.Equals(cacheItem.Url, VirtualPathUtility.ToAbsolute("~/" + urlHelper.ItemDisplayUrl(p)), StringComparison.OrdinalIgnoreCase)) {
                                evict.Add(cacheItem);
                            }
                        }
                    };

                    findAndEvict(part);
                    
                    // search the cache for containers too
                    var commonPart = part.As<CommonPart>();
                    if (commonPart != null) {
                        if (commonPart.Container != null) {
                            findAndEvict(commonPart.Container);
                        }
                    }

                    // remove all content to evict
                    foreach (var cacheItem in evict) {
                        _cacheService.Evict(cacheItem.CacheKey, workContext.HttpContext);
                    }

                });
        }
    }
}