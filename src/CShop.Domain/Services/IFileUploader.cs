using Microsoft.AspNetCore.Components.Forms;

namespace CShop.Domain.Services;
public interface IFileUploader
{
    Task<string> UploadFile(IBrowserFile file, string fileName);
    Task<string> UploadFileBase64(IBrowserFile file);
}
