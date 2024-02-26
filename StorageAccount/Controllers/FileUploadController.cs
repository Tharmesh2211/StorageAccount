using Microsoft.AspNetCore.Mvc;
using StorageAccount.Application.IServices;
using StorageAccount.Domain.Model;

namespace StorageAccount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureFileStoreController : ControllerBase
    {
       private readonly IAzureFileStore _fileStore;
        public AzureFileStoreController(IAzureFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        [HttpPost("UploadFile")]

        public async Task<IActionResult> UploadFile(AzureFileStore azureFileStore)
        {
            var result = await _fileStore.UploadFile(azureFileStore);

            if (result is BadRequestResult)
            {
                return BadRequest("Invalid file.");
            }

            return result;
        }
    }
}
