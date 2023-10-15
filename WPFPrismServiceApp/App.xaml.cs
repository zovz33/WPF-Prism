﻿using DryIoc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using WPFPrism.AnalyticsModule;
using WPFPrism.AuthModule;
using WPFPrism.HomeModule;
using WPFPrism.Infrastructure;
using WPFPrism.Infrastructure.Database;
using WPFPrism.Infrastructure.Services;
using WPFPrism.Infrastructure.Services.Interface;
using WPFPrism.ManagementModule;
using WPFPrism.ServiceModule;
using WPFPrismServiceApp.ViewModels;
using WPFPrismServiceApp.ViewModels.DialogViewModels;
using WPFPrismServiceApp.Views;
using WPFPrismServiceApp.Views.DialogViews;

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
            try
            {
                if (context is DbContext dbContext)
                {
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            ViewModelLocationProvider.Register<MainWindow, MainViewModel>();
            ViewModelLocationProvider.Register<NavigationControl, NavigationViewModel>();

            #region DataBase EntityFramework
            string connectionString = ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;

            containerRegistry.RegisterScoped<IApplicationDbContext>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new ApplicationDbContext(optionsBuilder.Options);
            });

            #endregion
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();


            #region Services
            containerRegistry.RegisterSingleton<IUserService, UserService>();
            #endregion



            containerRegistry.RegisterDialog<AlertDialogView, AlertDialogViewModel>();
            containerRegistry.RegisterDialog<SuccessDialogView, SuccessDialogViewModel>();
            containerRegistry.RegisterDialog<WarningDialogView, WarningDialogViewModel>();
            containerRegistry.RegisterDialogWindow<DialogWindow>();
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
