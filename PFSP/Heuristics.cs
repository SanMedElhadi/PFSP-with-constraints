using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PFSP_MLD_Console.PFSP 
{
    public class Heuristics
    {
        /**
         * This class groups a set of heuristics used in our methods
         * This heuristics are found during the phase of the state of art about the available methods in the articles we have found
        **/
        public Heuristic Name { get; set; }
        public enum Heuristic
        {
            NehRandom,
            Neh,
            NehInit,
            HeuristicCombinationFirstLast,
            HeuristicCombinationFirstLast1,
            HeuristicMinFirstLast,
            HeuristicDiffernceFirstLasts
        }
        public Heuristics(Heuristic h)
        {
            this.Name = h;
        }
        public Heuristics()
        {

        }
        public int[] Execute(int nbJobs, int nbMachines, double[][] operTimes,int[] init = null,bool pure = false) // Executes the heuristic with the name stored in 'Name'
        {
            switch (this.Name)
            {
                case Heuristic.NehRandom:
                    return this.NEH_random(nbJobs, nbMachines, operTimes,pure);
                    
                case Heuristic.Neh:
                    return this.Neh(nbJobs, nbMachines, operTimes);
                    
                case Heuristic.NehInit:
                    return this.NEHWithInit(nbJobs, nbMachines, operTimes,init);

                case Heuristic.HeuristicCombinationFirstLast:
                    return this.Heuristic_Combination_First_Last(nbJobs, nbMachines, operTimes);
                    
                case Heuristic.HeuristicCombinationFirstLast1:
                    return this.Heuristic_Combination_First_Last1(nbJobs, nbMachines, operTimes);
                    
                case Heuristic.HeuristicMinFirstLast:
                    return this.Heuristic_Min_First_Last(nbJobs, nbMachines, operTimes);
                    
                case Heuristic.HeuristicDiffernceFirstLasts:
                    this.Heuristic_Differnce_First_Last(nbJobs, nbMachines, operTimes);
                    break;
                default:
                    return null;
                    
            }
            return null;
        }

        
        public int[] Neh(int nbJobs,int nbMachines,double[][] operTimes)
            /* The decreasing order of the sum of completion time if a machine is supposed to executes all the jobs and then passed them to another machine
             * In other terms, the order if all the jobs are operations of one job
             */
        {
            double[] sumJobAllMachines = new double[nbJobs];
            double[] tmp = new double[nbJobs];
            int[] p = new int[nbJobs];
            int[] key = new int[nbJobs];
            int job = -1;
            int index = -1;
            for (int i = 0; i < nbJobs; i++)
            {
                sumJobAllMachines[i] = 0.0;
                for (int j = 0; j < nbMachines; j++)
                {
                    sumJobAllMachines[i] +=operTimes[j][i];
                }
                tmp[i] = sumJobAllMachines[i];
                key[i] = i;
            }

            Array.Sort(sumJobAllMachines, key);
            Array.Reverse(key);
            p[0] = key[0];
            for (int i = 1; i < nbJobs; i++)
            {
                index = min_partial_makespan_NEH(operTimes,p, i, nbMachines, key[i]);
                for (int j = i; j >index; j--)
                {
                    p[j] = p[j - 1];
                }
                p[index] = key[i];
                
            }
            
            return p;
         }
        public int[] NEH_random(int nbJobs, int nbMachines, double[][] operTimes,bool pure)
        {
            int[] init = new int[nbJobs];
            int[] p = new int[nbJobs];
            init = RandomPermutation(nbJobs, pure);
            int index = -1;
            p[0] = init[0];
            for (int i = 1; i < nbJobs; i++)
            {
                index = min_partial_makespan_NEH(operTimes, p, i, nbMachines, init[i]);
                for (int j = i; j > index; j--)
                {
                    p[j] = p[j - 1];
                }
                p[index] = init[i];
            }
            return p;
        }

        public int[] NEHWithInit(int nbJobs, int nbMachines, double[][] operTimes, int[] init)
        {
            int[] initP = new int[nbJobs];
            int[] p = new int[nbJobs];
            Array.Copy(init,initP,nbJobs);
            int index = -1;
            p[0] = init[0];
            for (int i = 1; i < nbJobs; i++)
            {
                index = min_partial_makespan_NEH(operTimes, p, i, nbMachines, init[i]);
                for (int j = i; j > index; j--)
                {
                    p[j] = p[j - 1];
                }
                p[index] = init[i];
            }
            return p;
        }
        public void Delete_element(int[] a,int value,int length)
            //Delete an integer element from a table a contains distinct values
        {
            int pos = -1;
            for (int i = 0; i < length; i++)
            {
                if (a[i]==value)
                {
                    pos = i;
                }
                if (pos !=-1 && i>pos)
                {
                    a[i - 1] = a[i];
                }
            }
        }
        public void Delete_index(int[] a,int index,int length)
        {
            for (int i = index; i < length-1; i++)
            {
                a[i] = a[i + 1];
            }
        }

        public int[] Heuristic_Combination_First_Last(int nbJobs, int nbMachines, double[][] operTimes)
        /* Taking the remark that the first and the last job in some sequence solutions provided in the taillard benchmarcks 
         * that the first and the last jobs are minimizing the sum over the first resp. the last Nb_machines-1 machines of the processing time
         * of these jobs, we had the idea that we use that remark in the follwing way : 
         * The job i optimise the following : (N-i)*Sum_over_the_first_m-1_machines + i* Sum_over_the_last_m-1_machines
         */
        {
            double[] sumM1First = new double[nbJobs];
            double[] sumM1Last = new double[nbJobs];
            double[] tmpSort ;
            double[] tmpFind ;
            int[] left = new int[nbJobs];
            int[] p = new int[nbJobs];
            int job = -1;
            int index = -1;
            for (int i = 0; i < nbJobs; i++)
            {
                left[i] = i;
            }
            for (int k = 0; k < nbJobs; k++)
            {
                tmpSort = new double[nbJobs - k];
                tmpFind = new double[nbJobs - k];
                for (int i = 0; i < nbJobs-k; i++)
                {
                    sumM1First[i] = 0.0;
                    sumM1Last[i] = 0.0;
                    for (int j = 0; j < nbMachines; j++)
                    {
                        if (j < nbMachines - 1)
                        {
                            sumM1First[i] += operTimes[j][left[i]];
                        }
                        if (j > 0)
                        {
                            sumM1Last[i] += operTimes[j][left[i]];
                        }
                    }
                    tmpSort[i] = (nbJobs - k) * sumM1First[i] + k * sumM1Last[i];
                    tmpFind[i] = tmpSort[i];
                }
                Array.Sort(tmpSort);
                p[k] = left[Array.IndexOf(tmpFind, tmpSort[0])];
                Delete_element(left, p[k], nbJobs - k);
            }
            return p;
        }

        public int[] Heuristic_Combination_First_Last1(int nbJobs, int nbMachines, double[][] operTimes)
        /* Taking the remark that the first and the last job in some sequence solutions provided in the taillard benchmarcks 
         * that the first and the last jobs are minimizing the sum over the first resp. the last Nb_machines-1 machines of the processing time
         * of these jobs, we had the idea that we use that remark in the follwing way : 
         * The job i optimise the following : (N-i)*Sum_over_the_first_m-1_machines + i* Sum_over_the_last_m-1_machines
         */
        {
            double[] sumM1First = new double[nbJobs];
            double[] sumM1Last = new double[nbJobs];
            double[] tmpSort;
            double[] tmpFind;
            int[] left = new int[nbJobs];
            int[] p = new int[nbJobs];
            int job = -1;
            int index = -1;
            for (int i = 0; i < nbJobs; i++)
            {
                left[i] = i;
            }
            for (int i = 0; i < nbJobs; i++)
            {
                sumM1First[i] = 0.0;
                sumM1Last[i] = 0.0;
                for (int j = 0; j < nbMachines; j++)
                {
                    if (j < nbMachines - 1)
                    {
                        sumM1First[i] += operTimes[j][left[i]];
                    }
                    if (j > 0)
                    {
                        sumM1Last[i] += operTimes[j][left[i]];
                    }
                }
            }
            for (int k = 0; k < nbJobs; k++)
            {

                tmpSort = new double[nbJobs - k];
                tmpFind = new double[nbJobs - k];
                for (int i = 0; i < nbJobs-k; i++)
                {
                    tmpSort[i] = (nbJobs - k) * sumM1First[i] + k * sumM1Last[i];
                    tmpFind[i] = tmpSort[i];
                }
                Array.Sort(tmpSort);
                p[k] = left[Array.IndexOf(tmpFind, tmpSort[0])];
                Delete_element(left, p[k], nbJobs - k);
            }
            return p;
        }

        public int[] Heuristic_Min_First_Last(int nbJobs, int nbMachines, double[][] operTimes)
        {
            int[] pFirst = new int[nbJobs];
            int[] pLast = new int[nbJobs];
            double[] sumM1First = new double[nbJobs];
            double[] findM1First = new double[nbJobs];
            double[] sumM1Last = new double[nbJobs];
            double[] findM1Last = new double[nbJobs];
            double cmax;
            for (int i = 0; i < nbJobs; i++)
            {
                sumM1First[i] = 0.0;
                sumM1Last[i] = 0.0;
                for (int j = 0; j < nbMachines; j++)
                {
                    if (j < nbMachines - 1)
                    {
                        sumM1First[i] += operTimes[j][i]; 
                    }
                    if (j > 0)
                    {
                        sumM1Last[i] += operTimes[j][i];
                    }
                }
                findM1First[i] = sumM1First[i];
                findM1Last[i] = sumM1Last[i];
            }
            Array.Sort(sumM1First);
            Array.Sort(sumM1Last);
            for (int i = 0; i < nbJobs; i++)
            {
                pFirst[i] = Array.IndexOf(findM1First,sumM1First[i]);
            }
            for (int i = 0; i < nbJobs; i++)
            {
                pLast[i] = Array.IndexOf(findM1Last, sumM1Last[nbJobs-1- i]);
            }
            // Mutation between p_first*p_last

            if (basic_makespan_dynamic(nbJobs,nbMachines,pFirst,operTimes)> basic_makespan_dynamic(nbJobs, nbMachines, pLast, operTimes))
            {
                return pLast;
            }
            else
            {
                return pFirst;
            }
            
        }

        public int[] Heuristic_Differnce_First_Last(int nbJobs, int nbMachines, double[][] operTimes)
        {
            int[] p = new int[nbJobs];
            int[] left = new int[nbJobs];
            int loopEnd = nbJobs - 1;
            double min;
            int length;
            int value;
            double[] sumM1First = new double[nbJobs];
            double[] findM1First = new double[nbJobs];
            double[] sumM1Last = new double[nbJobs];
            double[] findM1Last = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                left[i] = i;
                sumM1First[i] = 0.0;
                sumM1Last[i] = 0.0;
                for (int j = 0; j < nbMachines; j++)
                {
                    if (j < nbMachines - 1)
                    {
                        sumM1First[i] += operTimes[j][i];
                    }
                    if (j > 0)
                    {
                        sumM1Last[i] += operTimes[j][i];
                    }
                }
                findM1First[i] = sumM1First[i];
                findM1Last[i] = sumM1Last[i];
            }
            Array.Sort(sumM1First);
            Array.Sort(sumM1Last);
            length = nbJobs;
            p[0] = Array.IndexOf(findM1First, sumM1First[0]);
            Delete_element(left, p[0], length);
            length--;
            p[nbJobs - 1] = Array.IndexOf(findM1Last, sumM1Last[nbJobs - 1]);
            if (p[nbJobs-1] != p[0])
            {
                loopEnd = nbJobs - 1;
                Delete_element(left, p[nbJobs-1], length);
                length--;
            }
            else
            {
                loopEnd = nbJobs;
            }
            for (int i = 1; i < loopEnd; i++)
            {
                min = Double.PositiveInfinity;
                value = -1;
                foreach (var j in left)
                {
                    if (min > Math.Abs(findM1First[j] - findM1Last[p[i - 1]]) /*+ oper_times[nb_machines-1][j]*/)
                    {
                        min = Math.Abs(findM1First[j] - findM1Last[p[i - 1]]) /*+ oper_times[nb_machines - 1][j]*/;
                        value = j;
                    }
                }
                p[i] = value;
                Delete_element(left, value, length);
                length--;
            }
            
            return p;
        }
        
        public int min_partial_makespan_NEH(double[][] seqTime,int[] perm, int lengthPartial,int nbMachines,int position)
            // find the position of insertion of a new job to an existing permutation that minimise the total makespan 
        {
            double minPartial = Double.PositiveInfinity;
            int[] partialPermutation = new int[lengthPartial+1];
            Array.Copy(perm,partialPermutation,lengthPartial+1);
            int pos = -1;
            double cpartial;
            for (int i = 0; i < lengthPartial+1; i++)
            {
                Array.Copy(perm,partialPermutation,lengthPartial+1);
                for (int j = lengthPartial-1; j > i; j--)
                {
                    partialPermutation[j] = partialPermutation[j - 1];
                }
                partialPermutation[i] = position;
                cpartial = basic_makespan_dynamic(lengthPartial + 1, nbMachines, partialPermutation, seqTime);
                if (minPartial > cpartial)
                {
                    minPartial = cpartial;
                    pos = i;
                }
            }
            return pos;
        }

        public int[] RandomPermutation(int nbJobs,bool pure)
        {
            int[] permutation = new int[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                permutation[i] = i;
            }
            Random r = new Random();
            int nbMut = (r.Next() % nbJobs);
            int pos1, pos2;
            for (int i = 0; i < nbMut; i++)
            {
                if (pure)
                {
                    do
                    {
                        pos1 = (int)Math.Floor((nbJobs) * r.NextDouble());
                        pos2 = (int)Math.Floor((nbJobs) * r.NextDouble());
                    } while (pos1 == pos2);// for pure nb_mut permutations
                }
                else
                {
                    pos1 = (int)Math.Floor((nbJobs) * r.NextDouble());
                    pos2 = (int)Math.Floor((nbJobs) * r.NextDouble());
                }// for pure randomised permutation

                if (pos1!=pos2)
                {
                    permutation[pos1] = permutation[pos1] + permutation[pos2];// a=a+b
                    permutation[pos2] = permutation[pos1] - permutation[pos2];// b=a-b-->b=a(old)
                    permutation[pos1] = permutation[pos1] - permutation[pos2];//a=a-b--->a = b(old)
                } 
                
            }
            return permutation;
        }

        public int[] SPT_orderPermutation(int nbJobs,int nbMachines,double[][] processingTimes,int indexMachine)
        {
            int[] permutation = new int[nbJobs];
            double[] times = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                times[i] = processingTimes[indexMachine][i];
            }
            Array.Sort(times);
            for (int i = 0; i < nbJobs; i++)
            {
                permutation[Array.IndexOf(times, processingTimes[indexMachine][i])] = i;
            }
            return permutation;
        }
        public int[] WSPT_orderPermutation(int nbJobs, int nbMachines, double[][] processingTimes, int indexMachine,double[] weights)
        {
            int[] permutation = new int[nbJobs];
            double[] times = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                times[i] = processingTimes[indexMachine][i]/weights[i];
            }
            Array.Sort(times);
            for (int i = 0; i < nbJobs; i++)
            {
                permutation[Array.IndexOf(times, processingTimes[indexMachine][i]/weights[i])] = i;
            }
            return permutation;
        }
        public int[] LPT_orderPermutation(int nbJobs, int nbMachines, double[][] processingTimes, int indexMachine)
        {
            int[] permutation = new int[nbJobs];
            double[] times = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                times[i] = processingTimes[indexMachine][i];
            }
            Array.Sort(times);
            Array.Reverse(times);
            for (int i = 0; i < nbJobs; i++)
            {
                permutation[Array.IndexOf(times, processingTimes[indexMachine][i])] = i;
            }
            return permutation;
        }
        public int[] WLPT_orderPermutation(int nbJobs, int nbMachines, double[][] processingTimes, int indexMachine,double[] weights)
        {
            int[] permutation = new int[nbJobs];
            double[] times = new double[nbJobs];
            for (int i = 0; i < nbJobs; i++)
            {
                times[i] = processingTimes[indexMachine][i]/weights[i];
            }
            Array.Sort(times);
            Array.Reverse(times);
            for (int i = 0; i < nbJobs; i++)
            {
                permutation[Array.IndexOf(times, processingTimes[indexMachine][i]/weights[i])] = i;
            }
            return permutation;
        }

        /*public int[] GGH(int nb_jobs, int nb_machines, double[][] processing_times)
        {
            int[][] permutations = new int[nb_jobs][];
            

        }*/
        public int[] DestructionConstruction(int[] permutation, int d,int nbJobs,int nbMachines,double[][] operTimes)
        {
            int[] perm = new int[nbJobs];
            int[] posToInsert = new int[d];
            int position;
            Random random = new Random();
            Array.Copy(perm,permutation,nbJobs);
            for (int i = 0; i < d; i++)
            {
                position = random.Next() % (nbJobs - 1);
                posToInsert[i] = perm[position];
                Delete_index(perm,position,nbJobs-i);
            }
            for (int i = 0; i < d; i++)
            {
                int index = min_partial_makespan_NEH(operTimes,perm, nbJobs-d+1+i, nbMachines, posToInsert[i]);
                for (int j = i; j >index; j--)
                {
                    perm[j] = perm[j - 1];
                }
                perm[index] = posToInsert[i];
            }
            return perm;
        }
        public double basic_makespan_dynamic(int nbJobs,int nbMachines,int[] permutations, double[][] processingTimes)
            // Calculates the makespan using dynamic programming without acceleration
        {
            double[][] c = new double[nbMachines+1][];
            for (int i = 0; i < nbMachines+1; i++)
            {
                c[i] = new double[nbJobs+1];
            }
            for (int i = 0; i < nbJobs+1; i++)
            {
                c[0][i] = 0.0;
            }
            for (int i = 0; i < nbMachines+1; i++)
            {
                c[i][0] = 0.0;
            }

            for (int i = 1; i < nbMachines+1; i++)
            {
                for (int j = 1; j < nbJobs+1; j++)
                {
                    if (c[i - 1][j] > c[i][j - 1])
                    {
                        c[i][j] = c[i - 1][j] + processingTimes[i-1][permutations[j-1]];
                    }
                    else
                    {
                        c[i][j] = c[i][j-1] + processingTimes[i-1][permutations[j-1]];
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }

        public double Max(double a,double b)
        {
            if (a > b)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        public double basic_makespan_acceleration(int nbJobs, int nbMachines, int[] permutations /*The New permutation*/, double[][] processingTimes,int pos)
        {
            double cmax = 0.0;

            double[][] e = new double[nbJobs][];
            double[][] q = new double[nbJobs][];
            double[][] f = new double[nbJobs][];
            
            // Initialisation
            for (int j = 0; j < nbMachines; j++)
            {
                e[0][j] = 0;
                f[0][j] = 0;
            }
            for (int i = 0; i < pos; i++)
            {
                e[i][0] = 0;
            }
            for (int j = 0; j < nbJobs-pos+2; j++)
            {
                q[nbMachines][j] = 0;
            }
            for (int i = 0; i < nbMachines; i++)
            {
                q[i][pos - 1] = 0;
            }

            // Calculations
            for (int i = 0; i < nbMachines; i++)
            {
                for (int j = 0; j <= pos-1; j++)
                {
                    if (j<pos-1)
                    {
                        e[i][j] = Max(e[i][j - 1], e[i - 1][j]) + processingTimes[i][permutations[j]];
                    }
                    f[i][j] = Max(f[i-1][j],e[i][j-1])+ processingTimes[i][permutations[j]];
                }
            }
            for (int i = nbMachines; i > 0; i++)
            {
                for (int j = nbJobs-pos+1; j > 0; j++)
                {
                    q[i][j] = Max(q[i+1][j],e[i][j-1])+ processingTimes[i][permutations[j]];
                }
            }

            // Calculation of Cmax
            for (int i = 0; i < nbMachines; i++)
            {
                cmax = Max(f[i][pos] + q[i][pos],cmax) ;
            }
            return cmax;
        }
    }
}
