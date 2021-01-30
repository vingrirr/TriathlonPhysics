using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriathlonPhysics
{
    public class AthleteZone
    {
        public AthleteZone()
        {

        }
        public AthleteZone(int bottom, int top)
        {
            Bottom = bottom;
            Top = top;       
        }

        public int Bottom { get; set; }
        public int Top { get; set; }
        public int Mid { get; set; }
        //public TrainingZone Zone { get; set; }

    }

    public class TrainingZone
    {
        public TrainingZone()
        {

        }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}

