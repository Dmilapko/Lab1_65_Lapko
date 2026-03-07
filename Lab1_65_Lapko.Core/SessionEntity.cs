namespace Lab1_65_Lapko.Core
{
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; } // Foreign Key equivalent
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Topic { get; set; }
        public SessionType Type { get; set; }

        public SessionEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
