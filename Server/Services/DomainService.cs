using Agendo.Server.Models;
using Agendo.Server.Persistance;

namespace Agendo.Server.Services
{
    public interface IDomainService
    {
        Task<List<DomainDTO>> GetAllAsync();
    }

    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _domainRepository;

        public DomainService(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public async Task<List<DomainDTO>> GetAllAsync()
        {
            return await _domainRepository.GetAllAsync();
        }
    }
}
