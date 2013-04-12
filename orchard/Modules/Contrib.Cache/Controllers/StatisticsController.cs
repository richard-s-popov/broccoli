using System;
using System.Linq;
using System.Web.Mvc;
using Contrib.Cache.Services;
using Contrib.Cache.ViewModels;
using Orchard;
using Orchard.Environment.Configuration;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Admin;

namespace Contrib.Cache.Controllers {
    [Admin]
    public class StatisticsController : Controller {
        private readonly ICacheService _cacheService;
        private readonly ShellSettings _shellSettings;

        public StatisticsController(
            IOrchardServices services,
            ICacheService cacheService,
            ShellSettings shellSettings) {
            _cacheService = cacheService;
            _shellSettings = shellSettings;
            Services = services;
            }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index() {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage cache")))
                return new HttpUnauthorizedResult();

            var model = new StatisticsViewModel {
                CacheItems = _cacheService
                    .GetCacheItems()
                    .Where(x => x.Tenant.Equals(_shellSettings.Name, StringComparison.OrdinalIgnoreCase))
                    .ToList()
                    .OrderByDescending(x => x.CachedOnUtc),
            };

            return View(model);
        }

        public ActionResult Evict(string cacheKey) {
            if (!Services.Authorizer.Authorize(StandardPermissions.SiteOwner, T("Not allowed to manage cache")))
                return new HttpUnauthorizedResult();

            _cacheService.Evict(cacheKey, HttpContext);

            return RedirectToAction("Index");
        }
    }
}