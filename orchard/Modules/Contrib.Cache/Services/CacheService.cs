using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using Contrib.Cache.Models;
using Contrib.Cache.ViewModels;
using Orchard;
using Orchard.Caching;
using Orchard.Data;
using Orchard.Utility.Extensions;

namespace Contrib.Cache.Services {
    public class CacheService : ICacheService {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IRepository<CacheParameterRecord> _repository;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public CacheService(
            IWorkContextAccessor workContextAccessor,
            IRepository<CacheParameterRecord> repository,
            ICacheManager cacheManager,
            ISignals signals) {
            _workContextAccessor = workContextAccessor;
            _repository = repository;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public IEnumerable<CacheItem> GetCacheItems() {
            var workContext = _workContextAccessor.GetContext();

            foreach (DictionaryEntry cacheEntry in workContext.HttpContext.Cache) {
                var cacheItem = cacheEntry.Value as CacheItem;
                if (cacheItem != null) {
                    yield return cacheItem;
                }
            }
        }

        public void Evict(string cacheKey, HttpContextBase httpContext) {
            httpContext.Cache.Remove(cacheKey);
        }

        public string GetRouteDescriptorKey(HttpContextBase httpContext, RouteBase routeBase) {
            var route = routeBase as Route;

            var dataTokens = route != null ? route.DataTokens : routeBase.GetRouteData(httpContext).DataTokens;

            var keyBuilder = new StringBuilder();

            if (route != null) {
                keyBuilder.AppendFormat("url={0};", route.Url);
            }

            // the data tokens are used in case the same url is used by several features, like *{path} (Rewrite Rules and Home Page Provider)
            if (dataTokens != null) {
                foreach (var key in dataTokens.Keys) {
                    keyBuilder.AppendFormat("{0}={1};", key, dataTokens[key]);
                }
            }

            return keyBuilder.ToString().ToLowerInvariant();
        }

        public CacheParameterRecord GetCacheParameterByKey(string key) {
            return _repository.Get(c => c.RouteKey == key);
        }

        public IEnumerable<RouteConfiguration> GetRouteConfigurations() {
            return _cacheManager.Get("GetRouteConfigurations",
                ctx => {
                    ctx.Monitor(_signals.When("GetRouteConfigurations"));
                    return _repository.Fetch(c => true).Select(c => new RouteConfiguration { RouteKey = c.RouteKey, Duration = c.Duration }).ToReadOnlyCollection();
                });
        }

        public void SaveCacheConfigurations(IEnumerable<RouteConfiguration> routeConfigurations) {
            // remove all current configurations
            var configurations = _repository.Fetch(c => true);

            foreach (var configuration in configurations) {
                _repository.Delete(configuration);
            }

            // save the new configurations
            foreach (var configuration in routeConfigurations) {
                if (!configuration.Duration.HasValue) {
                    continue;
                }

                _repository.Create(new CacheParameterRecord {
                    Duration = configuration.Duration.Value,
                    RouteKey = configuration.RouteKey
                });
            }

            // invalidate the cache
            _signals.Trigger("GetRouteConfigurations");
        }
    }
}