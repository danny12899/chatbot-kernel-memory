using Markdig;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using chatbot_kernel_memory.Util;
using Microsoft.Extensions.Options;
using chatbot_kernel_memory.Plugins;

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

        public async Task<string> AskQuestion(string prompt, string documentId)
        {
            var kernel = _kernelService.GetKernel();
            var km = _kernelMemoryService.GetKernelMemory();

            var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            if (!string.IsNullOrEmpty(documentId))
            {
                var documentSearchPlugin = new DocumentSearchPlugin(km, documentId);
                kernel.ImportPluginFromObject(documentSearchPlugin, "memory");
                prompt += ". Search your memory. {{memory.Ask}}";
            }

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
