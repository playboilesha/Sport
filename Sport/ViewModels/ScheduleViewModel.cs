using Sport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.ViewModels
{
    public class ScheduleViewModel
    {
        public DateTime StartDate { get; set; }
        public List<List<GroupClassesRaspis>> Classes { get; set; }
    }
}
