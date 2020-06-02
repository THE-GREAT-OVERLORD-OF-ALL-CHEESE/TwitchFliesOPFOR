using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotAircraft : TwitchPilotBase
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            AIAircraftSpawn aircraftSpawn = (AIAircraftSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.Attack:
                    Actor target = GetActor(args[0]);
                    if (target != null) {
                        aircraftSpawn.AttackTarget(GetUnitReference(target));

                        return "Attacking " + target.name + "!";
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
                    }
                case PilotCommand.Cancel:
                    aircraftSpawn.CancelAttackTarget();
                    return "Canceling attack.";
                case PilotCommand.Orbit:
                    Actor target2 = GetActor(args[0]);
                    if (target2 != null)
                    {
                        waypoint.GetTransform().position = target2.position;
                        aircraftSpawn.SetOrbitNow(waypoint, 5000, 5000);
                        return "Orbiting  " + target2.name + "!";
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
                    }
                case PilotCommand.Formation:
                    Actor target3 = GetActor(args[0]);
                    if (target3 != null)
                    {
                        aircraftSpawn.FormOnPilot(GetUnitReference(target3));
                        return "Following  " + target3.name + "!";
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
                    }
                case PilotCommand.TakeOff:
                    aircraftSpawn.TakeOff();
                    return "Taking off!";
                case PilotCommand.RTB:
                    aircraftSpawn.RearmAt(new AirportReference(args[0]));
                    return "Landing!";
                case PilotCommand.A2ARefuel:
                    RefuelPlane rp = FindRefuelTanker();
                    if (rp != null)
                    {
                        aircraftSpawn.RefuelWithUnit(GetUnitReference(rp.actor));
                        return "Refueling!";
                    }
                    else
                    {
                        return "Found no refuel plane.";
                    }
                case PilotCommand.CM:
                    aircraftSpawn.CountermeasureProgram(true, true, 3, 0.2f);
                    return "Deploying CMs!";
                case PilotCommand.Flare:
                    aircraftSpawn.CountermeasureProgram(true, false, 3, 0.2f);
                    return "Deploying flares!";
                case PilotCommand.Chaff:
                    aircraftSpawn.CountermeasureProgram(false, true, 3, 0.2f);
                    return "Deploying chaff!";
                case PilotCommand.JetisonEmpty:
                    wm.MarkEmptyToJettison();
                    wm.JettisonMarkedItems();
                    return "Jettisoning empty!";
                case PilotCommand.JetisonFuel:
                    wm.MarkDroptanksToJettison();
                    wm.JettisonMarkedItems();
                    return "Jettisoning droptanks!";
                case PilotCommand.Jetison:
                    wm.MarkAllJettison();
                    wm.JettisonMarkedItems();
                    return "Jettisoning all weapons!";
                case PilotCommand.Eject:
                    actor.health.Kill();
                    return "Punch out!";
                default:
                    return base.Command(command, args);
            }
        }

        Runway GetRunway()
        {
            Vector3 b = VTMapManager.GlobalToWorldPoint(new Vector3D(actor.transform.position));
            Runway result = null;
            float num = float.MaxValue;
            foreach (AirportManager airportManager in VTMapManager.fetch.airports)
            {
                foreach (Runway runway in airportManager.runways)
                {
                    if (runway)
                    {
                        float sqrMagnitude = (runway.transform.position - b).sqrMagnitude;
                        if (sqrMagnitude < num)
                        {
                            result = runway;
                            num = sqrMagnitude;
                        }
                    }
                }
            }
            return result;
        }

        AirportManager GetAirport()
        {
            Vector3 b = VTMapManager.GlobalToWorldPoint(new Vector3D(actor.transform.position));
            AirportManager result = null;
            float num = float.MaxValue;
            foreach (AirportManager airportManager in VTScenario.current.GetAllAirports())
            {
                foreach (Runway runway in airportManager.runways)
                {
                    if (runway)
                    {
                        float sqrMagnitude = (runway.transform.position - b).sqrMagnitude;
                        if (sqrMagnitude < num)
                        {
                            result = airportManager;
                            num = sqrMagnitude;
                        }
                    }
                }
            }
            return result;
        }


        public AirportReference GetAirportReference(AirportManager airport)
        {
            List<AirportManager> airports = VTScenario.current.GetAllAirports();
            List<string> ids =  VTScenario.current.GetAllAirportIDs();
            string result = "";
            for(int i = 0; i < airports.Count; i++)
            {
                if (airports[i] == airport) {
                    result = ids[0];
                }
            }
            return new AirportReference(result);
        }

        AICarrierSpawn GetCarrier()
        {
            AICarrierSpawn result = UnityEngine.Object.FindObjectsOfType<AICarrierSpawn>()[0];
            return result;
        }

        RefuelPlane FindRefuelTanker()
        {
            RefuelPlane result = UnityEngine.Object.FindObjectsOfType<RefuelPlane>()[0];
            return result;
        }

        int GetSpawnID(AICarrierSpawn carrier) {
            Vector3 b = VTMapManager.GlobalToWorldPoint(new Vector3D(actor.transform.position));
            int result = 0;
            float num = float.MaxValue;
            for (int i = 0; i < carrier.spawnPoints.Count; i++)
            {
                float sqrMagnitude = (carrier.spawnPoints[i].spawnTf.position - b).sqrMagnitude;
                if (sqrMagnitude < num)
                {
                    result = i;
                    num = sqrMagnitude;
                }
            }
            return result;
        }
    }
}
