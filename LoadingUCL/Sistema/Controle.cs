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

        public static void PassarToken()
        {
            int indiceDesenhista = ListaJoadores.IndexOf(ListaJoadores.Find(x => x.Desenhando = true));

            ListaJoadores[indiceDesenhista].Desenhando = false;

            if (indiceDesenhista == ListaJoadores.Count - 1)
            {
                ListaJoadores[0].Desenhando = true;
            }
            else
            {
                ListaJoadores[indiceDesenhista + 1].Desenhando = true;
            }
        }

    }
}