using System.Threading.Tasks;

namespace EsbcProducer.Services
{
    public interface IProducer
    {
        Task Send(object message);
    }
}