namespace PFSP_MLD_Console.InterpolationBasedOptimisationPFSP 
{
    public class Interpolation
    {
        public double[] Inputs;
        public double[] Outputs;
        public int Degree;
        public Polynomial InterPoly;

        public Interpolation(double[] inputs, double[] outputs, int degree)
        {
            Inputs = inputs;
            Outputs = outputs;
            Degree = degree;
            InterPoly = InterpolationPolynomial();
        }

        public double CalculateInterpolation(double value)
        {
            double result = 0.0;
            double prod = 1.0;
            for (int i = 0; i < this.Degree; i++)
            {
                for (int j = 0; j < this.Degree; j++)
                {
                    prod = 1.0;
                    if (j != i)
                    {
                        prod *= (value - this.Inputs[j])/(this.Inputs[i]-this.Inputs[j]);
                    }
                }
                result += prod * this.Outputs[i];
            }
            return result;
        }

        private Polynomial InterpolationPolynomial()
        {
            Polynomial interpolation = new Polynomial();
            Polynomial prod = new Polynomial();
            Polynomial element = new Polynomial();
            PolOper operation = new PolOper();
            interpolation.degree = this.Degree;
            for (int i = 0; i < this.Degree; i++)
            {
                prod.Coefficient[0] = 1.0;
                prod.degree = 0;
                for (int j = 0; j < this.Degree; j++)
                {
                    element.Coefficient[0] = - this.Outputs[i] / (this.Inputs[i] - this.Inputs[j]);
                    element.Coefficient[1] =  this.Outputs[i] / (this.Inputs[i] - this.Inputs[j]);
                    element.degree = 1;
                    prod = operation.ProdPolyDegreeOne(prod,element);
                }

                interpolation = operation.add(interpolation,prod);
            }
            return interpolation;
        }
    }
}