using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public interface ISessionRepository
    {
        List<SessionEntity> GetBySubjectId(Guid subjectId);
        SessionEntity? GetById(Guid id);
    }
}
