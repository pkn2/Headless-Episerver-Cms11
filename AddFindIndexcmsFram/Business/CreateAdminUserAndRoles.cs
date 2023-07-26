using System.Configuration.Provider;
using System.Web.Security;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging.Compatibility;

namespace Alloy46.Business
{
    [InitializableModule]
    public class CreateAdminUserAndRoles : IInitializableModule
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CreateAdminUserAndRoles));

        public void Initialize(InitializationEngine context)
        {
#if DEBUG
            var mu = Membership.GetUser("Adminss");

            if (mu != null) return;

            try
            {
                Membership.CreateUser("Adminss", "Admin@123", "Admin@site.com");

                try
                {
                    this.EnsureRoleExists("WebEditors");
                    this.EnsureRoleExists("WebAdmins");
                    this.EnsureRoleExists("GlobalEditors");
                    this.EnsureRoleExists("Administrators");

                    Roles.AddUserToRoles("Adminss", new[] { "WebAdmins", "WebEditors" });
                }
                catch (ProviderException pe)
                {
                    Log.Error(pe);
                }
            }
            catch (MembershipCreateUserException mcue)
            {
                Log.Error(mcue);
            }
#endif
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private void EnsureRoleExists(string roleName)
        {
            if (Roles.RoleExists(roleName)) return;

            try
            {
                Roles.CreateRole(roleName);
            }
            catch (ProviderException pe)
            {
                Log.Error(pe);
            }
        }
    }
}