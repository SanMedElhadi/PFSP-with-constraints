using System;

namespace PFSP_MLD_Console.PFSP
{
    public class Cuckoo
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int PopSize;
        public int Iterations;
        public double SmartCuckoo;
        public double WorstPortion;
        public double lambda;
        public int[] BestIndividual;
        public double BestFitness;

        public int[] InitialSolution(Heuristics.Heuristic name)
        {
            Heuristics heuristic = new Heuristics();
            heuristic.Name = name;
            return heuristic.Execute(this.JobsNumber, this.MachinesNumber, this.JobsTimes);
        }
        public double Fitness(int[] permutation)
        {
            return basic_makespan_dynamic(this.JobsNumber, this.MachinesNumber, permutation, this.JobsTimes);
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

        public double Levy(double lambda)
        // Approximation of the levy flights distribution
        {
            Random random = new Random();
            return Math.Pow(1/(9*random.NextDouble()+1),this.lambda);
        }

        public int[] LocalSearch(int[] permutation, Neighborhood.Operation operation)
        {
            int[] perm = new int[this.JobsNumber];
            switch (operation)
            {
                case Neighborhood.Operation.Insert :
                    break;
                case Neighborhood.Operation.Swap :
                    break;
                case Neighborhood.Operation.AdjacentSwap :
                    break;
            }

            return perm;
        }
        public void Run()
        {
            int[][] population = new int[this.PopSize][];
            double[] popFitness = new double[this.PopSize];
            Neighborhood neighborhood = new Neighborhood();
            Random random = new Random();
            this.BestFitness = Double.MaxValue;
            this.BestIndividual = new int[this.JobsNumber];
            for (int i = 0; i < this.PopSize; i++)
            {
                population[i] = new int[this.JobsNumber];
                Array.Copy(this.InitialSolution(Heuristics.Heuristic.NehRandom),population[i],this.JobsNumber);
                popFitness[i] = Fitness(population[i]);
            }
            Array.Sort(popFitness,population);
            for (int i = 0; i < this.Iterations; i++)
            {
                // Smart Cuckoo Local Search
                for (int j = 0; j < this.SmartCuckoo*this.PopSize; j++)
                {
                    int[] sol = new int[this.JobsNumber];
                    int index = random.Next() % this.PopSize;
                    double levy = Levy(this.lambda);
                    if (levy < 0.3)
                    {
                        Array.Copy(LocalSearch(neighborhood.Random_Insert(population[index],this.JobsNumber,1),Neighborhood.Operation.Swap),sol,this.JobsNumber);
                    }
                    else if (levy <0.7)
                    {
                        Array.Copy(LocalSearch(neighborhood.Random_Swap(population[index],this.JobsNumber,1),Neighborhood.Operation.Swap),sol,this.JobsNumber);
                    }
                    else
                    {
                        Array.Copy(LocalSearch(neighborhood.Adjacent_Swap(population[index],this.JobsNumber,1),Neighborhood.Operation.Swap),sol,this.JobsNumber);
                    }

                    if (Fitness(sol) < Fitness(population[index]))
                    {
                        Array.Copy(LocalSearch(sol,Neighborhood.Operation.Swap),population[index],this.JobsNumber);
                        popFitness[index] = Fitness(population[index]);
                    }
                }
                Array.Sort(popFitness,population);
                // Random Individual Selection
                int[] bestLocal = new int[this.JobsNumber];
                Array.Copy(population[0],bestLocal,this.JobsNumber);
                int[] searchRes = new int[this.JobsNumber];
                double levySelect = Levy(this.lambda);
                int individual = random.Next() % this.PopSize;
                if (levySelect < 0.5)
                {
                    Array.Copy(LocalSearch(neighborhood.Random_Swap(bestLocal,this.JobsNumber,2),Neighborhood.Operation.Swap),searchRes,this.JobsNumber);
                }
                else
                {
                    Array.Copy(LocalSearch(neighborhood.Random_Insert(bestLocal,this.JobsNumber,2),Neighborhood.Operation.Swap),searchRes,this.JobsNumber);
                }

                double FitnessSe = Fitness(searchRes);
                if (popFitness[individual] > FitnessSe)
                {
                    Array.Copy(searchRes,population[individual],this.JobsNumber);
                    popFitness[individual] = FitnessSe;
                }
                Array.Sort(popFitness,population);
                // Worst Cuckoos Changing
                for (int j = this.PopSize-1; j > this.PopSize - (int) this.WorstPortion*this.PopSize; j--)
                {
                    int[] sol = new int[this.JobsNumber];
                    Array.Copy(LocalSearch(neighborhood.Random_Swap(population[j],this.JobsNumber,this.JobsNumber),Neighborhood.Operation.Swap),sol,this.JobsNumber);
                    double fitnessSol = Fitness(sol);
                    if (popFitness[j] > fitnessSol)
                    {
                        Array.Copy(sol,population[j],this.JobsNumber);
                        popFitness[j] = fitnessSol;
                    }
                }
                Array.Sort(popFitness,population);
            }
            Array.Copy(population[0],this.BestIndividual,this.JobsNumber);
            this.BestFitness = popFitness[0];
        }
    }
}