using System;
using System.Runtime.CompilerServices;
using PFSP_MLD_Console.PFSP;

namespace PFSP_MLD_Console.InterpolationBasedOptimisationPFSP
{
    public class AbcContInterp
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public Interpolation[] Interpolations;
        public int Iterations;
        public int EmployedNumber;
        public int ScoutNumber;
        public int Limit;
        public int[] BestIndividual;
        public double BestFitness;

        public int[] InitialSolution(Heuristics.Heuristic name)
        {
            Heuristics heuristics = new Heuristics();
            heuristics.Name = name;
            return heuristics.Execute(this.JobsNumber, this.MachinesNumber, this.JobsTimes);
        }

        public double Fitness(int[] permutation)
        {
            return this.basic_makespan_dynamic(this.JobsNumber, this.MachinesNumber, permutation, this.JobsTimes);
        }
        public double FitnessPosition(double[] permutation)
        {
            return this.basic_makespan_dynamic_interPoly(this.JobsNumber, this.MachinesNumber, permutation, this.Interpolations);
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
        public double basic_makespan_dynamic_interPoly(int nbJobs, int nbMachines, double[] permutations, Interpolation[] interpolations)
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
                    double processingTime = PolOper.CalculateInValue(interpolations[i-1].InterPoly,permutations[j-1]);
                    if (c[i - 1][j] > c[i][j - 1])
                    {
                        c[i][j] = c[i - 1][j] + processingTime;
                    }
                    else
                    {
                        c[i][j] = c[i][j - 1] + processingTime;
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }

        public void LocalSearch(double[] position)
        {
            
        }
        public int[] LOV_rule(int nbJobs,double[] x)
        {
            int[] perm = new int[nbJobs];
            double[] xprime = new double[nbJobs];
            Array.Copy(x, xprime, nbJobs);
            Array.Sort(xprime);
            Array.Reverse(xprime);
            for (int i = 0; i < nbJobs; i++)
            {
                perm[i] = Array.IndexOf(x,xprime[i]);
            }
            return perm;
        }
        public int[] SOV_rule(int nbJobs, double[] x)
        {
            int[] perm = new int[nbJobs];
            double[] xprime = new double[nbJobs];
            Array.Copy(x, xprime, nbJobs);
            Array.Sort(xprime);
            for (int i = 0; i < nbJobs; i++)
            {
                perm[i] = Array.IndexOf(x, xprime[i]);
            }
            return perm;
        }
        public int[] LRV_rule(int nbJobs, double[] x)
        {
            int[] perm = new int[nbJobs];
            double[] xprime = new double[nbJobs];
            Array.Copy(x, xprime, nbJobs);
            for (int i = 0; i < nbJobs; i++)
            {
                perm[i] = i;
            }
            Array.Sort(xprime,perm);
            Array.Reverse(perm);
            return perm;
        }
        public int[] SRV_rule(int nbJobs, double[] x)
        {
            int[] perm = new int[nbJobs];
            double[] xprime = new double[nbJobs];
            Array.Copy(x, xprime, nbJobs);
            Array.Sort(xprime);
            for (int i = 0; i < nbJobs; i++)
            {
                perm[i] = Array.IndexOf(xprime, x[i]);
            }
            return perm;
        }

        public void CalculateJobsTimes(double[] continousJobs)
        {
            
        }
        public void Run()
        {
            double[][] employedBees = new double[this.EmployedNumber][];
            double[] employedFitness = new double[this.EmployedNumber];
            int[] employedLimits = new int[this.EmployedNumber];
            double[] stdpermutation = new double[this.JobsNumber];
            for (int i = 0; i < this.JobsNumber; i++)
            {
                stdpermutation[i] = i;
            }
            this.Interpolations = new Interpolation[this.MachinesNumber];
            for (int i = 0; i < this.MachinesNumber; i++)
            {
                this.Interpolations[i] = new Interpolation(stdpermutation,this.JobsTimes[i],this.JobsNumber-1);
            }
            Random random = new Random();
            double[] newBee = new double[this.JobsNumber];
            for (int i = 0; i < this.EmployedNumber; i++)
            {
                employedBees[i] = new double[this.JobsNumber];
                Array.Copy(this.InitialSolution(Heuristics.Heuristic.NehRandom),employedBees[i],this.JobsNumber);
                employedFitness[i] = Fitness(this.LRV_rule(this.JobsNumber,employedBees[i]));
                employedLimits[i] = 1;
            }

            for (int i = 0; i < this.Iterations; i++)
            {
                // Employed Bees Phase
                for (int j = 0; j < this.EmployedNumber; j++)
                {
                    double change = -1 + 2 * random.NextDouble();
                    for (int k = 0; k < this.JobsNumber; k++)
                    {
                        newBee[k] = employedBees[j][k] + change * (this.JobsNumber - 1);
                        if (newBee[k] > this.JobsNumber -1)
                        {
                            newBee[k] -= this.JobsNumber - 1;
                        }
                    }
                    double fitnessNewBee = FitnessPosition(newBee);
                    if (fitnessNewBee < employedFitness[j])
                    {
                        employedFitness[j] = fitnessNewBee;
                        Array.Copy(newBee,employedBees[j],this.JobsNumber);
                    }
                    else
                    {
                        employedLimits[j]++;
                    }
                }
                
                // Scout Bees Phase
                for (int j = 0; j < this.ScoutNumber; j++)
                {
                    int index = random.Next() % this.EmployedNumber;
                    LocalSearch(employedBees[j]);
                    // if the new solution is better : Update the employed bee
                }
                // Onlooker Bees Phase
                for (int j = 0; j < this.EmployedNumber; j++)
                {
                    if (employedLimits[j]> this.Limit)
                    {
                        Array.Copy(InitialSolution(Heuristics.Heuristic.NehRandom),employedBees[j],this.JobsNumber);
                        employedLimits[j] = 1;
                    }
                }
            }
            Array.Sort(employedFitness,employedBees);
            this.BestFitness = employedFitness[0];
            Array.Copy(employedBees[0],this.BestIndividual,this.JobsNumber);
        }
    }
}