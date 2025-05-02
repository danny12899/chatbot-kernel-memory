using Microsoft.SemanticKernel;

namespace chatbot_kernel_memory.Services
{
    public interface IKernelService
    {
        Kernel GetKernel(string modelId);
    }
}
