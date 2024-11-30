using System.Web.Mvc;

namespace ShopProject.Areas.Shopper
{
    public class ShopperAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Shopper";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Shopper_default",
                url: "Shopper/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopProject.Areas.Shopper.Controllers" }
            );
        }
    }
}
