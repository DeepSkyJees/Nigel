using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Nigel.Extensions.Swashbuckle
{
    /// <summary>
    ///
    /// </summary>
    public static class MvcOptionsExtensions
    {
        /// <summary>
        /// 扩展方法
        /// </summary>
        /// <param name="opts">The opts.</param>
        /// <param name="routeAttribute">自定的前缀内容</param>
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }
    }
}