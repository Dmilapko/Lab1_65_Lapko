using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        public List<SubjectEntity> GetAll()
        {
            return MockStorage.Subjects;
        }

        public SubjectEntity? GetById(Guid id)
        {
            return MockStorage.Subjects.FirstOrDefault(s => s.Id == id);
        }
    }
}
