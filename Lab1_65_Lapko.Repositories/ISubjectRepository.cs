using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public interface ISubjectRepository
    {
        Task<List<SubjectEntity>> GetAllAsync();
        Task<SubjectEntity?> GetByIdAsync(Guid id);
        Task AddAsync(SubjectEntity subject);
        Task<bool> UpdateAsync(SubjectEntity subject);
        Task<bool> DeleteAsync(Guid id);
    }
}
