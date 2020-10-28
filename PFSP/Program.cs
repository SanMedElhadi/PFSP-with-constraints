using System;
using System.IO;

namespace PFSP_MLD_Console.PFSP 
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Test of Neighboors*/
            /* // Valid
            Neighborhood neighborhood = new Neighborhood();
            int[] perm = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20};
            int[] res = new int[20];
            int repeat = 150;
            for (int i = 0; i < repeat; i++)
            {
                Array.Copy(neighborhood.Adjacent_Swap(perm, 20,1),res,20);
                for (int j = 0; j < 20; j++)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        if (res[k] == res[j] && k!=j)
                        {
                            Console.WriteLine("changed");
                            break;
                        }
                    }

                    if (j == 19)
                    {
                        Console.WriteLine("Saved");
                    }
                }
            }*/
            /* Test of heuristics*/
            // Valid
            /*
            int machinesNumber = 5;
            int jobsNumber = 20;
            double[][] jobsTimes = new double[machinesNumber][];
            int[] solution = new int[jobsNumber];
            double fitness;
            StreamReader reader = new StreamReader(File.OpenRead("Tai_20_5_00.txt"));
            for (int i = 0; i < machinesNumber; i++)
            {
                jobsTimes[i] = new double[jobsNumber];
                string[] values = reader.ReadLine()?.Split(" ");
                for (int j = 0; j < jobsNumber; j++)
                {
                    if (values != null) Double.TryParse(values[j], out jobsTimes[i][j]);
                }
            }
            reader.Close();
            Heuristics heuristics = new Heuristics();
            heuristics.Name = Heuristics.Heuristic.HeuristicMinFirstLast;
            Array.Copy(heuristics.Execute(jobsNumber, machinesNumber, jobsTimes),solution,jobsNumber);
            fitness = heuristics.basic_makespan_dynamic(jobsNumber, machinesNumber, solution, jobsTimes);
            Console.WriteLine("fitness : "+fitness);
            for (int i = 0; i < jobsNumber; i++)
            {
                Console.WriteLine("solution : "+solution[i]);
            }*/
            /*PSO Test*/
            /*
            PSO test = new PSO();
            test.iterations = 250;
            test.jobs_number = 20;
            test.machines_number = 5;
            test.popSize = 150;
            test.xmax = 4;
            test.xmin = -4;
            test.wmax = 3;
            test.wmin = -3;
            test.c1min = 1;
            test.c1max = 2.5;
            test.c2max = 2;
            test.c2min = 0.5;
            test.inertia_weight = 1.5;
            test.constant_global = 1.0;
            test.constant_personal = 1.0;
            test.ratio_random = 0.2;
            test.chaotic_ratio = 0.2;
            test.constants_variant = false;
            test.weights_varinat = false;
            test.jobs_times = new double[test.machines_number][];
            StreamReader reader = new StreamReader(File.OpenRead("Tai_20_5_01.txt"));
            for (int i = 0; i < test.machines_number; i++)
            {
                test.jobs_times[i] = new double[test.jobs_number];
                string[] values = reader.ReadLine().Split(" ");
                for (int j = 0; j < test.jobs_number; j++)
                {
                    Double.TryParse(values[j], out test.jobs_times[i][j]);
                }
            }
            reader.Close();
            test.Run();
            for (int i = 0; i < test.jobs_number; i++)
            {
                Console.WriteLine(test.BestIndividual[i]);
            }
            Console.WriteLine(test.BestFitness);
            Console.WriteLine(test.Fitness(test.BestIndividual));*/
            //SA Test :
            /*
             int nb_individual = 1000;
             double T = 100;
             Sa test = new Sa();
             test.Iterations = nb_individual;
             test.Lambda = 0.96;
             test.NbJobs = 20;
             test.NbMachines = 5;
             test.Temperature = T;
             test.JobsTimes = new double[test.NbMachines][];
             StreamReader reader = new StreamReader(File.OpenRead("Tai_20_5_00.txt"));
             for (int i = 0; i < test.NbMachines; i++)
             {
                 test.JobsTimes[i] = new double[test.NbJobs];
                 string[] values = reader.ReadLine()?.Split(" ");
                 for (int j = 0; j < test.NbJobs; j++)
                 {
                     if (values != null) Double.TryParse(values[j], out test.JobsTimes[i][j]);
                 }
             }
             reader.Close();
             test.Run();
             Console.WriteLine("" +test.BestFitness);
             for (int i = 0; i < test.NbJobs; i++)
             {
                 Console.WriteLine(test.BestIndividual[i]);
             }
             */
            /*VNS Test*/
            /*
            Vns test = new Vns();
            test.Iterations = 200;
            test.JobsNumber = 20;
            test.MachinesNumber = 5;
            test.JobsTimes = new double[test.MachinesNumber][];
            test.NumberNeigh = 3;
            test.limit = 5;
            StreamReader reader = new StreamReader(File.OpenRead("Tai_20_5_00.txt"));
            for (int i = 0; i < test.MachinesNumber; i++)
            {
                test.JobsTimes[i] = new double[test.JobsNumber];
                string[] values = reader.ReadLine()?.Split(" ");
                for (int j = 0; j < test.JobsNumber; j++)
                {
                    if (values != null) Double.TryParse(values[j], out test.JobsTimes[i][j]);
                }
            }
            reader.Close();
            test.Run();
            Console.WriteLine("" + test.BestFitness);
            Console.WriteLine(""+test.basic_makespan_dynamic(test.JobsNumber,test.MachinesNumber,test.BestIndividual,test.JobsTimes));
            for (int i = 0; i < test.JobsNumber; i++)
            {
                Console.WriteLine(test.BestIndividual[i]);
            }*/
            /*Test ABC*/
            // valid
            
            Abc test = new Abc();
            test.Iterations = 60;
            test.Limit = 5;
            test.EmployedNumber = 80;
            test.ScoutNumber = 30;
            test.JobsNumber = 100;
            test.MachineNumber = 10;
            test.JobsTime = new double[test.MachineNumber][];
            StreamReader reader = new StreamReader(File.OpenRead("Tai_100_10_00.txt"));
            for (int i = 0; i < test.MachineNumber; i++)
            {
                test.JobsTime[i] = new double[test.JobsNumber];
                string[] values = reader.ReadLine()?.Split(" ");
                for (int j = 0; j < test.JobsNumber; j++)
                {
                    if (values != null) Double.TryParse(values[j], out test.JobsTime[i][j]);
                }
            }
            reader.Close();
            test.Run();
            Console.WriteLine("Fitness : "+test.BestFitness);
            Console.WriteLine("Fitness Calculated : "+test.basic_makespan_dynamic(test.JobsNumber,test.MachineNumber,test.BestIndividual,test.JobsTime));
            for (int i = 0; i < test.JobsNumber; i++)
            {
                Console.WriteLine("ind : "+test.BestIndividual[i]);
            }
        }

    }

}
