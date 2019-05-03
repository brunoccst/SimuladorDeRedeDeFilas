using SimuladorDeRedeDeFilas.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimuladorDeRedeDeFilas.Modelos;
using SimuladorDeRedeDeFilas.Geradores;

namespace SimuladorDeRedeDeFilas
{
    public partial class Main : Form
    {
        private GeradorCongruenteLinear gerador { get; set; }

        private Simulador simulador { get; set; }

        public Main()
        {
            InitializeComponent();

            Tempo tChegada = new Tempo(0, 1);
            Tempo tAtendimento = new Tempo(0, 1);
            int servidores = 1;
            int capacidade = 2;
            Fila fila = new Fila(tChegada, tAtendimento, servidores, capacidade);

            double estadoInicial = 1.0;

            gerador = new GeradorCongruenteLinear();

            int a = 1;
            int c = 2;
            int M = 3;
            int x0 = 4;
            int length = 5;
            List<double> numerosAleatorios = gerador.Gerar(a, c, M, x0, length).ToList();
            simulador = new Simulador(fila, estadoInicial, numerosAleatorios);
        }
    }
}
