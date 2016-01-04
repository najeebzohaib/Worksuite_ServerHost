using ServerHost.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ServerHost
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CreateFirstUser();
        }

        private void CreateFirstUser()
        {
            var username = WebConfigurationManager.AppSettings["FirstUserUsername"];
            var password = WebConfigurationManager.AppSettings["FirstUserPassword"];
            var email = WebConfigurationManager.AppSettings["FirstUserEmail"];

            var firstUserReg = new RegisterFirstAdminUser();
            firstUserReg.CreateFirstUser(username, password, email);
        }
    }
}