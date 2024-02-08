using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
   public  enum Experience
    {
        high,
        average,
        low

    }
    public class Students
    {
        public int Id { get; set; }
        
        public string AboutExpireance { get; set; }
        public Experience Experience { get; set; }
        public User User { get; set; }
        public Sports Sports { get; set; }
    }
}
