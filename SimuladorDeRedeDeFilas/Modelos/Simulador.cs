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
        public static int EXECUCOES = 5;
        public static int NUMEROS_ALEATORIOS = 100000;
        public static List<int> SEMENTES = new List<int>();
        public static int TAMANHO_DA_FILA_INFINITA = 100;

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

            for (int i = 0; i < execucoes.Count; i++)
            {
                double[] execucao = execucoes[i];

                for (int j = 0; j < execucao.Count(); j++)
                {
                    execucao[j] /= EXECUCOES;
                }

                Fila fila = filas[i];
                Console.WriteLine("--");
                Console.WriteLine(fila.ToString());

                for (int k = 0; k < fila.Capacidade; k++)
                {
                    Console.WriteLine("MEDIA DE TEMPO PARA FILA: " + execucao[k]);
                    if (execucao[k] == 0.0)
                        break;
                }
            }

            double[] primeiraExec = execucoes.First();
            double media = primeiraExec[primeiraExec.Count() - 2];
            Console.WriteLine("Média do tempo total de execução: " + media);
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
                                    Fila filaDestino = filas.First(x => x.Nome == destino);
                                    Evento evt1 = new Evento(TipoDeEvento.PASSAGEM, tempo1, filaDoEvento, filaDestino);
                                    eventos.Add(evt1);
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
                        filaDoEvento.TamanhoAtual--;

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
                                Fila filaDestino = filas.First(x => x.Nome == destino);
                                Evento evt1 = new Evento(TipoDeEvento.PASSAGEM, tempo1, filaDoEvento, filaDestino);
                                eventos.Add(evt1);
                            }
                        }

                        if (proximoEvento.Tipo == TipoDeEvento.PASSAGEM)
                        {
                            Fila filaDestino = proximoEvento.Destino;
                            if (filaDestino.TemEspaco)
                            {
                                filaDestino.TamanhoAtual++;

                                if (filaDestino.TemServidorSobrando)
                                {
                                    double[] temposDaFilaDestino = tempos[filaDestino.PosicaoNoArray];
                                    double proximoAleatorio = gerador.Gera();
                                    string destino = filaDestino.PegaDestino(proximoAleatorio);

                                    int arrayDoTempo = filaDestino.Capacidade + 1;
                                    proximoAleatorio = gerador.Gera(filaDestino.Atendimento.Minimo, filaDestino.Atendimento.Maximo);
                                    double tempo = temposDaFilaDestino[arrayDoTempo] + proximoAleatorio;

                                    nrosAleatorios += 2;

                                    if (string.IsNullOrWhiteSpace(destino))
                                    {
                                        Evento evt1 = new Evento(TipoDeEvento.SAIDA, tempo, filaDoEvento);
                                        eventos.Add(evt1);
                                    }
                                    else
                                    {
                                        filaDestino = filas.First(x => x.Nome == destino);
                                        Evento evt1 = new Evento(TipoDeEvento.PASSAGEM, tempo, filaDoEvento, filaDestino);
                                        eventos.Add(evt1);
                                    }
                                }
                            }
                            else
                            {
                                filaDestino.PerdaDeClientes++;
                            }
                        }

                        break;
                }
            }

            for (int i = 0; i < execucoes.Count; i++)
            {
                double[] execucao = execucoes[i];
                double[] tempo = tempos[i];
                for (int j = 0; j < tempo.Count(); j++)
                {
                    execucao[j] += tempo[j];
                }
                execucao[execucao.Count() - 1] += filas[i].PerdaDeClientes;
            }
        }

    }
}
