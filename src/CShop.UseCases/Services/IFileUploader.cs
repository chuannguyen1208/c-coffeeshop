using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.UseCases.Services;
public interface IFileUploader
{
    Task<string> UploadFile(IBrowserFile file, string fileName);
    Task<string> UploadFileBase64(IBrowserFile file);
}
