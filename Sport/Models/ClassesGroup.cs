using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public enum Check
    {

        None,

        Go,

        NotGo
    }
    public class ClassesGroup
    {
        public int Id { get; set; }
        public Students Students { get; set; }
        public GroupClassesRaspis GroupClassesRaspis { get; set; }
        public Status Status { get; set; }
        public Check Check { get; set; }
    }
}
