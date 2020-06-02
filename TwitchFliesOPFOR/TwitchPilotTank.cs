using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotTank : TwitchPilotBase
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            GroundUnitSpawn groundSpawn = (GroundUnitSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.North:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(0, 0, 500);
                    groundSpawn.MoveTo(waypoint);
                    return "Tank moving 500m North!";
                case PilotCommand.East:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(500, 0, 0);
                    groundSpawn.MoveTo(waypoint);
                    return "Tank moving 500m East!";
                case PilotCommand.South:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(0, 0, -500);
                    groundSpawn.MoveTo(waypoint);
                    return "Tank moving 500m South!";
                case PilotCommand.West:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(-500, 0, 0);
                    groundSpawn.MoveTo(waypoint);
                    return "Tank moving 500m West!";
                case PilotCommand.Stop:
                    groundSpawn.ParkNow();
                    return "Tank stopping.";
                default:
                    return base.Command(command, args);
            }
        }
    }
}
