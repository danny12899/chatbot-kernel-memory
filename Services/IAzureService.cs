namespace chatbot_kernel_memory.Services
{
    public interface IAzureService
    {
        Task<string> AskQuestion(string prompt);
    }
}
