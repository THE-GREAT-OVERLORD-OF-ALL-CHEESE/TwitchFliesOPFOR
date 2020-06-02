using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwitchFliesOPFOR
{
    class TwitchPilotCarrier : TwitchPilotBoat
    {
        public override string Command(PilotCommand command, List<string> args)
        {
            AICarrierSpawn carrierSpawn = (AICarrierSpawn)unitSpawn;
            switch (command)
            {
                case PilotCommand.LaunchAll:
                    carrierSpawn.LaunchAllAircraft();
                    return "Scrambling all aircraft!";
                default:
                    return base.Command(command, args);
            }
        }
    }
}
