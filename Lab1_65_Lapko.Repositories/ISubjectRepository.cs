using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    public interface ISubjectRepository
    {
        List<SubjectEntity> GetAll();
        SubjectEntity? GetById(Guid id);
    }
}
