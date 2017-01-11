#region using statements


using DMS_M306.App_Start;
using DMS_M306.DatabaseContext;
using DMS_M306.Interfaces;
using DMS_M306.Interfaces.Repositories;
using DMS_M306.Repositories;
using DMS_M306.Services;
using DMS_M306.Services.Interfaces;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Extensions.NamedScope;
using Ninject.Web.Common;
using System;
using System.Data.Entity;
using System.Web;
using WebActivatorEx;

#endregion

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace DMS_M306.App_Start
{
    public class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>
        /// The created kernel.
        /// </returns>
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
        /// Load your modules or register your services here!.
        /// </summary>
        /// <param name="kernel">
        /// The kernel.
        /// </param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDatabaseInitializer<DMSEntities>>().To<CreateDatabaseIfNotExist>();

            kernel.Bind<IDbContextFactory<DMSEntities>>().To<DbContextFactory>().InCallScope();
            kernel.Bind<IDbContextManager>().To<DbContextManager<DMSEntities>>().InCallScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InCallScope();

            kernel.Bind<IChangeRepository>().To<ChangeRepository>().InCallScope();
            kernel.Bind<IFileCategoryRepository>().To<FileCategoryRepository>().InCallScope();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>().InCallScope();
            kernel.Bind<IPhysicalStorageRepository>().To<PhysicalStorageRepository>().InCallScope();
            kernel.Bind<IReleaseRepository>().To<ReleaseRepository>().InCallScope();
            kernel.Bind<ICodeService>().To<QRCodeService>().InCallScope();
        }
    }
}