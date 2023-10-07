using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Interop;

namespace WPFPrismServiceApp.Styles
{
    partial class Window
    {
        public Window()
        {
            InitializeComponent();
        }
 
        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = (System.Windows.Window) ((FrameworkElement) sender).TemplatedParent;
            SystemCommands.MinimizeWindow(window);    
        }

        private void MaximizeRestoreButton_OnClick(object sender, RoutedEventArgs e)
        {

            var window = (System.Windows.Window) ((FrameworkElement) sender).TemplatedParent;
            window.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            window.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            if (window.WindowState == WindowState.Normal)
            {
                SystemCommands.MaximizeWindow(window);
            }
            else
                SystemCommands.RestoreWindow(window);
        }
        

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = (System.Windows.Window) ((FrameworkElement) sender).TemplatedParent;
            SystemCommands.CloseWindow(window);
        }
    }
}