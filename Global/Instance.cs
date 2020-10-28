using System;
using System.Collections.Generic;
using System.Text;

namespace PFSP_MLD_Console
{
    class Instance
    {
        public int JobsNumber;
        public int MachineNumber;
        public double[][] JobsTimes;
        public Job[] Jobs;
        public Machine[] Machines;

        public Instance(int nbJobs,int nbMachines,double[][] operTimes)
        {
            this.JobsNumber = nbJobs;
            this.MachineNumber = nbMachines;
            //this.jobs_times = oper_times;
            this.JobsTimes = new double[this.MachineNumber][];
            for (int i = 0; i < this.JobsNumber; i++)
            {
                this.JobsTimes[i] = new double[this.JobsNumber];
                Array.Copy(operTimes[i],this.JobsTimes[i],this.MachineNumber);
            }
        }
    }
}
