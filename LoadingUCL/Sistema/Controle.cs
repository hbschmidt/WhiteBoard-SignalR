using System.Collections.Generic;

namespace LoadingUCL.Sistema
{
    public static class Controle
    {
        public static List<Jogador> ListaJoadores = new List<Jogador>();

        public static List<string>ListaDeDesenhos = new List<string>();

        static Controle()
        {
            ListaDeDesenhos.Add("casa");
            ListaDeDesenhos.Add("aviao");

        }

    }
}