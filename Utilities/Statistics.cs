using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PFSP_MLD_Console.Utilities 
{
    class Statistics
    {
        public static double Sum(double[] arr,int length)
        {
            double s = arr[0];
            for (int i = 1; i < length; i++)
            {
                s += arr[i];
            }
            return s;
        }

        public static double Mean(double[] arr, int length)
        {
            return Sum(arr, length) / length;
        }

        public static double Variance(double[] arr, int length)
        {
            double var = 0.0;
            for (int i = 0; i < length; i++)
            {
                var += (arr[i] * arr[i])/length;
            }
            var -= Mean(arr, length);
            return var;
        }

        public static double Std_dev(double[] arr,int length)
        {
            return Math.Sqrt(Variance(arr, length));
        }

        public static double Std_err(double[] arr,int length)
        {
            return (length * Std_dev(arr, length)) / (length - 1);
        }

        public static double MinTable(double[] arr,int length)
        {
            double m = arr[0];
            for (int i = 1; i < length; i++)
            {
                m = Math.Min(m,arr[i]);
            }
            return m;
        }
        public static double MaxTable(double[] arr, int length)
        {
            double m = arr[0];
            for (int i = 1; i < length; i++)
            {
                m = Math.Max(m, arr[i]);
            }
            return m;
        }
        public static int Uniform(int a,int b)
        {
            Random r = new Random();
            return (int) Math.Floor(a + (b - a) * r.NextDouble());
        }
    }
}
