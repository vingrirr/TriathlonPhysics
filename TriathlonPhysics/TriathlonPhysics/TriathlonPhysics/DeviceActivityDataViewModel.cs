using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Devices;
using System.Web.Script.Serialization;
//using Prologas;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Net.Mime;


namespace MvcApplication1.Common.GraphObjects
{
    public class DeviceActivityDataViewModel : BaseViewModel
	{
		public DeviceActivityDataViewModel()
		{
            AsWorkoutLogItem = false;
			Data = new List<List<object[]>>();
            Model = new Activity();
            FileUploadInformation = new FileUploadModel(); 
            
		}

        public DeviceActivityDataViewModel(int forAssignedID)
        {
            AsWorkoutLogItem = true;
            ForAssignedWorkoutID = forAssignedID;
			Data = new List<List<object[]>>();
            Model = new Activity();
            FileUploadInformation = new FileUploadModel(); 
        }
		
		
		public DeviceActivityDataViewModel(Devices.Activity activity, string path, string fileName, int fileSize, DateTime uploadedOn)
		{
			FileUploadInformation = new FileUploadModel(); 
			Model = activity;
			LoadActivity(activity);
			FileUploadInformation.FilePath = path;
			FileUploadInformation.FileName = fileName;
			FileUploadInformation.FileExtention = activity.Format;
			Discipline = activity.Discipline;
			FileUploadInformation.FileSize = fileSize;
			FileUploadInformation.UploadOn = uploadedOn;
			Data = new List<List<object[]>>();
			ToLineGraph();
		}
		public DeviceActivityDataViewModel(FileUploadModel fileInfo, Devices.Activity activity)
		{
			Model = activity;
			LoadActivity(activity);
			FileUploadInformation = fileInfo;
			Data = new List<List<object[]>>();
			ToLineGraph();
		}
        public DeviceActivityDataViewModel(FileUploadModel fileInfo)
        {
            FileUploadInformation = fileInfo;
            List<Activity> activities = DataParser.Parse(fileInfo.FilePath);
            Data = new List<List<object[]>>();

            if (activities != null && activities.Count() > 0)
            {
                Model = activities[0];
                LoadActivity(Model);
                ToLineGraph();
            }
            else
            {
                Model = new Activity(); 
            }

            
        }		

        #region Properties
        public Activity Model { get; set; }
		public int ID { get; set; }
		
		public FileUploadModel FileUploadInformation { get; set; }


		public int ForClientID { get; set; }
        
		

        public bool AsWorkoutLogItem { get; set; }
        public int ForAssignedWorkoutID { get; set; }

		public List<List<object[]>> Data = new List<List<object[]>>();

		//public List<object[]> HeartRate30sMovingAverage { get; set; }
		//public List<object[]> Wattage30sMovingAverage { get; set; }
		//public List<object[]> Speed30sMovingAverage { get; set; }
		public List<object[]> HeartRateData { get; set; }
		public List<object[]> WattageData { get; set; }
		public List<object[]> SpeedData { get; set; }
		public List<object[]> DistanceData { get; set; }
		public List<object[]> CadenceData { get; set; }
       
		public string Discipline { get; set; }
		public double Duration { get; set; }
		public DateTime Date { get; set; }
		public double Distance {get; set;}

        public string AverageHR { get; set; }
        public string MaxHR { get; set; }
        public string AveragePace { get; set; }
        public string MaxPace { get; set; }
        public string AverageWatts { get; set; }
        public string MaxWatts { get; set; }
        public string AverageCadence { get; set; }
        public string MaxCadence { get; set; }



        //public int? AveHR { get; set; }
        //public int? MaxHR { get; set; }
        //public int? AveWatts { get; set; }
        //public int? MaxWatts { get; set; }
        //public double? AvePace { get; set; }
        //public double? MaxPace { get; set; }
        //public int? AverageCadence { get; set; }
        //public int? MaxCadence { get; set; }

        public int? Temperature_Start { get; set; }
        public int? Temperature_End { get; set; }

		//todo: calculate all this!!
		public int? TRIMP_Score { get; set; }
        public int? TSS_Score { get; set; }
        public double? DecopulePercent_PaceToHR { get; set; }
        public double? DecopulePercent_PowerToHR { get; set; }
        public double? VariabilityIndex { get; set; }
        
        public int? Kilojoules { get; set; }
        public string KilojoulesString { get; set; }

        public double? IntensityFactor { get; set; }
        public int? TotalElevationChange { get; set; }
        public double? AverageGrade { get; set; }
        public int? StartingAltitude { get; set; }
        public int? ChangeInAltitude { get; set; }
        public int? PeakOneMinPower { get; set; }
        public int? PeakFiveMinPower { get; set; }
        public int? PeakTwentyMinPower { get; set; }

        #endregion


        private void LoadActivity(Devices.Activity activity)
        {
            Date = activity.startTime;
            Discipline = activity.Discipline;
            Duration = activity.workInfo.TotalTime.HasValue ? activity.workInfo.TotalTime.Value : 0;

            //storing everything in meters!!??
            Distance = activity.workInfo.Distance.HasValue ? activity.workInfo.Distance.Value : 0;
            //AveHR = activity.workInfo.AvgHR;
            //MaxHR = activity.workInfo.MaxHR;
            //AveWatts = (int?)activity.workInfo.AvgWatts;
            //MaxWatts = (int?)activity.workInfo.MaxWatts;
            //AvePace = (double?)activity.workInfo.AvgSpeed;
            //MaxPace = (double?)activity.workInfo.MaxSpeed;

            Temperature_Start = (int?)activity.workInfo.MinTemp;
            Temperature_End = (int?)activity.workInfo.MaxTemp;
            //AverageCadence = (int?)activity.workInfo.AvgCadence;
            //MaxCadence = (int?)activity.workInfo.MaxCadence;



			if (activity.workInfo.AvgSpeed.HasValue)
				AveragePace = PrologasPace.ToPaceFromMetersPerSecond(activity.workInfo.AvgSpeed.Value, activity.Discipline.ToDisciplineInt(), false); //what about pool?!
			if (activity.workInfo.MaxSpeed.HasValue)
				MaxPace = PrologasPace.ToPaceFromMetersPerSecond(activity.workInfo.MaxSpeed.Value, activity.Discipline.ToDisciplineInt(), false); //what about pool?!


            AverageHR = activity.workInfo.AvgHR.HasValue ? activity.workInfo.AvgHR.Value.ToString() : "X";
            MaxHR = activity.workInfo.MaxHR.HasValue ? activity.workInfo.MaxHR.Value.ToString() : "X";
            AverageWatts = activity.workInfo.AvgWatts.HasValue ? activity.workInfo.AvgWatts.Value.ToString() : "X";
            MaxWatts = activity.workInfo.MaxWatts.HasValue ? activity.workInfo.MaxWatts.Value.ToString() : "X";
            AverageCadence = activity.workInfo.AvgCadence.HasValue ? activity.workInfo.AvgCadence.Value.ToString() : "X";
            MaxCadence = activity.workInfo.MaxCadence.HasValue ? activity.workInfo.MaxCadence.Value.ToString() : "X";

            //ok so the WeekList stuff ect needs to 
            //use the model and not viewmodel b/c these values need to be strings and not ints.
            Kilojoules = (int?)activity.workInfo.Calories;
            KilojoulesString = activity.workInfo.Calories.HasValue ? activity.workInfo.Calories.Value.ToString() : "X";


            //todo: calculate all this!!

            //TRIMP_Score = item.TRIMP_Score;
            //TSS_Score = item.TSS_Score;
            //DecopulePercent_PaceToHR = (double?)item.DecouplePercent_PaceToHR;
            //DecopulePercent_PowerToHR = (double?)item.DecouplePercent_PowerToHR;
            //VariabilityIndex = (double?)item.VariabilityIndex;
            //IntensityFactor = (double?)item.IntensityFactor;
            //PeakOneMinPower = item.PeakOneMinPower;
            //PeakFiveMinPower = item.PeakFiveMinPower;
            //PeakTwentyMinPower = item.PeakTwentyMinPower;
            //HasFemaleCycleStarted = item.FemaleCycleStarted;
            //NewPRWasSet = item.NewPRWasSet;
            //StartingAltitude = activity.
            //ChangeInAltitude = item.ChangeInAltitude;

            //Kilojoules = (int?)activity.workInfo.Calories;
            TotalElevationChange = (int?)activity.workInfo.elevationChange;
            AverageGrade = (double?)activity.workInfo.avgGrade;
        }

        public void CalculateValues(Athlete athlete)
        {
            //todo: athlete needs to have zones set in order to do these calcs.  And an FTP etc...
        }
        public void ToLineGraph()
		{
			string temp = String.Empty;
			//var serializer = new JavaScriptSerializer();
			DateTime offsetTime = Model.startTime;
			TimeSpan ts;

			List<object[]> hrData = new List<object[]>();
			List<object[]> cadenceData = new List<object[]>();
			List<object[]> speedData = new List<object[]>();
			List<object[]> wattsData = new List<object[]>();
			List<object[]> distanceData = new List<object[]>(); //todo: make graph distance based option?
			//List<object[]> altitudeData = new List<object[]>();
			//todo: get Lat & longitude data...
			List<List<object[]>> alllist = new List<List<object[]>>();
			try
			{

				double prevTS = -10.0;
				int count = 0;
				// foreach (GMan.Lap l in blah.Laps)
				//foreach ( t in l.Tracks)
				foreach (Devices.TrackPoint p in Model.Tracks)
				{

					ts = p.TimeStamp - offsetTime;

					object[] hr = new object[2];
					hr[0] = ts.TotalSeconds;
					hr[1] = p.HR.HasValue ? p.HR.Value : 0;

					object[] cad = new object[2];
					cad[0] = ts.TotalSeconds;
					cad[1] = p.Cadence.HasValue ? p.Cadence.Value : 0;

					//todo: this is in m/s ...convert to MPH or based on user selection...
					object[] speed = new object[2];
					object[] watts = new object[2];
					speed[0] = ts.TotalSeconds;
					watts[0] = ts.TotalSeconds;

                    if (p.Speed.HasValue)
                    {
                        if (Discipline.ToLower() == "bike")
                        {
                            VCoach.Speed s = new VCoach.Speed(Convert.ToDouble(p.Speed.Value));
                            speed[1] = s.MetersPerSecond.MetersPerSecondToMPH();
                        }
                        if (Discipline.ToLower() == "run")
                        {
                            VCoach.Speed s = new VCoach.Speed(Convert.ToDouble(p.Speed.Value));
                            speed[1] = s.MetersPerSecond.MetersPerSecondToMinutesPerMile();
                        }

                    }
                    else
                    {
                        speed[1] = 0;
                    }
                    watts[1] = p.Watts.HasValue ? p.Watts.Value : 0;
                    //}
                    //else
                    //{
                    //speed[1] = 0;
                    //watts[1] = 0;
                    //}

					object[] dist = new object[2];
					dist[0] = ts.TotalSeconds;
					dist[1] = p.Distance.HasValue ? p.Distance.Value : 0; //todo: convert to mph


					hrData.Add(hr);
					cadenceData.Add(cad);
					speedData.Add(speed);
					wattsData.Add(watts);
					distanceData.Add(dist);

				}

				alllist.Add(hrData);
				alllist.Add(cadenceData);
				alllist.Add(speedData);
				alllist.Add(wattsData);
				//alllist.Add(distanceData);
				Data = alllist; 
				//temp = serializer.Serialize(alllist.ToArray());
								
			}
			catch (Exception ex)
			{
				int u = 0;
			}

			//return temp;
		}

		//public int SaveToDatabase()
		//{

		//    //todo: are we saving the file as binary data or just save the file on the host?
		//    //how do we back up the files if on the host?
			
		//    //todo: need to make sure the filename is unique
		//    int id = 0;
		//    FileUpload f = new FileUpload();
		//    f.FileName = this.FileName;
		//    f.UploadDate = DateTime.Now;
		//    return id; 
		//}
		//public FileDataPoint GetBestPower(double secondsInterval)
		//{
		//    FileDataLists data = GetMovingAverages(secondsInterval);
			
		//}

		public FileDataLists GetMovingAverages(double secondsInterval)
		{

			FileDataLists data = new FileDataLists();
			DateTime offsetTime = Model.startTime;
			TimeSpan ts;
            
            //todo: use moving average logic from the class MovingAverage.cs
			#region moving average
			List<object[]> hrMovingAverage = new List<object[]>();
			List<object[]> wMovingAverage = new List<object[]>();
			List<object[]> sMovingAverage = new List<object[]>();

			List<double> hrPoints = new List<double>();
			List<double> wPoints = new List<double>();
			List<double> sPoints = new List<double>();
			

			DateTime startInterval = Model.Tracks[0].TimeStamp;
			DateTime endInterval = startInterval.AddSeconds(secondsInterval);
			try
			{
				foreach (Devices.TrackPoint p in Model.Tracks)
				{
					
					//todo: do i need to do this offset?
					ts = p.TimeStamp - offsetTime;
					bool next = true;
					while (next)
					{
						if (p.TimeStamp.CompareTo(startInterval) >= 0 && p.TimeStamp.CompareTo(endInterval) < 0)
						{
							if (p.Watts != null)
							{
								wPoints.Add((double)p.Watts);
							}

							if (p.HR != null)
							{
								hrPoints.Add((double)p.HR);
							}

							if (p.Speed != null && p.Speed.HasValue)
							{
								sPoints.Add((double)p.Speed.Value);
							}
							next = false;
						}

						if (p.TimeStamp.CompareTo(endInterval) >= 0)
						{
							if (hrPoints.Count() > 0)
							{
								object[] h = new object[2];
								h[0] = ts.TotalSeconds;
								h[1] = hrPoints.Average();

								hrMovingAverage.Add(h);
								hrPoints.RemoveAt(0);
							}

							if (wPoints.Count() > 0)
							{
								object[] w = new object[2];
								w[0] = ts.TotalSeconds;
								w[1] = wPoints.Average();
								
								wMovingAverage.Add(w);
								wPoints.RemoveAt(0);
							}

							if (sPoints.Count() > 0)
							{
								object[] s = new object[2];
								s[0] = ts.TotalSeconds;
								s[1] = sPoints.Average();

								sMovingAverage.Add(s);
								sPoints.RemoveAt(0);
							}


							startInterval = startInterval.AddSeconds(1.0);
							endInterval = startInterval.AddSeconds(secondsInterval);
						}
					}
				}
			}
			catch (Exception ex)
			{
			}

			//todo: drop anything with zeros?
			data.HeartRateData = hrMovingAverage;
			data.SpeedData = sMovingAverage;
			data.WattageData = wMovingAverage;

			return data; 

			#endregion
		}
	}

	public class FileDataPoint
	{
		public FileDataPoint()
		{
			Timestamp = DateTime.MinValue;
			Value = 0; 
		}

		public DateTime Timestamp { get; set; }
		public object Value { get; set; }
	}

	public class FileDataLists
	{
		public FileDataLists()
		{
			HeartRateData = new List<object[]>();
			SpeedData = new List<object[]>();
			WattageData = new List<object[]>();
		}

		public List<object[]> HeartRateData { get; set; }
		public List<object[]> SpeedData { get; set; }
		public List<object[]> WattageData { get; set; }
	}

	public class FileUploadModel
	{
		public FileUploadModel()
		{

		}

		public FileUploadModel(FileUpload file)
		{
			FileUploadID = file.FileUploadID; 
			FileName = file.FileName;
			FileExtention = file.FileExtension;
			HashedFileName = file.HashedFileName;
			FilePath = file.FilePath;
			UploadOn = file.UploadDate;
			FileSize = file.FileSize;
            ForClientID = file.ForClientID;
		}

		public FileUploadModel(HttpPostedFileBase file, string path, int forClientID)
		{
			FileUploadID = 0; 
			FileName = Path.GetFileNameWithoutExtension(file.FileName);
			FileExtention = Path.GetExtension(file.FileName);
			HashedFileName = GetHashFileName(FileName) + FileExtention;
			FilePath = Path.Combine(path, HashedFileName);
			UploadOn = DateTime.Now;
			FileSize = file.ContentLength;
            ForClientID = forClientID;
								
		}

		public int FileUploadID { get; set; }
		public string FileName { get; set; }

        private string _filePath { get; set; }
        public string FilePath
        {
            get
            {
                return _filePath.Trim();
            }
            set
            {
                _filePath = value;
            }
        }
		public string HashedFileName{ get; set; }
		public string FileExtention { get; set; }
		public long? FileSize { get; set; }
		public string DeviceType { get; set; }
        public int ForClientID { get; set; }
		public DateTime UploadOn { get; set; }

		private string GetHashFileName(string fileName)
		{
			string salted = fileName + "pepper";
			return MD5Core.GetHashString(salted); 
		}
	}
	
}