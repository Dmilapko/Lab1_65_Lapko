using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_65_Lapko
{
    public class AcademicService
    {
        // Get all Subjects (Parent Level)
        public List<Subject> GetAllSubjects()
        {
            var subjectEntities = MockStorage.Subjects;
            var result = new List<Subject>();

            foreach (var entity in subjectEntities)
            {
                result.Add(MapToDomain(entity));
            }
            return result;
        }

        // Get Details for a specific Subject (including loading sessions)
        public Subject GetSubjectDetails(Guid subjectId)
        {
            var entity = MockStorage.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (entity == null) return null;

            var domainSubject = MapToDomain(entity);

            var sessionEntities = MockStorage.Sessions.Where(s => s.SubjectId == subjectId).ToList();
            domainSubject.Sessions = sessionEntities.Select(MapToDomain).ToList();

            return domainSubject;
        }

        // Mappers
        private Subject MapToDomain(SubjectEntity entity)
        {
            return new Subject
            {
                Id = entity.Id,
                Name = entity.Name,
                EctsCredits = entity.EctsCredits,
                Area = entity.Area
            };
        }

        private Session MapToDomain(SessionEntity entity)
        {
            return new Session
            {
                Id = entity.Id,
                SubjectId = entity.SubjectId,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                Topic = entity.Topic,
                Type = entity.Type
            };
        }
    }
}
