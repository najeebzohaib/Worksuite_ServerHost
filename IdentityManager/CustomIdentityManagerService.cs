using IdentityManager.MembershipReboot;
using ServerHost.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerHost.IdentityManager
{
    public class CustomIdentityManagerService : MembershipRebootIdentityManagerService<CustomUser, CustomGroup>
    {
        public CustomIdentityManagerService(CustomUserAccountService userSvc, CustomGroupService groupSvc)
            : base(userSvc, groupSvc)
        {
        }
    }
}