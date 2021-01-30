using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouTri.Devices;

namespace TriathlonPhysics
{
    public class FileDataLists
    {
        public FileDataLists()
        {
            HeartRateData = new List<FileDataPoint>();
            SpeedData = new List<FileDataPoint>();
            WattageData = new List<FileDataPoint>();
            HRMovingAverage = new TimeMovingAverage(30);
            SpeedMovingAverage = new TimeMovingAverage(30);
            WattageMovingAverage = new TimeMovingAverage(30);
        }

        public List<FileDataPoint> HeartRateData { get; set; }
        public List<FileDataPoint> SpeedData { get; set; }
        public List<FileDataPoint> WattageData { get; set; }
        public TimeMovingAverage HRMovingAverage { get; set; }
        public TimeMovingAverage SpeedMovingAverage { get; set; }
        public TimeMovingAverage WattageMovingAverage { get; set; }
    }

    public class FileDataPoint
    {
        public FileDataPoint()
        {
            Timestamp = 0.0;
            Value = 0;
        }

        public FileDataPoint(string datatype, double timestamp, object value)
        {
            DataType = datatype;
            Timestamp = timestamp;
            Value = value;
        }
        public string DataType { get; set; }
        public double Timestamp { get; set; }
        public object Value { get; set; }
    }

    public class MyFileData
    {
        public MyFileData()
        {

        }
        public MyFileData(List<YouTri.Devices.Activity> act, bool takeMovingAverages = true)
        {
            WorkInfo = act[0].workInfo;
            Discipline = act[0].Discipline;
            Date = act[0].startTime.ToShortDateString();
            Averages = new FileDataLists();

            if (takeMovingAverages)
            {
                foreach (TrackPoint p in act[0].Tracks)
                {
                    Averages.HRMovingAverage.Add(p.TimeStamp, p.HR.Value);
                    //Averages.WattageMovingAverage.Add(p.TimeStamp, p.Watts.Value);
                    //Averages.SpeedMovingAverage.Add(p.TimeStamp, p.Speed.Value);
                }
            }
        }
        public YouTri.Devices.WorkInfo WorkInfo { get; set; }
        public string Discipline { get; set; }
        public string Date { get; set; }
        public FileDataLists Averages { get; set; }
    }
}
