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
        float A;
        float B;
        int size;
        static Random rnd = new Random();
        private List<float> gaussDistribution_;
        Dictionary<string, float> borders;  // Границы распределения
        public Worker(int N_, float A_, float B_, int size_)
        {
            // FIX SIZE +
            // На одном графике выборчаная и теоретическая (Ступеньчетую)
            // Вычислить D
            N = N_; 
            A = A_;
            B = B_;
            size = size_;
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
        public List<float> gaussDistribution()
        {

            List<float> theta = new List<float>();
            for(int i = 0; i < size; i++)
            {
                List<float> temp = getUniformDistribution();
                float temp2 = totalDepositAmount(temp);
                theta.Add(temp2);
            }
            theta.Sort();
            gaussDistribution_ = theta;
            // 
            return theta;
        }

        // --->Часть 2
        // Определение теоретических и выборочных числовых характеристикик
        public Dictionary<string, float> statisticalCharacteristics()
        {
            float gaussSum = gaussDistribution_.Sum();
            Dictionary<string, float> df = new Dictionary<string, float>();
   
            float E_theta = A * N;  // Теоричетическое математическое ожидание
            float x_ = gaussDistribution_.Sum() / size; // Выборочное среднее
            float E_minus_x = Math.Abs(E_theta - x_);

            float D_theta = B * N;  // Теоретическая диспресия
            float S_2 = 0;  // // Выборочная дисперсия
            for (int i = 0; i < size; i++)
                S_2 += (float)(Math.Pow(gaussDistribution_[i] - x_, 2) / size);
            float D_minus_S = Math.Abs(D_theta - S_2);
            float Me;
            Me = (size % 2 == 1) ? gaussDistribution_[size / 2] : (gaussDistribution_[size / 2 - 1] + gaussDistribution_[size / 2]) / 2;// Выборочная медиана
            float R = gaussDistribution_.Last() - gaussDistribution_.First();  // Размах выборки

            df.Add("Мат_ожидание", E_theta);
            df.Add("Выб_мат_ожидание", x_);
            df.Add("Абс_мат_ожидание", E_minus_x);
            df.Add("Дисперсия", D_theta);
            df.Add("Выб_дисперсия", S_2);
            df.Add("Абс_дисперсия", D_minus_S);
            df.Add("Выб_медиана", Me);
            df.Add("Размах_выборки", R);
            return df;
        }

        // Построение графиков теоретической Fη(x) и выборочной F'η(x) функций распределения
        //--------------------------------------------------------------------------------------------------------------------
        // Теоретическая функция
        public float Erf(double x)
        {
            double a1 = 0.254829592 ;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;
            int sign = 1;
            if (x < 0)
                sign = -1;
            x = Math.Abs(x);
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);
            return (float)(sign * y);
        }
        //--------------------------------------------------------------------------------------------------------------------
    }
}
