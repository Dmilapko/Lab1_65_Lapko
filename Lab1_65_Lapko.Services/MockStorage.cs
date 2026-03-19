using Lab1_65_Lapko.Data;

namespace Lab1_65_Lapko.Services
{
    public static class MockStorage
    {
        public static List<SubjectEntity> Subjects { get; private set; }
        public static List<SessionEntity> Sessions { get; private set; }

        static MockStorage()
        {
            Subjects = new List<SubjectEntity>();
            Sessions = new List<SessionEntity>();
            SeedData();
        }

        private static void SeedData()
        {
            var sub1 = new SubjectEntity { Name = "C# Programming", EctsCredits = 5, Area = KnowledgeArea.Programming };
            var sub2 = new SubjectEntity { Name = "Linear Algebra", EctsCredits = 4, Area = KnowledgeArea.Mathematics };
            var sub3 = new SubjectEntity { Name = "Lollygagging", EctsCredits = 3, Area = KnowledgeArea.Humanities };

            Subjects.AddRange([sub1, sub2, sub3]);

            for (int i = 1; i <= 10; i++)
            {
                Sessions.Add(new SessionEntity
                {
                    SubjectId = sub1.Id,
                    Topic = $"C# Lesson {i}",
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(11, 30),
                    Type = i % 2 == 0 ? SessionType.Laboratory : SessionType.Lecture
                });
            }

            Sessions.Add(new SessionEntity
            {
                SubjectId = sub2.Id,
                Topic = "Matrices Intro",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                Type = SessionType.Lecture
            });
            Sessions.Add(new SessionEntity
            {
                SubjectId = sub2.Id,
                Topic = "Vectors",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30),
                Type = SessionType.Seminar
            });
        }
    }
}
