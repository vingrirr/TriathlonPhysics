using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MvcApplication1.Common;

namespace DeviceParseTesting
{
	public class GarminGPXParser
	{
		public double rise = 0;
		public double run = 0;
		public double totalDistance = 0;
		public double totalRise = 0;
		public double slope = 0;
		private double _minElevation = 0;

		public GarminGPXParser()
		{
			ElevationReadings = new List<double>();
			DistanceReadings = new List<double>();
			DistanceElevationReadings = new List<double[]>();
            DistanceSlopeReadings = new List<double[]>(); 
			Tuple<double, double, double> RiseRunSlope = Tuple.Create<double, double, double>(0, 0, 0); 
		}
		public List<double> ElevationReadings { get; set; }
		public List<double> DistanceReadings { get; set; }
		public List<double[]> DistanceElevationReadings { get; set; }
        public List<double[]> DistanceSlopeReadings { get; set; }
		public void Parse(string filePath)
		{
					//                  select piece).ToList();

				XmlDocument doc = new XmlDocument();
				//doc.Load("c:\\Temp\\IMLou.gpx");
				doc.Load(filePath);
				XmlNodeList l = doc.SelectNodes("//gpx/trk");
				XmlNode el = doc.ChildNodes[1];
				XmlNode trk = el.ChildNodes[3];
				XmlNode trksList = trk.ChildNodes[1];


				int mycount = 0;
				double latEven = 0;
				double latOdd = 0;
				double longEven = 0;
				double longOdd = 0;
				double elevEven = 0;
				double elevOdd = 0;


				DistanceElevationDataPoint point = new DistanceElevationDataPoint(); 
				foreach (XmlNode n in trksList.ChildNodes)
				{

					double[] distelev = new double[2]; 
                    double[] distslope = new double[2]; 
					if (mycount == 0)  //starting   1st loop iteration
					{
						latEven = Convert.ToDouble(n.Attributes["lat"].Value);
						longEven = Convert.ToDouble(n.Attributes["lon"].Value);
						elevEven = Convert.ToDouble(n.FirstChild.InnerXml);
						ElevationReadings.Add(elevEven);
						DistanceReadings.Add(0);
						distelev[0] = 0;
						distelev[1] = elevEven * 3.28;						

						DistanceElevationReadings.Add(distelev);
                      //  DistanceSlopeReadings.Add(distslope);
					}
					else if (mycount % 2 == 0) //even 3rd loop iteration
					{
						latEven = Convert.ToDouble(n.Attributes["lat"].Value);
						longEven = Convert.ToDouble(n.Attributes["lon"].Value);
						elevEven = Convert.ToDouble(n.FirstChild.InnerXml);


						point = Calc(latOdd, latEven, longOdd, longEven, elevOdd, elevEven);

						if (!Double.IsNaN(point.Distance))
						{
							totalRise += Math.Round(point.Elevation, 2);
							totalDistance += Math.Round(point.Distance, 2);

							DistanceReadings.Add(totalDistance);
							ElevationReadings.Add(elevEven * 3.28); //convert to feet

							distelev[0] = Math.Round(totalDistance, 2);
							distelev[1] = elevEven * 3.28;

                            distslope[0] = Math.Round(totalDistance, 2);
                            distslope[1] = point.Slope;
							DistanceElevationReadings.Add(distelev);
                            DistanceSlopeReadings.Add(distslope);
						}
					}
					else  //odd  2nd loop iteration
					{
						latOdd = Convert.ToDouble(n.Attributes["lat"].Value);
						longOdd = Convert.ToDouble(n.Attributes["lon"].Value);
						elevOdd = Convert.ToDouble(n.FirstChild.InnerXml);

						point = Calc(latEven, latOdd, longEven, longOdd, elevEven, elevOdd);

						if (!Double.IsNaN(point.Distance))
						{
							totalRise += Math.Round(point.Elevation, 2);
							totalDistance += Math.Round(point.Distance, 2);

							DistanceReadings.Add(totalDistance);
							ElevationReadings.Add(elevOdd * 3.28); //convert to feet

							distelev[0] = Math.Round(totalDistance, 2);
							distelev[1] = elevOdd * 3.28;

                            distslope[0] = Math.Round(totalDistance, 2);
                            distslope[1] = point.Slope;
							DistanceElevationReadings.Add(distelev);
                            DistanceSlopeReadings.Add(distslope);
						}
					}

					
					mycount++;



				}

				_minElevation = ElevationReadings.Min();
				//DistanceElevationReadings = DistanceElevationReadings.Select(x => { x[1] = x[1] - _minElevation; return x; }).ToList();

               
                

		}

		public void FindClimbs()
		{
			double current = 0;
			double previous = 0;
			double difference = 0;

			double distanceCovered = 0; 

			bool areClimbing = false;
			bool areDescending = false; 

			double TotalAscend = 0;
			double TotalDescend = 0;


			double currentSegmentDistance = 0;
            double currentElevationChange = 0;
            double changeInGrade = 0;


			previous = ElevationReadings[1]; 
			current = ElevationReadings[0];
 		
			for (int x = 2; x < ElevationReadings.Count; x++ )
			{
				current = ElevationReadings[x];
				previous = ElevationReadings[x - 1];

				difference = current - previous;

                double iterationRun = FindHorizontalDistance(currentSegmentDistance, currentElevationChange);
                
                double slope = difference / iterationRun;
                
                if (difference != 0)
				{
					if (difference > 0)
					{
						TotalAscend += difference;

						if (!areClimbing)
						{
							areClimbing = true;
							areDescending = false;

                            double run = FindHorizontalDistance(currentSegmentDistance, currentElevationChange);
                            changeInGrade = (currentElevationChange / run) * 100;

                            if (changeInGrade > 1)
                            {
                                int dd = 0;
                            }
						}
						else
						{
							currentSegmentDistance += DistanceReadings[x] * 5280;
                            currentElevationChange += difference;
						}
					}
					else
					{
						TotalDescend += (difference * -1);

						if (!areDescending)
						{
							areClimbing = false;
							areDescending = true;

                            double run = FindHorizontalDistance(currentSegmentDistance, currentElevationChange);
                            changeInGrade = (currentElevationChange / run) * 100;
                            
                            if (changeInGrade > 1)
                            {
                                int cc = 0;
                            }
						}
						else
						{
							currentSegmentDistance += DistanceReadings[x] * 5280;
                            currentElevationChange += difference;
						}
					}
				}
			}

			
		}

        public double FindHorizontalDistance(double distance, double elevation)
        {
            //a^2 + b^2 + c^2

            double aSqr = Math.Pow(distance, 2) - Math.Pow(elevation, 2);
            return Math.Sqrt(aSqr);
        }

		public DistanceElevationDataPoint Calc(double lat1, double lat2, double long1, double long2, double elev1, double elev2)
		{
			DistanceElevationDataPoint point = new DistanceElevationDataPoint(); 

            //http://theclimbingcyclist.com/gradients-and-cycling-an-introduction/
            double run = 0;
			
			Tuple<double, double, double> retVals;
			double dist = LatLongDistance(lat1, long1, lat2, long2, 'M'); //in Miles
			rise = elev2 - elev1;// if we calc grade after all this, does this make sense to do this here?

			point.Distance = dist;
			point.Elevation = rise * 3.28;  //convert to feet
			
			//LatLongDistance(lat1, long1, lat2, long2, 'K'); //in Km



			//TODO: DO THE GRADE CALCULATION AFTER IT'S PARSED
			//Find points where we change from gainingElev to losing it and then calc Grade for those changes...
            if (!Double.IsNaN(dist) && dist != 0)  //sometime have repeating trackpoints?
            {
                double distFeet = dist * 5280;
                run = FindHorizontalDistance(distFeet, (elev2 - elev1));




                if (run != 0)
                    slope = (rise / run) * 100;
                else
                    slope = 0;

                point.Slope = slope;
            }

            

			return point;
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//:::                                                                         :::
		//:::  This routine calculates the distance between two points (given the     :::
		//:::  latitude/longitude of those points). It is being used to calculate     :::
		//:::  the distance between two locations using GeoDataSource(TM) products    :::
		//:::                                                                         :::
		//:::  Definitions:                                                           :::
		//:::    South latitudes are negative, east longitudes are positive           :::
		//:::                                                                         :::
		//:::  Passed to function:                                                    :::
		//:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
		//:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
		//:::    unit = the unit you desire for results                               :::
		//:::           where: 'M' is statute miles                                   :::
		//:::                  'K' is kilometers (default)                            :::
		//:::                  'N' is nautical miles                                  :::
		//:::                                                                         :::
		//:::  Worldwide cities and other features databases with latitude longitude  :::
		//:::  are available at http://www.geodatasource.com                          :::
		//:::                                                                         :::
		//:::  For enquiries, please contact sales@geodatasource.com                  :::
		//:::                                                                         :::
		//:::  Official Web site: http://www.geodatasource.com                        :::
		//:::                                                                         :::
		//:::           GeoDataSource.com (C) All Rights Reserved 2014                :::
		//:::                                                                         :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

		public double LatLongDistance(double lat1, double lon1, double lat2, double lon2, char unit)
		{
			double theta = lon1 - lon2;
			double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
			dist = Math.Acos(dist);
			dist = rad2deg(dist);
			dist = dist * 60 * 1.1515;
			if (unit == 'K')
			{
				dist = dist * 1.609344;
			}
			else if (unit == 'N')
			{
				dist = dist * 0.8684;
			}
			return (dist);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts decimal degrees to radians             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		public double deg2rad(double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts radians to decimal degrees             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		public double rad2deg(double rad)
		{
			return (rad / Math.PI * 180.0);
		}
	}

	public class DistanceElevationDataPoint
	{
		public DistanceElevationDataPoint()
		{

		}
		public DistanceElevationDataPoint(double distance, double elevation)
		{

		}
		public double Distance {get; set;}
		public double Elevation {get; set;}
        public double Slope { get; set; }

	}
	
}
