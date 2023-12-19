using Microsoft.AspNetCore.Components.Forms;

namespace CShop.UseCases.Services;
public interface IFileUploader
{
    Task<string> UploadFile(IBrowserFile file, string fileName);
    Task<string> UploadFileBase64(IBrowserFile file);
}
