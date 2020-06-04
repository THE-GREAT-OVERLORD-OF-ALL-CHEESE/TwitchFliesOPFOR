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
                case PilotCommand.Bomb:
                    Actor target4 = GetActor(args[0]);
                    if (target4 != null)
                    {
                        waypoint.GetTransform().position = target4.position;
                        aircraftSpawn.BombWaypoint(waypoint, UnityEngine.Random.Range(0f, 360f), 5, aircraftSpawn.aiPilot.defaultAltitude);
                        return "Bombing " + target4.name + "!";
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
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
                case PilotCommand.Kamikaze:
                    Actor target5 = GetActor(args[0]);
                    if (target5 != null)
                    {
                        if (target5.role == Actor.Roles.Ground || target5.role == Actor.Roles.GroundArmor || target5.role == Actor.Roles.Ship) {
                            aircraftSpawn.aiPilot.gunRunMinAltitude = float.MinValue;
                            aircraftSpawn.aiPilot.gunGroundMaxRange = 50;
                            aircraftSpawn.aiPilot.minAltClimbThresh = float.MinValue;
                            aircraftSpawn.aiPilot.minAltitude = float.MinValue;
                            aircraftSpawn.aiPilot.obstacleCheckAheadTime = 0;
                            wm.MarkAllJettison();
                            wm.JettisonMarkedItems();
                            aircraftSpawn.AttackTarget(GetUnitReference(target5));
                            return "BANZAI!";
                        }
                        else
                        {
                            return "Can only kamikaze ground targets.";
                        }
                        
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
                    }
                default:
                    return base.Command(command, args);
            }
        }

        public override string SITREP()
        {
            AIAircraftSpawn aircraftSpawn = (AIAircraftSpawn)unitSpawn;

            string output = base.SITREP();
            output += "Mode: " + aircraftSpawn.aiPilot.commandState.ToString() + "\n";
            output += "Heading: " + Mathf.Round(aircraftSpawn.aiPilot.autoPilot.flightInfo.heading) + "\n";
            output += "Pitch: " + Mathf.Round(aircraftSpawn.aiPilot.autoPilot.flightInfo.pitch) + "\n";
            output += "Velocity: " + Mathf.Round(aircraftSpawn.aiPilot.autoPilot.flightInfo.surfaceSpeed * 1.94384f) + " knots\n";
            output += "Altitude: " + Mathf.Round(aircraftSpawn.aiPilot.autoPilot.flightInfo.altitudeASL * 3.28084f) + " feet\n";
            output += Mathf.Round(aircraftSpawn.aiPilot.autoPilot.flightInfo.currentInstantaneousG) + "G\n";
            return output;
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
