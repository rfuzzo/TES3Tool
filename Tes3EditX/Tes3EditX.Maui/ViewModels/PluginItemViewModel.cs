using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tes3EditX.Maui.ViewModels
{
    public partial class PluginItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _enabled;

        [ObservableProperty]
        private FileInfo _fileInfo;

        public string Name => FileInfo.Name;

        public PluginItemViewModel(FileInfo item)
        {
            _fileInfo = item;
        }
    }
}
