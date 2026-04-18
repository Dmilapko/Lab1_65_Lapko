using System.Text.Json;
using Lab1_65_Lapko.Repositories.Models;

namespace Lab1_65_Lapko.Repositories
{
    /// <summary>
    /// Thread-safe JSON-backed persistent store for Subjects and Sessions.
    /// Caches data in memory after first load; saves to file after every mutation.
    /// Seeds with initial data on first run (when the file does not exist).
    /// </summary>
    public class JsonDataStore
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
        private AcademicData? _data;

        public JsonDataStore(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<SubjectEntity>> GetAllSubjectsAsync()
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                return data.Subjects.ToList();
            }
            finally { _lock.Release(); }
        }

        public async Task<SubjectEntity?> GetSubjectByIdAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                return data.Subjects.FirstOrDefault(s => s.Id == id);
            }
            finally { _lock.Release(); }
        }

        public async Task AddSubjectAsync(SubjectEntity subject)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                data.Subjects.Add(subject);
                await SaveInternalAsync(data);
            }
            finally { _lock.Release(); }
        }

        public async Task<bool> UpdateSubjectAsync(SubjectEntity subject)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                var existing = data.Subjects.FirstOrDefault(s => s.Id == subject.Id);
                if (existing == null) return false;

                existing.Name = subject.Name;
                existing.EctsCredits = subject.EctsCredits;
                existing.Area = subject.Area;

                await SaveInternalAsync(data);
                return true;
            }
            finally { _lock.Release(); }
        }

        public async Task<bool> DeleteSubjectAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                var subject = data.Subjects.FirstOrDefault(s => s.Id == id);
                if (subject == null) return false;

                data.Subjects.Remove(subject);
                // Cascade: sessions cannot exist without their subject
                data.Sessions.RemoveAll(s => s.SubjectId == id);

                await SaveInternalAsync(data);
                return true;
            }
            finally { _lock.Release(); }
        }

        public async Task<List<SessionEntity>> GetSessionsBySubjectAsync(Guid subjectId)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                return data.Sessions.Where(s => s.SubjectId == subjectId).ToList();
            }
            finally { _lock.Release(); }
        }

        public async Task<SessionEntity?> GetSessionByIdAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                return data.Sessions.FirstOrDefault(s => s.Id == id);
            }
            finally { _lock.Release(); }
        }

        public async Task AddSessionAsync(SessionEntity session)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                data.Sessions.Add(session);
                await SaveInternalAsync(data);
            }
            finally { _lock.Release(); }
        }

        public async Task<bool> UpdateSessionAsync(SessionEntity session)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                var existing = data.Sessions.FirstOrDefault(s => s.Id == session.Id);
                if (existing == null) return false;

                existing.SubjectId = session.SubjectId;
                existing.Topic = session.Topic;
                existing.Type = session.Type;
                existing.Date = session.Date;
                existing.StartTime = session.StartTime;
                existing.EndTime = session.EndTime;

                await SaveInternalAsync(data);
                return true;
            }
            finally { _lock.Release(); }
        }

        public async Task<bool> DeleteSessionAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                var data = await EnsureLoadedAsync();
                var session = data.Sessions.FirstOrDefault(s => s.Id == id);
                if (session == null) return false;

                data.Sessions.Remove(session);
                await SaveInternalAsync(data);
                return true;
            }
            finally { _lock.Release(); }
        }

        // Assumes the caller holds the lock.
        private async Task<AcademicData> EnsureLoadedAsync()
        {
            if (_data != null) return _data;

            if (!File.Exists(_filePath))
            {
                _data = SeedData();
                await SaveInternalAsync(_data);
            }
            else
            {
                var json = await File.ReadAllTextAsync(_filePath);
                _data = JsonSerializer.Deserialize<AcademicData>(json, _jsonOptions) ?? new AcademicData();
            }
            return _data;
        }

        private async Task SaveInternalAsync(AcademicData data)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private static AcademicData SeedData()
        {
            var data = new AcademicData();

            var sub1 = new SubjectEntity { Name = "C# Programming", EctsCredits = 5, Area = KnowledgeArea.Programming };
            var sub2 = new SubjectEntity { Name = "Linear Algebra", EctsCredits = 4, Area = KnowledgeArea.Mathematics };
            var sub3 = new SubjectEntity { Name = "Lollygagging", EctsCredits = 3, Area = KnowledgeArea.Humanities };

            data.Subjects.Add(sub1);
            data.Subjects.Add(sub2);
            data.Subjects.Add(sub3);

            for (int i = 1; i <= 10; i++)
            {
                data.Sessions.Add(new SessionEntity
                {
                    SubjectId = sub1.Id,
                    Topic = $"C# Lesson {i}",
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(11, 30),
                    Type = i % 2 == 0 ? SessionType.Laboratory : SessionType.Lecture
                });
            }

            data.Sessions.Add(new SessionEntity
            {
                SubjectId = sub2.Id,
                Topic = "Matrices Intro",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0),
                Type = SessionType.Lecture
            });
            data.Sessions.Add(new SessionEntity
            {
                SubjectId = sub2.Id,
                Topic = "Vectors",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 30),
                Type = SessionType.Seminar
            });

            return data;
        }

        public class AcademicData
        {
            public List<SubjectEntity> Subjects { get; set; } = new();
            public List<SessionEntity> Sessions { get; set; } = new();
        }
    }
}
