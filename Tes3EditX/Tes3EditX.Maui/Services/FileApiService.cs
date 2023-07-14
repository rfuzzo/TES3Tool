using CommunityToolkit.Maui.Storage;
using Tes3EditX.Maui.Services;
using Tes3EditX.Maui.WinUI;

namespace Tes3EditX.Backend.Services
{
    public class FileApiService : IFileApiService
    {



        public async Task<string> PickAsync(CancellationToken none)
        {
            var _folderPicker = App.Current.Services.GetService<IFolderPicker>();

            var result = await _folderPicker.PickAsync(CancellationToken.None);
            result.EnsureSuccess();

            return result.Folder.Path;
        }
    }
}
