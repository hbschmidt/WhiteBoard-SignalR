using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using LoadingUCL.Sistema;

namespace LoadingUCL.SignalR
{
    [HubName("whiteboardHub")]
    public class WhiteboardHub : Hub
    {
        public void JoinGroup(string groupName)
        {

            Groups.Add(Context.ConnectionId, groupName);


        }
        public void JoinChat(string name, string groupName)
        {


            Clients.Group(groupName).ChatJoined(name, groupName);

        }

        public void SendDraw(string drawObject, string sessionId, string groupName, string name)
        {
            Clients.Group(groupName).HandleDraw(drawObject, sessionId, name);
        }

        public void SendChat(string message, string groupName, string name)
        {
            Clients.Group(groupName).Chat(name, message);
        }

        public void AddJogadorLista(string name, string groupName)
        {
            //if(Controle.ListaJoadores.Exists(x => x.Nome == name)) return;

            Controle.ListaJoadores.Add(Controle.ListaJoadores.Count == 0
                ? new Jogador() {Desenhando = true, Nome = name}
                : new Jogador() {Desenhando = false, Nome = name});

           
            Clients.Group(groupName).broadCastJogadores(Controle.ListaJoadores);


            if (Controle.ListaJoadores.Count != Controle.MinimoJogadores || Controle.PartidaIniciada) return;

            Controle.PartidaIniciada = true;

            Clients.Group(groupName).broadCastInicioPartida(Controle.ListaJoadores, groupName);
        }

        public bool VerficarSePodeDesenhar(string name, string groupName)
        {
            return Controle.ListaJoadores.Find(x => x.Nome == name).Desenhando;
        }

        public int QuantidadeDeJogadores()
        {
            return Controle.ListaJoadores.Count;
        }

        public void ReceberConfiguracao(int rodadas, int jogadores)
        {
            Controle.QuantidadeRodadas = rodadas;
            Controle.MinimoJogadores = jogadores;
        }

        public string PalavraDaRodada(bool sortear)
        {
            if (sortear) Controle.SortearPalavra();
            return Controle.PalavraRodada;
        }

        public void InicializarDadosDeRodada()
        {
            Controle.JogadoresQueAcertaramNaRodada = 0;
            Controle.RodadaAtual++;
        }

        public bool VerficarMensagemPontuar(string message, string nomeJogador)
        {
            if (!string.Equals(message, Controle.PalavraRodada, StringComparison.CurrentCultureIgnoreCase))
                return false;
            Controle.SetarPontuacao(nomeJogador);
            Controle.JogadoresQueAcertaramNaRodada++;
            return true;
        }

    }
}