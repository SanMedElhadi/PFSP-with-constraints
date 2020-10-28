using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSP 
{
    class Pso
    {
        // Intervals for the population
        public int Xmax;
        public int Xmin;
        // Interval for the velocity 
        public int Vmax;
        public int Vmin;
        // Interval for the personal constant
        public double C1Min;
        public double C1Max;
        // Interval for the global constant
        public double C2Min;
        public double C2Max;
        // Interval for the inertia weight
        public double Wmin;
        public double Wmax;
        // Problem Parameters
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        // Results of the executions
        public int[] BestIndividual;
        public double[] BestPosition;
        public double BestFitness;
        //Number of iteratiobs
        public int Iterations;
        //the population size
        public int PopSize;
        // Ratio of randomized population
        public double RatioRandom;
        // Parameters for the update of velocity
        public double InertiaWeight;
        public double ConstantPersonal;
        public double ConstantGlobal;
        // Introduce the variation of the constants of velocity
        public bool ConstantsVariant;
        public bool WeightsVarinat;
        // Introduce the chaotic methods of generation of population
        public double ChaoticRatio;
        public bool ChaoticVariant;
        // Methods of conversion Methods from position vectors to a permutation of a problem
        public enum ConversionMethode
        {
            Lvo,
            Svo,
            Lvr,
            Svr
        }

        public int[] Initial_Solution_Heuristics(Heuristics.Heuristic name,int nbJobs,int nbMachines,double[][] processingTimes)
        {
            Heuristics h = new Heuristics();
            int[] perm = new int[nbJobs];
            h.Name = name;
            bool pure = false;
            if (name == Heuristics.Heuristic.HeuristicMinFirstLast)
            {
                pure = false;
            }
            Array.Copy(h.Execute(nbJobs, nbMachines, processingTimes,null, pure),perm,nbJobs);
            return perm;
        }
        public double[] PositionsGenrations(int nbJobs)
        {
            double[] x = new double[nbJobs];
            Random rand = new Random();
            for (int i = 0; i < nbJobs; i++)
            {
                x[i] = this.Xmin + (this.Xmax - this.Xmin) * rand.NextDouble();
            }
            return x;
        }
        public double[] PositionChaoticGeneration(int nbJobs)
        {
            double[] x = new double[nbJobs];
            double[] notAllowedValues = new double[] { 0.0, 0.25, 0.5, 0.75 };
            double phi = 4;
            Random rand = new Random();
            double y;
            do
            {
                y = rand.NextDouble();
            } while (Array.IndexOf(notAllowedValues,y) != -1);
            for (int i = 0; i < nbJobs; i++)
            {
                x[i] = this.Xmin + (this.Xmax - this.Xmin) * y;
                y = phi * y * (1 - y);
            }
            return x;
        }
        public double[] VelocityGeneration(int nbJobs)
        {
            double[] v = new double[nbJobs];
            Random rand = new Random();
            for (int i = 0; i < nbJobs; i++)
            {
                v[i] = this.Vmin + (this.Vmax - this.Vmin) * rand.NextDouble();
            }
            return v;
        }
        public double[] RepairVelocity(int nbJobs,double[] x)
        {
            Random rand = new Random();
            double[] xr = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                if (x[i]>this.Xmax)
                {
                    if (this.Xmax >=0)
                    {
                        xr[i] =x[i] -  rand.NextDouble() * this.Xmax;
                    }
                    else
                    {
                        xr[i] = x[i]+rand.NextDouble() * this.Xmax;
                    }
                }
                else if (x[i]<this.Xmin)
                {
                    if (this.Xmin >= 0)
                    {
                        xr[i] = x[i]+rand.NextDouble() * this.Xmax;
                    }
                    else
                    {
                        xr[i] = x[i]-rand.NextDouble() * this.Xmax;
                    }
                }   
            }
            return xr;
        }
        public double[] ConvertPermutationToPositionVector(int nbJobs,int[] permutation)
        {
            double[] x = new double[nbJobs];
            Random rand = new Random();
            for (int i = 0; i < nbJobs; i++)
            {
                //X[i] = this.xmin + ((this.xmax - this.xmin) / nb_jobs) * (nb_jobs - permutation[i] - 1 + rand.NextDouble());
                x[i] = this.Xmin + ((this.Xmax - this.Xmin) / (double) nbJobs) * ((double)nbJobs - (double)Array.IndexOf(permutation,i) - 1.0 + rand.NextDouble());
            }
            return x;
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
            Array.Sort(xprime);
            Array.Reverse(xprime);
            for (int i = 0; i < nbJobs; i++)
            {
                perm[i] = Array.IndexOf(xprime, x[i]);
            }
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

        public int[] ConvertPositionVectorToPermutation(int nbJobs,double[] x,ConversionMethode methode)
        {
            switch (methode)
            {
                case ConversionMethode.Lvo:
                    return LOV_rule(nbJobs, x);
                case ConversionMethode.Svo:
                    return SOV_rule(nbJobs, x);
                case ConversionMethode.Lvr:
                    return LRV_rule(nbJobs, x);
                case ConversionMethode.Svr:
                    return SRV_rule(nbJobs, x);
                default:
                    break;
            }
            return null;
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
        public double Fitness(int[] perm)
        {
            return basic_makespan_dynamic(this.JobsNumber, this.MachinesNumber, perm, this.JobsTimes);
        }

        

        public int[] LocalSearch(int[] perm,Neighborhood.Structure structure)
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
                    default:
                        break;
                }
                if (Fitness(res)<Fitness(opt))
                {
                    Array.Copy(res,opt,this.JobsNumber);
                }
            }
            return opt;
        }
        public int[] Deepest_Descent(int[] permutation, Neighborhood.Structure structure)
            // Finding Local minimal in simple neighborhood structures
        {
            int[] perm = new int[this.JobsNumber];
            int[] neighPerm = new int[this.JobsNumber];
            double neighFitness;
            int[] bestPerm = new int[this.JobsNumber];
            double bestFitness = Fitness(permutation);
            Neighborhood n = new Neighborhood();
            switch (structure)
            {
                case Neighborhood.Structure.RandomInsert:
                    for (int i = 0; i < this.JobsNumber; i++)
                    {
                        for (int j = 0; j < this.JobsNumber; j++)
                        {
                            if (j !=i)
                            {
                                Array.Copy(n.Insert(perm, this.JobsNumber, i, j),neighPerm,this.JobsNumber);
                                neighFitness = Fitness(neighPerm);
                                if (neighFitness < bestFitness)
                                {
                                    bestFitness = neighFitness;
                                    Array.Copy(neighPerm,bestPerm,this.JobsNumber);
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
                                neighFitness = Fitness(neighPerm);
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
                            neighFitness = Fitness(neighPerm);
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
            double[][] population = new double[this.PopSize][];
            double[] bestEvaluation = new double[PopSize];
            double[][] bestPersonal = new double[this.PopSize][];
            double[][] velocities = new double[this.PopSize][];
            this.BestPosition = new double[this.JobsNumber];
            this.BestIndividual = new int[this.JobsNumber];
            ConversionMethode methode = ConversionMethode.Svr;

            double evaluation;
            int i;
            this.BestFitness = Double.PositiveInfinity;
            for (i = 0; i < this.RatioRandom*this.PopSize; i++)
            {
                population[i] = new double[this.JobsNumber];
                Array.Copy(PositionsGenrations(this.JobsNumber), population[i],this.JobsNumber);
                bestPersonal[i] = new double[this.JobsNumber];
                Array.Copy(population[i], bestPersonal[i], this.JobsNumber);
                bestEvaluation[i] = Fitness(ConvertPositionVectorToPermutation(this.JobsNumber, population[i], methode));
                if (bestEvaluation[i] < this.BestFitness)
                {
                    this.BestFitness = bestEvaluation[i];
                    Array.Copy(population[i], this.BestPosition, this.JobsNumber);
                }
                velocities[i] = new double[this.JobsNumber];
                Array.Copy(VelocityGeneration(this.JobsNumber), velocities[i],this.JobsNumber);
            }
            for (; i < (this.RatioRandom+this.ChaoticRatio)*this.PopSize; i++)
            {
                population[i] = new double[this.JobsNumber];
                Array.Copy(PositionChaoticGeneration(this.JobsNumber), population[i], this.JobsNumber);
                bestPersonal[i] = new double[this.JobsNumber];
                Array.Copy(population[i], bestPersonal[i], this.JobsNumber);
                bestEvaluation[i] = Fitness(ConvertPositionVectorToPermutation(this.JobsNumber, population[i], methode));
                if (bestEvaluation[i] < this.BestFitness)
                {
                    this.BestFitness = bestEvaluation[i];
                    Array.Copy(population[i], this.BestPosition, this.JobsNumber);
                }
                velocities[i] = new double[this.JobsNumber];
                Array.Copy(VelocityGeneration(this.JobsNumber), velocities[i], this.JobsNumber);
            }
            for (int j = i; j < this.PopSize; j++)
            {
                population[j] = new double[this.JobsNumber];
                int[] perm = new int[this.JobsNumber];
                Array.Copy(Initial_Solution_Heuristics(Heuristics.Heuristic.NehRandom, this.JobsNumber, this.MachinesNumber, this.JobsTimes),perm,this.JobsNumber);
                Array.Copy(ConvertPermutationToPositionVector(this.JobsNumber,perm), population[j], this.JobsNumber);
                bestPersonal[j] = new double[this.JobsNumber];
                Array.Copy(population[j], bestPersonal[j], this.JobsNumber);
                bestEvaluation[j] = Fitness(ConvertPositionVectorToPermutation(this.JobsNumber, population[j], methode));
                if (bestEvaluation[j] < this.BestFitness)
                {
                    this.BestFitness = bestEvaluation[j];
                    Array.Copy(population[j], this.BestPosition, this.JobsNumber);
                }
                velocities[j] = new double[this.JobsNumber];
                Array.Copy(VelocityGeneration(this.JobsNumber), velocities[j], this.JobsNumber);
            }
            

            for (int k = 0; k < this.Iterations; k++)
            {
                for (int j = 0; j < this.PopSize; j++)
                {
                    for (int l = 0; l < this.JobsNumber; l++)
                    {
                        population[j][l] = population[j][l] + velocities[j][l];
                    }
                    RepairVelocity(this.JobsNumber, population[j]);
                    evaluation = Fitness(ConvertPositionVectorToPermutation(this.JobsNumber,population[j], methode));
                    if (evaluation < bestEvaluation[j])
                    {
                        bestEvaluation[j] = evaluation;
                        Array.Copy(population[j], bestPersonal[j],this.JobsNumber);
                        if (bestEvaluation[j] < this.BestFitness)
                        {
                            this.BestFitness = bestEvaluation[j];
                            Array.Copy(population[j], this.BestPosition,this.JobsNumber);
                        }
                    }   
                }
                Random rand = new Random();
                if (this.ConstantsVariant)
                {
                    this.ConstantPersonal += (this.C1Max - this.C1Min) / (double)this.Iterations;
                    this.ConstantGlobal += (this.C2Max - this.C2Min) / (double)this.Iterations;
                }
                if (this.WeightsVarinat)
                {
                    this.InertiaWeight += (this.Wmin - this.Wmax) / (double)this.Iterations;
                }
                for (int j = 0; j < this.PopSize; j++)
                {
                    for (int l = 0; l < this.JobsNumber; l++)
                    {
                        
                        velocities[j][l] = this.InertiaWeight * velocities[j][l] + this.ConstantPersonal * rand.NextDouble() * (bestPersonal[j][l] - population[j][l]) + this.ConstantGlobal * rand.NextDouble() * (this.BestPosition[l] - population[j][l]);
                    }
                }
            }
            Array.Copy(ConvertPositionVectorToPermutation(this.JobsNumber,this.BestPosition, methode), this.BestIndividual,this.JobsNumber);
        }
    }
}
