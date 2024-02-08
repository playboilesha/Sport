using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sport.Models
{
    public enum Speciality
    {
        Skate,
        Fitness,
        Box


    }
    public class Coach
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string About { get; set; }
        public string Image { get; set; }
        public Speciality Speciality { get; set; }
        public User User { get; set; }

        // group
    }
}
