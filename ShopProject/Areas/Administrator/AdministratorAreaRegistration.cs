using System.Web.Mvc;

namespace ShopProject.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Administrator";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Administrator_default",
                url: "Administrator/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopProject.Areas.Administrator.Controllers" }
            );
        }
    }
}
