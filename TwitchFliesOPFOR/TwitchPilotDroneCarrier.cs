using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotDroneCarrier : TwitchPilotBoat
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            AIDroneCarrierSpawn carrierSpawn = (AIDroneCarrierSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.LaunchAll:
                    carrierSpawn.LaunchDrones();
                    return "Launching all drones!";
                default:
                    return base.Command(command, args);
            }
        }
    }
}
