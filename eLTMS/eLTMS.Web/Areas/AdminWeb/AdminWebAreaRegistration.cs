using System.Web.Mvc;

namespace eLTMS.Web.Areas.AdminWeb
{
    public class AdminWebAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminWeb";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AdminWeb_default",
                "AdminWeb/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}