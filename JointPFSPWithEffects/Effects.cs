using System;

namespace PFSP_MLD_Console.JointPFSPWithEffects
{
    public class Effects
    {
        public EffectType Type;
        public EffectModel model;
        public Dependency Depend;
        public int Position;
        public double Time;
        public double LearningRate;
        public double DeteriorationRate;
        public enum EffectType
        {
            Learning,
            Deterioration
        }

        public enum EffectModel
        {
            Linear,
            Expo,
            LogLinear,
            Polynomial,
            Escalier
        }

        public enum Dependency
        {
            Time,
            Position,
            TimeAndPosition,
            Experience
        }
        /*
        public double Calculate()
        {
            double realTime;
            return realTime;
        }*/
        public double DeterPosLogLinear(int position,double initial,double ratio)
        {
            return initial * Math.Pow(position, ratio);
        }

        public double DeterPosLinear(int position,double[] coef)
        {
            return coef[0] + coef[1] * position;
        }

        public double DeterPosExpo(int position, double initial, double ratio)
        {
            return initial * Math.Pow(ratio, position - 1);
        }

        public double DeterPosPoly(int position, double[] coef)
        {
            double value = 0.0;
            // Horner Method
            for (int i = 0; i < coef.Length; i++)
            {
                value = coef[coef.Length - i] + value * position;
            }

            return value;
        }
        public double DeterTimePoly(double time, double[] coef)
        {
            double value = 0.0;
            // Horner Method
            for (int i = 0; i < coef.Length; i++)
            {
                value = coef[coef.Length - i] + value * time;
            }

            return value;
        }
        public double DeterTimeExpo(double time, double initial, double ratio)
        {
            return initial * Math.Exp(time*ratio);
        }
        public double DeterTimeLinear(double time,double[] coef)
        {
            return coef[0] + coef[1] * time;
        }
        
    }
}