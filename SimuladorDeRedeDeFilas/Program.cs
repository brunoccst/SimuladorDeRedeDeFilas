using SimuladorDeRedeDeFilas.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorDeRedeDeFilas
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            List<Fila> filas = new List<Fila>();

            string caminho;
            if (args.Length == 1)
            {
                caminho = args[0];
            }
            else
            {
                Console.WriteLine("Digite o caminho do arquivo desejado e pressione enter:");
                caminho = Console.ReadLine();
            }

            try
            {
                using (StreamReader leitor = new StreamReader(caminho))
                {
                    while (!leitor.EndOfStream)
                    {
                        string linha = leitor.ReadLine();

                        // TAMANHO_DA_FILA_INFINITA
                        if (linha.StartsWith("TAMANHO_DA_FILA_INFINITA"))
                        {
                            string valor = leitor.ReadLine().Trim();
                            Simulador.TAMANHO_DA_FILA_INFINITA = Convert.ToInt32(valor);
                        }
                        // FILAS
                        else if (linha.StartsWith("FILAS"))
                        {
                            int posicaoNoArray = 0;
                            while (true)
                            {
                                string[] valores = leitor.ReadLine().Split(' ');

                                // Linha em branco, passar para proxima entrada
                                if (valores.Length == 0 || "".Equals(valores[0]))
                                {
                                    break;
                                }
                                // Quatro valores, fila INFINITA que NAO possui entrada externa
                                else if (valores.Length == 4)
                                {
                                    var tAtendimento = new Intervalo(double.Parse(valores[2]), double.Parse(valores[3]));
                                    filas.Add(new Fila(valores[0], Convert.ToInt32(valores[1]), Simulador.TAMANHO_DA_FILA_INFINITA, tAtendimento));
                                }
                                // Cinco valores, fila que NAO possui entrada externa
                                else if (valores.Length == 5)
                                {
                                    var tAtendimento = new Intervalo(double.Parse(valores[3]), double.Parse(valores[4]));
                                    filas.Add(new Fila(valores[0], Convert.ToInt32(valores[1]), Convert.ToInt32(valores[2]), tAtendimento));
                                }
                                // Seis valores, fila INFINITA que possui entrada externa
                                else if (valores.Length == 6)
                                {
                                    var tChegada = new Intervalo(double.Parse(valores[2]), double.Parse(valores[3]));
                                    var tAtendimento = new Intervalo(double.Parse(valores[4]), double.Parse(valores[5]));
                                    filas.Add(new Fila(valores[0], Convert.ToInt32(valores[1]), Simulador.TAMANHO_DA_FILA_INFINITA, tChegada, tAtendimento));
                                }
                                // Sete valores, fila que possui entrada externa
                                else if (valores.Length == 7)
                                {
                                    var tChegada = new Intervalo(double.Parse(valores[3]), double.Parse(valores[4]));
                                    var tAtendimento = new Intervalo(double.Parse(valores[5]), double.Parse(valores[6]));
                                    filas.Add(new Fila(valores[0], Convert.ToInt32(valores[1]), Convert.ToInt32(valores[2]), tChegada, tAtendimento));
                                }
                                // Entrada inválida
                                else
                                {
                                    throw new ArgumentException("Ocorreu um erro ao processar o seu arquivo na tag FILAS.");
                                }
                                filas[filas.Count - 1].PosicaoNoArray = posicaoNoArray++;
                            }
                        }
                        // PRIMEIRA_CHEGADA
                        else if (linha.StartsWith("PRIMEIRA_CHEGADA"))
                        {
                            if (filas.Count == 0)
                            {
                                throw new ArgumentException("Você não definiu nenhum valor para a tag PRIMEIRA_CHEGADA.");
                            }
                            while (true)
                            {
                                string[] valores = leitor.ReadLine().Split(' ');

                                // Linha em branco, passar para proxima entrada
                                if (valores.Length == 0 || "".Equals(valores[0]))
                                {
                                    break;
                                }
                                // Dois valores, nome da fila e primeira chegada
                                else if (valores.Length == 2)
                                {
                                    foreach (Fila fila in filas)
                                    {
                                        if (valores[0].Equals(fila.Nome))
                                        {
                                            fila.PrimeiraChegada = double.Parse(valores[1]);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Ocorreu um erro ao processar o seu arquivo na tag PRIMEIRA_CHEGADA.");
                                }
                            }
                        }
                        // REDE
                        else if (linha.StartsWith("REDE"))
                        {
                            if (filas.Count == 0)
                            {
                                throw new ArgumentException("Você não definiu nenhum valor para a tag REDE.");
                            }
                            while (true)
                            {
                                string[] valores = leitor.ReadLine().Split(' ');

                                if (valores.Length == 0 || "".Equals(valores[0]))
                                {
                                    break;
                                }
                                // Tres valores, nome fila origem, nome fila destino e probabilidade
                                else if (valores.Length == 3)
                                {
                                    foreach (Fila fila in filas)
                                    {
                                        if (valores[0].Equals(fila.Nome))
                                        {
                                            if (fila.AdicionaDestino(valores[1], double.Parse(valores[2])))
                                                break;
                                            else
                                                throw new ArgumentException("Soma das probabilidades para " + valores[0] + " é maior que 1.0.");
                                        }
                                    }
                                }
                                else
                                {
                                    throw new ArgumentException("Ocorreu um erro ao processar o seu arquivo na tag REDE.");
                                }
                            }
                        }
                        // NUMERO_DE_ALEATORIOS_POR_SEMENTE
                        else if (linha.StartsWith("NUMERO_DE_ALEATORIOS_POR_SEMENTE"))
                        {
                            string[] values = leitor.ReadLine().Split(' ');
                            Simulador.NUMEROS_ALEATORIOS = Convert.ToInt32(values[0]);
                        }
                        // NUMERO_DE_SEMENTES
                        else if (linha.StartsWith("NUMERO_DE_SEMENTES"))
                        {
                            string[] values = leitor.ReadLine().Split(' ');
                            Simulador.EXECUCOES = Convert.ToInt32(values[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
                return;
            }

            Simulador s = new Simulador();
            s.Executar(filas);
            Console.ReadKey();
        }
    }
}
