using Prism.Commands;
using System;


namespace WPFPrism.Infrastructure
{
    public interface IApplicationCommands
    {
        public CompositeCommand ShowCommand { get; }
    }

    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _showCommand = new CompositeCommand();

        public CompositeCommand ShowCommand
        {
            get { return _showCommand; }
        }

    }
}