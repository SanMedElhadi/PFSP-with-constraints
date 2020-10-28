using System;
using PFSP_MLD_Console;
using System.Linq;
using PFSP_MLD_Console.PFSP;

namespace PFSP_MLD_Console.PFSPWithStaticMaintenance
{
    public class NeighborhoodsSm
    {
        public Neighborhood ProductionNeighbrohood;
        public bool WithMaintenanceAdjusting;
        public NeighborhoodsSm()
        {
            ProductionNeighbrohood = new Neighborhood();
        }
        public void MaintenanceMove(int[][] mPos,int NbJobs,int NbMachines)
        {
            Random random = new Random();
            int index = -1;
            int machine = random.Next() % NbMachines;
            int pos = random.Next()%NbJobs;
            int sens = random.Next() % 2;
            for (int i = 0; i < mPos[machine].Length; i++)
            {
                if (mPos[machine][i]>=pos)
                {
                    index = i;
                    break;
                }
            }
            if (sens == 0)
            {
                mPos[machine][index]--;
                if (mPos[machine][index] < 0)
                {
                    mPos[machine][index]++;
                }
            }
            else
            {
                mPos[machine][index]++;
                if (mPos[machine][index] >=NbJobs)
                {
                    mPos[machine][index]--;
                }
            }
        }

        public void AdjastMaintenance(int[][] mPos,int NbJobs,int NbMachines)
        {
            
        }
    }
}