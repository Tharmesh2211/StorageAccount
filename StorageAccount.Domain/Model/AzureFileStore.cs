using Microsoft.AspNetCore.Http;

namespace StorageAccount.Domain.Model
{
    public class AzureFileStore
    {
        public string connectionString {  get; set; }
        public string containerName {  get; set; }
        public string blobName {  get; set; }
        public IFormFile formFile {  get; set; }
    }
}
