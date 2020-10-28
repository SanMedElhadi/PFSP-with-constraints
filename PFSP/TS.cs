using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSP 
{
    class Ts
    {
        public int ElementsList;
        public int Iterations;
        public int Limit;
        public int JobsNumber;
        public int MachineNumber;
        public double[][] JobsTime;
        public int[] BestIndividual;
        public int[] BestFitness;
        public int[][] TabuList;
        
        public int[] InitialSolution()
        {
            Heuristics heuristic = new Heuristics();
            heuristic.Name = Heuristics.Heuristic.Neh;
            return heuristic.Execute(this.JobsNumber, this.MachineNumber, this.JobsTime);
        }
        public double Fitness(int[] permutation)
        {
            return this.basic_makespan_dynamic(this.JobsNumber, this.MachineNumber, permutation, this.JobsTime);
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
        public void Run()
        {
            this.TabuList = new int[this.ElementsList][];
            int[] individual = new int[this.JobsNumber];
            for (int i = 0; i < this.ElementsList; i++)
            {
                this.TabuList[i] = new int[this.JobsNumber];
            }
            Array.Copy(this.InitialSolution(),individual,this.JobsNumber);
            for (int i = 0; i < this.Iterations; i++)
            {
                
            }
        }
    }
}
