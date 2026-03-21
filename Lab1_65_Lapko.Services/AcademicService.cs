using Lab1_65_Lapko.Repositories;
using Lab1_65_Lapko.Repositories.Models;
using Lab1_65_Lapko.Services.DTOs;

namespace Lab1_65_Lapko.Services
{
    public class AcademicService : IAcademicService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISessionRepository _sessionRepository;

        public AcademicService(ISubjectRepository subjectRepository, ISessionRepository sessionRepository)
        {
            _subjectRepository = subjectRepository;
            _sessionRepository = sessionRepository;
        }

        public List<SubjectListDto> GetAllSubjects()
        {
            return _subjectRepository.GetAll()
                .Select(e => new SubjectListDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    EctsCredits = e.EctsCredits,
                    Area = e.Area.ToString()
                })
                .ToList();
        }

        public SubjectDetailDto? GetSubjectDetail(Guid id)
        {
            var entity = _subjectRepository.GetById(id);
            if (entity == null) return null;

            var sessions = _sessionRepository.GetBySubjectId(id);
            var sessionDtos = sessions.Select(MapToSessionListDto).ToList();

            var totalDuration = TimeSpan.Zero;
            foreach (var s in sessions)
            {
                totalDuration += s.EndTime - s.StartTime;
            }

            return new SubjectDetailDto
            {
                Id = entity.Id,
                Name = entity.Name,
                EctsCredits = entity.EctsCredits,
                Area = entity.Area.ToString(),
                TotalDuration = totalDuration,
                Sessions = sessionDtos
            };
        }

        public SessionDetailDto? GetSessionDetail(Guid id)
        {
            var entity = _sessionRepository.GetById(id);
            if (entity == null) return null;

            return new SessionDetailDto
            {
                Id = entity.Id,
                SubjectId = entity.SubjectId,
                Topic = entity.Topic,
                Type = entity.Type.ToString(),
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Duration = entity.EndTime - entity.StartTime
            };
        }

        private SessionListDto MapToSessionListDto(SessionEntity entity)
        {
            return new SessionListDto
            {
                Id = entity.Id,
                Topic = entity.Topic,
                Type = entity.Type.ToString(),
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime
            };
        }
    }
}
