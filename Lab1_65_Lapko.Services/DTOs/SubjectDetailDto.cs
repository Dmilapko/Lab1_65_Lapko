namespace Lab1_65_Lapko.Services.DTOs
{
    public class SubjectDetailDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int EctsCredits { get; init; }
        public string Area { get; init; } = string.Empty;
        public TimeSpan TotalDuration { get; init; }
        public List<SessionListDto> Sessions { get; init; } = new();
    }
}
