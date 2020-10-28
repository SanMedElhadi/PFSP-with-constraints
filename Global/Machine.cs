using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console
{
    class Machine
    {
        public enum Affection
        {
            Static,
            LearningProduction,
            DeteriorationProduction,
            LearningMaintenance,
            DeteriorationMaintenance,
            CombinationProduction,
            CombinationMaintenance
        }
        public int Index { get; set; }
        public double InitialMaintenanceTime { get; set; }
        public double[] CoefLearning;
        public double[] CoefDeterioration;
        public double[] FactorsLearning;
        public double[] FactorsDeterioration;
        public double Rma;
        public Affection[] Effects;
        
        
        public Machine(int i = -1,double m = 0.0,double[] coefL = null, double[] coefD = null, double[] factL = null, double[] factD = null,Affection[] effect = null)
        {
            this.Index = i;
            this.InitialMaintenanceTime = m;
            this.CoefLearning = coefL;
            this.CoefDeterioration = coefD;
            this.FactorsLearning = factL;
            this.FactorsDeterioration = factD;
        }


        public double calculate_effect()
        {
            double value = this.InitialMaintenanceTime;//To change
            foreach (var effect in Effects)
            {
                switch (effect)
                {
                    case Affection.Static:
                        break;
                    case Affection.LearningProduction:
                        break;
                    case Affection.DeteriorationProduction:
                        break;
                    case Affection.LearningMaintenance:
                        break;
                    case Affection.DeteriorationMaintenance:
                        break;
                    case Affection.CombinationProduction:
                        break;
                    case Affection.CombinationMaintenance:
                        break;
                    default:
                        break;
                }
            }
            return value;
        }
    }
}
