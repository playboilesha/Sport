
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public enum Status
    {
        
        Good,
        
        Wait,

        Cancel
    }
    public class ClassesIndividual
    {
        public int Id { get; set; }
        public Students Students { get; set; }
        public RaspisIndividual RaspisIndividual { get; set; }
        public Status Status { get; set; }
    }
}
