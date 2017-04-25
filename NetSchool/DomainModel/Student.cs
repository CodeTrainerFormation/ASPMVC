using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomainModel
{
    public class Student : Person
    {
        public double Average { get; set; }

        public bool IsClassDelegate { get; set; }

        public virtual Classroom Classroom { get; set; }
    }
}