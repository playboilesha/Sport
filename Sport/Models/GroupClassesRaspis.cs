using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{

    public enum StatusGroup
    {

        Good,

        Filled,
        Сancel


    }
    public class GroupClassesRaspis
    {
        public int Id { get; set; }
        public Sports Sports { get; set; }
        public Coach Coach { get; set; }
        public Hall Hall { get; set; }
        public int ReservedPlace { get; set; }
       
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public StatusGroup StatusGroup { get; set; }
        public Experience Experience { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
