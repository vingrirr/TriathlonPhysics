using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCoach;
using YouTri.Devices;

namespace TriathlonPhysics
{
    public static class Constants
    {

        public const double CONST_CHO_Metabolism = 5.05; //aerobic metabolism carbohydrates = 5.05 kcal/L of O2
        public const double CONST_KiloJoulesPerLiterO2Glycogen = 21.1;
        public const double CONST_KiloJoulesPerLiterO2Fat = 19.8;

        public const double CONST_KCalIntakeHRBike_Low = 240;
        public const double CONST_KCalIntakeHRBike_High = 280;

        public const double CONST_DriveTrainEfficency = 0.976;
        public const double CONST_BikeEfficiency_High = 0.21;
        public const double CONST_BikeEfficiency_Low = 0.24;

        public const double CONST_CoeffRR_Track = 0.001;
        public const double CONST_CoeffRR_Concrete = 0.002;

        public const double CONST_CoeffRR_Road = 0.004;
        public const double CONST_CoeffRR_RoughRoad = 0.008;

        public const double CONST_AirDensity_SeaLevel = 1.226;
        public const double CONST_AirDensity_1500m = 1.056;
        public const double CONST_AirDensity_3000m = 0.905;

        public const double CONST_FrontalArea = 0.5;
        public const double CONST_DragCoeff = 0.5;

        public const double CONST_WattToKCal = 0.01433; //per min
        public const double CONST_JouleToCalorie = 0.25;

        public const double CONST_IntensityFactorIronmanLow = 0.65;
        public const double CONST_IntensityFactorIronmanHigh = 0.71;

        public const double CONST_KCalGlycogenPoundMuscle_Trained = 20;
        public const double CONST_KCalGlycogenLiverBlood_Trained = 500;

        //check both of these...
        public const double CONST_KCalGlycogenPoundMuscle_UnTrained = 10;
        public const double CONST_KCalGlycogenLiverBlood_UnTrained = 250;

    }
    public static class Solvers
    {
        //public delegate double Function(double x);
        public delegate double Function(double x, double Fw, double Fr, double Fg, double avePower);

        public static double F(double x, double Fw, double Fr, double Fg, double avePower)
        {
            return (Fw * (x * x * x) + (Fr + Fg) * x) - avePower;
        }


        #region Solvers

        public static double Bisection(Function f, double a, double b, double epsilon, double Fw, double Fr, double Fg, double avePower)
        {
            double x1 = a; double x2 = b;
            double fb = f(b, Fw, Fr, Fg, avePower);
            while (Math.Abs(x2 - x1) > epsilon)
            {
                double midpt = 0.5 * (x1 + x2);
                if (fb * f(midpt, Fw, Fr, Fg, avePower) > 0)
                    x2 = midpt;
                else
                    x1 = midpt;
            }
            return x2 - (x2 - x1) * f(x2, Fw, Fr, Fg, avePower) / (f(x2, Fw, Fr, Fg, avePower) - f(x1, Fw, Fr, Fg, avePower));
        }

        public static double BisectionCriticalPowerInterval(Function f, double a, double b, double epsilon, double Fw, double Fr, double Fg, double avePower)
        {
            double x1 = a; double x2 = b;
            double fb = f(b, Fw, Fr, Fg, avePower);
            while (Math.Abs(x2 - x1) > epsilon)
            {
                double midpt = 0.5 * (x1 + x2);
                if (fb * f(midpt, Fw, Fr, Fg, avePower) > 0)
                    x2 = midpt;
                else
                    x1 = midpt;
            }
            return x2 - (x2 - x1) * f(x2, Fw, Fr, Fg, avePower) / (f(x2, Fw, Fr, Fg, avePower) - f(x1, Fw, Fr, Fg, avePower));
        }
        public static double[] LeastSquaresBestFitLine(double[] x, double[] y)
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
            return new double[] { bestfitYintercept, bestfitSlope, sigma };
        }

        #endregion
    }

    //public delegate double Function(double x, double Fw, double Fr, double Fg, double avePower);

    //static double CPInterval(double x, double CP, double AWC, double Fg, double avePower)
    //{
    //    return (Fw * (x * x * x) + (Fr + Fg) * x) - avePower;
    //}


    // static double F(double x)
    //{
    //  return x * x * x - 5.0 * x + 3.0;
    //}

    //static void Main(string[] args)
    //{

    // //   VCoach.Pace bpa = new VCoach.Pace(9, "mph");


    //   // VCoach.Time tt = new VCoach.Time("05:30:00");
    //   // double kj = KJoulesExpended(130, tt.Seconds);


    //   // double[] time = new double[3];
    //   // double[] power = new double[3];
    //   // time[0] = 60 * 3;
    //   // time[1] = 60 * 5;
    //   // time[2] = 60 * 20;

    //   // power[0] = 175;
    //   // power[1] = 167;
    //   // power[2] = 144;
    //   // var result = EstimateCriticalPowerAndAWC(time,power);

    //   //var res2 = EstimateCriticalPowerFromTwoWorkouts(20, 144, 3, 175);
    //   //// var res3 = EstimateCriticalPowerFromTwoWorkouts(20, 144, 6, 167);

    //   //// //112Mi = 180247meters

    //   // var newts = AvgEffectivePedalForce(500, 90, 175);  //this shit is wrong??

    //   // var newt = TorqueGivenPowerCadence(490, 90);

    //   // var res5 = TargetDurationForGivenDistance(180247, 110, 49, 6, 1.57, CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 0.00001);
    //   // VCoach.Pace p = new VCoach.Pace("9:00m/M");


    //    double ftp = 250;
    //    double TSSTarget = 300;  //for the bike

    //    double powerForIM = (ftp * 0.65); //call this normalized power

    //    //double test = RollingResistance(72, 3, CONST_CoeffRR_Road, 8.92);

    //    //measured in study was 0.209, this has 0.21 !!
    //    //double cda = FindFrontalArea(1.8, 75) * CONST_DragCoeff;

    //    Console.WriteLine("\n\n Testing Bisection Method\n");

    //    //double Fw = WindResistance(CONST_DragCoeff, CONST_AirDensity_SeaLevel, 0.5);
    //    //double Fr = RollingResistance(70, 5, CONST_CoeffRR_Road);
    //    //double Fg = GravityResistance(70, 5, 0.03); 


    //   // double x = Bisection(F, 20.0, 0, 0.0001, Fw, Fr, Fg, 250);



    //    //166lbs, 24lb bike,
    //    //double powz = PowerRequiredForSpeed(75.3,10.8, 0.00, 0.32, CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 13.07);            
    //    double myDistance = 180247;//112Mi = 180247meters
    //    double myPower = 262;
    //    double myGoalTime = 19800;

    //    //double mysp = SpeedGivenPower(50, 8, 0.00, 0.55, 0.5, 1.226, CONST_CoeffRR_Road, myPower);  // in m/s
    //    //double myps = PowerRequiredForSpeed(50,8, 0.00, 0.55, 0.5, 1.226, CONST_CoeffRR_Road, 11.09);

    //    ////FrontalArea(

    //    //double myPwr = PowerRequiredForTime(50, 8, 0.00, 0.55, 0.5, 1.226, CONST_CoeffRR_Road, 1452, 16093);
    //    //double myKJ = KJoulesExpended(myPwr, myGoalTime);

    //    DeviceParseTesting.GarminGPXParser par = new DeviceParseTesting.GarminGPXParser();
    //    par.Parse("C:\\Temp\\IMLou.gpx");
    //   // List<double[]> distevel = par.DistanceElevationReadings;

    //    par.FindClimbs();


    //    //KJoulesExpended();
    //    //28.7mph
    //    //double powz =  PowerRequiredForSpeed(97.5, 9, 0.00, FrontalArea(2.025, 97.5), CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 13.07);


    //    VCoach.Speed gabbySpeed = new VCoach.Speed(20, "M/h");
    //    //gabby going 20mph
    //    //double gabbyPower = PowerRequiredForSpeed(58, 9, 0.00, FrontalArea(1.57, 97.5), CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, gabbySpeed.MetersPerSecond);
    //    VCoach.Distance gabbyDist = new VCoach.Distance("26.2M");
    //    VCoach.Time gabbyTime = new VCoach.Time("04:19:00");
    //    VCoach.Pace gabbyPace = gabbyDist.PaceNeeded(gabbyTime);
    //   // gabbyPace.SetDisplayUnits("m/M");  //mins Per Mile
    //    //gabbyPace.SetDisplayUnits("Km/h"); //for run wattage
    //   //VCoach.Speed gabbyMarrySpeed = gabbyPace.GetSpeed();


    //    //180lbs to kg, 0.06215 = 6min/Mi to hr/Km
    //    //double runWatttest = RunWatts(81.81, 0.06215);
    //    //double runWatttest2 = RunWatts(68.03, 0.06215);

    //    double gabbyWeight = 130;
    //    double gabbyWeightKg = Math.Round(130 / 2.2);
    //    //double gPace = (1/((gabbyPace.value * 3600) / 1000));

    //    double gPace = (0.22 * 3600) / 1000;

    //    //double gabbyRunWatts = RunWatts(59.09, 0.09321);

    //    //double power3 = PowerRequiredForSpeed(97.5, 9, 0.00, FrontalArea(2.025, 97.5), CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 13.07);
    //    //double speed = SpeedGivenPower(58, 9, 0.00, FrontalArea(1.6, 58), CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 125);

    //    //Console.WriteLine(speed);
    //    Console.ReadLine();

    //    //20mph 165lbs, 23lb bike, 1% grade = 75kg, 10kg           
    //    //double power2 = PowerRequiredForSpeed(75, 10, 0.0, FrontalArea(1.80, 75), CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 8.9);

    //   // Console.WriteLine(power);
    //    //double kirkPower = ((5.244820) * 20) + ((0.019168) * 20 * 3);
    //    //Console.WriteLine("Solution from bisection method:" + x.ToString());
    //    //Console.WriteLine("Solution confirmation:f(x) =" + F(x).ToString());
    //    Console.ReadLine();

    //  //  double velocity = FindSpeedGivenPower(71, 4, 0.03, 0.5, CONST_DragCoeff, CONST_AirDensity_SeaLevel, CONST_CoeffRR_Road, 250);

    //}

    public static class TrainingZoneCalculations
    {
        public static List<AthleteZone> CogganFTHRZones(int functionalThresHR)
        {
            List<AthleteZone> zones = new List<AthleteZone>();
            int t = 0;
            int b = 0;

            t = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.68));

            AthleteZone z1 = new AthleteZone(0, b);

            b = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.69));
            t = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.83));
            AthleteZone z2 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.84));
            t = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.94));
            AthleteZone z3 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(functionalThresHR * 0.95));
            t = System.Convert.ToInt32(Math.Floor(functionalThresHR * 1.05));
            AthleteZone z4 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(functionalThresHR * 1.06));
            t = 0;
            AthleteZone z5 = new AthleteZone(t, b);



            zones.Add(z1);
            zones.Add(z2);
            zones.Add(z3);
            zones.Add(z4);
            zones.Add(z5);

            return zones;
        }

        public static List<AthleteZone> CogganPowerZones(int ftp)
        {
            List<AthleteZone> zones = new List<AthleteZone>();
            int t = 0;
            int b = 0;

            t = System.Convert.ToInt32(Math.Floor(ftp * 0.55));

            AthleteZone z1 = new AthleteZone(0, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 0.56));
            t = System.Convert.ToInt32(Math.Floor(ftp * 0.75));
            AthleteZone z2 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 0.76));
            t = System.Convert.ToInt32(Math.Floor(ftp * 0.90));
            AthleteZone z3 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 0.91));
            t = System.Convert.ToInt32(Math.Floor(ftp * 1.05));
            AthleteZone z4 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 1.06));
            t = System.Convert.ToInt32(Math.Floor(ftp * 1.20));
            AthleteZone z5 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 1.21));
            t = System.Convert.ToInt32(Math.Floor(ftp * 1.50));
            AthleteZone z6 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(ftp * 1.51));
            t = 0;
            AthleteZone z7 = new AthleteZone(t, b);


            zones.Add(z1);
            zones.Add(z2);
            zones.Add(z3);
            zones.Add(z4);
            zones.Add(z5);
            zones.Add(z6);
            zones.Add(z7);

            return zones;
        }

        public static List<AthleteZone> FiveZoneModel(int maxHR)
        {
            List<AthleteZone> zones = new List<AthleteZone>();
            int t = 0;
            int b = 0;
            t = System.Convert.ToInt32(Math.Floor(maxHR * 0.68));
            t = System.Convert.ToInt32(Math.Floor(maxHR * 0.73));

            AthleteZone z1 = new AthleteZone(0, b);

            b = System.Convert.ToInt32(Math.Floor(maxHR * 0.73));
            t = System.Convert.ToInt32(Math.Floor(maxHR * 0.80));
            AthleteZone z2 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(maxHR * 0.80));
            t = System.Convert.ToInt32(Math.Floor(maxHR * 0.87));
            AthleteZone z3 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(maxHR * 0.87));
            t = System.Convert.ToInt32(Math.Floor(maxHR * 0.93));
            AthleteZone z4 = new AthleteZone(t, b);

            b = System.Convert.ToInt32(Math.Floor(maxHR * 0.93));
            t = System.Convert.ToInt32(Math.Floor(maxHR * 1.00));
            AthleteZone z5 = new AthleteZone(t, b);

            zones.Add(z1);
            zones.Add(z2);
            zones.Add(z3);
            zones.Add(z4);
            zones.Add(z5);
            return zones;
        }

    }

    public static class IntervalCalculations
    {
        //todo: make this an enumeration
        public static AthleteZone WhenToStop(int targetPowerForThirdInterval, int intervalLengthMins)
        {
            int b = 0;
            int t = 0;
            AthleteZone z = new AthleteZone();

            switch(intervalLengthMins)
            {
                case 20:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.05));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.03));
                    z = new AthleteZone(t, b);
                    break;
                case 10:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.06));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.04));
                    z = new AthleteZone(t, b);
                    break;
                case 5:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.07));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.05));
                    z = new AthleteZone(t, b);
                    break;
                case 3:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.09));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.08));
                    z = new AthleteZone(t, b);
                    break;
                case 2:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.12));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.10));
                    z = new AthleteZone(t, b);
                    break;
                case 1:
                    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.12));
                    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.10));
                    z = new AthleteZone(t, b);
                    break;
                //case 0.5:
                //    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.05));
                //    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.03));
                //    z = new AthleteZone(t, b);
                //    break;
                //case 0.25:
                //    b = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.05));
                //    t = System.Convert.ToInt32(Math.Floor(targetPowerForThirdInterval * 0.03));
                //    z = new AthleteZone(t, b);
                //    break;
                default:
                    throw new Exception("Interval Duration not found.  Valid values are 20,10,5,3,2,1,0.5, 0.25");
                    break;
            }

            return z; 
        }
    }

    public static class TSSHelper
    {
        public static int[] _tss = { 20, 30, 40, 50, 60, 70, 80, 100, 120, 140 };
        //http://www.trainingbible.com/joesblog/2009/09/estimating-tss.html
        public static double EstimateTSSviaRPE(int rpe, int durationInSeconds)
        {
            if (rpe > 10 || rpe < 1)
                throw new System.ArgumentOutOfRangeException("RPE", "RPE must be 1-10");

            int rpeIndex = rpe - 1;   //rpe is 1-10; array is zero based
            double hours = (double)durationInSeconds / 60 / 60;

            return hours * _tss[rpeIndex];
        }

        public static double EstimateTSSviaHRZone(int zone, double durationInSeconds, string lowMidHigh)
        {
            double hours = (double)durationInSeconds / 60 / 60;
            double tss = 0;
            if (zone == 1)
            {
                switch (lowMidHigh)
                {
                    case "low":
                        tss = _tss[0] * hours;
                        break;
                    case "mid":
                        tss = _tss[1] * hours;
                        break;
                    case "high":
                        tss = _tss[2] * hours;
                        break;

                    default:
                        tss = _tss[2] * hours;
                        break;
                }
            }
            else if (zone == 2)
            {
                switch (lowMidHigh)
                {

                    case "low":
                        tss = _tss[3] * hours;
                        break;
                    case "mid":
                        tss = _tss[4] * hours;
                        break;
                    case "high":
                        tss = _tss[4] * hours;
                        break;
                    default:
                        tss = _tss[4] * hours;
                        break;
                }
            }
            else if (zone == 3)
            {
                tss = _tss[5] * hours;
            }
            else if (zone == 4)
            {
                tss = _tss[6] * hours;
            }
            else if (zone == 5)
            {
                tss = _tss[7] * hours;
            }
            else if (zone == 6)
            {
                tss = _tss[8] * hours;
            }
            else if (zone == 7)
            {
                tss = _tss[9] * hours;
            }

            return tss;
        }
    }

    public static class TrainingEstimates
    {

        /// <summary>
        /// Double bodyweight (in lbs), subtract 0.5% for every year older than 35, if female, subtract 10%
        /// </summary>
        /// <param name="weightKg"></param>
        /// <param name="age"></param>
        /// <param name="isMale"></param>
        /// <returns></returns>
        public static double FrielFTPEstimateBike(double weightKg, int age, bool isMale)
        {
            double val = 0.0;

            val = (weightKg * 2.2) * 2;

            if (age > 35)
            {
                double temp = ((age - 35) * 0.005) * val;
                val = val - temp;
            }

            if (!isMale)
            {
                double temp2 = val * 0.1;
                val = val - temp2;
            }
            return val;
        }


        public static double KJoulesExpended(double averagePower, double durationInSec)
        {
            return Math.Round(((averagePower * durationInSec) / 1000), 2);
        }


        public static double EstimateDurationInHours(int TSS, double intensityFactor)
        {

            //TSS = IF^2 * 100 * Duration via Couzens
            //duration = TSS / IF^2 * 100; 

            return (TSS / (Math.Pow(intensityFactor, 2) * 100));
        }

        public static double TargetDurationForGivenDistance(int distanceInMeters, int FTP, double riderWeightKG, double bikeWeightKg, double heightInMeters, double intensityFactor, double coeffDrag, double airDensity, double coeffRollingResis, double gradeOfHill)
        {

            //20mph = 8.9408m/s
            double NPower = Calculate.NormalizedPower(intensityFactor, FTP);
            double FA = Calculate.FrontalArea(heightInMeters, riderWeightKG);
            double velocity = Calculate.SpeedGivenPower(riderWeightKG, bikeWeightKg, gradeOfHill, FA, Constants.CONST_DragCoeff, Constants.CONST_AirDensity_SeaLevel, Constants.CONST_CoeffRR_Road, NPower);

            //use vcoach stuff
            return distanceInMeters / velocity;

        }

        public static double TargetDurationForGivenDistance(int distanceInMeters, int NPower, double riderWeightKG, double bikeWeightKg, double heightInMeters, double coeffDrag, double airDensity, double coeffRollingResis, double gradeOfHill)
        {

            //20mph = 8.9408m/s            
            double FA = Calculate.FrontalArea(heightInMeters, riderWeightKG);
            double velocity = Calculate.SpeedGivenPower(riderWeightKG, bikeWeightKg, gradeOfHill, FA, Constants.CONST_DragCoeff, Constants.CONST_AirDensity_SeaLevel, Constants.CONST_CoeffRR_Road, NPower);

            //use vcoach stuff
            return distanceInMeters / velocity;

        }

        public static double EstimateIronmanMarathonPace(double marathonPRPace)
        {
            return (marathonPRPace * 1.2);
        }
    }

    public static class Convert
    {
        public static double ConvertTSSToKCalGlyc(double KCalGlycStorage)
        {
            //100 TSS = ~100% of your personal glycogen stores.
            //If you have 1000kcal of glycogen at your disposal then each TSS is worth ~10kcal of glycogen.
            //If you have 1500kcal at your disposal then each TSS is worth ~15kcal of glycogen.
            return (KCalGlycStorage / 100);
        }

        public static double MetersPerSecondToMPH(double meterspersec)
        {
            Speed s = new Speed(meterspersec);
            SpeedUnits su = new SpeedUnits();
            return su.ConvertTo(meterspersec, "m/s", "M/hr");
        }

        public static double MetersPerSecondToMinPerMile(double meterspersec)
        {
            PaceUnits pacU = new PaceUnits();
            Pace p = new Pace(1.0 / meterspersec, "s/m");
            return pacU.ConvertTo(p.value, "s/m", "min/M");
        }
    }

    public static class Calculate
    {

        public static double MovingAverageBestPower(YouTri.Devices.Activity activity, int windowSize)
        {
            double best = 0; 
            TimeMovingAverage tma = new TimeMovingAverage(30);
            foreach (TrackPoint tp in activity.Tracks)
            {
                if (tp.Watts.HasValue)
                    tma.Add(tp.TimeStamp, tp.Watts);
            }

            //best = tma.
            return best; 
        }
        //find diameter of other 650rim and other common ones
        public static double GearInches700c(int chainring, int cogteeth, int tireSize)
        {
            //25.4mm = 1inch
            return ((chainring / cogteeth) * ((622 + (2 * tireSize) / 25.4)));
        }





        public static double EfficencyFactorBike(int normalizedPower, int averageHR)
        {
            //<= 5%  is ok
            return ((double)normalizedPower / (double)averageHR);
        }
        public static double EfficencyFactorRun(int normalizedPace, int averageHR)
        {
            //normalizedGradedPace technically...look into calc...
            //<= 5%  is ok
            return ((double)normalizedPace / (double)averageHR);
        }

        //public static double EfficencyFactor(int kiloJoules, int seconds)
        //{
        //    //<= 5%  is ok
        //    return ((double)normalizedPower / (double)averageHR);
        //}

        //todo: couzens says IF^3 for swim?  see his Fitness vs Fatigue Rocky article...
        public static double TSSCalc(double durationSec, int normalizedPower, double intensityFactor, int FTP)
        {
            return (durationSec * normalizedPower * intensityFactor) / (FTP / 3600) * 100;
        }

        public static double RunWatts(double weightKg, double pace)
        {
            //note!! pace is in Km/Hr!!  Use VCoach to Convert this to Km/Hr!  6min/Mi * 1Mi/1.609Km * 1hr/60 = 0.0621 
            double temp1 = ((pace / 0.0006944) / 1.6) * weightKg; //210/((0.0621/0.0006944)/1.6) * weightKg;
            double temp2 = 210 / temp1;
            double temp3 = temp2 / 1000;
            return temp2 * 75;
        }

        //public static double KCalories(int watts, double durationSeconds)
        //{
        //     this seems wrong...
        //    //where did i find this?  seconds or minutes?
        //    //return Math.Round(CONST_WattToKCal * watts * (durationSeconds * 60), 0);
        //    return Math.Round(CONST_WattToKCal * watts * (durationSeconds), 0);
        //}
        public static double NormalizedPower(double intensityFactor, int FTP)
        {
            return intensityFactor * FTP;
        }
        public static double IntensityFactor(double normalizedPower, double FTP)
        {
            return Math.Round((normalizedPower / FTP), 2);
        }
        public static double VariabilityIndex(double normalizedPower, double averagePower)
        {
            return Math.Round((normalizedPower / averagePower), 2);
        }

        public static double FrontalArea(double heightInMeters, double weightKg)
        {
            //area =((0.0276*( heigh (meters) ^0.725))*(weight (meters) ^0.425))+0.1647
            return ((0.0276 * (Math.Pow(heightInMeters, 0.725))) * (Math.Pow(weightKg, 0.425)) + 0.1647);
        }



        // if you assume we're effiecient at 18-25%, then Kj to Calories is just 1:1
        //public static double CaloriesToKilojoules(int calories)
        //{
        //    return Math.Round(calories * 0.004184);
        //}

        //public static double KilojoulesToCalories(int kj)
        //{
        //    return Math.Round(kj * 4.184);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="airDensity"></param>
        /// <param name="airVelocity"></param>
        /// <param name="riderVelocity"></param>
        /// <param name="coeffOfDrag"></param>
        /// <param name="frontalArea"></param>
        /// <param name="wheelRotatFactor"></param>
        /// <param name="massOfRider"></param>
        /// <param name="massOfBike"></param>
        /// <param name="momentOfInertiaWheel"></param>
        /// <param name="tireRadius"></param>
        /// <param name="finalVelocity"></param>
        /// <param name="initialVelocity"></param>
        /// <param name="finalTimeInSec"></param>
        /// <param name="initialTime"></param>
        /// <param name="coeffRollingResist"></param>
        /// <param name="roadGradient"></param>
        /// <param name="driveTrainEffiency"></param>
        /// <returns></returns>
        public static double TotalPower(double airDensity, double airVelocity, double riderVelocity, double coeffOfDrag, double frontalArea,
            double wheelRotatFactor, double massOfRider, double massOfBike, double momentOfInertiaWheel, double tireRadius, double finalVelocity,
            double initialVelocity, double finalTimeInSec, double initialTime, double coeffRollingResist, double roadGradient, double driveTrainEffiency)
        {

            //http://www.recumbents.com/wisil/MartinDocs/Validation%20of%20a%20mathematical%20model%20for%20road%20cycling.pdf

            double tMass = (massOfRider + massOfBike);
            double temp = (0.5 * airDensity * airVelocity * riderVelocity * (coeffOfDrag + wheelRotatFactor));

            double temp2 = 0.5 * (tMass + momentOfInertiaWheel / Math.Pow(tireRadius, 2));
            double temp2a = (Math.Pow(finalVelocity, 2) - Math.Pow(initialVelocity, 2));
            double temp2b = (finalTimeInSec - initialTime);

            double temp3 = riderVelocity * coeffRollingResist * tMass * 9.8 * Math.Cos(Math.Atan(roadGradient));

            double temp4 = riderVelocity * (0.091 + (0.0087 * riderVelocity));
            double temp5 = riderVelocity * tMass * 9.8 * Math.Sin(Math.Atan(roadGradient));


            double temp2c = (temp2 * temp2a / temp2b);

            double total = (temp + temp2c + temp3 + temp4 + temp5) / driveTrainEffiency;

            return total;
        }


        // todo: reverse this for FindPowerGivenSpeed...
        //http://www.analyticcycling.com/ForcesSpeed_Page.html


        public static double PowerRequiredForTime(double massOfRiderKg, double massOfBikeKg, double grade, double frontalArea, double coeffDrag, double airDensity, double coeffRollingResis, double timeInSeconds, double distanceInMeters)
        {

            double aveVelocity = distanceInMeters / timeInSeconds;
            return PowerRequiredForSpeed(massOfRiderKg, massOfBikeKg, grade, frontalArea, coeffDrag, airDensity, coeffRollingResis, aveVelocity);
        }
        public static double PowerRequiredForSpeed(double massOfRiderKg, double massOfBikeKg, double grade, double frontalArea, double coeffDrag, double airDensity, double coeffRollingResis, double velocity)
        {

            double Fw = WindResistance(coeffDrag, airDensity, frontalArea, velocity);
            double Fr = RollingResistance(massOfRiderKg, massOfBikeKg, coeffRollingResis, velocity);
            double Fg = GravityResistance(massOfRiderKg, massOfBikeKg, grade, velocity);

            return Fw + Fr + Fg;
        }


        public static double SpeedGivenPower(double massOfRiderKg, double massOfBikeKg, double grade, double frontalArea, double coeffDrag, double airDensity, double coeffRollingResis, double averagePower)
        {

            double Fw = WindResistance(coeffDrag, airDensity, frontalArea);
            double Fr = RollingResistance(massOfRiderKg, massOfBikeKg, coeffRollingResis);
            double Fg = GravityResistance(massOfRiderKg, massOfBikeKg, grade);



            double totalVelocity = Solvers.Bisection(Solvers.F, 20.0, 0, 0.0001, Fw, Fr, Fg, averagePower);

            return totalVelocity;
        }


        /// <summary>
        /// Mass * gravity * GradOfHill
        /// </summary>
        /// <param name="massOfRiderKg"></param>
        /// <param name="massOfBikeKg"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double GravityResistance(double massOfRiderKg, double massOfBikeKg, double grade, double velocity)
        {
            return (massOfBikeKg + massOfRiderKg) * 9.8 * grade * velocity;
        }

        public static double GravityResistance(double massOfRiderKg, double massOfBikeKg, double grade)
        {
            return (massOfBikeKg + massOfRiderKg) * 9.8 * grade;
        }

        /// <summary>
        /// Watts required to overcome this windresistance
        /// 1/2 frontalArea * DragCoeff * airDensity * velocity^3
        /// </summary>
        /// <param name="coeffDrag"></param>
        /// <param name="airDensity"></param>
        /// <param name="frontalArea"></param>
        /// <param name="velocity">in m/s</param>
        /// <returns></returns>
        public static double WindResistance(double coeffDrag, double airDensity, double frontalArea, double velocity)
        {

            //air density is also rho in physics & can be calc with barometric press and temp if we want to get nitty gritty
            return 0.5 * frontalArea * coeffDrag * airDensity * velocity * velocity * velocity;
        }

        public static double WindResistance(double coeffDrag, double airDensity, double frontalArea)
        {
            return 0.5 * frontalArea * coeffDrag * airDensity;
        }

        /// <summary>
        /// Watts required to overcome this rolling resistance
        /// </summary>
        /// <param name="massOfRiderKg"></param>
        /// <param name="massOfBikeKg"></param>
        /// <param name="coeffRollingResis"></param>
        /// <param name="velocity"></param>
        /// <returns></returns>
        public static double RollingResistance(double massOfRiderKg, double massOfBikeKg, double coeffRollingResis, double velocity)
        {
            return (massOfRiderKg + massOfBikeKg) * 9.8 * coeffRollingResis * velocity;
        }


        public static double RollingResistance(double massOfRiderKg, double massOfBikeKg, double coeffRollingResis)
        {
            return (massOfRiderKg + massOfBikeKg) * 9.8 * coeffRollingResis;
        }

    }

    #region Critical Power

    public static class CriticalPower
    {
        public static double EstimateTimeAboveCriticalPowerNonLinear3(int awc, int power, int criticalPower, int maxPower)
        {
            return ((awc / (power - criticalPower)) - (awc / (maxPower - criticalPower)));
        }

        public static double EstimateCriticalPowerFromTwoWorkouts(int LongTestMinutes, int LongTestAvgPower, int ShortTestMinutes, int ShortTestAvgPower)
        {

            int longSec = (LongTestMinutes * 60);
            int shortSec = (ShortTestMinutes * 60);
            int joulesLong = (LongTestAvgPower * longSec);  //avgPower * seconds
            int joulesShort = (ShortTestAvgPower * shortSec);  //avgPower * seconds

            //todo: need to throw exception if things are off...
            return ((joulesLong - joulesShort) / (longSec - shortSec));

        }

        public static double EstimateCriticalVelocityFromTwoWorkouts(int LongTestMinutes, int LongTestMeters, int ShortTestMinutes, int ShortTestMeters)
        {


            int metersDiff = LongTestMeters - ShortTestMeters;
            int secondsDiff = (LongTestMinutes * 60) - (ShortTestMinutes * 60);

            double criticalVelocity = (metersDiff / secondsDiff);  //this is in metersPerSec, use VCoach!!

            //todo: need to throw exception if things are off...
            return criticalVelocity;

        }

        public static EstimatedCriticalPowerResult EstimateCriticalPowerAndAWC(double[] durationDataInSeconds, double[] powerData)
        {
            //double[] powerdata = new double[] { 425, 375, 355 };
            //double[] hrdata = new double[] { 0.005, 0.002, 0.001 };

            double[] durationModified = new double[durationDataInSeconds.Length];

            for (int x = 0; x < durationDataInSeconds.Length; x++)
            {
                durationModified[x] = 1 / durationDataInSeconds[x];  //time must be in the form:  1/t  or 1/numSeconds
            }

            double[] results1 = Solvers.LeastSquaresBestFitLine(durationModified, powerData);  //gives slope and b in y=mx+ b; 

            double cp = results1[0]; // critical power
            double awc = results1[1];  //anaerobic work capacity in joules

            EstimatedCriticalPowerResult res = new EstimatedCriticalPowerResult(cp, awc);

            return res;

        }


        //public static CriticalPowerPowerInterval TargetPowerCriticalPowerInterval(int intervalDuration, int recoveryDuration, double AWC, int CP)
        //{
        //    CriticalPowerPowerInterval iv = new CriticalPowerPowerInterval(); 


        //}


    }


    #endregion


    #region Monotony and overtraining

    public static class Monotony
    {
        public static double MonotonyScore(List<double> trainingLoad)
        {

            return 0.0;
            //Tuple<double, double> avgStd = MathNet.Numerics.Statistics.ArrayStatistics.MeanStandardDeviation(trainingLoad.ToArray());

            //if ((avgStd.Item1 / avgStd.Item2) >= 10)
            //    return 10; //capped at 10 based on rules below..?
            //else
            //    return (avgStd.Item1 / avgStd.Item2);

            //http://fellrnr.com/wiki/Training_Monotony
            //   I cap Monotony to a maximum value of 10. Without this cap, the value tends to be unreasonably sensitive to 
            //  high levels of monotony. Values of Monotony over 2.0 are generally considered too high, 
            //  and values below 1.5 are preferable. A high value for Monotony indicates that the training program is ineffective. 
            // This could be because the athlete is doing a low level of training; an extreme example would be a well-trained runner 
            // doing a single easy mile every day. This would allow for complete recovery, but would not provide the stimulus for improvement 
            // and would likely lead to rapid detraining. At the other extreme, doing a hard work out every day would be monotonous and not 
            //allow sufficient time to recover.           
        }
    }

    public static class Strain
    {
        public static double StrainScore(List<double> trainingLoad)
        {
            double strain = 0;

            double monotonyScore = Monotony.MonotonyScore(trainingLoad);
            //Training Strain = sum(TRIMP) * Monotony
            strain = trainingLoad.Sum() * monotonyScore;
            return strain;
            //The value of Training Strain that leads to actual Overtraining Syndrome would be specific to each athlete. 
            //An elite level athlete will be able to train up much higher levels than a beginner. 
            //However this Training Strain provides a better metric of the overall stress that an athlete is undergoing than 
            //simply looking at training volume.
        }
    }
    #endregion



    public static class Couzen
    {

        public static double CouzenFindSteadyHR(int maxHR, int restingHR)
        {
            double steady = 0.65;
            return ((maxHR - restingHR) * steady) + restingHR;
        }
        public static double CouzenFindModerateHR(int maxHR, int restingHR)
        {
            double mod = 0.75;
            return ((maxHR - restingHR) * mod) + restingHR;
        }
        public static double CouzenFindThresholdHR(int maxHR, int restingHR)
        {
            double ftpPerc = 0.85;
            return ((maxHR - restingHR) * ftpPerc) + restingHR;
        }
        public static double CouzensSubmaxFTP(int maxHR, int restingHR, int thresholdHR, double[] powerdata, double[] hrdata)
        {
            //do 2 submax tests - one at steady and one at modearte
            //2 submax efforts on a flat road/trainer where you lap for power or pace and HR. 
            //Ideally you'll come back to the same course for each test. Also, ideally you'll do the test under similar 
            //conditions (similar time of day, temp etc).
            //These efforts should be sufficiently long that HR stabilizes for a good period of time but sufficiently short that decoupling isn't significant. 
            //Depending on fitness, something in the range of 2x10min to 2x20min works well. 

            //example:
            //  double[] powerdata = new double[] { 200, 250 };
            //double[] hrdata = new double[] { 136, 150 };

            double[] results1 = Solvers.LeastSquaresBestFitLine(powerdata, hrdata);  //gives slope and b in y=mx+ b; 

            double b = results1[0];
            double slope = results1[1];

            double temp = Math.Abs(b - thresholdHR);   //0 = mx+b => b-y = mx
            double ftp = Math.Round((temp / slope));  //solve for x; 

            return ftp;
        }
        public static double TSSCouzen(double intensityFactor, int hours)
        {
            //IF^2*100*Ride Duration in Hours
            return (intensityFactor * intensityFactor) * 100 * hours;
        }
        public static double CouzenFindStrokePerLength(double height)
        {
            return 0.55 * height;
        }

        //swim version?  Based on steady state?

        //requires long effort...60 - 90mins
        public static double CouzensModifiedVDOTBike(int normalizedPower, double weightInKg, int trainingHR, int maxHR, int restingHR)
        {
            // (W/75*1000/BW)/((THR-RHR)/(MHR-RHR))
            //http://www.endurancecorner.com/Alan_Couzens/benchmarking

            //TODO: CONSIDER something like this but it is matched up against the assigned workout TSS as a ratio
            // so if they score X : 100TSS  then X-1 : 100TSS  etc etc...what does that give us?  Compare only 100TSS to other 100TSS or 
            //can we scale it out somehow?

            return ((normalizedPower / 75 * 1000 / weightInKg) / ((trainingHR - restingHR) / (maxHR - restingHR)));
        }
        public static double CouzensModifiedVDOTRun(double paceInMinPerKm, double weightInKg, int trainingHR, int maxHR, int restingHR)
        {
            //(210/P)/((THR-RHR)/(MHR-RHR))
            //http://www.endurancecorner.com/Alan_Couzens/benchmarking
            return ((210 / paceInMinPerKm) / ((trainingHR - restingHR) / (maxHR - restingHR)));
        }


        public static string CouzensGradeModifiedVDOT(double vdotScore)
        {
            //run may be marginally higher so consider skewing it down some
            string grade = String.Empty;

            if (vdotScore >= 65)
            {
                return "elite";
            }
            else if (vdotScore <= 64 || vdotScore >= 60)
            {
                return "topagegroup";
            }
            else if (vdotScore <= 59 || vdotScore >= 50)
            {
                return "mop";
            }
            else if (vdotScore <= 49 || vdotScore >= 40)
            {
                return "bop";
            }
            else
            {
                return "untrained";
            }


        }
    }



    public static class CadenceTorque
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watts"></param>
        /// <param name="cadence"></param>
        /// <param name="crankLength">in mm</param>
        /// <returns>newtons</returns>
        public static double AvgEffectivePedalForce(double watts, int cadence, double crankLength)
        {
            //crankLenght needs to be in Meters, assume coming in as mm...so 180 = 180 * 1000
            double clMeteres = crankLength * 1000;
            return (watts * 60) / (cadence * 2 * Math.PI * clMeteres);
        }

        public static double TorqueGivenPowerCadence(double power, double cadence)
        {
            return ((power * 60) / (2 * Math.PI * cadence));
        }

        public static double PowerGivenTorqueCadence(double torque, double cadence)
        {
            return ((cadence * 2 * Math.PI * torque) / 60);
        }

        public static double CadenceGivenTorquePower(double torque, double power)
        {
            return ((torque / (power * 60)) / (2 * Math.PI));
        }

    }


    #region Muscle stuff

    public static class Muscles
    {
        public static Tuple<double, double> FastOxidativeGlycTwitchPowerRange(double powerAtVo2Max)
        {
            //http://www.endurancecorner.com/Intervals_for_Base
            Tuple<double, double> range = new Tuple<double, double>(powerAtVo2Max * 0.85, powerAtVo2Max * 1.16);
            return range;
        }

        public static Tuple<double, double> FastGlycTwitchPowerRange(double powerAtVo2Max)
        {
            //http://www.endurancecorner.com/Intervals_for_Base
            Tuple<double, double> range = new Tuple<double, double>(powerAtVo2Max * 1.00, powerAtVo2Max * 1.20);
            return range;
        }

        public static Tuple<double, double> SlowTwitchPowerRange(double powerAtVo2Max)
        {
            //http://www.endurancecorner.com/Intervals_for_Base
            Tuple<double, double> range = new Tuple<double, double>(powerAtVo2Max * 0.6, powerAtVo2Max * 0.84);
            return range;
        }
    }
    #endregion

    #region TRIMP
    public static class TRIMP
    {
        public static double TRIMPScore(bool isMale, double maxHR, double restingHR, List<HeartRateDuration> sessionHR)
        {
            //http://fellrnr.com/wiki/TRIMP
            //TRIMPexp = sum(D x HRr x 0.64ey)
            double y = 1.92;
            if (!isMale)
                y = 1.67;

            double score = 0;

            foreach (HeartRateDuration item in sessionHR)
            {
                double hrr = HeartRate.FractionalHeartRateReserve(maxHR, restingHR, item.HeartRate);
                score += (item.DurationSeconds * 60) * hrr * (0.64 * Math.Exp(hrr * y));
            }

            return score;
        }

        public static double TRIMPScoreRPE(int rpe, int seconds)
        {
            return (rpe * (seconds * 60));
        }

        public static double NormalizedTRIMPScore(bool isMale, double maxHR, double restingHR, List<HeartRateDuration> sessionHR)
        {
            double score = 0;
            if (isMale)
            {
                score = TRIMPScore(isMale, maxHR, restingHR, sessionHR) * 0.436;
            }
            else
            {
                score = TRIMPScore(isMale, maxHR, restingHR, sessionHR) * 0.34;
            }
            return score;
        }
    }
    #endregion


    #region Beta stuff from Triathlonbook - new way to quantify

    public static class Beta
    {
        public static double BetaIntensityFactor(double heartRateReserve)
        {
            //http://thetriathlonbook.blogspot.com/2008/07/death-of-tss.html
            //http://thetriathlonbook.blogspot.com/2008/07/tss-related-rambling.html

            //basically, FTP = 1IF
            //There is a linear, 1:1 relationship between IF and Duration
            //which makes sense because 1FTP = 1IF
            //So we can say that for efforts over 1 hour
            //that the IF is 90% of the HRR (especially since we have to run afterwards)
            return heartRateReserve * 0.9;
        }

        public static double BetaTIFScore(double duration, double IF, bool isMale)
        {
            double y = 1.92;
            if (!isMale)
                y = 1.67;
            return (duration * 60) * 0.9 * IF * (0.64 * Math.Exp(0.9 * IF * y));
        }

        public static double BetaTIFScore(bool isMale, double maxHR, double restingHR, List<HeartRateDuration> sessionHR)
        {

            double y = 1.92;
            if (!isMale)
                y = 1.67;

            double score = 0;

            foreach (HeartRateDuration item in sessionHR)
            {
                double IF = HeartRate.FractionalHeartRateReserve(maxHR, restingHR, item.HeartRate) * 0.9;
                score += (item.DurationSeconds / 60) * 0.9 * IF * (0.64 * Math.Exp(0.9 * IF * y));
            }

            return score;
        }

        public static double BetaPowerTIFScore(bool isMale, double maxHR, double restingHR, List<HeartRateDuration> sessionHR)
        {
            //http://thetriathlonbook.blogspot.com/2008/07/death-of-tss.html
            double y = 1.92;
            if (!isMale)
                y = 1.67;

            double score = 0;

            foreach (HeartRateDuration item in sessionHR)
            {
                double IF = HeartRate.FractionalHeartRateReserve(maxHR, restingHR, item.HeartRate) * 0.9;
                score += (item.DurationSeconds / 60) * 0.9 * IF * (0.64 * Math.Exp(0.9 * IF * y));
            }

            //This just scales it as a factor of FTP like TSS is.
            //100 TSS = 1hr @ FTP  ; f
            //To do the same "TIF Score" we can do (item.DurationSeconds / 60) * 0.9 * IF * (0.64 * Math.Exp(0.9 * IF * y));
            //which is just 194.5 or 195...so 1hr @ FTP = 195;

            return Math.Floor(score / 195);
        }
    }
    #endregion

    #region Heart Rate stuff

    public static class HeartRate
    {


        public static double FractionalHeartRateReserve(double maxHR, double restingHR, double sessionHR)
        {

            //http://fellrnr.com/wiki/Heart_Rate_Reserve
            //(HRex – HRrest)/(HRmax – HRrest)
            return ((sessionHR - restingHR) / (maxHR = restingHR)) * 100;
        }

        public static double DecoupleRate(double aveSpeed1stHalf, double aveHR1stHalf, double aveSpeed2ndHalf, double aveHR2ndHalf)
        {
            //aveSpeed1stHalf = speed1stHalf / HRreadingCount1sthalf;
            //aveHR1stHalf = HR1stHalf / HRreadingCount1sthalf;

            //avSpeed2ndHalf = speed2ndHalf / HRreadingCount2ndhalf;
            //aveHR2ndHalf = HR2ndHalf / HRreadingCount2ndhalf;
            double firstHalf = (aveSpeed1stHalf / aveHR1stHalf);
            double secondHalf = (aveSpeed2ndHalf / aveHR2ndHalf);

            double RPrime = firstHalf - secondHalf;

            double decoupleRate = Math.Round((RPrime / firstHalf) * 100);

            return decoupleRate;
        }

        /// <summary>
        /// Estimate Vo2max % from Fractional Heart Rate Reserve
        /// </summary>
        /// <param name="maxHR"></param>
        /// <param name="restingHR"></param>
        /// <param name="sessionHR"></param>
        /// <returns></returns>
        public static double FractionalVo2MaxPercentEstimate(double maxHR, double restingHR, double sessionHR)
        {

            //http://fellrnr.com/wiki/Heart_Rate_Reserve
            double hrReserve = FractionalHeartRateReserve(maxHR, restingHR, sessionHR);
            return ((hrReserve - 37) / 64) * 100;
        }

    }
    #endregion

    #region KIMECO model

    public static class KIMECO
    {
        public static double FindResidual(double watts, double predicted)
        {
            return ((watts - predicted) / predicted) * 100;
        }

        public static double FindPredicted(double CP5, double uOCD, double time)
        {
            return CP5 + (uOCD / time);
        }


        public static double FindVo2max(double CP5, double cODC)
        {
            //same as F6 or  Predicted @ 5mins which is CP5 + (codc / 5mins)
            return CP5 + (cODC / 5);
        }

        public static double FindVo2(double CP5, double efficiency = 0.23)
        {
            return (((CP5 / 5.05) * 0.0143) / efficiency);
        }

        public static double Find_CODC(double CP5)
        {
            return (CP5 - CP5) * 5;
        }

        public static double Find_VOCD(double CP1, double CP5)
        {
            //todo: error chcek for cp1 >  on this and all others...assume good data for now
            return (CP1 - CP5);
        }

        public static double FindCPSlope(double CP90, double CP45)
        {
            return (CP90 - CP45 / (90 - 45));
        }

        public static double FindTP45Percent(double TP45, double vo2max)
        {
            return ((TP45 / vo2max) * 100);
        }
    }
    #endregion

}


public class EstimatedCriticalPowerResult
{
    public EstimatedCriticalPowerResult()
    {

    }
    public EstimatedCriticalPowerResult(double cp, double awc)
    {
        CriticalPower = cp;
        AnaerobicWorkCapacity = awc;
    }
    public double CriticalPower { get; set; }
    public double AnaerobicWorkCapacity { get; set; }
}

public class CriticalPowerPowerInterval
{
    public CriticalPowerPowerInterval()
    {

    }
    public int TargetPower { get; set; }
    public int NumberReps { get; set; }

}

public class HeartRateDuration
{
    public HeartRateDuration()
    {

    }
    public HeartRateDuration(int durationSeconds, int heartRate)
    {
        DurationSeconds = durationSeconds;
        HeartRate = heartRate;
    }

    public int DurationSeconds { get; set; }
    public int HeartRate { get; set; }
}


