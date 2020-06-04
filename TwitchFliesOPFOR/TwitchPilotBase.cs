using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace TwitchFliesOPFOR
{
    class TwitchPilotBase
    {
        public GameObject player;

        public Actor actor;
        public AIUnitSpawn unitSpawn;
        public WeaponManager wm;
        public Waypoint waypoint;

        public string username;
        
        public TextMeshPro textMesh;

        public float fontSize = 36;
        public float nameTagHeight = 10;

        public enum PilotCommand{
            Engage,
            Disengage,
            Attack,
            Cancel,
            North,
            East,
            South,
            West,
            Stop,
            Orbit,
            Formation,
            LaunchAll,
            TakeOff,
            RTB,
            A2ARefuel,
            Bomb,
            CM,
            Chaff,
            Flare,
            JetisonEmpty,
            JetisonFuel,
            Jetison,
            Eject,
            Kamikaze
        }

        public void Start()
        {
            waypoint = new Waypoint();
            GameObject temp = new GameObject();
            GameObject temp2 = UnityEngine.Object.Instantiate(temp);
            temp2.AddComponent<FloatingOriginTransform>();
            waypoint.SetTransform(temp2.transform);

            GameObject temp3 = UnityEngine.Object.Instantiate(temp);

            textMesh = temp3.AddComponent<TextMeshPro>();
            textMesh.text = username;
            textMesh.outlineColor = Color.black;
            textMesh.outlineWidth = 0.2f;
            textMesh.fontSize = fontSize;
            textMesh.enableWordWrapping = false;
            textMesh.alignment = TextAlignmentOptions.Center;

            unitSpawn = (AIUnitSpawn)actor.unitSpawn;
        }

        public void Remove()
        {
            if (waypoint.GetTransform() != null)
            {
                GameObject.Destroy(waypoint.GetTransform().gameObject);
            }
            if (textMesh != null) {
                GameObject.Destroy(textMesh.gameObject);
            }
        }

        public void UpdateNameTag()
        {
            if (textMesh != null && actor != null)
            {
                textMesh.transform.position = actor.position + new Vector3(0, nameTagHeight, 0);
                textMesh.transform.LookAt(textMesh.transform.position + (textMesh.transform.position - player.transform.position));
            }
        }

        public virtual string Command(PilotCommand command, List<string> args)
        {
            switch (command)
            {
                case PilotCommand.Engage:
                    unitSpawn.SetEngageEnemies(true);
                    return "Engaging!";
                case PilotCommand.Disengage:
                    unitSpawn.SetEngageEnemies(false);
                    return "Disengaging!";
                default:
                    return "This vehicle doesnt support that command.";
            }
        }

        public virtual string SITREP()
        {
            string output = actor.name + "\n";
            output += "Controlled by: " + username + "\n";
            output += "Health: " + unitSpawn.health.currentHealth + "/" + unitSpawn.health.maxHealth + "\n";
            if (unitSpawn.engageEnemies)
            {
                output += "Fire at will!\n";
            }
            else {
                output += "Cease fire.\n";
            }
            return output;
        }

        public Actor GetActor(string name)
        {
            Actor result = null;
            for (int i = 0; i < TargetManager.instance.allActors.Count; i++)
            {
                if (TargetManager.instance.allActors[i].name.ToLower().Contains(name.ToLower())) {
                    result = TargetManager.instance.allActors[i];
                }
            }
            return result;
        }

        public UnitReference GetUnitReference(Actor target)
        {
            UnitReference reference = new UnitReference(target.unitSpawn.unitID);
            return reference;
        }

        public int GetHeading(Transform transform)
        {
            int heading = Mathf.RoundToInt(Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg);
            return heading;
        }
    }
}
