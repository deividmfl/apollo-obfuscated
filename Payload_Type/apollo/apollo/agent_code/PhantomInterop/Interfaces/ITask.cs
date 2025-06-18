using System.Threading.Tasks;
namespace PhantomInterop.Interfaces
{
    public interface AdaptiveGateway3DAB
    {
        string ID();
        void Start();
        Task CreateTasking();
        void Kill();
    }
}
