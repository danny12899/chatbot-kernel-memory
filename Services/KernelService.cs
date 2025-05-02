using chatbot_kernel_memory.Util;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

namespace chatbot_kernel_memory.Services
{
    public class KernelService : IKernelService
    {
        private readonly AppSettings _appSettings;

        public KernelService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Kernel GetKernel(string modelId)
        {
            // Create a kernel with Azure OpenAI chat completion
            var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, _appSettings.Endpoint, _appSettings.ApiKey);

            // Add enterprise components
            builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

            // Build the kernel
            return builder.Build();
        }
    }
}
