using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PFSP_MLD_Console.PFSP
{
    class Ga
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int PopSize;
        public int Iterations;
        public int NumberSelections;
        public double CrossoverProb;
        public double MutationProb;
        public int Limit;
        public int[] BestIndividual;
        public double BestFitness;

        public int[] InitialSolution(Heuristics.Heuristic name)
        {
            Heuristics heuristic = new Heuristics();
            heuristic.Name = name;
            return heuristic.Execute(this.JobsNumber, this.MachinesNumber, this.JobsTimes);
        }

        public void Crossover1X(int[] parent1,int[] parent2,int length,out int[] child1,out int[] child2)
        {
            Random random = new Random();
            int point = random.Next() % (length - 1);
            child1 = new int[length];
            child2 = new int[length];
            int index;
            for (int j = 0; j < length; j++)
            {
                index = Array.IndexOf(parent2, parent1[j]);
                if (index < point)
                {
                    child1[j] = parent1[j];
                }
                else
                {
                    child1[j] = parent2[j];
                }
                index = Array.IndexOf(parent1, parent2[j]);
                if (index < point)
                {
                    child1[j] = parent2[j];
                }
                else
                {
                    child1[j] = parent1[j];
                }
            } 
        }

        public void Crossover2PX(int[] parent1, int[] parent2, int length, out int[] child1, out int[] child2)
        {
            Random random = new Random();
            int point1 = random.Next() % (length - 1);
            int point2 = random.Next() % (length - 1);
            child1 = new int[length];
            child2 = new int[length];
            int index;
            for (int j = 0; j < length; j++)
            {
                index = Array.IndexOf(parent2, parent1[j]);
                if (index < Math.Min(point1,point2) || index > Math.Max(point1,point2))
                {
                    child1[j] = parent1[j];
                }
                else
                {
                    child1[j] = parent2[j];
                }
                index = Array.IndexOf(parent1, parent2[j]);
                if (index < Math.Min(point1,point2) || index > Math.Max(point1,point2))
                {
                    child1[j] = parent2[j];
                }
                else
                {
                    child1[j] = parent1[j];
                }
            }
        }

        public int[] DeleteElementByIndex(int[] ints, int pos)
        {
            for (int i = pos; i < ints.Length; i++)
            {
                ints[i] = ints[i + 1];
            }

            return ints;
        }
        public double Fitness(int[] Permutation)
        {
            return this.basic_makespan_dynamic(this.JobsNumber, this.MachinesNumber, Permutation, this.JobsTimes);
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
            int[][] population = new int[this.PopSize][];
            double[] popFitness = new double[this.PopSize];
            double[] selectionProbs = new double[this.PopSize];
            int[] indexesSelected = new int[this.NumberSelections];
            Neighborhood neighborhood = new Neighborhood();
            int[] crossoverChild1;
            int[] crossoverChild2;
            double sumPopFitness = 0.0;
            Random random = new Random();
            this.BestIndividual = new int[this.JobsNumber];
            this.BestFitness = Double.PositiveInfinity;
            // Population Initialization
            for (int i = 0; i < this.PopSize; i++)
            {
                population[i] = new int[this.JobsNumber];
                Array.Copy(this.InitialSolution(Heuristics.Heuristic.NehRandom),population[i],this.JobsNumber);
                popFitness[i] = Fitness(population[i]);
                sumPopFitness += popFitness[i];
                if (this.BestFitness > popFitness[i])
                {
                    this.BestFitness = popFitness[i];
                    Array.Copy(population[i],this.BestIndividual,this.JobsNumber);
                }
            }
            // Process
            for (int i = 0; i < this.Iterations; i++)
            {
                // Calculate Selection probability
                for (int j = 0; j < this.PopSize; j++)
                {
                    selectionProbs[j] = popFitness[j] / sumPopFitness;
                }
                // Selection of Parents for the Crossover
                for (int j = 0; j < this.NumberSelections; j++)
                {
                    indexesSelected[j] = random.Next() % (this.PopSize - 1);
                }
                // Crossover
                for (int j = 0; j < this.NumberSelections/2; j++)
                {
                    int parent1 = random.Next() % (this.NumberSelections - 1-2*j);
                    int parent2;
                    do
                    {
                        parent2 = random.Next() % (this.NumberSelections - 1-2*j);
                    } while (parent2 != parent1);
                    if (random.NextDouble() <= this.CrossoverProb)
                    {
                        // Execute Crossover and update the population
                        Crossover1X(population[indexesSelected[parent1]],population[indexesSelected[parent2]],this.JobsNumber,out crossoverChild1,out crossoverChild2);
                        double fitnessChild1 = this.Fitness(crossoverChild1); 
                        double fitnessChild2 = this.Fitness(crossoverChild2); 
                        if (fitnessChild1 < popFitness[indexesSelected[parent1]])
                        {
                            Array.Copy(crossoverChild1,population[indexesSelected[parent1]],this.JobsNumber);
                            popFitness[parent1] = fitnessChild1;
                        }
                        if (fitnessChild2 < popFitness[indexesSelected[parent2]])
                        {
                            Array.Copy(crossoverChild2,population[indexesSelected[parent2]],this.JobsNumber);
                            popFitness[parent2] = fitnessChild2;
                        }
                    }
                    Array.Copy(DeleteElementByIndex(DeleteElementByIndex(indexesSelected,parent2),parent2),indexesSelected,this.NumberSelections);
                }
                // Mutation
                if (random.NextDouble() <= this.MutationProb)
                {
                    // Execute Mutation and update the population
                    //neighborhood.Random_Swap();
                }
            }
        }
    }
}
