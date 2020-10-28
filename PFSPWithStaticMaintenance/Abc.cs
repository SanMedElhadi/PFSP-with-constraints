using System;
using PFSP_MLD_Console.PFSP;

namespace PFSP_MLD_Console.PFSPWithStaticMaintenance
{
    public class Abc
    {
        public int Iterations;
        public int EmployedNumber;
        public int ScoutNumber;
        public int Limit;
        public int JobsNumber;
        public int MachineNumber;
        public double[][] JobsTime;
        public double MaintenanceTime;
        public int[] BestIndividual;
        public double BestFitness;
        
        public int[] InitialSolution(int nbJobs,int nbMachine,double[][] operTimes)
        {
            Heuristics h = new Heuristics();
            Random r = new Random();
            h.Name = Heuristics.Heuristic.Neh;
            return h.Execute(nbJobs,nbMachine,operTimes);
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
        public double makespan_with_maintenance(double[][] processingTimes, int nbJobs,int nbMachines, int[] permutations,int [][] maintenancePositions,double maintenanceTimes)
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
                        c[i][j] = c[i - 1][j] + processingTimes[i][permutations[j]];
                    }
                    else
                    {
                        c[i][j] = c[i][j - 1] + processingTimes[i][permutations[j]];
                    }
                    if (Array.IndexOf(maintenancePositions[i],j)!=-1)
                    {
                        c[i][j] += maintenanceTimes;
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }
        public double Fitness(int nbJobs,int nbMachine,int[] perm,double[][] operTimes)
        {
            return basic_makespan_dynamic(nbJobs,nbMachine,perm,operTimes);
        }
        public double FitnessMaintenance(int[] perm,int[][] maintenancePosition)
        {
            return makespan_with_maintenance(this.JobsTime,this.JobsNumber,this.MachineNumber,perm,maintenancePosition,this.MaintenanceTime);
        }
        public int[] LocalSearch(Neighborhood.Structure structure,int[] perm)
        {
            Neighborhood n = new Neighborhood();
            int[] opt = new int[this.JobsNumber];
            int[] res = new int[this.JobsNumber];
            Array.Copy(perm,opt,this.JobsNumber);
            for (int i = 0; i < this.JobsNumber*(this.JobsNumber-1)/2; i++)
            {
                switch (structure)
                {
                    case Neighborhood.Structure.RandomInsert:
                        Array.Copy(n.Random_Insert(opt, this.JobsNumber, 1), res, this.JobsNumber);
                        break;
                    case Neighborhood.Structure.RandomSwap:
                        Array.Copy(n.Random_Swap(opt, this.JobsNumber, 1), res, this.JobsNumber);
                        break;
                    case Neighborhood.Structure.AdjacentSwap:
                        Array.Copy(n.Adjacent_Swap(opt, this.JobsNumber, 1), res, this.JobsNumber);
                        break;
                }
                if (Fitness(this.JobsNumber,this.MachineNumber,res,this.JobsTime)<Fitness(this.JobsNumber,this.MachineNumber,opt,this.JobsTime))
                {
                    Array.Copy(res,opt,this.JobsNumber);
                }
            }
            return opt;
        }
        public void Run()
        {
            int[][] employedBees = new int[this.EmployedNumber][];
            int[] limits = new int[this.EmployedNumber];
            double[] emlpoyedFitness = new double[this.EmployedNumber];
            
            // Initialisation
            this.BestFitness = Double.MaxValue;
            this.BestIndividual = new int[this.JobsNumber];
            for (int i = 0; i < this.EmployedNumber; i++)
            {
                employedBees[i] = new int[this.JobsNumber];
                Array.Copy(sourceArray:InitialSolution(nbJobs:this.JobsNumber,nbMachine:this.MachineNumber,operTimes:this.JobsTime),destinationArray:employedBees[i],length:this.JobsNumber);
                limits[i] = 1;
                emlpoyedFitness[i] = Fitness(this.JobsNumber, this.MachineNumber, employedBees[i], this.JobsTime);
                if (this.BestFitness > emlpoyedFitness[i])
                {
                    this.BestFitness = emlpoyedFitness[i];
                    Array.Copy(employedBees[i],this.BestIndividual,this.JobsNumber);
                }
            }
            // Principal Algorithm
            for (int iter = 0; iter < this.Iterations; iter++)
            {
                Console.WriteLine(iter);
                // Employed Bees Phase
            for (int i = 0; i < this.EmployedNumber; i++)
            {
                int[] neighboor = new int[this.JobsNumber];
                Neighborhood n = new Neighborhood();
                Neighborhood.Operation operation;
                if (i%2 ==0)
                {
                    operation = Neighborhood.Operation.Insert;
                }
                else
                {
                    operation = Neighborhood.Operation.Swap;
                }
                
                switch (operation)
                {
                    case Neighborhood.Operation.Insert :
                        Array.Copy(n.Random_Insert(employedBees[i], this.JobsNumber, 1),neighboor,this.JobsNumber); 
                        break;
                    case Neighborhood.Operation.Swap :
                        Array.Copy(n.Random_Swap(employedBees[i], this.JobsNumber, 1),neighboor,this.JobsNumber); 
                        break;
                }

                if (Fitness(this.JobsNumber,this.MachineNumber,neighboor,this.JobsTime)<emlpoyedFitness[i])
                {
                    emlpoyedFitness[i] = Fitness(this.JobsNumber, this.MachineNumber, neighboor, this.JobsTime);
                    Array.Copy(neighboor,employedBees[i],this.JobsNumber);
                    if (this.BestFitness > emlpoyedFitness[i])
                    {
                        this.BestFitness = emlpoyedFitness[i];
                        Array.Copy(employedBees[i],this.BestIndividual,this.JobsNumber);
                    }
                }
                else
                {
                    limits[i] = limits[i] + 1;
                }
            }
            // Scout Bees Phase
            /*
            * Selection with fitness
            */
            /*
            double[] employedProbabilities = new double[this.EmployedNumber];
            double sumFitnesses = Utilities.Statistics.Sum(emlpoyedFitness, this.EmployedNumber);
            for (int j = 0; j < this.EmployedNumber; j++)
            {
                employedProbabilities[j] = emlpoyedFitness[j] /sumFitnesses;
            }
            */
            for (int i = 0; i < this.ScoutNumber; i++)
            {
                Random random = new Random();
                // Random Selection
                int index = random.Next() % this.EmployedNumber;
                // Fitness Selection
                /*int index = 0;
                while (emlpoyedFitness[j] < random.NextDouble())
                {
                    index++;
                }*/
                int[] resLocalSearch = new int[this.JobsNumber];
                Array.Copy(LocalSearch(Neighborhood.Structure.RandomInsert,employedBees[index]),resLocalSearch,this.JobsNumber);
                if (Fitness(this.JobsNumber,this.MachineNumber,resLocalSearch,this.JobsTime)<Fitness(this.JobsNumber,this.MachineNumber,employedBees[i],this.JobsTime))
                {
                    Array.Copy(resLocalSearch,employedBees[index],this.JobsNumber);
                    emlpoyedFitness[index] =
                        Fitness(this.JobsNumber, this.MachineNumber, resLocalSearch, this.JobsTime);
                    limits[index] = 1;
                    if (this.BestFitness > emlpoyedFitness[i])
                    {
                        this.BestFitness = emlpoyedFitness[i];
                        Array.Copy(employedBees[i],this.BestIndividual,this.JobsNumber);
                    }
                }
            }
            // Onlooker Bees Phase
            for (int i = 0; i < this.EmployedNumber; i++)
            {
                if (limits[i]>Limit)
                {
                    // Change the permutation
                    Neighborhood n = new Neighborhood();
                    if (i%this.JobsNumber == 0)
                    {
                        Array.Copy(n.AddModulu(employedBees[i], this.JobsNumber, 1), employedBees[i],
                            this.JobsNumber);
                    }
                    emlpoyedFitness[i] = Fitness(this.JobsNumber, this.MachineNumber, employedBees[i],
                        this.JobsTime);
                    if (this.BestFitness > emlpoyedFitness[i])
                    {
                        this.BestFitness = emlpoyedFitness[i];
                        Array.Copy(employedBees[i],this.BestIndividual,this.JobsNumber);
                    }
                    limits[i] = 1;
                }
            }
            }
            
        }
    }
}