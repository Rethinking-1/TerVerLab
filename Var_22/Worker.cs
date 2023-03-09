using System;
using System.Collections.Generic;
using System.Data;
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
        static Random rnd = new Random();
        private List<int> gaussDistribution_;
        Dictionary<string, float> borders;  // Границы распределения
        public Worker(int N_, int A_, int B_)
        {
            N = N_; 
            A = A_;
            B = B_;
            borders = new Dictionary<string, float>()
            {
                {"left", 2 * A_ - ((float)Math.Sqrt(12 * B_) + 2 * A_ ) / 2},
                {"right", ((float)Math.Sqrt(12 * B_) +  2 * A_) / 2}
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
            List<float> result = new List<float>();
            for(int i = 0; i < N; i++)
            {

                float y = (float)rnd.NextDouble();
                float x = inverseDistributionFunction(y);
                result.Add(x);
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
        public List<int> gaussDistribution()
        {

            List<int> theta = new List<int>();
            for(int i = 0; i < N; i++)
            {
                List<float> temp = getUniformDistribution();
                float temp2 = totalDepositAmount(temp);
                theta.Add((int)temp2);
            }
            theta.Sort();
            gaussDistribution_ = theta;
            // /N?  
            return theta;
        }

        // --->Часть 2
        // Определение теоретических и выборочных числовых характеристикик
        public Dictionary<string, int> statisticalCharacteristics()
        {
            float gaussSum = gaussDistribution_.Sum();
            Dictionary<string, int> df = new Dictionary<string, int>();
   
            float E_theta = A * N;  // Теоричетическое математическое ожидание
            float x_ = gaussDistribution_.Sum() / N; // Выборочное среднее
            float E_minus_x = Math.Abs(E_theta - x_);

            float D_theta = B * N;  // Теоретическая диспресия
            float S_2 = 0;  // // Выборочная дисперсия
            for (int i = 0; i < N; i++)
                S_2 += (float)(Math.Pow(gaussDistribution_[i] - x_, 2) / N);
            float D_minus_S = Math.Abs(D_theta - S_2);

            float Me = (N % 2 == 1) ? gaussDistribution_[N / 2 + 1] : gaussDistribution_[N / 2] + gaussDistribution_[N / 2 + 1];// Выборочная медиана 
            float R = gaussDistribution_.Last() - gaussDistribution_.First();  // Размах выборки

            df.Add("Мат_ожидание", (int)E_theta);
            df.Add("Выб_мат_ожидание", (int)x_);
            df.Add("Абс_мат_ожидание", (int)E_minus_x);
            df.Add("Дисперсия", (int)D_theta);
            df.Add("Выб_дисперсия", (int)S_2);
            df.Add("Абс_дисперсия", (int)D_minus_S);
            df.Add("Выб_медиана", (int)Me);
            df.Add("Размах_выборки", (int)R);
            return df;
        }

        // Построение графиков теоретической Fη(x) и выборочной F'η(x) функций распределения
        
    }
}
