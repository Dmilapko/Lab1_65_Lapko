namespace Lab1_65_Lapko.Services.DTOs
{
    public class SessionDetailDto
    {
        public Guid Id { get; init; }
        public Guid SubjectId { get; init; }
        public string Topic { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public DateOnly Date { get; init; }
        public TimeOnly StartTime { get; init; }
        public TimeOnly EndTime { get; init; }
        public TimeSpan Duration { get; init; }
    }
}
