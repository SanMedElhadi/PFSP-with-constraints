using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PFSP_MLD_Console.PFSP 
{
    class Vns
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int[] BestIndividual;
        public double BestFitness;
        public int Iterations;
        public int NumberNeigh;
        public bool isInit;
        public int[] initSolution;
        public int limit;
        public int[] InitialSolution(int nbJobs, int nbMachines, double[][] operTimes)
        {
            Heuristics h = new Heuristics();
            h.Name = Heuristics.Heuristic.Neh;
            return h.Execute(nbJobs, nbMachines, operTimes);
        }

        public int[] InitialSolutionMH(int nbJobs, int nbMachines, double[][] operTimes)
        {
            Sa init = new Sa();
            init.Iterations = 250;
            init.Temperature = 10000;
            init.Lambda = 0.93;
            init.NbJobs = nbJobs;
            init.NbMachines = nbMachines;
            init.JobsTimes = new double[init.NbMachines][];
            for (int i = 0; i < init.NbMachines; i++)
            {
                init.JobsTimes[i] = new double[init.NbJobs];
                Array.Copy(operTimes[i],init.JobsTimes[i],init.NbJobs);
            }
            init.Run();
            return init.BestIndividual;
        }
        public void LocalSearch(int[] perm,int length)
        {

        }
        public double Fitness(int[] perm,int nbJobs,int nbMachines,double[][] operTimes)
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
        public int[] Deepest_Descent(int[] permutation, Neighborhood.Structure structure)
        // Finding Local minimal in simple neighborhood structures
        {
            int[] perm = new int[this.JobsNumber];
            int[] neighPerm = new int[this.JobsNumber];
            double neighFitness;
            int[] bestPerm = new int[this.JobsNumber];
            double bestFitness = Fitness(permutation, this.JobsNumber, this.MachinesNumber, this.JobsTimes);
            Neighborhood n = new Neighborhood();
            switch (structure)
            {
                case Neighborhood.Structure.RandomInsert:
                    for (int i = 0; i < this.JobsNumber; i++)
                    {
                        for (int j = 0; j < this.JobsNumber; j++)
                        {
                            if (j != i)
                            {
                                Array.Copy(n.Insert(perm, this.JobsNumber, i, j), neighPerm, this.JobsNumber);
                                neighFitness = Fitness(neighPerm,this.JobsNumber,this.MachinesNumber,this.JobsTimes);
                                if (neighFitness < bestFitness)
                                {
                                    bestFitness = neighFitness;
                                    Array.Copy(neighPerm, bestPerm, this.JobsNumber);
                                }
                            }
                        }
                    }
                    break;
                case Neighborhood.Structure.RandomSwap:
                    for (int i = 0; i < this.JobsNumber; i++)
                    {
                        for (int j = 0; j < this.JobsNumber; j++)
                        {
                            if (j != i)
                            {
                                Array.Copy(n.Swap(perm, this.JobsNumber, i, j), neighPerm, this.JobsNumber);
                                neighFitness = Fitness(neighPerm, this.JobsNumber, this.MachinesNumber, this.JobsTimes);
                                if (neighFitness < bestFitness)
                                {
                                    bestFitness = neighFitness;
                                    Array.Copy(neighPerm, bestPerm, this.JobsNumber);
                                }
                            }
                        }
                    }
                    break;
                case Neighborhood.Structure.AdjacentSwap:
                    for (int i = 0; i < this.JobsNumber; i++)
                    {
                        for (int j = 0; j < this.JobsNumber; j++)
                        {
                            Array.Copy(n.Swap_Adjacent(perm, this.JobsNumber, j), neighPerm, this.JobsNumber);
                            neighFitness = Fitness(neighPerm, this.JobsNumber, this.MachinesNumber, this.JobsTimes);
                            if (neighFitness < bestFitness)
                            {
                                bestFitness = neighFitness;
                                Array.Copy(neighPerm, bestPerm, this.JobsNumber);
                            }

                        }
                    }
                    break;
                default:
                    break;
            }
            return bestPerm;
        }

        public void Run()
        {
            int[] s = new int[this.JobsNumber];
            int[] sn = new int[this.JobsNumber];
            int l = 1;
            //Array.Copy(InitialSolution(this.JobsNumber, this.MachinesNumber, this.JobsTimes),s,this.JobsNumber);
            Array.Copy(InitialSolutionMH(this.JobsNumber, this.MachinesNumber, this.JobsTimes),s,this.JobsNumber);
            this.BestIndividual = new int[this.JobsNumber];
            Array.Copy(s, this.BestIndividual, this.JobsNumber);

            this.BestFitness = Fitness(s,this.JobsNumber,this.MachinesNumber,this.JobsTimes);

            Neighborhood n = new Neighborhood();


            for (int i = 0; i < this.Iterations; i++)
            {
                int v = 1;
                while (v <= this.NumberNeigh)
                {
                    if (v == 1)
                    {
                        Array.Copy(n.Random_Insert(s, this.JobsNumber, 1),sn,this.JobsNumber); // Change to methods with multiple Neighborhood Structure
                        Array.Copy(Deepest_Descent(sn,Neighborhood.Structure.RandomInsert),sn,this.JobsNumber) ; // Change to methods with mutiple Neighborhood Structure
                    }
                    else if (v == 2)
                    {
                        Array.Copy(n.Random_Swap(s, this.JobsNumber, 1),sn, this.JobsNumber); // Change to methods with multiple Neighborhood Structure
                        Array.Copy(Deepest_Descent(sn, Neighborhood.Structure.RandomSwap), sn, this.JobsNumber); // Change to methods with mutiple Neighborhood Structure
                    }
                    else if (v == 3)
                    {
                        Array.Copy(n.Adjacent_Swap(s, this.JobsNumber, 1),sn, this.JobsNumber); // Change to methods with multiple Neighborhood Structure
                        //Array.Copy(Deepest_Descent(sn, Neighborhood.Structure.AdjacentSwap), sn, this.JobsNumber); // Change to methods with mutiple Neighborhood Structure
                    }
                    //LocalSearch(SN, this.jobs_number);
                    double res = Fitness(sn, this.JobsNumber, this.MachinesNumber, this.JobsTimes);

                    if (res < this.BestFitness)
                    {
                        Array.Copy(sn,s,this.JobsNumber);
                        Array.Copy(s, this.BestIndividual, this.JobsNumber);
                        //Console.WriteLine("changed");
                        this.BestFitness = res;
                        v = 1;
                    }
                    else
                    {
                        v++;
                        l++;
                        if (l > this.limit)
                        {
                            Heuristics heuristics = new Heuristics();
                            //Array.Copy(n.AddModulu(s, this.JobsNumber, i),s,this.JobsNumber);
                            Array.Sort(s,n.AddModulu(s, this.JobsNumber, i));
                            Array.Copy(heuristics.NEHWithInit(this.JobsNumber, this.MachinesNumber, this.JobsTimes, s),s,this.JobsNumber); 
                            l = 1;
                        }
                    }
                }           
            }
        }
    }
}
