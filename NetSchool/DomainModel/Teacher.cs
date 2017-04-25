using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomainModel
{
    public class Teacher : Person
    {
        public string Discipline { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Hiring Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime HiringDate { get; set; }

        public virtual Classroom Classroom { get; set; }

    }
}