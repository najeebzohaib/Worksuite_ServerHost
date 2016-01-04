
using Owin;

using IdentityManager.Configuration;
using IdentityServer3.Core.Configuration;

using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using Serilog;
using ServerHost.IdentityManager;
using ServerHost.IdentityServer;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ServerHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Trace()
               .CreateLogger();


            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions()
            {
                AuthenticationType = "Cookies",
                
                LoginPath = new PathString("/Home/Login")
            });


            // Adding below stuff to secure identity manager 

            //JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            //app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Cookies",
            //});


            //app.UseOpenIdConnectAuthentication(new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationOptions
            //{
            //    AuthenticationType = "oidc",
            //    Authority = WebConfigurationManager.AppSettings["IdentityServerURL"], // https://localhost:44301/identity,
            //    ClientId = "WorksuiteThinClient",
            //    RedirectUri = WebConfigurationManager.AppSettings["IdentityServerAuthRedirectURL"], //"https://localhost:44301",
            //    ResponseType = "id_token",
            //    UseTokenLifetime = false,
            //    Scope = "openid idmgr",
            //    SignInAsAuthenticationType = "Cookies",
            //    Notifications = new Microsoft.Owin.Security.OpenIdConnect.OpenIdConnectAuthenticationNotifications
            //    {
            //        SecurityTokenValidated = n =>
            //        {
            //            n.AuthenticationTicket.Identity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
            //            return Task.FromResult(0);
            //        },
            //        RedirectToIdentityProvider = async n =>
            //        {
            //            if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
            //            {
            //                var result = await n.OwinContext.Authentication.AuthenticateAsync("Cookies");
            //                if (result != null)
            //                {
            //                    var id_token = result.Identity.Claims.GetValue("id_token");
            //                    if (id_token != null)
            //                    {
            //                        n.ProtocolMessage.IdTokenHint = id_token;
            //                        n.ProtocolMessage.PostLogoutRedirectUri = WebConfigurationManager.AppSettings["IdentityManagerURL"];  //"https://localhost:44337/idm";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //});



            ////////////////

            var connectionString = "MembershipReboot";

            app.Map("/admin", adminApp =>
            {
                var factory = new IdentityManagerServiceFactory();
                factory.Configure(connectionString);

                adminApp.UseIdentityManager(new IdentityManagerOptions()
                {
                    Factory = factory,
                    SecurityConfiguration = new HostSecurityConfiguration()
                    {
                        HostAuthenticationType = "Cookies",
                        NameClaimType = "name",
                        RoleClaimType = "role",
                        AdminRoleName = "UsersAdmin"
                    }
                });
            });

            app.Map("/identity", core =>
            {
                var idSvrFactory = Factory.Configure();
                idSvrFactory.ConfigureCustomUserService(connectionString);

                var options = new IdentityServerOptions
                {
                    SiteName = "IdentityServer3 - UserService-MembershipReboot",

                    SigningCertificate = Certificate.Get(),
                    Factory = idSvrFactory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        
                    }
                    
                };

                core.UseIdentityServer(options);
            });
        }

      
    }
}