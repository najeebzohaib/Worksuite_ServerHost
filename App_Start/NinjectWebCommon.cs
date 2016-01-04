[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ServerHost.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(ServerHost.App_Start.NinjectWebCommon), "Stop")]

namespace ServerHost.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using BrockAllen.MembershipReboot;
    using MembershipReboot;
    using BrockAllen.MembershipReboot.Ef;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var config = MembershipRebootConfig.Create();
            kernel.Bind<UserAccountService<CustomUser>>().ToSelf();
            kernel.Bind<MembershipRebootConfiguration<CustomUser>>().ToConstant(config);
            kernel.Bind<IUserAccountRepository<CustomUser>>().To<CustomUserRepository>().InRequestScope();
            kernel.Bind<CustomDatabase>().ToSelf().InRequestScope();
            kernel.Bind<IUserAccountRepository>().To<DefaultUserAccountRepository>().InRequestScope();
            //kernel.Bind<AuthenticationService>().To<SamAuthenticationService>();
        }        
    }
}