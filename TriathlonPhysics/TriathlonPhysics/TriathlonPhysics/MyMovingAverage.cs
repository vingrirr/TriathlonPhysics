using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MyMovingAverage
/// </summary>
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

public class TimeMovingAverage
{
    private Queue<double> items;
    private int WindowSize;

    private List<double> Averages;

    public TimeMovingAverage(int secondsWindow)
    {
        WindowSize = secondsWindow;
        items = new Queue<double>();
        Averages = new List<double>();

    }
  
    public double Add(DateTime time, object value)
    {
        if (items.Count >= WindowSize)
            items.Dequeue();

        
        items.Enqueue(Convert.ToDouble(value));
        
        Averages.Add(items.Average());
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
		
		Alpha = 0.3; //todo - create ctor with user defined alpha
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

		return Math.Round(PreviousEstimate.Value,1);
	}
	public double Alpha { get; set; }

	public List<double> GetAverages()
	{
		return Averages;
	}


}

/// <summary>
/// Bad Name, this should be smoothing or something, this is the one 
/// just grabs a chunk of size window, gets the average, and adds it to the list. 
/// </summary>
public class DataSampleAverage
{
	public List<double> Items;
	public DataSampleAverage()
	{
		Items = new List<double>(); 
	}
	public DataSampleAverage(List<double>items)
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