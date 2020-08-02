using System.Threading.Tasks;

namespace EsbcProducer.Repositories
{
    public interface IMessages
    {
        Task Send(object payload);
    }
}