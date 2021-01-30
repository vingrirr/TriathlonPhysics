using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Common
{
	public class FatigueCurveCalculator
	{
		
		/// <summary>
		/// Estimate Rigel's Fatigue curve = t = ax^b  where t is time; a is units (FTP if cycling/running) (b*2/3) is fatigue factor
		/// </summary>
		/// <param name="results">NP or AP or Pace as double...distance maybe?</param>
		/// <param name="timeInSeconds"></param>
		public FatigueCurveCalculator(double[] results, double[] timeInSeconds)
		{

			Grades = new Dictionary<double, string>();

			Grades.Add(-0.08, "Elite");
			Grades.Add(-0.11, "Top AGer Ironman");
			Grades.Add(-0.15, "MOP AGer Ironman");
			Grades.Add(-0.18, "BOP AGer Ironman");
			Grades.Add(-0.20, "Deconditioned");


			double[] xdata = new double[timeInSeconds.Length];
			double[] ydata = new double[results.Length];

			//make linear...
			for (int i =0; i< timeInSeconds.Length; i++)
				xdata[i] = Math.Log((timeInSeconds[i] * 60)); //conver to minutes
				
			for (int j =0; j< results.Length; j++)
				ydata[j] =Math.Log(results[j]); 


		
			LeastSquaresResult result = LeastSquaresBestFitLine(xdata, ydata);

			FTP = Math.Pow(Math.E, result.YIntercept);
			FatigueRate = result.Slope;
			
			//As a general rule of thumb, your % drop off each time the duration doubles will be ~2/3 of this index e.g. if that superscript # is -0.10, your fatigue rate is ~6.7% (0.10 x 2/3).
			FatigueIndex = 0.66 * result.Slope;

			for (int g =0; g<Grades.Count; g++)
			{
				if (Grades.Keys.ElementAt(g) >= FatigueRate) 
				{
					Grade = Grades.Values.ElementAt(g); 
					break; 
				}
			}
		}


		public double FTP { get; set; }		
		public double FatigueRate { get; set; }
		public double FatigueIndex {get; set;}
		public string Grade { get; set; }

		public Dictionary<double, string> Grades { get; set; }

		public static LeastSquaresResult LeastSquaresBestFitLine(double[] x, double[] y)
		{
			//Calculates equation of best-fit line using shortcuts
			int n = x.Length;
			double xMean = 0.0;
			double yMean = 0.0;
			double numeratorSum = 0.0;
			double denominatorSum = 0.0;
			double bestfitYintercept = 0.0;
			double bestfitSlope = 0.0;
			double sigma = 0.0;
			double sumOfResidualsSquared = 0.0;
			//Calculates the mean values for x and y arrays
			for (int i = 0; i < n; i++)
			{
				xMean += x[i] / n;
				yMean += y[i] / n;
			}
			//Calculates the numerator and denominator for best-fit slope
			for (int i = 0; i < n; i++)
			{
				numeratorSum += y[i] * (x[i] - xMean);
				denominatorSum += x[i] * (x[i] - xMean);
			}
			//Calculate the best-fit slope and y-intercept
			bestfitSlope = numeratorSum / denominatorSum;
			bestfitYintercept = yMean - xMean * bestfitSlope;
			//Calculate the best-fit standard deviation
			for (int i = 0; i < n; i++)
			{
				sumOfResidualsSquared +=
				(y[i] - bestfitYintercept - bestfitSlope * x[i]) *
				(y[i] - bestfitYintercept - bestfitSlope * x[i]);
			}
			sigma = Math.Sqrt(sumOfResidualsSquared / (n - 2));
			
			
			return new LeastSquaresResult(sigma, sumOfResidualsSquared, bestfitSlope, bestfitYintercept); 
			//return new double[] { bestfitYintercept, bestfitSlope, sigma };
		}
	}

	public class LeastSquaresResult
	{
		public LeastSquaresResult()
		{

		}
		public LeastSquaresResult(double sigma, double residual, double slope, double yintercept)
		{
			Sigma = sigma;
			ResidualsSum = residual;
			Slope = slope;
			YIntercept = yintercept;
		}
		public double Sigma { get; set; }
		public double ResidualsSum { get; set; }
		public double Slope { get; set; }
		public double YIntercept { get; set; }
	}

}