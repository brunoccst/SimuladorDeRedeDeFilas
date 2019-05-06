using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorDeRedeDeFilas.Geradores
{
    public class GeradorCongruenteLinear
    {
        public int A { get; set; }
        public int C { get; set; }
        public int M { get; set; }
        public long Semente { get; set; }
        public double NumeroAleatorio { get; set; }

        public GeradorCongruenteLinear(int a, int c) : this(a, c, int.MaxValue) { }

        public GeradorCongruenteLinear(int a, int c, int m)
        {
            A = a;
            C = c;
            M = m;
        }
        
        public void ConfiguraSemente()
        {
            var agora = DateTime.Now;
            var dataBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (agora.ToUniversalTime() - dataBase).TotalMilliseconds;

            ConfiguraSemente((long)unixDateTime);
        }

        public void ConfiguraSemente(long semente)
        {
            Semente = semente;
        }

        public double Gera()
        {
            Semente = (A * Semente + C) % M;
            NumeroAleatorio = (double)Semente/ M;
            return NumeroAleatorio;
        }

        public double Gera(double min, double max)
        {
            Gera();
            return (max - min) * NumeroAleatorio + min;
        }
    }
}
