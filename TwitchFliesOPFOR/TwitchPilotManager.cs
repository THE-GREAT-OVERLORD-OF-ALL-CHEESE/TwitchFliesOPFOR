using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotManager
    {
        public enum ActorType
        {
            Plane,
            Boat,
            Tank,
            Carrier,
            DroneCarrier,
            Artillery,
            Other
        }

        public List<TwitchPilotBase> pilots;

        public void Start()
        {
            pilots = new List<TwitchPilotBase>();
        }

        public string TryRegister(string username, string targetName)
        {
            Actor actor = GetActor(targetName);
            if (actor != null) {
                return Register(actor, username);
            }
            else
            {
                return username + ", could not find an actor called: " + targetName;
            }
        }

        public string Register(Actor actor, string username)
        {
            if (GetTwitchPilot(username) == null) {
                if (IsActorTaken(actor))
                {
                    return username + ", that vehicle is already taken.";
                }
                else
                {
                    if (actor.gameObject == VTOLAPI.instance.GetPlayersVehicleGameObject())
                    {
                        return "You aren't allowed to control the player.";
                    }
                    else
                    {
                        ActorType type = GetActorType(actor);
                        string message = "you have control!";

                        TwitchPilotBase pilot = null;
                        switch (type)
                        {
                            case ActorType.Plane:
                                TwitchPilotAircraft temp = new TwitchPilotAircraft();
                                pilot = temp;
                                break;
                            case ActorType.Boat:
                                TwitchPilotBoat temp2 = new TwitchPilotBoat();
                                pilot = temp2;
                                pilot.nameTagHeight = 50;
                                pilot.fontSize = 50;
                                message = "you have boat!";
                                break;
                            case ActorType.Carrier:
                                TwitchPilotCarrier temp3 = new TwitchPilotCarrier();
                                pilot = temp3;
                                pilot.nameTagHeight = 50;
                                pilot.fontSize = 50;
                                message = "you have boat!";
                                break;
                            case ActorType.DroneCarrier:
                                TwitchPilotDroneCarrier temp4 = new TwitchPilotDroneCarrier();
                                pilot = temp4;
                                pilot.nameTagHeight = 50;
                                pilot.fontSize = 50;
                                message = "you have boat!";
                                break;
                            case ActorType.Tank:
                                TwitchPilotTank temp5 = new TwitchPilotTank();
                                pilot = temp5;
                                message = "PANZER VOR!";
                                break;
                            case ActorType.Artillery:
                                TwitchPilotArty temp6 = new TwitchPilotArty();
                                pilot = temp6;
                                break;
                            default:
                                TwitchPilotBase temp7 = new TwitchPilotBase();
                                pilot = temp7;
                                break;
                        }
                        pilot.actor = actor;
                        pilot.wm = pilot.actor.weaponManager;
                        pilot.username = username;
                        pilot.player = VTOLAPI.instance.GetPlayersVehicleGameObject();
                        pilot.Start();
                        pilots.Add(pilot);
                        return username + ", " + message;
                    }
                }
            }
            else
            {
                return username + ", you already have control of a vehicle.";
            }
        }

        public string Command(string username, TwitchPilotBase.PilotCommand command, List<string> args)
        {
            TwitchPilotBase pilot = GetTwitchPilot(username);
            if (pilot != null)
            {
                return pilot.Command(command, args);
            }
            else
            {
                return username + ", you dont have a vehicle, type \"!register <actor_name>\" to get control of a vehicle";
            }
        }

        public string Release(string username)
        {
            TwitchPilotBase pilot = GetTwitchPilot(username);
            if (pilot != null)
            {
                int id = GetPilotId(username);
                pilot.Remove();
                pilots.RemoveAt(id);
                return "Control released.";
            }
            else {
                return "You have no vehicle.";
            }
        }

        public void ReleaseDead()
        {
            string pilotName = "";
            for (int i = 0; i < pilots.Count; i++)
            {
                if (!pilots[i].actor.alive) {
                    pilotName = pilots[i].username;
                }
            }
            if (pilotName != "") {
                Release(pilotName);
            }
        }

        public void UpdateNameTags() {
            for (int i = 0; i < pilots.Count; i++)
            {
                pilots[i].UpdateNameTag();
            }
        }

        public ActorType GetActorType(Actor actor)
        {
            ActorType type = ActorType.Other;
            if (actor.unitSpawn as AIAircraftSpawn != null) {
                type = ActorType.Plane;
            }
            if (actor.unitSpawn as AISeaUnitSpawn != null)
            {
                type = ActorType.Boat;
            }
            if (actor.unitSpawn as AICarrierSpawn != null)
            {
                type = ActorType.Carrier;
            }
            if (actor.unitSpawn as AIDroneCarrierSpawn != null)
            {
                type = ActorType.DroneCarrier;
            }
            if (actor.unitSpawn as GroundUnitSpawn != null)
            {
                type = ActorType.Tank;
            }
            if (actor.unitSpawn as ArtilleryUnitSpawn != null)
            {
                type = ActorType.Artillery;
            }
            Debug.Log(actor.name + " is a " + type.ToString());
            return type;
        }

        public Actor GetActor(string name)
        {
            Actor result = null;
            for (int i = 0; i < TargetManager.instance.allActors.Count; i++)
            {
                if (TargetManager.instance.allActors[i].name.ToLower().Contains(name.ToLower()))
                {
                    result = TargetManager.instance.allActors[i];
                }
            }
            return result;
        }

        public TwitchPilotBase GetTwitchPilot(string username)
        {
            TwitchPilotBase result = null;
            for (int i = 0; i < pilots.Count; i++)
            {
                if (pilots[i].username == username)
                {
                    result = pilots[i];
                }
            }
            return result;
        }

        public int GetPilotId(string username)
        {
            int result = -1;
            for (int i = 0; i < pilots.Count; i++)
            {
                if (pilots[i].username == username)
                {
                    result = i;
                }
            }
            return result;
        }

        public bool IsActorTaken(Actor actor)
        {
            bool result = false;
            for (int i = 0; i < pilots.Count; i++)
            {
                if (pilots[i].actor == actor)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
