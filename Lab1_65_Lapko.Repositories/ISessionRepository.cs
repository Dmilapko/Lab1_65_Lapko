using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public interface ISessionRepository
    {
        Task<List<SessionEntity>> GetBySubjectIdAsync(Guid subjectId);
        Task<SessionEntity?> GetByIdAsync(Guid id);
        Task AddAsync(SessionEntity session);
        Task<bool> UpdateAsync(SessionEntity session);
        Task<bool> DeleteAsync(Guid id);
    }
}
