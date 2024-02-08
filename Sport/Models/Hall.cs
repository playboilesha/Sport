using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPeople { get; set; }
        public string Image { get; set; }
        public string About { get; set; }
        public Sports Sports { get; set; }
        public Klub Klub { get; set; }
    }
}
