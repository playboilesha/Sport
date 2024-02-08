using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public enum Status1
    {

        Good,

        Resorved,
        Over

        
    }
    public class RaspisIndividual
    {
        public int Id { get; set; }
        public Sports Sports { get; set; }
        public Coach Coach { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Hall Hall { get; set; }
       public Status1 Status1 { get; set; }
    }
}
