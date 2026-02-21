using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_65_Lapko
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
