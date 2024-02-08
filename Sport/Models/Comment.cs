using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public Coach coach { get; set; }
        public string Comments { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
