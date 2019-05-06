using SimuladorDeRedeDeFilas.Geradores;
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
        public const int EXECUCOES = 5;
        public const int NUMEROS_ALEATORIOS = 100000;
        public static List<int> SEMENTES = new List<int>();
        public const int TAMANHO_FILA_INFINITA = 100;

        private double tempoPassado { get; set; }
        private double ultimoEvento { get; set; }

        private GeradorCongruenteLinear gerador { get; set; }

        public Simulador()
        {
            // TODO: Deixar o usuario selecionar.
            int a = 2342;
            int c = 11;
            gerador = new GeradorCongruenteLinear(a, c);
        }

        private List<double[]> calculaAsExecucoes(List<Fila> filas)
        {
            List<double[]> execucoes = new List<double[]>();
            for (int i = 0; i < filas.Count; i++)
            {
                execucoes.Add(new double[filas[i].Capacidade + 3]);
            }
            return execucoes;
        }

        private void zeraAsFilas(List<Fila> filas)
        {
            // Zera os dados para simular novamente.
            foreach (Fila fila in filas)
            {
                fila.TamanhoAtual = 0;
                fila.PerdaDeClientes = 0;
            }
        }

        private void configuraASementeAtual(int indexDaSemente)
        {
            if (SEMENTES.Count == 0)
            {
                gerador.ConfiguraSemente();
            }
            else
            {
                int sementeAtual = SEMENTES[indexDaSemente];
                gerador.ConfiguraSemente(sementeAtual);
            }
        }

        private List<double[]> configuraOsTempos(List<Fila> filas)
        {
            List<double[]> tempos = new List<double[]>();
            for (int i = 0; i < filas.Count; i++)
            {
                int tamanhoDoArray = filas[i].Capacidade + 2;
                double[] arrayDeTempos = new double[tamanhoDoArray];
                tempos.Add(arrayDeTempos);
            }
            return tempos;
        }

        private List<Evento> pegaOsEventosDaFila(List<Fila> filas)
        {
            List<Evento> eventos = new List<Evento>();
            foreach (Fila fila in filas)
            {
                if (fila.PrimeiraChegada >= 0.0)
                {
                    Evento evt = new Evento(TipoDeEvento.CHEGADA, fila.PrimeiraChegada, fila);
                    eventos.Add(evt);
                }
            }
            return eventos;
        }

        private Evento pegaOProximoEvento(List<Evento> eventos)
        {
            Evento evt = eventos.First();
            for (int i = 1; i < eventos.Count; i++)
            {
                Evento eventoAtual = eventos[i];
                if (eventoAtual.Tempo < evt.Tempo)
                {
                    evt = eventoAtual;
                }
            }
            eventos.Remove(evt);
            return evt;
        }

        /// <summary>
        /// Executa a simulação das filas.
        /// </summary>
        /// <param name="filas">Filas para executar.</param>
        public void Executar(List<Fila> filas)
        {
            List<double[]> execucoes = calculaAsExecucoes(filas);
            
            for (int execucao = 0; execucao < EXECUCOES; execucao++)
            {
                zeraAsFilas(filas);
                configuraASementeAtual(execucao);
                List<double[]> tempos = configuraOsTempos(filas);

                List<Evento> eventos = pegaOsEventosDaFila(filas);

                executa(filas, execucoes, tempos, eventos);
            }
        }

        private void atualizaOsTempos(List<Fila> filas, List<double[]> tempos, Evento proximoEvento)
        {
            tempoPassado = proximoEvento.Tempo - ultimoEvento;
            for (int i = 0; i < tempos.Count; i++)
            {
                Fila filaAux = filas[i];
                tempos[i][filaAux.TamanhoAtual] += tempoPassado;
                tempos[i][filaAux.Capacidade + 1] += proximoEvento.Tempo;
            }
            ultimoEvento = proximoEvento.Tempo;
        }

        private void executa(List<Fila> filas, List<double[]> execucoes, List<double[]> tempos, List<Evento> eventos)
        {
            for (int nrosAleatorios = 0; nrosAleatorios < NUMEROS_ALEATORIOS;)
            {
                Evento proximoEvento = pegaOProximoEvento(eventos);
                Fila filaDoEvento = proximoEvento.Origem;
                double[] arrayDeTemposDaFila = tempos[filaDoEvento.PosicaoNoArray];
                atualizaOsTempos(filas, tempos, proximoEvento);

                switch (proximoEvento.Tipo)
                {
                    case TipoDeEvento.CHEGADA:
                        if (filaDoEvento.TemEspaco)
                        {
                            filaDoEvento.TamanhoAtual++;

                            if (filaDoEvento.TemServidorSobrando)
                            {
                                double numeroAleatorio = gerador.Gera();
                                string destino = filaDoEvento.PegaDestino(numeroAleatorio);

                                double numeroAleatorioComMinMax = gerador.Gera(filaDoEvento.Atendimento.Minimo, filaDoEvento.Atendimento.Maximo);
                                double tempo1 = arrayDeTemposDaFila[filaDoEvento.Capacidade + 1] + numeroAleatorioComMinMax;
                                nrosAleatorios += 2;

                                if (string.IsNullOrWhiteSpace(destino))
                                {
                                    Evento evt1 = new Evento(TipoDeEvento.SAIDA, tempo1, filaDoEvento);
                                    eventos.Add(evt1);
                                }
                                else
                                {
                                    // TODO: Implementar a passagem.
                                    //events.add(new Event(EventType.PASSAGE, time, eventQueue, getQueueByName(queues, destiny)));
                                }
                            }
                        }
                        else
                        {
                            filaDoEvento.PerdaDeClientes++;
                        }

                        double tempo2 = arrayDeTemposDaFila[filaDoEvento.Capacidade + 1] + gerador.Gera(filaDoEvento.Chegada.Minimo, filaDoEvento.Chegada.Maximo);
                        Evento evt2 = new Evento(TipoDeEvento.CHEGADA, tempo2, filaDoEvento);
                        eventos.Add(evt2);

                        nrosAleatorios += 1;

                        break;

                    case TipoDeEvento.SAIDA:
                        // TODO: Implementar essa parte. (linha 126)
                        break;
                }
            }
        }

    }
}
