using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Modelos
{
    /// <summary>
    /// Intervalo de tempo mínimo e máximo.
    /// </summary>
    public class Intervalo
    {
        /// <summary>
        /// Tempo mínimo do intervalo.
        /// </summary>
        public double Minimo { get; private set; }

        /// <summary>
        /// Tempo máximo do intervalo.
        /// </summary>
        public double Maximo { get; private set; }

        /// <summary>
        /// Cria uma nova instancia.
        /// </summary>
        /// <param name="minimo">Tempo mínimo do intervalo.</param>
        /// <param name="maximo">Tempo máximo do intervalo.</param>
        /// <exception cref="ArgumentException">Se o tempo mínimo for maior que o máximo, ou se um dos dois parâmetros for menor que zero.</exception>
        public Intervalo(double minimo, double maximo)
        {
            if (minimo > maximo)
                throw new ArgumentException("O tempo mínimo deve ser menor ou igual ao tempo máximo.", nameof(minimo));

            if (minimo < 0)
                throw new ArgumentException("O tempo mínimo deve ser maior ou igual a zero.", nameof(minimo));

            if (maximo < 0)
                throw new ArgumentException("O tempo máximo deve ser maior ou igual a zero.", nameof(maximo));

            Minimo = minimo;
            Maximo = maximo;
        }

        /// <summary>
        /// Apresenta o objeto como uma string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Coloca todas as propriedades do objeto no formato especificado.
            string formatoDePropriedade = "{0}={1}";
            List<string> propriedades = new List<string>();
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Minimo), Minimo));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Maximo), Maximo));

            // Gera a representação em string final com as propriedades.
            string propriedadesStr = string.Join(";", propriedades);
            string resultado = string.Format("Intervalo [{0}]", propriedadesStr);

            return resultado;
        }
    }
}
