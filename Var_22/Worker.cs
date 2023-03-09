using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Var_22
{
    internal class Worker
    {
        int N;
        int A;
        int B;
        Dictionary<string, float> borders;  // Границы распределения
        public Worker(int N_, int A_, int B_)
        {
            N = N_; 
            A = A_;
            B = B_;
            borders = new Dictionary<string, float>()
            {
                {"left", 2 * A_ - (float)Math.Sqrt(12 * B_) + 2 * A_ },
                {"right", ((float)Math.Sqrt(12 * B_) +  A_) / 2}
            };
        }

        // Обратная функция распределения, равномерное распределение
        public float inverseDistributionFunction(float y)   // y ∈ [0; 1)
        {
            float x = y * (borders["right"] - borders["left"]) + borders["left"];  // x ∈ [left: right)
            return x;
        }

        // Получение равномерно распределенной случайной величины [left: right)
        public List<float>getUniformDistribution()
        {
            List<float> result = new List<float>(N);
            for(int i = 0; i < N; i++)
            {
                Random rnd = new Random();
                float y = (float)rnd.NextDouble();
                float x = inverseDistributionFunction(y);
                result[i] = x;
            }
            return result;
        }

        // Случайная величина - общая сумма вклада
        public float totalDepositAmount(List<float> contributions)  // Contributions - вклады
        {
            float result = contributions.Sum();
            return result;
        }

        // Нормальное распределение по центральной предельной теореме
        // ---> Часть 1
        // Розыгрыш случайной величины
        public List<float> gaussDistribution()
        {
            List<float> theta = new List<float>(N);
            for(int i = 0; i < N; i++)
            {
                List<float> temp = getUniformDistribution();
                float temp2 = totalDepositAmount(temp);
                theta[i] = temp2;
            }
            theta.Sort();
            // /N?  
            return theta;
        }
    }
}
