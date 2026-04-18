using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly JsonDataStore _store;

        public SubjectRepository(JsonDataStore store)
        {
            _store = store;
        }

        public Task<List<SubjectEntity>> GetAllAsync() => _store.GetAllSubjectsAsync();

        public Task<SubjectEntity?> GetByIdAsync(Guid id) => _store.GetSubjectByIdAsync(id);

        public Task AddAsync(SubjectEntity subject) => _store.AddSubjectAsync(subject);

        public Task<bool> UpdateAsync(SubjectEntity subject) => _store.UpdateSubjectAsync(subject);

        public Task<bool> DeleteAsync(Guid id) => _store.DeleteSubjectAsync(id);
    }
}
