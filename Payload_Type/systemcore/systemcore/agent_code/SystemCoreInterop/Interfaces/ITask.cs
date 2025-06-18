using System.Threading.Tasks;
namespace SystemAgentInterop.Interfaces
{
    public interface ITask
    {
        string ID();
        void Start();
        Task CreateTasking();
        void Kill();
    }
}
