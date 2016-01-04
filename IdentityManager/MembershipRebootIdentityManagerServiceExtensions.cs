using IdentityManager;
using IdentityManager.Configuration;
using ServerHost.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerHost.IdentityManager
{
    public static class MembershipRebootIdentityManagerServiceExtensions
    {
        public static void Configure(this IdentityManagerServiceFactory factory, string connectionString)
        {
            factory.IdentityManagerService = new Registration<IIdentityManagerService, CustomIdentityManagerService>();
            factory.Register(new Registration<CustomUserAccountService>());
            factory.Register(new Registration<CustomGroupService>());
            factory.Register(new Registration<CustomUserRepository>());
            factory.Register(new Registration<CustomGroupRepository>());
            factory.Register(new Registration<CustomDatabase>(resolver => new CustomDatabase(connectionString)));
            factory.Register(new Registration<CustomConfig>(CustomConfig.Config));
        }
    }
}