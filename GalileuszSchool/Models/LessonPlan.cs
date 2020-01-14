using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalileuszSchool.Models
{
    public class LessonPlan
    {
        public int Id { get; set; }
        public int classroom { get; set; }
        public int time { get; set; }
        public int monday { get; set; }
        public int tuesday { get; set; }
        public int wednesday { get; set; }
        public int thursday { get; set; }
        public int friday { get; set; }
        public int saturday { get; set; }
        public int sunday { get; set; }
}
}
