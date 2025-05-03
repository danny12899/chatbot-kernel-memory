using Markdig;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;

namespace chatbot_kernel_memory.Services
{
    public class AzureService : IAzureService
    {
        private readonly IKernelService _kernelService;

        public AzureService(IKernelService kernelService)
        {
            _kernelService = kernelService;
        }

        public async Task<string> AskQuestion(string modelId, string prompt)
        {
            var kernel = _kernelService.GetKernel(modelId);
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
