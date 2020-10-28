using System;
using System.IO;
using PFSP_MLD_Console.PFSP;

namespace PFSP_MLD_Console
{
    class TestProcessPFSP
    {
        private string TypeTest { get; set; }
        enum TestType
        {
            Heuristics,
            MetaHeuristics,
            Hyperheuristics
        }

        public int Unif(int seed,int high,int low)
        {
            int m = Int32.MaxValue;
            int a = 16807;
            int b = 127773;
            int c = 2836;
            int k;
            double value01;
            k = seed - (seed % b) / b;
            seed = a * (seed % b) - k * c;
            if (seed <0)
            {
                seed += m;
            }
            value01 = seed / m;
            return low +(int) Math.Truncate(value01 * (high - low + 1));
        }

        public void generate_flow_shop(int timeSeed,int nbJobs,int nbMachines,out int[][] d)
        {
            d = new int[nbMachines][];
            for (int i = 0; i < nbMachines; i++)
            {
                d[i] = new int[nbJobs];
                for (int j = 0; j < nbJobs; j++)
                {
                    d[i][j] = Unif(timeSeed, 1, 99);
                }
            }
        }
        public Sequence[] generate_instance(int nbJobs,int nbMachines,int number) // Genrating an instance to be executed by the algorithme
        {
            Random r = new Random();
            Sequence[] benchmarks = new Sequence[number];
            int seed = r.Next();
            int[][] d;
            for (int i = 0; i < number; i++)
            {
                generate_flow_shop(seed, nbJobs, nbMachines, out d);
                benchmarks[i] = new Sequence();
                // Sequence treatement
                seed = r.Next();
            }
            return benchmarks;
        }

        public void generate_instance_with_maintenance(int nbJobs,int nbMachines,int number)
        {
            Sequence[] benchmarks = new Sequence[number];
            benchmarks = generate_instance(nbJobs, nbMachines, number);
        }

        public void read_basic_benchmarks(string path)
        {
            string value = null;
            StreamReader reader = new StreamReader(File.OpenRead(path));
            do
            {
                value = reader.ReadLine();
            } while (value != null);
        }



        /********************************************************Heuristics Test****************************************************************/
        public void Test_Heuristics()
        {
            int nbMachines;
            bool moreInstance = true;
            bool begin = true;
            double[][] operTimes = null;
            bool pure = true;
            string values;
            string[] splitedValues = null;
            int upperBound = 1;
            int[] jobs = new int[] { 20, 50, 100, 200, 500 };
            int[] machines = new int[] { 5, 10, 20 };
            foreach (var numberJobs in jobs)
            {
                if (numberJobs <= 100)
                {
                    foreach (var numberMachines in machines)
                    {
                        operTimes = new double[numberMachines][];
                        for (int i = 0; i < 10; i++)
                        {
                            if (i > 0)
                            {
                                begin = false;
                            }
                            StreamReader reader = new StreamReader(File.OpenRead("Taillard_With_Maintenance/tai" + numberJobs + "_" + numberMachines + "_M1_" + (i + 1) + ".txt"));
                            for (int j = 0; j < 2 * numberMachines + 4; j++)
                            {
                                values = reader.ReadLine();
                                if (j == 2)
                                {
                                    Int32.TryParse(values, out upperBound);
                                }
                                else if (j >= 4 && j % 2 == 0)
                                {
                                    splitedValues = values.Split(' ');
                                    operTimes[(j - 4) / 2] = new double[numberJobs];
                                    for (int k = 0; k < numberJobs; k++)
                                    {
                                        Double.TryParse(splitedValues[k], out operTimes[(j - 4) / 2][k]);
                                    }
                                }
                            }
                            reader.Close();
                            calculate_instance(numberJobs, numberMachines, upperBound, operTimes, moreInstance, begin);
                        }

                    }
                }
                else if (numberJobs == 200)
                {
                    for (int l = 0; l < 2; l++)
                    {
                        nbMachines = 10 + l * 10;
                        operTimes = new double[nbMachines][];
                        for (int k = 0; k < 10; k++)
                        {
                            StreamReader reader = new StreamReader(File.OpenRead("Taillard_With_Maintenance/tai" + numberJobs + "_" + nbMachines + "_M1_" + (k + 1) + ".txt"));
                            for (int i = 0; i < 2 * nbMachines + 4; i++)
                            {
                                values = reader.ReadLine();
                                if (i == 2)
                                {
                                    Int32.TryParse(values, out upperBound);
                                }
                                else if (i >= 4 && i % 2 == 0)
                                {
                                    splitedValues = values.Split(' ');
                                    operTimes[(i - 4) / 2] = new double[numberJobs];
                                    for (int j = 0; j < numberJobs; j++)
                                    {
                                        Double.TryParse(splitedValues[j], out operTimes[(i - 4) / 2][j]);
                                    }
                                }
                            }
                            reader.Close();
                            calculate_instance(numberJobs, nbMachines, upperBound, operTimes, moreInstance, begin);
                        }

                    }
                }
                else
                {
                    nbMachines = 20;
                    operTimes = new double[nbMachines][];
                    for (int k = 0; k < 10; k++)
                    {
                        StreamReader reader = new StreamReader(File.OpenRead("Taillard_With_Maintenance/tai" + numberJobs + "_" + nbMachines + "_M1_1" + (k + 1) + ".txt"));
                        for (int i = 0; i < 2 * nbMachines + 4; i++)
                        {
                            values = reader.ReadLine();
                            if (i == 2)
                            {
                                Int32.TryParse(values, out upperBound);
                            }
                            else if (i >= 4 && i % 2 == 0)
                            {
                                splitedValues = values.Split(' ');
                                operTimes[(i - 4) / 2] = new double[numberJobs];
                                for (int j = 0; j < numberJobs; j++)
                                {
                                    Double.TryParse(splitedValues[j], out operTimes[(i - 4) / 2][j]);
                                }
                            }
                        }
                        reader.Close();
                        if (k == 9)
                        {
                            moreInstance = false;
                        }
                        calculate_instance(numberJobs, nbMachines, upperBound, operTimes, moreInstance, begin);
                    }

                }
            }

        }
        public void calculate_instance(int nbJobs, int nbMachines, int upperbound, double[][] operTimes, bool more, bool begin)
        {
            int[] permutation = new int[nbJobs];
            StreamWriter writer = null;
            bool pure = true;
            double neh, nehRandomPure, nehRandomNotpure, nehValue, cfl, cfl1, mfl, dfl;
            if (begin)
            {
                writer = new StreamWriter(File.OpenWrite("Tests_Heuristics.csv"));
                writer.WriteLine("jobs_number;machine_number;NEH;NEH_random_pure;NEH_random_Notpure;NEH1;NEH2;CFL;CFL1;NEH3;MFL;DFL;UpperBound");
            }
            Heuristics h = new Heuristics();
            permutation = h.Heuristic_Combination_First_Last(nbJobs, nbMachines, operTimes);
            cfl = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            permutation = h.Neh(nbJobs, nbMachines, operTimes);
            neh = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            permutation = h.NEH_random(nbJobs, nbMachines, operTimes, pure);
            nehRandomPure = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            pure = false;
            permutation = h.NEH_random(nbJobs, nbMachines, operTimes, pure);
            nehRandomNotpure = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            permutation = h.Heuristic_Combination_First_Last1(nbJobs, nbMachines, operTimes);
            cfl1 = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            permutation = h.Heuristic_Min_First_Last(nbJobs, nbMachines, operTimes);
            mfl = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            permutation = h.Heuristic_Differnce_First_Last(nbJobs, nbMachines, operTimes);
            dfl = h.basic_makespan_dynamic(nbJobs, nbMachines, permutation, operTimes);
            if (writer != null) writer.WriteLine("" + nbJobs + ";" + nbMachines + ";" + neh + ";" + nehRandomPure + ";" + nehRandomNotpure  + ";" + cfl + ";" + cfl1 + ";"  + mfl + ";" + dfl + ";" + upperbound);
            if (!more)
            {
                writer.Close();
            }
        }
        
        /***************************************************************Test MetaHeuristics**********************************************************************/
    }
}
