using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Modelos
{
    /// <summary>
    /// Fila de clientes (modelo g/g/x/y).
    /// </summary>
    public class Fila
    {
        /// <summary>
        /// Tempo de chegada.
        /// </summary>
        public Tempo Chegada { get; private set; }

        /// <summary>
        /// Tempo de atendimento.
        /// </summary>
        public Tempo Atendimento { get; private set; }

        /// <summary>
        /// Número de servidores (x).
        /// </summary>
        public int Servidores { get; private set; }

        /// <summary>
        /// Capacidade de clientes (y).
        /// </summary>
        public int Capacidade { get; private set; }

        /// <summary>
        /// Quantidade de clientes na fila (z).
        /// </summary>
        public int QuantidadeDeClientes { get; set; }

        /// <summary>
        /// Cria uma nova instancia com zero clientes na fila.
        /// </summary>
        /// <param name="tChegada">Tempo de chegada.</param>
        /// <param name="tAtendimento">Tempo de atendimento.</param>
        /// <param name="servidores">Número de servidores.</param>
        /// <param name="capacidade">Capacidade de clientes.</param>
        public Fila(Tempo tChegada, Tempo tAtendimento, int servidores, int capacidade)
        {
            Chegada = tChegada;
            Atendimento = tAtendimento;

            if (servidores < 0)
                throw new ArgumentException("Os número de servidores deve ser maior que 0.", nameof(servidores));
            else
                Servidores = servidores;

            if (capacidade < 0)
                throw new ArgumentException("A capacidade deve ser maior que 0.", nameof(capacidade));
            else
                Capacidade = capacidade;

            QuantidadeDeClientes = 0;
        }

        public override string ToString()
        {
            string msg = "Simulador.Modelos.Fila [{0}={1};{2}={3};{4}={5};{6}={7};{8}={9}]";
            msg = string.Format(msg, nameof(Chegada), Chegada.ToString(), nameof(Atendimento), Atendimento.ToString(),
                nameof(Servidores), Servidores, nameof(Capacidade), Capacidade, nameof(QuantidadeDeClientes), QuantidadeDeClientes);
            return msg;
        }
    }
}
