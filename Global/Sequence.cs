using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console
{
    class Sequence
    {
        private int JobsNumber { get; set; }
        private int MachineNumber { get; set; }
        private int[] Permutations { get; set; }
        private double[][] OperationTimes { get; set; }
        private int[][] MaintenancePositions { get; set; }
        private double[][] MaintenanceTimes { get; set; } 
        private double[][] Degradations { get; set; } 
        private double[] ObjectiveValue { get; set; } 
        private double[][] DueDates { get; set; }
        private ObjectiveFunctions[] Objectives { get; set; } // the name of objectives
        private double[] Coefficients { get; set; }
        private MultiObjectiveMethods ObjectiveMethod { get; set; }


        private bool MultiObjective { get; set; }
        private bool IncludeMaintenance { get; set; }
        private bool MaintenanceVariation { get; set; }
        private bool ProductionVariation { get; set; }
        private bool RessourceLimit { get; set; }
        private bool MaintenanceObjective { get; set; }
        private bool IncludeDueDates { get; set; }

        public enum MultiObjectiveMethods
        {
            Separate,
            Linear,
            Constraint,
            Domination
        }
        
        public enum TypeMaintenanceInsertion
        {
            Random,
            Left,
            Right,
            Best
        }

        public enum Variation
        {
            Constant,
            LearningEffect,
            DeteriorationEffect
        }
        public enum ObjectiveFunctions
        {
            Makespan,
            SumCompletionTime,
            Tardiness,
            Latenssy,
            TotalTardiness
        }


        public void calculate_objective()
        {
            if (this.MultiObjective)
            {
                switch (this.ObjectiveMethod)
                {
                    case MultiObjectiveMethods.Separate:
                        for (int i = 0; i < Objectives.Length; i++)
                        {
                            switch (Objectives[i])
                            {
                                case ObjectiveFunctions.Makespan:
                                    if (this.IncludeMaintenance && !this.IncludeDueDates)
                                    {
                                        this.ObjectiveValue[0] = makespan_with_maintenance(this.OperationTimes,this.JobsNumber,this.MachineNumber,this.Permutations,this.MaintenancePositions,this.MaintenanceTimes);
                                    }
                                    else if (this.IncludeMaintenance && this.IncludeDueDates)
                                    {

                                    }
                                    else if(!this.IncludeMaintenance && this.IncludeDueDates)
                                    {

                                    }
                                    else if (!this.IncludeMaintenance && !this.IncludeDueDates)
                                    {
                                        this.ObjectiveValue[0] = Basic_makespan_dynamic(this.OperationTimes,this.Permutations,this.JobsNumber,this.MachineNumber);
                                    }
                                    break;
                                case ObjectiveFunctions.SumCompletionTime:
                                    break;
                                case ObjectiveFunctions.Tardiness:
                                    break;
                                case ObjectiveFunctions.Latenssy:
                                    break;
                                case ObjectiveFunctions.TotalTardiness:
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case MultiObjectiveMethods.Linear:
                        this.ObjectiveValue[0] = 0.0;
                        double value = 0.0;
                        for (int i = 0; i < Objectives.Length; i++)
                        {
                            switch (Objectives[i])
                            {
                                case ObjectiveFunctions.Makespan:
                                    break;
                                case ObjectiveFunctions.SumCompletionTime:
                                    break;
                                case ObjectiveFunctions.Tardiness:
                                    break;
                                case ObjectiveFunctions.Latenssy:
                                    break;
                                case ObjectiveFunctions.TotalTardiness:
                                    break;
                                default:
                                    break;
                            }
                            this.ObjectiveValue[0] += Coefficients[i] *value;
                        }
                        break;
                    case MultiObjectiveMethods.Constraint:
                        break;
                    case MultiObjectiveMethods.Domination:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Objectives[0])
                {
                    case ObjectiveFunctions.Makespan:
                        if (this.IncludeMaintenance && !this.IncludeDueDates)
                        {
                            this.ObjectiveValue[0]=makespan_with_maintenance(this.OperationTimes, this.JobsNumber, this.MachineNumber, this.Permutations, this.MaintenancePositions, this.MaintenanceTimes);
                        }
                        else if (this.IncludeMaintenance && this.IncludeDueDates)
                        {

                        }
                        else if (!this.IncludeMaintenance && this.IncludeDueDates)
                        {

                        }
                        else if (!this.IncludeMaintenance && !this.IncludeDueDates)
                        {
                            this.ObjectiveValue[0] = Basic_makespan_dynamic(this.OperationTimes, this.Permutations, this.JobsNumber, this.MachineNumber);
                        }
                        break;
                    case ObjectiveFunctions.SumCompletionTime:
                        break;
                    case ObjectiveFunctions.Tardiness:
                        break;
                    case ObjectiveFunctions.Latenssy:
                        break;
                    case ObjectiveFunctions.TotalTardiness:
                        break;
                    default:
                        break;
                }
            }
        }

        private double Max(double a, double b) 
            // Calculate the Max between two functions
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
        private int[][] maintenance_insertion(int nbJobs,int nbMachines,int[] permutation,double[][] degradations,double[][] processingTimes,int delta,TypeMaintenanceInsertion t)
        {
            int[][] mtPos = new int[nbMachines][];
            double[][] mtTimes = new double[nbMachines][]; // To find a way to calculate it
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

        /* Calculation of the objective function: 
         * There exists two types functions : mono-objectives and multi-objectives
         * The mono-objective are the function that interests in one objective
         * The multi-objective is for optimizing multiple objectives in the same time
         * The mono-objective are the simple to optimise whether minimizing or maximizing
         * The multi-objective are not simple because of the correlation between the objectives in it
        */




        /*****************************************************************Mono-Objective***********************************************************************/
        private double Basic_makespan_dynamic(double[][] processingTimes,int[] permutations, int nbJobs, int nbMachines)
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
                        c[i][j] = c[i - 1][j] + processingTimes[i][permutations[j]];
                    }
                    else
                    {
                        c[i][j] = c[i][j - 1] + processingTimes[i][permutations[j]];
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }

        struct CriticalPath
        {
            public double makespan;
            public int[] Path;
        }
        private CriticalPath BasicMakespanDynamicWithPath(double[][] processingTimes,int[] permutations, int nbJobs, int nbMachines)
            // Calculates the makespan using dynamic programming without acceleration
        {
            Sequence.CriticalPath criticalPath = new CriticalPath();
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
                }
            }
            int[] path = new int[nbMachines];
            int indexJobs = nbJobs;
            int indexMachines = nbMachines;
            while (indexJobs !=0 && indexMachines !=0)
            {
                if (c[indexMachines-1][indexJobs]<c[indexMachines][indexJobs-1])
                {
                    path[indexJobs] = nbJobs-indexJobs+1;
                    indexJobs--;
                }
                else
                {
                    indexMachines--;
                }
            }
            Array.Copy(path,criticalPath.Path,nbJobs);
            criticalPath.makespan = c[nbMachines][nbJobs];
            return criticalPath;
        }
        
        
        private double makespan_with_maintenance(double[][] processingTimes, int nbJobs,int nbMachines, int[] permutations,int [][] maintenancePositions,double[][]maintenanceTimes)
        {
            double[][] c = new double[nbMachines + 1][];
            int indexMaintenance = 0;
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
                    if ((indexMaintenance = Array.IndexOf(maintenancePositions[i],j))!=-1)
                    {
                        c[i][j] += maintenanceTimes[i][indexMaintenance];
                    }
                }
            }
            return c[nbMachines][nbJobs];
        }

        
        private double sum_completion_times(double[][] processingTimes, int nbJobs, int nbMachines,double[] weights)
            // Calculates the sum of completion time
        {
            double sumC = 0.0;
            double[][] c = new double[nbMachines + 1][];
            int indexMaintenance = 0;
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
                        c[i][j] = c[i - 1][j] + processingTimes[i][Permutations[j]];
                    }
                    else
                    {
                        c[i][j] = c[i][j - 1] + processingTimes[i][Permutations[j]];
                    }
                    if ((indexMaintenance = Array.IndexOf(MaintenancePositions[i], j)) != -1)
                    {
                        c[i][j] += MaintenanceTimes[i][indexMaintenance];
                    }
                }
            }
            for (int j = 0; j < nbJobs; j++)
            {
                sumC += c[nbMachines + 1][j]*weights[j];
            }
            return sumC;
        }
        
        /*****************************************************************Multi-objective*********************************************************************/
        
        private double linear_combination()
        {
            double c = 0.0;
            return c;
        }

        private double constraint_objective()
        {
            double c = 0.0;
            return c;
        }
    }
}
