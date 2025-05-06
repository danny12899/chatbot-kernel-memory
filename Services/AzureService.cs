using Markdig;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;
using chatbot_kernel_memory.Util;
using Microsoft.Extensions.Options;

namespace chatbot_kernel_memory.Services
{
    public class AzureService : IAzureService
    {
        private readonly AppSettings _appSettings;
        private readonly IKernelService _kernelService;

        public AzureService(IOptions<AppSettings> appSettings, IKernelService kernelService)
        {
            _appSettings = appSettings.Value;
            _kernelService = kernelService;
        }

        public async Task<string> AskQuestion(string prompt)
        {
            var kernel = _kernelService.GetKernel();
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
    }
}
