using Lab1_65_Lapko.Services.DTOs;

namespace Lab1_65_Lapko.Services
{
    /// <summary>
    /// Interface for academic data operations
    /// </summary>
    public interface IAcademicService
    {
        List<SubjectListDto> GetAllSubjects();
        SubjectDetailDto? GetSubjectDetail(Guid id);
        SessionDetailDto? GetSessionDetail(Guid id);
    }
}
