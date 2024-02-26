using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using StorageAccount.Application.IServices;
using StorageAccount.Domain.Model;

namespace StorageAccount.Infrastructure.Repositories
{
    public class AzureFileStoreRepository : IAzureFileStore
    {
        public static IWebHostEnvironment _hostEnvironment;
        public AzureFileStoreRepository(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<ActionResult> UploadFile(AzureFileStore azureFileStore)
        {
            try
            {
                if (azureFileStore.formFile == null || azureFileStore.formFile.Length == 0)
                {
                    return new BadRequestResult();
                }

                string uploadPath = Path.Combine(_hostEnvironment.ContentRootPath, "Images");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                //string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(azureFileStore.formFile.FileName);

                string uniqueFileName = azureFileStore.blobName + Path.GetExtension(azureFileStore.formFile.FileName);
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await azureFileStore.formFile.CopyToAsync(stream);
                }
                azureFileStore.blobName += Path.GetExtension(azureFileStore.formFile.FileName);
                await UploadFileAsync(filePath, azureFileStore);

                return new ContentResult
                {
                    Content = filePath
                };
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
           
        }
        public async Task<ActionResult> UploadFileAsync(string filePath, AzureFileStore azureFileStore)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(azureFileStore.connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(azureFileStore.containerName);
                var blobClient = blobContainerClient.GetBlobClient(azureFileStore.blobName);

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await blobClient.UploadAsync(stream, true);
                    Console.WriteLine(stream);
                }
                return new ContentResult { };
            }
            catch (Exception ex)
            {
                 return new BadRequestObjectResult(ex);
            }
           
        }
    }
}
