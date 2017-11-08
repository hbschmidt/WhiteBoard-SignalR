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


            Clients.Group(groupName).ChatJoined(name);

        }

        public void SendDraw(string drawObject, string sessionId, string groupName, string name)
        {
            Clients.Group(groupName).HandleDraw(drawObject, sessionId, name);
        }

        public void SendChat(string message, string groupName, string name)
        {
            Clients.Group(groupName).Chat(name, message);
        }

        public void AddJogadorLista(string name)
        {
            Controle.ListaJoadores.Add(Controle.ListaJoadores.Count == 0
                ? new Jogador() {Desenhando = true, Nome = name}
                : new Jogador() {Desenhando = false, Nome = name});

            var indiceDesenhista = Controle.ListaJoadores.IndexOf(Controle.ListaJoadores.Find(x => x.Desenhando = true));

            Controle.ListaJoadores[indiceDesenhista].Desenhando = false;
            Controle.ListaJoadores[indiceDesenhista + 1].Desenhando = true;

            Clients.All.broadCastJogadores(Controle.ListaJoadores);
        }


    }
}