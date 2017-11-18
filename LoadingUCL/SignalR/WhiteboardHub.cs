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


            if (Controle.ListaJoadores.Count != 2 || Controle.PartidaIniciada) return;

            Controle.PartidaIniciada = true;
            Clients.Group(groupName).broadCastInicioPartida(Controle.ListaJoadores, groupName);
        }

        public bool VerficarSePodeDesenhar(string name, string groupName)
        {
            return Controle.ListaJoadores.Find(x => x.Nome == name).Desenhando;
        }


    }
}