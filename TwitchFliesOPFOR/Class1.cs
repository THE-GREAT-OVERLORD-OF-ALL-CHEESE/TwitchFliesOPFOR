using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using System.IO;

namespace TwitchFliesOPFOR
{
    public class TwitchFliesOPFOR : VTOLMOD
    {
        string USERNAME_FROM_OAUTH_TOKEN = "";
        string OAUTH_TOKEN = "";
        string help = "";
        private Client _client;
        TwitchPilotManager manager;
        TwitchChat chat;
        VTOLAPI vtol;

        public override void ModLoaded()
        {
            string address = Directory.GetCurrentDirectory() + @"\VTOLVR_ModLoader\mods\Twitch Flies OPFOR\";
            USERNAME_FROM_OAUTH_TOKEN = File.ReadAllText(address + "channel.txt");
            OAUTH_TOKEN = File.ReadAllText(address + "token.txt");
            help = File.ReadAllText(address + "help.txt");

            VTOLAPI.SceneLoaded += SceneLoaded;

            vtol = new VTOLAPI();

            //USERNAME_FROM_OAUTH_TOKEN = File.ReadAllText("username.txt")
            //OAUTH_TOKEN = File.ReadAllText("token.txt");

            chat = new TwitchChat();
            chat.StartTwitchChat();
            manager = new TwitchPilotManager();
            manager.Start();
            manager.vtol = vtol;
            ConnectTwitchClient();

            base.ModLoaded();
        }

        private void SceneLoaded(VTOLScenes scene)
        {
            chat.Create3DChat(vtol.GetPlayersVehicleGameObject(), VTOLAPI.GetPlayersVehicleEnum());
        }

        void ConnectTwitchClient()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(USERNAME_FROM_OAUTH_TOKEN, OAUTH_TOKEN);

            // Create new instance of Chat Client
            _client = new Client();

            // Initialize the client with the credentials instance, and setting a default channel to connect to.
            _client.Initialize(credentials, USERNAME_FROM_OAUTH_TOKEN);

            // Bind callbacks to events
            _client.OnConnected += OnConnected;
            _client.OnJoinedChannel += OnJoinedChannel;
            _client.OnMessageReceived += OnMessageReceived;
            _client.OnChatCommandReceived += OnChatCommandReceived;

            // Connect
            _client.Connect();
        }

        private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
                Debug.Log($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            Debug.Log($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
            _client.SendMessage(e.Channel, "VTOL VR connected!");
        }

        private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            Debug.Log($"Message received from {e.ChatMessage.Username}: {e.ChatMessage.Message}");
            chat.NewMessage(new TwitchChat.Message(e.ChatMessage.Username, e.ChatMessage.Message));
        }

        private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
        {
            string username = e.Command.ChatMessage.Username;
            List<string> args = e.Command.ArgumentsAsList;
            string response = "";

            switch (e.Command.CommandText.ToLower())
            {
                case "register":
                    response = manager.TryRegister(username, args[0]);
                    break;
                case "release":
                    response = manager.Release(username);
                    break;
                case "help":
                    response = help;
                    break;
                case "n":
                case "north":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.North, args);
                    break;
                case "e":
                case "east":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.East, args);
                    break;
                case "s":
                case "south":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.South, args);
                    break;
                case "w":
                case "west":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.West, args);
                    break;
                case "stop":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Stop, args);
                    break;
                case "orbit":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Orbit, args);
                    break;
                case "form":
                case "formup":
                case "formation":
                case "follow":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Formation, args);
                    break;
                case "scramble":
                case "launchall":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.LaunchAll, args);
                    break;
                case "engage":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Engage, args);
                    break;
                case "disengage":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Disengage, args);
                    break;
                case "attack":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Attack, args);
                    break;
                case "cancel":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Cancel, args);
                    break;
                case "takeoff":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.TakeOff, args);
                    break;
                case "land":
                case "rtb":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.RTB, args);
                    break;
                case "refuel":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.A2ARefuel, args);
                    break;
                case "fox2":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Fox2, args);
                    break;
                case "fox3":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Fox3, args);
                    break;
                case "cm":
                case "cms":
                case "countermeasure":
                case "countermeasures":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.CM, args);
                    break;
                case "chaff":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Chaff, args);
                    break;
                case "flare":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Flare, args);
                    break;
                case "jettisonempty":
                case "dropempty":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.JetisonEmpty, args);
                    break;
                case "jettisonfuel":
                case "jettisontank":
                case "jettisontanks":
                case "jettisondroptank":
                case "jettisondroptanks":
                case "dropfuel":
                case "droptank":
                case "droptanks":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.JetisonFuel, args);
                    break;
                case "jettison":
                case "drop":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Jetison, args);
                    break;
                case "eject":
                    response = manager.Command(username, TwitchPilotBase.PilotCommand.Eject, args);
                    break;
                default:
                    _client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
                    break;
            }

            if (response != "") {
                _client.SendMessage(e.Command.ChatMessage.Channel, response);
            }
        }

        void LateUpdate()
        {
            manager.UpdateNameTags();
        }

        void FixedUpdate()
        {
            manager.ReleaseDead();
        }

        private void OnGUI()
        {
            chat.DrawChat();
            if (TargetManager.instance != null)
            {
                string temp = "";
                for (int i = 0; i < TargetManager.instance.allActors.Count; i++)
                {
                    temp += TargetManager.instance.allActors[i].name + "\n";
                }
                GUI.TextArea(new Rect(1920 - 300, 100, 200, 800), temp);
            }
            if (VTScenario.current != null)
            {
                string temp2 = "";
                foreach (string airportId in VTScenario.current.GetAllAirportIDs())
                {
                    temp2 += airportId + "\n";
                }
                GUI.TextArea(new Rect(1920 - 400, 100, 100, 100), temp2);
            }
        }

        public string StringCombiner(List<string> args) {
            string result = "";
            for (int i = 0; i < args.Count; i++)
            {
                result += TargetManager.instance.allActors[i].actorName + " ";
                if (i < args.Count - 1) {
                    result += " ";
                }
            }
            return result;
        }
    }
}
