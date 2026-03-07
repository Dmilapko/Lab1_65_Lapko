namespace Lab1_65_Lapko.Core
{
    public class SubjectEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea Area { get; set; }

        public SubjectEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
