using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Maui.Services
{
    public class MauiNavigationService : INavigationService
    {
        private readonly ISettingsService _settingsService;

        public MauiNavigationService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public Task InitializeAsync()
        {
            return NavigateToAsync(_settingsService.GetWorkingDirectory().Exists
                ? "//Main/About"
                : "//Main/Main");
        }

        public Task NavigateToAsync(string route, IDictionary<string, object> routeParameters = null)
        {
            return
                routeParameters != null
                    ? Shell.Current.GoToAsync(route, routeParameters)
                    : Shell.Current.GoToAsync(route);
        }

        public Task PopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
