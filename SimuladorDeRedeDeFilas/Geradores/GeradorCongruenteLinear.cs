using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Geradores
{
    public class GeradorCongruenteLinear
    {
        /// <summary>
        /// Gera um array de números aleatórios.
        /// </summary>
        /// <param name="a">O multiplicador.</param>
        /// <param name="c">Constnate usada para uma maior variação de números gerados.</param>
        /// <param name="M">O número máximo a ser gerado.</param>
        /// <param name="x0">A semente e primeiro valor.</param>
        /// <param name="length">A quantidade de números a serem gerados.</param>
        /// <returns>Um array com os números aleatórios gerados, sendo <paramref name="x0"/> o primeiro.</returns>
        public double[] Gerar(int a, int c, int M, double x0, int length)
        {
            double[] x = new double[length];

            x[0] = x0;
            for (int i = 1; i < length; i++)
            {
                x[i] = (a * x[i - 1] + c) % M;
            }

            return x;
        }
    }
}
