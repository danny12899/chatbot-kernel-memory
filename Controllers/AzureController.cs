using chatbot_kernel_memory.Services;
using Microsoft.AspNetCore.Mvc;

namespace chatbot_kernel_memory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AzureController : Controller
    {
        private readonly IAzureService _azureService;
        public AzureController(IAzureService azureService)
        {
            _azureService = azureService;
        }

        [HttpGet]
        [Route("AskQuestion")]
        public async Task<IActionResult> AskQuestion(string modelId, string prompt)
        {
            var response = await _azureService.AskQuestion(modelId, prompt);
            return Ok(response);
        }
    }
}
