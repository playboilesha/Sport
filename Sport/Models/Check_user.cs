using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Models
{
    public class Check_user
    {
        public enum Check1
        {
            None,
            Go,

            NoGo

           
        }
        public int Id { get; set; }
        public Students Students { get; set; }
        public ClassesGroup ClassesGroup { get; set;}
        public Check1 check { get; set; }
    }
}
