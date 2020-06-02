using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotArty : TwitchPilotTank
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            ArtilleryUnitSpawn artySpawn = (ArtilleryUnitSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.Attack:
                    artySpawn.ParkNow();
                    Actor target = GetActor(args[0]);
                    if (target != null)
                    {
                        waypoint.GetTransform().position = target.position;
                        artySpawn.FireOnWaypoint(waypoint, 1);
                        return "Attacking " + target.name + "!";
                    }
                    else
                    {
                        return "No such target " + args[0] + " exists.";
                    }
                default:
                    return base.Command(command, args);
            }
        }
    }
}
