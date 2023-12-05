using Agendo.Server.Models;
using Agendo.Server.Persistance;

namespace Agendo.Server.Services
{
    public interface IDomainService
    {
        Task<List<DomainDTO>> GetAllAsync(int superior);
        Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains);
    }

    public class DomainService(IDomainRepository _domainRepository) : IDomainService
    {

        public async Task<List<DomainDTO>> GetAllAsync(int superior)
        {
            return await _domainRepository.GetAllAsync(superior);
        }
        public async Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains)
        {
            return await _domainRepository.GetListAsync(superior,domains);
        }
    }
}
