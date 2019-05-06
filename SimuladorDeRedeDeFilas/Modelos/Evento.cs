using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Modelos
{
    /// <summary>
    /// Tipos de evento.
    /// </summary>
    public enum TipoDeEvento
    {
        /// <summary>
        /// Uma chegada de cliente.
        /// </summary>
        CHEGADA = 1,

        /// <summary>
        /// Uma saída de cliente.
        /// </summary>
        SAIDA = 0
    }

    /// <summary>
    /// Um evento ocorrido na fila.
    /// </summary>
    public class Evento
    {
        /// <summary>
        /// Tipo de evento.
        /// </summary>
        public TipoDeEvento Tipo { get; set; }

        /// <summary>
        /// Tempo gasto do evento.
        /// </summary>
        public double Tempo { get; set; }

        /// <summary>
        /// Fila de origem.
        /// </summary>
        public Fila Origem { get; set; }
        
        /// <summary>
        /// Fila de destino.
        /// </summary>
        public Fila Destino { get; set; }

        /// <summary>
        /// Cria uma nova instância sem fila de destino.
        /// </summary>
        /// <param name="tipo">Tipo.</param>
        /// <param name="tempo">Tempo</param>
        /// <param name="origem">Fila de origem.</param>
        public Evento(TipoDeEvento tipo, double tempo, Fila origem) : this(tipo, tempo, origem, null) { }

        /// <summary>
        /// Cria uma nova instância.
        /// </summary>
        /// <param name="tipo">Tipo.</param>
        /// <param name="tempo">Tempo</param>
        /// <param name="origem">Fila de origem.</param>
        /// <param name="destino">Fila de destino.</param>
        public Evento(TipoDeEvento tipo, double tempo, Fila origem, Fila destino)
        {
            Tipo = tipo;
            Tempo = tempo;
            Origem = origem;
            Destino = destino;
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
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Tipo), Tipo));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Tempo), Tempo));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Origem), Origem.ToString()));
            propriedades.Add(string.Format(formatoDePropriedade, nameof(Destino), Destino.ToString()));

            // Gera a representação em string final com as propriedades.
            string propriedadesStr = string.Join(";", propriedades);
            string resultado = string.Format("Evento [{0}]", propriedadesStr);

            return resultado;
        }

    }
}
