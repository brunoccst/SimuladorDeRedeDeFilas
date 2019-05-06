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
        public const double PROBABILIDADE_MAXIMA = 1.0;
        public const double NAO_TEM_CHEGADAS_EXTERNAS = -1.0;

        public string Nome { get; set; }

        public double PrimeiraChegada { get; set; }

        /// <summary>
        /// Tempo de chegada.
        /// </summary>
        public Intervalo Chegada { get; private set; }

        /// <summary>
        /// Tempo de atendimento.
        /// </summary>
        public Intervalo Atendimento { get; private set; }

        /// <summary>
        /// Número de servidores (x).
        /// </summary>
        public int Servidores { get; private set; }

        /// <summary>
        /// Capacidade de clientes (y).
        /// </summary>
        public int Capacidade { get; private set; }

        public List<string> Filas { get; set; }

        public List<double> Probabilidades { get; set; }

        public double SomaDasProbabilidades
        {
            get
            {
                return Probabilidades.Sum();
            }
        }

        public int PosicaoNoArray { get; set; }

        public int TamanhoAtual { get; set; }

        public int PerdaDeClientes { get; set; }

        public bool TemEspaco
        {
            get
            {
                return TamanhoAtual < Capacidade;
            }
        }

        public bool TemServidorSobrando
        {
            get
            {
                return TamanhoAtual < Servidores;
            }
        }

        public Fila(string nome, int servidores, int capacidade, Intervalo tAtendimento)
            : this(nome, servidores, capacidade, null, tAtendimento) { }

        public Fila(string nome, int servidores, int capacidade, Intervalo tChegada, Intervalo tAtendimento)
        {
            Nome = nome;
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

            Filas = new List<string>();
            Probabilidades = new List<double>();
            PrimeiraChegada = NAO_TEM_CHEGADAS_EXTERNAS;

            if (tChegada == null)
            {
                tChegada = new Intervalo(NAO_TEM_CHEGADAS_EXTERNAS, NAO_TEM_CHEGADAS_EXTERNAS);
            }
        }

        public bool AdicionaDestino(string fila, double probabilidade)
        {
            // Só pode adicionar o destino se a probabilidade não ultrapassar a probabilidade máxima.
            if (SomaDasProbabilidades + probabilidade > PROBABILIDADE_MAXIMA)
            {
                return false;
            }
            else
            {
                Filas.Add(fila);
                Probabilidades.Add(probabilidade);
                return true;
            }
        }

        public string PegaDestino(double numeroAleatorio)
        {
            double probabilidade = 0.0;
            string fila = null;

            for (int i = 0; i < Probabilidades.Count; i++)
            {
                probabilidade += Probabilidades[i];
                if (numeroAleatorio <= probabilidade)
                {
                    fila = Filas[i];
                    break;
                }
            }

            return fila;
        }

        public override string ToString()
        {
            // Coloca todas as propriedades do objeto no formato especificado.
            string formatoDePropriedade = "{0}={1}";
            List<string> propriedades = new List<string>();
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Nome), Nome));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(PrimeiraChegada), PrimeiraChegada));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Chegada), Chegada.ToString()));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Atendimento), Atendimento.ToString()));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Servidores), Servidores));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Capacidade), Capacidade));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Filas), string.Join(",", Filas)));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Probabilidades), string.Join(",", Probabilidades)));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(SomaDasProbabilidades), SomaDasProbabilidades));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(PosicaoNoArray), PosicaoNoArray));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(TamanhoAtual), TamanhoAtual));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(PerdaDeClientes), PerdaDeClientes));

            // Gera a representação em string final com as propriedades.
            string propriedadesStr = string.Join(";", propriedades);
            string resultado = string.Format("Fila [{0}]", propriedadesStr);

            return resultado;
        }
    }
}
