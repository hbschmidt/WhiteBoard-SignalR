using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            Controle.GroupId = groupName;
        }
        public void JoinChat(string name, string groupName)
        {
            Clients.Group(groupName).ChatJoined(name, groupName);
        }

        public void SendDraw(string drawObject, string sessionId, string groupName, string name)
        {
            Clients.Group(groupName).HandleDraw(drawObject, sessionId, name);
        }

        public void SendEraser(string drawObject, string sessionId, string groupName, string name)
        {
            Clients.Group(groupName).HandleEraser(drawObject, sessionId, name);
        }

        public void SendChat(string message, string groupName, string name)
        {
            Clients.Group(groupName).Chat(name, message);
        }

        public void AddJogadorLista(string name, string groupName)
        {
            //if(Controle.ListaJoadores.Exists(x => x.Nome == name)) return;

            Controle.ListaJoadores.Add(Controle.ListaJoadores.Count == 0
                ? new Jogador() { Desenhando = true, Nome = name }
                : new Jogador() { Desenhando = false, Nome = name });


            Clients.Group(groupName).broadCastJogadores(Controle.ListaJoadores);

            if (Controle.ListaJoadores.Count != Controle.MinimoJogadores || Controle.PartidaIniciada) return;

            InicializarDadosDeRodada();

            Clients.Group(groupName).broadCastInicioPartida(Controle.ListaJoadores, groupName, Controle.RodadaAtual);
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
            Controle.PartidaIniciada = true;
        }

        public bool VerficarMensagemPontuar(string message, string nomeJogadorEnviouMensagem, string nomeJogadorDaSecao)
        {
            VerificarPalavraAnterior(message);

            Controle.ListaJoadores.Find(x => x.Nome == nomeJogadorDaSecao).PontuacaoVerificada = true;

            //Se o jogador da seção n é o mesmo da mensagem enviada, retornar verdadeiro se acertou a msg para colocar no painel
            if (!string.Equals(nomeJogadorEnviouMensagem, nomeJogadorDaSecao, StringComparison.CurrentCultureIgnoreCase))
                return string.Equals(message, Controle.PalavraRodada, StringComparison.CurrentCultureIgnoreCase);

            if (!string.Equals(message, Controle.PalavraRodada, StringComparison.CurrentCultureIgnoreCase))
                return false;

            Controle.JogadoresQueAcertaramNaRodada++;
            Controle.SetarPontuacao(nomeJogadorEnviouMensagem);

            if (VerficarFimDaRodada())
            {
                Controle.PartidaIniciada = false;
                if (!VerificarFimPartida())
                {
                    InicializarRodada();
                }               
            }


            return true;
        }

        private bool VerificarFimPartida()
        {
            if (Controle.RodadaAtual >= Controle.QuantidadeRodadas)
            {
                //Decretar o fim
                var max = Controle.ListaJoadores.Max(x => x.Pontuacao);
                var result = Controle.ListaJoadores.First(x => x.Pontuacao == max);

                Clients.Group(Controle.GroupId).FinalizarPartida(Controle.ListaJoadores, result);

                return true;
            }

            return false;
        }

        public void VerificarPalavraAnterior(string message)
        {
            if (message != Controle.PalavraAnterior)
            {
                Controle.ZerarVerificacaoDePontosDaRodada();
            }

            Controle.PalavraAnterior = message;
        }

        public bool VerficarFimDaRodada()
        {
            var exists = Controle.ListaJoadores.Exists(jogador => jogador.PontuacaoVerificada = false);
            return Controle.JogadoresQueAcertaramNaRodada == (Controle.ListaJoadores.Count - 1) && !exists;
        }

        public void InicializarRodada()
        {
            if (Controle.PartidaIniciada) return;

            Controle.PassarToken();

            Controle.ZerarVerificacaoDePontosDaRodada();

            InicializarDadosDeRodada();

            Clients.Group(Controle.GroupId).broadCastInicioPartida(Controle.ListaJoadores, Controle.GroupId, Controle.RodadaAtual);
        }

        public bool VerificouTodasAsVezes()
        {
            var exists = Controle.ListaJoadores.Exists(jogador => jogador.PontuacaoVerificada = false);
            return !exists;
        }

    }
}