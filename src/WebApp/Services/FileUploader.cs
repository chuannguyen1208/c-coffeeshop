using CShop.UseCases.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApp.Services;

internal class FileUploader(IHostEnvironment env) : IFileUploader
{
    private readonly long maxFileSize = 1024 * 150; // 150kb

    public async Task<string> UploadFile(IBrowserFile file, string fileName)
    {
        var trustedFileNameForFileStorage = $"{fileName}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{Path.GetExtension(file.Name)}";
        var path = Path.Combine(env.ContentRootPath,
                "wwwroot", "img",
                trustedFileNameForFileStorage);

        await using FileStream fs = new(path, FileMode.Create);
        await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

        return path;
    }
}
