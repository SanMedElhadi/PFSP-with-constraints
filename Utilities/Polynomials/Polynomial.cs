using System;
using System.Collections;

namespace PFSP_MLD_Console
{
    public class Polynomial
    {
        public double[] Coefficient;
        public int degree;

        public Polynomial(double[] coef,int deg)
        {
            Array.Copy(coef,this.Coefficient,deg+1);
            this.degree = deg;
        }

        public Polynomial(double[] coef)
        {
            Array.Copy(coef,this.Coefficient,coef.Length);
            this.degree = coef.Length;
        }
        public Polynomial()
        {
            
        }
    }
}