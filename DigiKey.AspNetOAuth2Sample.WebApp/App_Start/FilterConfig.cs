using System.Web;
using System.Web.Mvc;

namespace DigiKey.AspNetOAuth2Sample.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
