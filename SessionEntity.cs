using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_65_Lapko
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
