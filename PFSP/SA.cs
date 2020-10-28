using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSP 
{
    class Sa
    {
        public double Temperature { get; set; }
        public int Iterations { get; set; }
        public double Lambda;
        public double[][] JobsTimes { get; set; }
        public double BestFitness;
        public int[] BestIndividual;
        public int NbJobs; 
        public int NbMachines;
        public bool RepairEnabled { get; set; }
        public bool LocalSearchEnabled { get; set; }

        public int[] InitialSolution()
        {
            Heuristics h = new Heuristics();
            h.Name = Heuristics.Heuristic.Neh;
            return h.Execute(this.NbJobs,this.NbMachines,this.JobsTimes,null,false) ;
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
            return basic_makespan_dynamic(this.NbJobs,this.NbMachines,perm,this.JobsTimes);
        }
        public void Run()
        {
            double T = this.Temperature;
            int[] init = new int[NbJobs];
            Array.Copy(this.InitialSolution(),init,this.NbJobs);
            for (int i = 0; i < this.NbJobs; i++)
            {
                Console.WriteLine("init : " + init[i]);
            }
            double fitInit = this.Fitness(init);
            int[] individual = new int[this.NbJobs];
            double fitIndividual;
            double probability;



            Neighborhood n = new Neighborhood();
            

            this.BestIndividual = new int[this.NbJobs];
            Array.Copy(init,this.BestIndividual,this.NbJobs);
            this.BestFitness = fitInit;
            for (int i = 0; i < this.Iterations; i++)
            {
                Random random = new Random();
                
                //Array.Copy(N.Adjacent_Swap(init, nb_jobs, 3), individual, this.nb_jobs);
                //Array.Copy(N.Random_Insert(init, nb_jobs, 3), individual, this.nb_jobs);
                Array.Copy(n.Random_Swap(init, NbJobs, 3), individual, this.NbJobs); // Is optimal than other neighborhoods
                fitIndividual = Fitness(individual);
                probability = Math.Min(1,Math.Exp((fitInit-fitIndividual)/T));
                // Method 1 : 
                /*if (fitIndividual < this.BestFitness)
                {
                    Array.Copy(individual, this.BestIndividual, this.nb_jobs);
                    this.BestFitness = fitInit;
                }
                else
                {
                    if (probability > random.NextDouble())
                    {
                        Array.Copy(individual, init, this.nb_jobs);
                        fitInit = fitIndividual;
                    }
                }*/
                // Method 2
                
                if (probability>random.NextDouble())
                {
                    Array.Copy(individual, init, this.NbJobs);
                    fitInit = fitIndividual;
                }
                if (fitInit < this.BestFitness)
                {
                    Array.Copy(init, this.BestIndividual, this.NbJobs);
                    this.BestFitness = fitInit;
                }
                //T = T / (1 + lambda * T);
                //T = lambda * T;
                //T = T / Math.Log(1+lambda*i);
                //T = T / Math.Log(1+lambda);
                //T = T / i;
                //T = this.Temperature / i;
                T = this.Temperature * Math.Exp(- Lambda * i / this.NbJobs);
            }
        }
    }
}
