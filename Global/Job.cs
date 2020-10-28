using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PFSP_MLD_Console
{
    class Job
    {
        public int Index { get; set; }
        public double SetupTime { get; set; }
        public double ProcessingTime { get; set; }
        public double ReleaseDate { get; set; }
        public double StartDate { get; set; }
        public double CompletionTime { get; set; }
        public double DueDate { get; set; }
        public double FlowTime { get; set; }
        public double Lateness { get; set; }
        public double Tardiness { get; set; }
        public double Earliness { get; set; }
        public double Weight { get; set; }
        public int RessourcesNeeded { get; set; }

        public Job(int i = -1,double s=0.0,double p=0.0,double r=0.0,double d=0.0,double c = 0.0,double due = 0.0,double w = 0.0)
        {
            this.Index = i;
            this.SetupTime = s;
            this.ProcessingTime = p;
            this.ReleaseDate = r;
            this.DueDate = d;
            this.CompletionTime = c;
            this.DueDate = due;
            this.FlowTime = this.CompletionTime - this.ReleaseDate;
            this.Lateness = this.CompletionTime - this.DueDate;
            this.Tardiness = Math.Max(this.Lateness, 0.0);
            this.Earliness = Math.Max(-this.Lateness, 0.0);
            this.Weight = 0.0;
        }

    }
}
