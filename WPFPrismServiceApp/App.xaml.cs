using WPFPrism.AuthModule;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System.Windows;
using WPFPrism.HomeModule;
using WPFPrism.ServiceModule;
using WPFPrismServiceApp.ViewModels;
using WPFPrismServiceApp.Views;
using WPFPrism.ManagementModule;
using WPFPrism.AnalyticsModule;
using WPFPrism.Infrastructure.Services.Interface;
using WPFPrism.Infrastructure.Services;
using WPFPrism.Infrastructure;
using Prism.DryIoc;
using Microsoft.EntityFrameworkCore;
using WPFPrism.Infrastructure.Database;
using DryIoc;
using System.Configuration;
using WPFPrism.Infrastructure.Models;
using Microsoft.Data.SqlClient;

namespace WPFPrismServiceApp
{

    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MigrateDatabase();
        }
        private void MigrateDatabase()
        {
            var context = Container.GetContainer().Resolve<IApplicationDbContext>();
            if (context is DbContext dbContext)
            {
                dbContext.Database.Migrate();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
            ViewModelLocationProvider.Register<NavigationControl, NavigationViewModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;

            containerRegistry.RegisterScoped<IApplicationDbContext>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new ApplicationDbContext(optionsBuilder.Options);
            });


            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();


            #region Services
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            #endregion
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HomeModule>();       // Home
            moduleCatalog.AddModule<ServiceModule>();    // View of Service
            moduleCatalog.AddModule<AuthModule>();       // Authentification x Registration
            moduleCatalog.AddModule<ManagementModule>(); // Data Management[For Admins]
            moduleCatalog.AddModule<AnalyticsModule>();  // Analytics/Statistics [For Admins]
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
