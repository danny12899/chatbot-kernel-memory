using Markdig;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using chatbot_kernel_memory.Util;
using Microsoft.Extensions.Options;
using Microsoft.KernelMemory;

namespace chatbot_kernel_memory.Services
{
    public class AzureService : IAzureService
    {
        private readonly AppSettings _appSettings;
        private readonly IKernelService _kernelService;
        private readonly IKernelMemoryService _kernelMemoryService;

        public AzureService(IOptions<AppSettings> appSettings,
            IKernelService kernelService,
            IKernelMemoryService kernelMemoryService)
        {
            _appSettings = appSettings.Value;
            _kernelService = kernelService;
            _kernelMemoryService = kernelMemoryService;
        }

        public async Task<string> AskQuestion(string prompt)
        {
            var kernel = _kernelService.GetKernel();
            var km = _kernelMemoryService.GetKernelMemory();
            var memoryPlugin = kernel.ImportPluginFromObject(new MemoryPlugin(km, waitForIngestionToComplete: true), "memory");

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await chatCompletionService.GetChatMessageContentAsync(
                prompt,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

            if (!string.IsNullOrEmpty(result.Content))
                return Markdown.ToHtml(result.Content);

            return string.Empty;
        }

        public async Task<string> ImportDocumentAsync(Stream stream, string fileName)
        {
            var km = _kernelMemoryService.GetKernelMemory();
            return await km.ImportDocumentAsync(stream, fileName);
        }
    }
}
