using Microsoft.AspNetCore.Mvc;
using StorageAccount.Domain.Model;

namespace StorageAccount.Application.IServices
{
    public interface IAzureFileStore
    {
        public Task<ActionResult> UploadFile(AzureFileStore azureFileStore);
    }
}
