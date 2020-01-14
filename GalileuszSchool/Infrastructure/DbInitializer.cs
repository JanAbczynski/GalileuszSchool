using GalileuszSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalileuszSchool.Infrastructure
{
    public class DbInitializer
    {
        public static void Initialize(GalileuszSchoolContext context)
        {
            context.Database.EnsureCreated();
            if (context.LessonPlan.Any())
            {
                return;
            }

            LessonPlan[] lessonPlan = new LessonPlan[]
            {
                new LessonPlan{time = 10, monday = 1, tuesday = 2, wednesday = 3, thursday = 4, friday = 5, saturday = 6, sunday = 7}
            };


            context.LessonPlan.AddRange(lessonPlan);
            context.SaveChanges();

        }
    }
}
