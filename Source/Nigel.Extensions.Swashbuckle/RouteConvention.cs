using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Nigel.Extensions.Swashbuckle
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IApplicationModelConvention" />
    public class RouteConvention : IApplicationModelConvention
    {
        /// <summary>
        /// The central prefix
        /// </summary>
        private readonly AttributeRouteModel _centralPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteConvention"/> class.
        /// </summary>
        /// <param name="routeTemplateProvider">The route template provider.</param>
        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        //接口的Apply方法
        /// <summary>
        /// Called to apply the convention to the <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.
        /// </summary>
        /// <param name="application">The <see cref="T:Microsoft.AspNetCore.Mvc.ApplicationModels.ApplicationModel" />.</param>
        public void Apply(ApplicationModel application)
        {
            //遍历所有的 Controller
            foreach (var controller in application.Controllers)
            {
                // 已经标记了 RouteAttribute 的 Controller
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        // 在 当前路由上 再 添加一个 路由前缀
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);

                        // 在 当前路由上 不再 添加任何路由前缀
                        //selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }

                // 没有标记 RouteAttribute 的 Controller
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        // 添加一个 路由前缀
                        //selectorModel.AttributeRouteModel = _centralPrefix;

                        // 不添加前缀(说明：不使用全局路由，重构action，实现自定义、特殊的action路由地址)
                        selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel;
                    }
                }
            }
        }
    }
}