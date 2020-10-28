using System;
using PFSP_MLD_Console;

namespace PFSP_MLD_Console.PFSPWithStaticMaintenance
{
    public class HeuristicsSm
    {
        public int JobsNumber;
        public int MachinesNumber;
        public double[][] JobsTimes;
        public int[] Permutation;
        public double MaintenanceTime;
        public int[][] MaintenancePositions;
        public double[][] Degradations;
        
        public enum HeuristicName
        {
            NehRandom,
            Neh,
            NehInit,
            HeuristicCombinationFirstLast,
            HeuristicCombinationFirstLast1,
            HeuristicMinFirstLast,
            HeuristicDiffernceFirstLasts
        }
        public enum TypeMaintenanceInsertion
        {
            Random,
            Left,
            Right,
            Best
        }
        private int[][] MaintenanceInsertion(int nbJobs,int nbMachines,int[] permutation,double[][] degradations,double[][] processingTimes,int delta,double mtTimes,TypeMaintenanceInsertion t)
        {
            int[][] mtPos = new int[nbMachines][];
            double sumDegradation = 0.0;
            double cmax = 0.0;
            int index = 0;
            for (int i = 0; i < nbMachines; i++)
            {
                index = 0;
                for (int j = 0; j < nbJobs; j++)
                {
                    sumDegradation += degradations[i][permutation[j]];
                    if (sumDegradation >=delta)
                    {
                        switch (t)
                        {
                            case TypeMaintenanceInsertion.Random:
                                Random random = new Random();
                                if (random.NextDouble() > 0.5)
                                {
                                    mtPos[i][index] = j;
                                }
                                else
                                {
                                    mtPos[i][index] = j-1;
                                }
                                break;
                            case TypeMaintenanceInsertion.Left:
                                mtPos[i][index] = j - 1;
                                break;
                            case TypeMaintenanceInsertion.Right:
                                mtPos[i][index] = j;
                                break;
                            case TypeMaintenanceInsertion.Best:
                                mtPos[i][index] = j;
                                cmax = makespan_with_maintenance(processingTimes, nbJobs, nbMachines,permutation, mtPos, mtTimes);
                                mtPos[i][index] = j - 1;
                                if (makespan_with_maintenance(processingTimes, nbJobs, nbMachines, permutation, mtPos, mtTimes)>cmax)
                                {
                                    mtPos[i][index] = j;
                                }
                                break;
                            default:
                                break;
                        }
                        index++;
                        sumDegradation = 0.0;
                    }
                }
            }
            return mtPos;
        }
        
        private double makespan_with_maintenance(double[][] processingTimes, int nbJobs,int nbMachines, int[] permutations,int [][] maintenancePositions,double maintenanceTimes)
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

        public void ExecuteHeuristic(HeuristicName name,int NbJobs,int NbMachines,double[][] OperTimes,TypeMaintenanceInsertion type)
        {
            this.Permutation = new int[NbJobs];
            PFSP.Heuristics heuristic = new PFSP.Heuristics();
            heuristic.Name = (PFSP.Heuristics.Heuristic) name;
            Array.Copy(heuristic.Execute(NbJobs,NbMachines,OperTimes),this.Permutation,NbJobs);
            Array.Copy(this.MaintenanceInsertion(NbJobs,NbMachines,this.Permutation,this.Degradations,OperTimes,1,this.MaintenanceTime,type),this.MaintenancePositions,NbMachines*NbJobs);
        }
    }
}