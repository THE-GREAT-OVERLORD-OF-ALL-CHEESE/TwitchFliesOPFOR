using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotBoat : TwitchPilotBase
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            AISeaUnitSpawn shipSpawn = (AISeaUnitSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.North:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(0, 0, 2000);
                    shipSpawn.MoveTo(waypoint);
                    return "Ship moving 2k North.";
                case PilotCommand.East:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(2000, 0, 0);
                    shipSpawn.MoveTo(waypoint);
                    return "Ship moving 2k East.";
                case PilotCommand.South:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(0, 0, -2000);
                    shipSpawn.MoveTo(waypoint);
                    return "Ship moving 2k South.";
                case PilotCommand.West:
                    waypoint.GetTransform().position = actor.transform.position + new Vector3(-2000, 0, 0);
                    shipSpawn.MoveTo(waypoint);
                    return "Ship moving 2k West.";
                default:
                    return base.Command(command, args);
            }
        }
    }
}
