using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public List<SessionEntity> GetBySubjectId(Guid subjectId)
        {
            return MockStorage.Sessions.Where(s => s.SubjectId == subjectId).ToList();
        }

        public SessionEntity? GetById(Guid id)
        {
            return MockStorage.Sessions.FirstOrDefault(s => s.Id == id);
        }
    }
}
