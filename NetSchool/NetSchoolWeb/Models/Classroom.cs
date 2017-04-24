using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetSchoolWeb.Models
{
    public class Classroom
    {
        public int ClassroomID { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public string Corridor { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}