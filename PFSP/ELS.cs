using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSP
{
    class Els
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int[] BestIndividual;
        public double BestFitness;
        public int Iterations;

        public int[] InitialSolution(int nbJobs, int nbMachines, double[][] operTimes)
        {
            Heuristics h = new Heuristics();
            h.Name = Heuristics.Heuristic.Neh;
            return h.Execute(nbJobs, nbMachines, operTimes);
        }
        public void LocalSearch(int[] perm, int length)
        {

        }
        public double Fitness(int[] perm, int nbJobs, int nbMachines, double[][] operTimes)
        {
            return basic_makespan_dynamic(nbJobs, nbMachines, perm, operTimes);
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

        }
    }
}
