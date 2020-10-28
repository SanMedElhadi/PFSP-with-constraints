using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace PFSP_MLD_Console.PFSP 
{
    public class Neighborhood
    {
        enum VariableType
        {
            Descrete,
            Continous
        }
        public enum Structure
        {
            RandomInsert,
            RandomSwap,
            AdjacentSwap
        }
        public enum Operation
        {
            Insert,
            Swap,
            AdjacentSwap
        }
        public int[] Shift(int[] permutation,int length,bool left,int repeat)
        {
            int[] perm = new int[length];
            int save;
            Array.Copy(permutation,perm,length);
            if (left)
            {
                save = perm[length];
            }
            else
            {
                save = perm[0];
            }
            for (int i = 0; i < repeat; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (left && j<length-1)
                    {
                        perm[i + 1] = perm[i];
                    }
                    else if (left && j ==length-1)
                    {
                        perm[0] = save;
                    }
                    else if (!left && j<length-1)
                    {
                        perm[i] = perm[i + 1];
                    }else
                    {
                        perm[j] = save;
                    }
                }
            }
            return perm;
        }
        public int[] AddModulu(int[] permutation,int length,int constant)
        {
            int[] perm = new int[length];
            Array.Copy(permutation, perm,length);
            for (int i = 0; i < length; i++)
            {
                perm[i] = (perm[i] + constant) % length;
            }
            return perm;
        }

        public int[] Random_Insert(int[] permutation,int length,int repeat)
        {
            Random r = new Random();
            int[] perm = new int[length];
            int save;
            Array.Copy(permutation,perm,length);
            int pos1 = r.Next() % length;//i
            int pos2 = r.Next() % length;//j
            if (pos1 != pos2)
            {
                for (int j = 0; j < repeat; j++)
                {
                    pos1  = r.Next() % length;
                    pos2 = r.Next() % length;
                    save = perm[pos1];
                    if (pos2 >pos1)
                    {
                        for (int i = pos1; i < pos2; i++)
                        {
                            perm[i] = perm[i+1];
                        }
                    }
                    else if (pos2 < pos1)
                    {
                        for (int i = pos1; i > pos2; i--)
                        {
                            perm[i] = perm[i-1];
                        }
                    }
                    perm[pos2] = save;
                }
            }
            return perm;
        }

        public int[] Random_Swap(int[] permutation,int length,int repeat)
        {
            Random r = new Random();

            int[] perm = new int[length];
            Array.Copy(permutation, perm, length);
            int save;
            for (int j = 0; j < repeat; j++)
            {
                int pos1 = r.Next() % length;//i
                int pos2 = r.Next() % length;//j
                if (pos1 != pos2)
                {
                    save = perm[pos1];
                    perm[pos1] = perm[pos2];
                    perm[pos2] = save;
                }
            }
            return perm;
        }

        public int[] Adjacent_Swap(int[] permutation,int length,int repeat)
        {
            Random r = new Random();
            int[] perm = new int[length];
            Array.Copy(permutation, perm, length);
            int pos;
            int save;
            for (int i = 0; i < repeat; i++)
            {
                pos = r.Next() % length;
                if (pos<length-1)
                {
                    save = perm[pos];
                    perm[pos] = perm[pos + 1];
                    perm[pos+1] = save;
                }
                else
                {
                    save = perm[pos];
                    perm[pos] = perm[pos-1];
                    perm[pos-1] = save;
                }
            }
            return perm;
        }
        public int[] Insert(int[] permutation,int length,int pos1,int pos2)
        {
            int[] perm = new int[length];
            int save;
            Array.Copy(permutation, perm, length);
            save = perm[pos1];
            if (pos2<pos1)
            {
                for (int i = pos1; i > pos2; i--)
                {
                    perm[i] = perm[i-1];
                }
            }
            else if (pos2>pos1)
            {
                for (int i = pos1; i < pos2; i++)
                {
                    perm[i] = perm[i + 1];
                }
            }
            perm[pos2] = save;
            return perm;
        }
        public int[] Swap(int[] permutation,int length,int pos1,int pos2)
        {
            int[] perm = new int[length];
            Array.Copy(permutation,perm,length);
            int save = perm[pos1];
            perm[pos1] = perm[pos2];
            perm[pos2] = save;
            return perm;
        }
        public int[] Swap_Adjacent(int[] permutation,int length,int pos)
        {
            int[] perm = new int[length];
            int save;
            Array.Copy(permutation, perm, length);
            if (pos < length-1)
            {
                save = perm[pos];
                perm[pos] = perm[pos + 1];
                perm[pos + 1] = save;
            }
            else
            {
                save = perm[0];
                perm[0] = perm[pos];
                perm[pos] = save;
            }
            return perm;
        }
        
        public int[] ThreeExchange(int[] permutation,int length){
            int[] perm = new int[length];
            Random random = new Random();
            int pos1 = random.Next() % length;
            int pos2;
            do
            {
                pos2 = random.Next() % length;
            } while (pos2!=pos1);
            int pos3;
            do
            {
                pos3 = random.Next() % length;
            } while (pos3 != pos1 && pos3 != pos2);
            int save = perm[pos2];
            perm[pos2] = perm[pos1];
            perm[pos1] = perm[pos3];
            perm[pos3] = save; 
            return perm;
        }
        
        public int[] Optimized_Neighborhood(int[] perm)
        {
            return perm;
        }
    }
}
