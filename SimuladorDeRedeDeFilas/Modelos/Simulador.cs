using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Modelos
{
    /// <summary>
    /// Simulador de rede de filas.
    /// </summary>
    public class Simulador
    {
        /// <summary>
        /// Fila a ser executada a simulação.
        /// </summary>
        private Fila fila { get; set; }

        /// <summary>
        /// Estado inicial da fila.
        /// </summary>
        private double estadoInicial { get; set; }

        /// <summary>
        /// Lista de números aleatórios para executar a simulação.
        /// </summary>
        private List<double> numerosAleatorios { get; set; }

        /// <summary>
        /// Lista de eventos ocorridos.
        /// </summary>
        private List<Evento> listaDeEventos { get; set; }

        /// <summary>
        /// Lista de tempos da fila.
        /// </summary>
        private List<double> temposDaFila { get; set; }

        /// <summary>
        /// Tempo total da simulação.
        /// </summary>
        private double tempoTotal { get; set; }

        /// <summary>
        /// Números de chegadas que caíram.
        /// </summary>
        private int perda { get; set; }

        /// <summary>
        /// Cria uma nova instância.
        /// </summary>
        /// <param name="fila">Fila para ser simulada.</param>
        /// <param name="estadoInicial">Estado inicial.</param>
        /// <param name="numerosAleatorios">Lista de números aleatórios para a execução.</param>
        public Simulador(Fila fila, double estadoInicial, List<double> numerosAleatorios)
        {
            this.fila = fila;
            this.estadoInicial = estadoInicial;
            this.numerosAleatorios = numerosAleatorios;

            listaDeEventos = new List<Evento>();

            temposDaFila = new List<double>();
            for (int i = 0; i < numerosAleatorios.Count; i++)
            {
                temposDaFila.Add(0.0);
            }
        }

        /// <summary>
        /// Executa a simulação da fila.
        /// </summary>
        public void Executar()
        {
            Console.WriteLine(fila.ToString());
            Console.WriteLine("Estado inicial: " + estadoInicial);
            //Console.WriteLine("Números aleatórios: [" + string.Join(",", numerosAleatorios) + "]");
            Console.WriteLine("----------------------");

            // Inicia a fila pelo estado inicial;
            chegada(estadoInicial);

            double menorTempo = 0;
            int posMenor = 0;

            // Executa até todos os números aleatórios serem utilizados.
            while (numerosAleatorios.Count > 0)
            {
                // Se a fila já atingiu sua capacidade..
                if (fila.QuantidadeDeClientes == fila.Capacidade)
                {
                    menorTempo = listaDeEventos[0].Tempo;
                    posMenor = 0;

                    // Separa o evento de menor tempo.
                    for (int i = 0; i < listaDeEventos.Count; i++)
                    {
                        if (listaDeEventos[posMenor].Tempo > listaDeEventos[i].Tempo)
                        {
                            menorTempo = listaDeEventos[i].Tempo;
                            posMenor = i;
                        }
                    }

                    // Executa chegada ou saída dependendo do evento.
                    if (listaDeEventos[posMenor].Tipo == TipoDeEvento.CHEGADA)
                    {
                        perda++;
                        listaDeEventos.RemoveAt(posMenor);
                        chegada(menorTempo);
                    }
                    else
                    {
                        saida(menorTempo);
                        listaDeEventos.RemoveAt(posMenor);
                    }
                }
                // Se não, caso ainda não esteja cheia..
                else
                {
                    menorTempo = listaDeEventos[0].Tempo;
                    posMenor = 0;
                    for (int i = 0; i < listaDeEventos.Count; i++)
                    {
                        if (listaDeEventos[posMenor].Tempo > listaDeEventos[i].Tempo)
                        {
                            menorTempo = listaDeEventos[i].Tempo;
                            posMenor = i;
                        }

                    }

                    // Executa chegada ou saída dependendo do evento.
                    if (listaDeEventos[posMenor].Tipo == TipoDeEvento.CHEGADA)
                    {
                        chegada(menorTempo);
                        listaDeEventos.RemoveAt(posMenor);
                    }
                    else
                    {
                        saida(menorTempo);
                        listaDeEventos.RemoveAt(posMenor);
                    }
                    menorTempo = 0;
                    posMenor = 0;
                }
            }

            // Apresenta os dados finais.
            for (int i = 0; i <= fila.Capacidade; i++)
            {
                Console.WriteLine("Tempo total que " + i + " ficaram na fila: " + temposDaFila[i]);
            }
            Console.WriteLine("Número de perdas: " + perda);
            Console.WriteLine("Tempo total da simulação: " + tempoTotal);
        }

        /// <summary>
        /// Evento de chegada.
        /// </summary>
        /// <param name="tempo">Tempo.</param>
        private void chegada(double tempo)
        {
            contabilizaTempo(tempo);
            if (fila.QuantidadeDeClientes < fila.Capacidade)
            {
                fila.QuantidadeDeClientes += 1;
                if (fila.QuantidadeDeClientes <= fila.Servidores)
                {
                    agendaSaida();
                }
            }
            agendaChegada();
        }

        /// <summary>
        /// Evento de saída.
        /// </summary>
        /// <param name="tempo">Tempo.</param>
        private void saida(double tempo)
        {
            contabilizaTempo(tempo);
            fila.QuantidadeDeClientes -= 1;
            if (fila.QuantidadeDeClientes >= fila.Servidores)
            {
                agendaSaida();
            }
        }

        /// <summary>
        /// Agenda um evento de chegada.
        /// </summary>
        private void agendaChegada()
        {
            double aux = numerosAleatorios.First();
            numerosAleatorios.RemoveAt(0);

            double tempoResultante = tempoTotal + (fila.Chegada.Maximo - fila.Chegada.Minimo * aux) + fila.Chegada.Minimo;
            Evento evento = new Evento(TipoDeEvento.CHEGADA, tempoResultante);
            listaDeEventos.Add(evento);
        }

        /// <summary>
        /// Agenda um evento de saída.
        /// </summary>
        private void agendaSaida()
        {
            double aux = numerosAleatorios.First();
            numerosAleatorios.RemoveAt(0);

            double tempoResultante = tempoTotal + (fila.Atendimento.Maximo - fila.Atendimento.Minimo * aux) + fila.Atendimento.Minimo;
            Evento evento = new Evento(TipoDeEvento.SAIDA, tempoResultante);
            listaDeEventos.Add(evento);
        }

        /// <summary>
        /// Contabiliza o tempo.
        /// </summary>
        /// <param name="tempo">Tempo.</param>
        private void contabilizaTempo(double tempo)
        {
            int aux = fila.QuantidadeDeClientes;

            double tempoAnterior = tempoTotal;
            tempoTotal = tempo;
            double posTemAux = tempoTotal - tempoAnterior;
            double tempoAux = temposDaFila[aux] + posTemAux;
            temposDaFila[aux] = tempoAux;
        }
    }
}
