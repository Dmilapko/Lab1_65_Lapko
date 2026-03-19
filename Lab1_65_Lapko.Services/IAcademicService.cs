using Lab1_65_Lapko.Models;

namespace Lab1_65_Lapko.Services
{
    /// <summary>
    /// Interface for academic data operations
    /// </summary>
    public interface IAcademicService
    {
        List<Subject> GetAllSubjects();
        List<Session> GetAllSessions();
        void AddSubject(Subject subject);
        bool UpdateSubject(Guid id, Subject templateSubject);
        bool DeleteSubject(Guid id);
        void AddSession(Session session);
        bool UpdateSession(Guid id, Session templateSession);
        bool DeleteSession(Guid id);
    }
}
