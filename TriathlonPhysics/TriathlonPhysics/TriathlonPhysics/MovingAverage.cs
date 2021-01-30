using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Common
{
    public class SimpleMovingAverage
    {
        private Queue<double> items;
        private int WindowSize;

        private List<double> Averages;

        public SimpleMovingAverage(int windowSize)
        {
            WindowSize = windowSize;
            items = new Queue<double>();
            Averages = new List<double>();

        }
        public SimpleMovingAverage(List<double> values, int windowSize)
        {
            WindowSize = windowSize;
            items = new Queue<double>();
            Averages = new List<double>();

            foreach (double d in values)
                Averages.Add(this.Add(d));
        }

        public double Add(double value)
        {
            if (items.Count >= WindowSize)
                items.Dequeue();


            items.Enqueue(value);
            return items.Average();
        }

        public List<double> GetAverages()
        {
            return Averages;
        }


    }


    public class ExponentialMovingAverage
    {
        private Queue<double> items;

        private double? PreviousEstimate;
        private double PreviousValue;
        private double? PreviousActual;

        private List<double> Averages;

        public ExponentialMovingAverage()
        {
            PreviousEstimate = null;
            PreviousActual = null;

            Alpha = 0.3;
            items = new Queue<double>();
            Averages = new List<double>();

        }

        public double Add(double value)
        {
            if (!PreviousEstimate.HasValue && !PreviousActual.HasValue)
            {
                PreviousEstimate = value;
            }
            else if (PreviousEstimate.HasValue && !PreviousActual.HasValue)
            {
                PreviousActual = value;
            }
            else
            {
                double y = PreviousEstimate.Value + Alpha * (PreviousActual.Value - PreviousEstimate.Value);
                PreviousEstimate = y;
                PreviousActual = value;
            }

            return Math.Round(PreviousEstimate.Value, 1);
        }
        public double Alpha { get; set; }

        public List<double> GetAverages()
        {
            return Averages;
        }


    }


    public class DataSampleAverage
    {
        public List<double> Items;
        public DataSampleAverage()
        {
            Items = new List<double>();
        }
        public DataSampleAverage(List<double> items)
        {
            Items = items;

        }
        public void Add(double value)
        {
            Items.Add(value);
        }

        public List<double> GetSamples(int windowSize)
        {
            List<double> samples = new List<double>();
            double temp = Items.Count / windowSize;

            int finalSkip = Convert.ToInt32(windowSize * temp);
            int finalTake = Items.Count % windowSize;


            for (int i = 0; i < temp; i++)
            {
                var r = Items.Skip(i * windowSize);
                samples.Add(Math.Round(Items.Skip(i * windowSize).Take(windowSize).Average()));
            }

            if (finalTake != 0)
                samples.Add(Math.Round(Items.Skip(finalSkip).Take(finalTake).Average()));


            return samples;

        }



    }
}