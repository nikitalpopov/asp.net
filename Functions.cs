using System;
namespace asp.net
{
    public class Functions
    {
        public static double schaffer(double[] xValues)
        {
            double result = 0;
            result = 0.5 + (Math.Pow(Math.Sin(Math.Sqrt(xValues[0] * xValues[0] + xValues[1] * xValues[1])), 2) - 0.5) / (Math.Pow(1 + 0.001 * (xValues[0] * xValues[0] + xValues[1] * xValues[1]), 2));
            return result;
        }

        public static double sphere(double[] xValues)
        {
            double result = 0;
            for (int i = 0; i < xValues.Length; ++i)
            {
                result += Math.Pow(xValues[i], 2);
            }
            return result;
        }

        public static double griewank(double[] xValues)
        {
            double result = 0;
            double currentA = 0;
            double currentB = 1;
            for (int currentI = 1; currentI < xValues.Length; ++currentI)
            {
                currentA += Math.Pow((xValues[currentI] - 100), 2);
                currentB *= Math.Cos((xValues[currentI] - 100) / (Math.Sqrt(currentI)));

            }
            result = (currentA / 4000) - currentB + 1;
            return result;
        }

        public static double rastrigin(double[] xValues)
        {
            double result = 0;
            for (int i = 0; i < xValues.Length; ++i)
            {
                result += Math.Pow(xValues[i], 2) - 10 * Math.Cos(2 * Math.PI * xValues[i]) + 10;
            }
            return result;
        }

        public static double rosenbrock(double[] xValues)
        {
            double result = 0;
            for (int i = 0; i < xValues.Length; ++i)
            {
                result += 100 * Math.Pow((xValues[i] - Math.Pow(xValues[i], 2)), 2) + Math.Pow((xValues[i] - 1), 2);
            }
            return result;
        }
    }
}
