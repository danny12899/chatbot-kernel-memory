using Microsoft.KernelMemory;

namespace chatbot_kernel_memory.Services
{
    public interface IKernelMemoryService
    {
        IKernelMemory GetKernelMemory();
    }
}
