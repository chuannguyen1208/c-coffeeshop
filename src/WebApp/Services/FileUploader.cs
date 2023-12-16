using CShop.UseCases.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApp.Services;

internal class FileUploader(IHostEnvironment env) : IFileUploader
{
    private readonly long maxFileSize = 1024 * 150; // 150kb

    public async Task<string> UploadFile(IBrowserFile file, string fileName)
    {
        var trustedFileNameForFileStorage = $"{fileName}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}{Path.GetExtension(file.Name)}";
        var returnFilePath = Path.Combine("img", trustedFileNameForFileStorage);
        var path = Path.Combine(env.ContentRootPath, "wwwroot", returnFilePath);

        await using FileStream fs = new(path, FileMode.Create);
        await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

        return returnFilePath;
    }

    public async Task<string> UploadFileBase64(IBrowserFile file)
    {
        using var ms = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(ms);
        return Convert.ToBase64String(ms.ToArray());
    }
}
