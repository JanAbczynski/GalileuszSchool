﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GalileuszSchool.Models
{
    public class StudentCourseConnection
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        //[ForeignKey("StudenId")]
        //public virtual Student Student { get; set; }

        //[ForeignKey("CourseId")]
        //public virtual Course Course { get; set; }


    }
}