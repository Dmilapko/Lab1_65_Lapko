using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly JsonDataStore _store;

        public SessionRepository(JsonDataStore store)
        {
            _store = store;
        }

        public Task<List<SessionEntity>> GetBySubjectIdAsync(Guid subjectId) => _store.GetSessionsBySubjectAsync(subjectId);

        public Task<SessionEntity?> GetByIdAsync(Guid id) => _store.GetSessionByIdAsync(id);

        public Task AddAsync(SessionEntity session) => _store.AddSessionAsync(session);

        public Task<bool> UpdateAsync(SessionEntity session) => _store.UpdateSessionAsync(session);

        public Task<bool> DeleteAsync(Guid id) => _store.DeleteSessionAsync(id);
    }
}
