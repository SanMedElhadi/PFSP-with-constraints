using System;

namespace PFSP_MLD_Console
{
    public class PolOper
    {
        public Polynomial add(Polynomial P, Polynomial Q)
        {
            Polynomial S = new Polynomial();
            S.Coefficient = new double[Math.Max(P.degree,Q.degree)];
            for (int i = 0; i <= Math.Max(P.degree,Q.degree); i++)
            {
                if (i<Math.Min(P.degree,Q.degree))
                {
                    S.Coefficient[i] = P.Coefficient[i] + Q.Coefficient[i];
                }
                else if (P.degree>Q.degree)
                {
                    S.Coefficient[i] = P.Coefficient[i];
                }
                else if (P.degree<Q.degree)
                {
                    S.Coefficient[i] = Q.Coefficient[i];
                }
            }
            if (P.degree == Q.degree)
            {
                int k = S.degree;
                while (k>=0 && S.Coefficient[k] == 0)
                {
                    S.degree--;
                    k--;
                }
            }
            return S;
        }

        public Polynomial Neg(Polynomial P)
        {
            Polynomial Q = new Polynomial();
            for (int i = 0; i <= P.degree; i++)
            {
                Q.Coefficient[i] = -P.Coefficient[i];
            }

            Q.degree = P.degree;
            return Q;
        }

        public static double CalculateInValue(Polynomial P, double value)
        {
            double results = 0.0;
            for (int i = 0; i < P.degree+1; i++)
            {
                results = P.Coefficient[P.degree - i] + value * results;
            }
            return results;
        }

        public Polynomial ProdPolyDegreeOne(Polynomial P, Polynomial Q)
        {
            Polynomial product = new Polynomial();
            if (P.degree == 1)
            {
                for (int i = 0; i <= Q.degree-1; i++)
                {
                    product.Coefficient[i+1] = P.Coefficient[1] * Q.Coefficient[i]+P.Coefficient[0]*Q.Coefficient[i+1];
                }
                product.Coefficient[0] = P.Coefficient[0] * Q.Coefficient[0];
                product.Coefficient[Q.degree] = P.Coefficient[1] * Q.Coefficient[Q.degree - 1];
            }
            else if (Q.degree == 1)
            {
                for (int i = 0; i <= P.degree-1; i++)
                {
                    product.Coefficient[i+1] = Q.Coefficient[1] * P.Coefficient[i]+Q.Coefficient[0]*P.Coefficient[i+1];
                }
                product.Coefficient[0] = Q.Coefficient[0] * P.Coefficient[0];
                product.Coefficient[Q.degree] = Q.Coefficient[1] * P.Coefficient[P.degree - 1];
            }
            else
            {
                product = null;
            }
            return product;
        }
    }
}