using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using ServerHost.MembershipReboot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServerHost.App_Start
{
    public class RegisterFirstAdminUser
    {
        UserAccountService<CustomUser> userAccountService;
        GroupService groupSvc;

        public RegisterFirstAdminUser()
        {
            var repo = new CustomUserRepository(new CustomDatabase("MembershipReboot"));
            this.userAccountService = new UserAccountService<CustomUser>(repo);
            this.groupSvc = new GroupService(new DefaultGroupRepository(new DefaultMembershipRebootDatabase("MembershipReboot")));
        }

        public void CreateFirstUser(string username, string password, string email)
        {
            try
            {
                var groups = this.groupSvc.Query.Query("UsersAdmin");
                if(!groups.Any())
                {
                    this.groupSvc.Create("default", "UsersAdmin");
                }
                if (!this.userAccountService.UsernameExists(username))
                {
                    var account = this.userAccountService.CreateAccount(username, password, email);
                    
                    this.userAccountService.AddClaim(account.ID, "role", "UsersAdmin");

                }
            }
            catch (ValidationException vex)
            {
                throw vex;
            }

        }
    }


}