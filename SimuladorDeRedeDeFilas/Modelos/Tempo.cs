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
    public class Tempo
    {
        /// <summary>
        /// Tempo mínimo do intervalo.
        /// </summary>
        public int Minimo { get; private set; }

        /// <summary>
        /// Tempo máximo do intervalo.
        /// </summary>
        public int Maximo { get; private set; }

        /// <summary>
        /// Cria uma nova instancia.
        /// </summary>
        /// <param name="minimo">Tempo mínimo do intervalo.</param>
        /// <param name="maximo">Tempo máximo do intervalo.</param>
        /// <exception cref="ArgumentException">Se o tempo mínimo for maior que o máximo, ou se um dos dois parâmetros for menor que zero.</exception>
        public Tempo(int minimo, int maximo)
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

        public override string ToString()
        {
            string msg = "Simulador.Modelos.Tempo [{0}={1};{2}={3}]";
            msg = string.Format(msg, nameof(Minimo), Minimo, nameof(Maximo), Maximo);
            return msg;
        }
    }
}
