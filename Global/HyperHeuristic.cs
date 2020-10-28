using System;
using PFSP_MLD_Console.PFSP;

namespace PFSP_MLD_Console.Global 
{
    class HyperHeuristic
    {
        private string Variant { get; set; }
        public Heuristics Heuristics;
        public MetaHeuristics MetaHeuristics;
        public int jobsNumber;
        public int machinesNumber;
        public double[][] jobsTimes;
        public double bestFitness;
        public int[] bestIndividual;

        public HyperHeuristic()
        {
            this.Heuristics = new Heuristics();
            this.MetaHeuristics = new MetaHeuristics();
        }

        public double Fitness(int[] permutation)
        {
            return basic_makespan_dynamic(this.jobsNumber, this.machinesNumber, permutation, this.jobsTimes);
        }
        public double basic_makespan_dynamic(int nbJobs, int nbMachines, int[] permutations, double[][] processingTimes)
            // Calculates the makespan using dynamic programming without acceleration
        {
            double[][] c = new double[nbMachines + 1][];
            for (int i = 0; i < nbMachines + 1; i++)
            {
                c[i] = new double[nbJobs + 1];
            }
            for (int i = 0; i < nbJobs + 1; i++)
            {
                c[0][i] = 0.0;
            }
            for (int i = 0; i < nbMachines + 1; i++)
            {
                c[i][0] = 0.0;
            }

            for (int i = 1; i < nbMachines + 1; i++)
            {
                for (int j = 1; j < nbJobs + 1; j++)
                {
                    if (c[i - 1][j] > c[i][j - 1])
                    {
                        c[i][j] = c[i - 1][j] + processingTimes[i - 1][permutations[j - 1]];
                    }
                    else
                    {
                        c[i][j] = c[i][j - 1] + processingTimes[i - 1][permutations[j - 1]];
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }
        public void VerticalHybridationHeuristics(Heuristics.Heuristic[] methodes)
        {
            int[] opt = new int[this.jobsNumber];
            int[] res = new int[this.jobsNumber];
            double fitnessOpt = Double.MaxValue;
            double fitnessRes;
            foreach (var method in methodes)
            {
                Array.Copy(this.Heuristics.Execute(this.jobsNumber,this.machinesNumber,this.jobsTimes),res,this.jobsNumber);
                fitnessRes = Fitness(res);
                if (fitnessRes<fitnessOpt)
                {
                    Array.Copy(res,opt,this.jobsNumber);
                    fitnessOpt = fitnessRes;
                }
            }
            Array.Copy(opt,this.bestIndividual,this.jobsNumber);
            this.bestFitness = fitnessOpt;
        }

        public void HorizentalHybridationHeuristics(Heuristics.Heuristic[] methodes)
        {
            
        }

        public void VerticalHybridationMetaheuristics()
        {
            
        }

        public void HorizentalHybridationMetaheuristics()
        {
            
        }
    }
}
