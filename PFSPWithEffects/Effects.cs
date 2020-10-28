using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSPWithEffects
{
    class Effects
    {
        public EffectType Type;
        public EffectModel model;
        public Dependency Depend;
        public int Position;
        public double Time;
        public double[] LinearCoef;
        public double[] PolyCoef;
        public double Initial;
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
            Experience,
            TimeAndExperience
        }
        
        public double Calculate()
        {
            double realTime = Double.Epsilon;
            switch (this.Type)
            {
                case EffectType.Learning :
                    switch (Depend)
                    {
                        case Dependency.Position :
                            switch (model)
                            {
                                case EffectModel.Linear :
                                    break;
                                case EffectModel.LogLinear :
                                    break;
                                case EffectModel.Polynomial :
                                    break;
                                case EffectModel.Expo :
                                    break;
                                case EffectModel.Escalier :
                                    break;
                            }
                            break;
                        case Dependency.Time :
                            switch (model)
                            {
                                case EffectModel.Linear :
                                    break;
                                case EffectModel.LogLinear :
                                    break;
                                case EffectModel.Polynomial :
                                    break;
                                case EffectModel.Expo :
                                    break;
                                case EffectModel.Escalier :
                                    break;
                            }
                            break;
                        case Dependency.Experience :
                            switch (model)
                            {
                                case EffectModel.Linear :
                                    break;
                                case EffectModel.LogLinear :
                                    break;
                                case EffectModel.Polynomial :
                                    break;
                                case EffectModel.Expo :
                                    break;
                                case EffectModel.Escalier :
                                    break;
                            }
                            break;
                        case Dependency.TimeAndPosition :
                            break;
                        case Dependency.TimeAndExperience :
                            break;
                    }
                    break;
                case EffectType.Deterioration:
                    switch (Depend)
                    {
                        case Dependency.Position :
                            switch (model)
                            {
                                case EffectModel.Linear :
                                    realTime = this.DeterPosLinear(this.Position, this.LinearCoef);
                                    break;
                                case EffectModel.LogLinear :
                                    realTime = this.DeterPosLogLinear(this.Position, this.Initial,
                                        this.DeteriorationRate);
                                    break;
                                case EffectModel.Polynomial :
                                    break;
                                case EffectModel.Expo :
                                    realTime = this.DeterPosExpo(this.Position, this.Initial, this.DeteriorationRate);
                                    break;
                                case EffectModel.Escalier :
                                    break;
                            }
                            break;
                        case Dependency.Time :
                            switch (model)
                            {
                                case EffectModel.Linear :
                                    realTime = this.DeterTimeLinear(this.Time, this.LinearCoef);
                                    break;
                                case EffectModel.LogLinear :
                                    break;
                                case EffectModel.Polynomial :
                                    realTime = this.DeterTimePoly(this.Time, this.PolyCoef);
                                    break;
                                case EffectModel.Expo :
                                    realTime = this.DeterTimeExpo(this.Time, this.Initial, this.DeteriorationRate);
                                    break;
                                case EffectModel.Escalier :
                                    break;
                            }
                            break;
                        case Dependency.TimeAndPosition :
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return realTime;
        }
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
