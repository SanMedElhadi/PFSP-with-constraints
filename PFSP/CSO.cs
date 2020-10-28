using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console.PFSP
{
    class Cso // Cat Swarm Optimisation
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int Iterations;
        public int CatsNumber;
        public int Smp;
        public int Srd;
        public int Cdc;
        public double IntertiaWeight;
        public double ConstantBest;
        public int[] BestIndividual;
        public double[] BestPosition;
        public double BestFitness;

        public Cso(int machinesNumber, int jobsNumber, double[][] jobsTimes, int iterations, int catsNumber, int smp, int srd, int cdc, double constantBest, double[] bestPosition = null, int[] bestIndividual = null)
        {
          this.MachinesNumber = machinesNumber;
          JobsNumber = jobsNumber;
          this.Iterations = iterations;
          CatsNumber = catsNumber;
          Smp = smp;
          Srd = srd;
          Cdc = cdc;
          ConstantBest = constantBest;
          BestPosition = bestPosition;
          BestIndividual = bestIndividual;
          Array.Copy(jobsTimes,this.JobsTimes,jobsNumber);
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
        public int[] InitialSolution(Heuristics.Heuristic name){
            Heuristics h = new Heuristics();
            h.Name = name;
            return h.Execute(this.JobsNumber,this.MachinesNumber,this.JobsTimes);
        }
        public double[] PermutationToPosition(int[] permutation){
          double[] pos = new double[this.JobsNumber];
          return pos;
        }
        public int[] PositionToPermutation(double[] position){
          int[] perm = new int[this.JobsNumber];
          return perm;
        }
        public double[] RepairVelocity(double[] velocity){
          double[] v = new double[this.JobsNumber];
          return v;
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
        public double[] RepairPosition(double[] position){
          double[] pos = new double[this.JobsNumber];
          return pos;
        }
        public void MutationOperator(){

        }
        public void Run(){
          double[][] cats = new double[this.CatsNumber][];
          double[][] velocities =new double[this.CatsNumber][];
          bool[] spc = new bool[this.CatsNumber];

          // Initialisation of the population
          
          //Process of search and optimising
          for (int iter = 1; iter<this.Iterations+1; iter++) {
            for (int i = 1; i<this.CatsNumber+1; i++) {
              if (spc[i-1]) {
                // Seeking Mode : Local Optimisation
                double[][] copies = new double[this.Smp][];
                double[] copiesFitness = new double[this.Smp];
                double[] copiesProbability = new double[this.Smp];
                for (int j=1; j<this.Smp+1; j++)
                {
                  copies[j - 1] = new double[this.JobsNumber];
                  for (int k=1; k<this.JobsNumber+1; k++) {
                    copies[j-1][k-1] = cats[i-1][k-1];
                  }
                }
                // Build the mutation operator
                MutationOperator();
                for (int j=1; j<this.Smp+1; j++) {
                  copiesFitness[j-1] = Fitness(PositionToPermutation(copies[j-1]));
                }
                double min = Double.MaxValue;
                double max = Double.MinValue;
                for (int j = 0; j < this.Smp; j++)
                {
                  if (min > copiesFitness[j])
                  {
                    min = copiesFitness[j];
                  }
                  if (max < copiesFitness[j])
                  {
                    max = copiesFitness[j];
                  }
                }
                for (int j=1; j<this.Smp+1; j++) {
                  copiesProbability[j-1] =Math.Abs(copiesFitness[j-1]-min)/(max-min);
                }
                double probMax = Double.MinValue;
                for (int j = 0; j < this.Smp; j++)
                {
                  if (probMax < copiesProbability[j])
                  {
                    probMax = copiesProbability[j];
                  }
                }
                int index = Array.IndexOf(copiesProbability, probMax);
                Array.Copy(copies[index],cats[i],this.JobsNumber);
                if (copiesFitness[index]<this.BestFitness) {
                  this.BestFitness = copiesFitness[index];
                  Array.Copy(copies[index],this.BestPosition,this.JobsNumber);
                }
              }else{
                //Tracking Mode : Global Optimisation
                for (int j = 1; j<this.CatsNumber+1; j++) {
                  Random r = new Random();
                  double[] copyVelocity = new double[this.JobsNumber];
                  // Update of the velocity
                  for (int k=1; k<this.JobsNumber+1; k++) {
                    copyVelocity[k-1] = velocities[j-1][k-1];
                    velocities[j-1][k-1] = this.IntertiaWeight*velocities[j-1][k-1] + this.ConstantBest*r.NextDouble()*(this.BestPosition[k-1]-cats[j-1][k-1]);
                  }
                  Array.Copy(RepairVelocity(velocities[j-1]),velocities[j-1],this.JobsNumber);
                  // Update of the intertia_weight
                  this.IntertiaWeight = (velocities[j-1][0]-copyVelocity[0])*(velocities[j-1][0]-copyVelocity[0]);
                  for (int k=1; k<this.JobsNumber; k++)
                  {
                    this.IntertiaWeight += (velocities[j - 1][k] - copyVelocity[k]) *
                                           (velocities[j - 1][k] - copyVelocity[k]);
                  }
                  this.IntertiaWeight = Math.Sqrt(this.IntertiaWeight);
                  // Update of the cats
                  for (int k=1; k<this.JobsNumber+1; k++) {
                    cats[j-1][k-1] = cats[j-1][k-1] + velocities[j-1][k-1];
                  }
                  Array.Copy(RepairPosition(cats[j]),cats[j],this.JobsNumber);
                }
              }
            }
            //Introduce a limit factor to change the worst cats positions with new one
          }
        }
    }
}
