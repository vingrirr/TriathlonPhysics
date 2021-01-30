using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouTri.Devices;

namespace DeviceParseTesting
{
    public static class Decouple
    {
        public static DecoupleResult Calculate(Activity activities)
        {
            DecoupleResult r = new DecoupleResult(); 

            int halfWaySeconds = Convert.ToInt32(Math.Floor(Convert.ToDouble(activities.workInfo.TotalTime.Value / 2)));
            DateTime halfWayTime = activities.startTime.AddSeconds(halfWaySeconds);

            double aveHR1stHalf = 0;
            double aveHR2ndHalf = 0;
            double HR1stHalf = 0;
            double HR2ndHalf = 0;


            float speed1stHalf = 0;
            float speed2ndHalf = 0;
            float aveSpeed1stHalf = 0;
            float aveSpeed2ndHalf = 0;

            int HRreadingCount1sthalf = 0;
            int HRreadingCount2ndhalf = 0;

            int i = 0;


            foreach (TrackPoint t in activities.Tracks)
            {
                if (t.HR.HasValue && t.Speed.HasValue)
                {
                    if (t.TimeStamp.TimeOfDay <= halfWayTime.TimeOfDay)
                    {
                        HR1stHalf += t.HR.Value;
                        HRreadingCount1sthalf += 1;
                        speed1stHalf += t.Speed.Value;
                    }
                    else
                    {
                        HR2ndHalf += t.HR.Value;
                        HRreadingCount2ndhalf += 1;
                        speed2ndHalf += t.Speed.Value;
                    }
                }
                i++;
            }

            aveSpeed1stHalf = speed1stHalf / HRreadingCount1sthalf;
            aveHR1stHalf = HR1stHalf / HRreadingCount1sthalf;

            aveSpeed2ndHalf = speed2ndHalf / HRreadingCount2ndhalf;
            aveHR2ndHalf = HR2ndHalf / HRreadingCount2ndhalf;


            double firstHalf = (aveSpeed1stHalf / aveHR1stHalf);
            double secondHalf = (aveSpeed2ndHalf / aveHR2ndHalf);

            r.RPrime = firstHalf - secondHalf;

            r.DecoupleRate = Math.Round((r.RPrime / firstHalf) * 100);

            return r; 

        }


    }

    public class DecoupleResult
    {
        public DecoupleResult()
        {
                
        } 
        public double DecoupleRate { get; set; }
        public double RPrime { get; set; }
    }
}
