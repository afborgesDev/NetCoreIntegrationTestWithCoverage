using System.Threading.Tasks;

namespace EntryPoint.Processor.Services
{
    public interface IWorkerService
    {
        Task ConsumeAsync();
    }
}
