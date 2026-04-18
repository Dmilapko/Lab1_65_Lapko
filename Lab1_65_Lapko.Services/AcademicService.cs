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

        public async Task<List<SubjectListDto>> GetAllSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return subjects
                .Select(e => new SubjectListDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    EctsCredits = e.EctsCredits,
                    Area = e.Area.ToString()
                })
                .ToList();
        }

        public async Task<SubjectDetailDto?> GetSubjectDetailAsync(Guid id)
        {
            var entity = await _subjectRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var sessions = await _sessionRepository.GetBySubjectIdAsync(id);
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

        public async Task AddSubjectAsync(SubjectInputDto input)
        {
            var entity = new SubjectEntity
            {
                Name = input.Name,
                EctsCredits = input.EctsCredits,
                Area = ParseKnowledgeArea(input.Area)
            };
            await _subjectRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateSubjectAsync(Guid id, SubjectInputDto input)
        {
            var existing = await _subjectRepository.GetByIdAsync(id);
            if (existing == null) return false;

            var updated = new SubjectEntity
            {
                Id = existing.Id,
                Name = input.Name,
                EctsCredits = input.EctsCredits,
                Area = ParseKnowledgeArea(input.Area)
            };
            return await _subjectRepository.UpdateAsync(updated);
        }

        public Task<bool> DeleteSubjectAsync(Guid id) => _subjectRepository.DeleteAsync(id);

        public async Task<SessionDetailDto?> GetSessionDetailAsync(Guid id)
        {
            var entity = await _sessionRepository.GetByIdAsync(id);
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

        public async Task AddSessionAsync(SessionInputDto input)
        {
            var entity = new SessionEntity
            {
                SubjectId = input.SubjectId,
                Topic = input.Topic,
                Type = ParseSessionType(input.Type),
                Date = input.Date,
                StartTime = input.StartTime,
                EndTime = input.EndTime
            };
            await _sessionRepository.AddAsync(entity);
        }

        public async Task<bool> UpdateSessionAsync(Guid id, SessionInputDto input)
        {
            var existing = await _sessionRepository.GetByIdAsync(id);
            if (existing == null) return false;

            var updated = new SessionEntity
            {
                Id = existing.Id,
                SubjectId = input.SubjectId,
                Topic = input.Topic,
                Type = ParseSessionType(input.Type),
                Date = input.Date,
                StartTime = input.StartTime,
                EndTime = input.EndTime
            };
            return await _sessionRepository.UpdateAsync(updated);
        }

        public Task<bool> DeleteSessionAsync(Guid id) => _sessionRepository.DeleteAsync(id);

        private static SessionListDto MapToSessionListDto(SessionEntity entity)
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

        private static KnowledgeArea ParseKnowledgeArea(string value)
        {
            return Enum.TryParse<KnowledgeArea>(value, ignoreCase: true, out var parsed)
                ? parsed
                : KnowledgeArea.Engineering;
        }

        private static SessionType ParseSessionType(string value)
        {
            return Enum.TryParse<SessionType>(value, ignoreCase: true, out var parsed)
                ? parsed
                : SessionType.Lecture;
        }
    }
}
