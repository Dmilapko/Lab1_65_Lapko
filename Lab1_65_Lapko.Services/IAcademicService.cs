using Lab1_65_Lapko.Services.DTOs;

namespace Lab1_65_Lapko.Services
{
    /// <summary>
    /// Interface for academic data operations. All operations are async.
    /// </summary>
    public interface IAcademicService
    {
        Task<List<SubjectListDto>> GetAllSubjectsAsync();
        Task<SubjectDetailDto?> GetSubjectDetailAsync(Guid id);
        Task AddSubjectAsync(SubjectInputDto input);
        Task<bool> UpdateSubjectAsync(Guid id, SubjectInputDto input);
        Task<bool> DeleteSubjectAsync(Guid id);

        Task<SessionDetailDto?> GetSessionDetailAsync(Guid id);
        Task AddSessionAsync(SessionInputDto input);
        Task<bool> UpdateSessionAsync(Guid id, SessionInputDto input);
        Task<bool> DeleteSessionAsync(Guid id);
    }
}
