using System;
using System.Collections.Generic;

namespace LoadingUCL.Sistema
{
    public static class Controle
    {
        public static string GroupId = "";

        public const int PontuacaoMaximaNaRodada = 50;

        public static List<Jogador> ListaJoadores = new List<Jogador>();

        public static List<string> ListaDeDesenhos = new List<string>();

        public static bool PartidaIniciada = false;

        public static string PalavraRodada;

        public static int JogadoresQueAcertaramNaRodada = 0;

        public static int QuantidadeRodadas = 0;

        public static int RodadaAtual = 0;

        public static int MinimoJogadores;

        static Controle()
        {
            ListaDeDesenhos.Add("casa");
            ListaDeDesenhos.Add("avião");
            ListaDeDesenhos.Add("carro");
            ListaDeDesenhos.Add("faculdade");
            ListaDeDesenhos.Add("igreja");
            ListaDeDesenhos.Add("cinema");
            ListaDeDesenhos.Add("futebol");
            ListaDeDesenhos.Add("camisa");
            ListaDeDesenhos.Add("praia");
            ListaDeDesenhos.Add("tênis");
            ListaDeDesenhos.Add("computador");
            ListaDeDesenhos.Add("cafeteira");
            ListaDeDesenhos.Add("arvore");
            ListaDeDesenhos.Add("cavalo");
            ListaDeDesenhos.Add("cachorro");
            ListaDeDesenhos.Add("balão");
            ListaDeDesenhos.Add("festa");
            ListaDeDesenhos.Add("montanha");
            ListaDeDesenhos.Add("pirâmide");
            ListaDeDesenhos.Add("buraco negro");
        }

        public static void SortearPalavra()
        {
            var valores = DateTime.Now.Millisecond;

            var rdn = new Random(valores);

            PalavraRodada = ListaDeDesenhos[rdn.Next(0, ListaDeDesenhos.Count - 1)];

        }

        public static string PalavraAnterior = "";

        public static int testeQtd = 0;

        public static void PassarToken()
        {
            var indiceDesenhista = ListaJoadores.IndexOf(ListaJoadores.Find(x => x.Desenhando == true));
           

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

        public static void SetarPontuacao(string nomeJogador)
        {
            var jogador = ListaJoadores.Find(x => x.Nome == nomeJogador);

            jogador.Pontuacao += (PontuacaoMaximaNaRodada - (JogadoresQueAcertaramNaRodada - 1)* 2);
        }

        public static void ZerarVerificacaoDePontosDaRodada()
        {
            foreach (var j in ListaJoadores)
                j.PontuacaoVerificada = false;
        }
    }
}