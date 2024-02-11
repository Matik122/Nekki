using System;
using Windows;

namespace Services.WindowService
{
    public class WindowResolver
    {
        private readonly WindowsService _windowsService;

        public WindowResolver(WindowsService windowsService)
        {
            _windowsService = windowsService;
        }
        
        public OptionsWindow.Model GetOptionsWindowModel(Action onClick)
        {
            return new(onClick, _windowsService);
        }

        public FailWindow.Model GetFailWindow(Action toLobby, Action restart)
        {
            return new(toLobby, restart, _windowsService);
        }
    }
}