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

        public List<Session> GetAllSessions()
        {
            var sessionEntities = MockStorage.Sessions;
            var result = new List<Session>();

            foreach (var entity in sessionEntities)
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


        public void AddSubject(Subject subject)
        {
            MockStorage.Subjects.Add(new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = subject.Name,
                EctsCredits = subject.EctsCredits,
                Area = subject.Area
            });
        }

        public bool UpdateSubject(Guid id, Subject templateSubjct)
        {
            var entity = MockStorage.Subjects.FirstOrDefault(s => s.Id == id);
            if (entity == null) return false;

            if (templateSubjct.Name != null)
                entity.Name = templateSubjct.Name;

            if (templateSubjct.EctsCredits != 0)
                entity.EctsCredits = templateSubjct.EctsCredits;

            if (templateSubjct.Area != 0)
                entity.Area = templateSubjct.Area;

            return true;
        }

        public bool DeleteSubject(Guid id)
        {
            var entity = MockStorage.Subjects.FirstOrDefault(s => s.Id == id);
            if (entity == null) return false;

            // Sessions cannot exist without a subject
            MockStorage.Sessions.RemoveAll(s => s.SubjectId == id);
            MockStorage.Subjects.Remove(entity);
            return true;
        }

        public void AddSession(Session session)
        {
            MockStorage.Sessions.Add(new SessionEntity
            {
                Id = Guid.NewGuid(),
                SubjectId = session.SubjectId,
                Date = session.Date,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Topic = session.Topic,
                Type = session.Type
            });
        }

        public bool UpdateSession(Guid id, Session templateSession)
        {
            var entity = MockStorage.Sessions.FirstOrDefault(s => s.Id == id);
            if (entity == null) return false;
            if (templateSession.SubjectId != Guid.Empty)
                entity.SubjectId = templateSession.SubjectId;
            if (templateSession.Date != default)
                entity.Date = templateSession.Date;
            if (templateSession.StartTime != default)
                entity.StartTime = templateSession.StartTime;
            if (templateSession.EndTime != default)
                entity.EndTime = templateSession.EndTime;
            if (templateSession.Topic != null)
                entity.Topic = templateSession.Topic;
            if (templateSession.Type != 0)
                entity.Type = templateSession.Type;
            return true;
        }

        public bool DeleteSession(Guid id)
        {
            var entity = MockStorage.Sessions.FirstOrDefault(s => s.Id == id);
            if (entity == null) return false;
            MockStorage.Sessions.Remove(entity);
            return true;
        }
    }
}
