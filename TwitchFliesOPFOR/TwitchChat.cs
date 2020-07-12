using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace TwitchFliesOPFOR
{
    class TwitchChat
    {
        public class Message
        {
            public string user;
            public string message;

            public Message(string sender, string nMessage)
            {
                user = sender;
                message = nMessage;
            }
        }

        public List<Message> messages;
        public int maximumMessages = 16;
        public TextMeshPro textMesh;

        public void StartTwitchChat()
        {
            messages = new List<Message>();
            Create3DChat(null, VTOLVehicles.None, VTOLScenes.ReadyRoom);
            NewMessage(new Message("TwitchFliesOPFOR", "Mod Loaded!"));
        }

        public void NewMessage(Message message)
        {
            messages.Add(message);
            if (messages.Count > maximumMessages)
            {
                messages.RemoveAt(0);
            }

            Update3DChat();
        }

        public void Create3DChat(GameObject jet, VTOLVehicles type, VTOLScenes scene) {
            GameObject temp = new GameObject();
            GameObject temp3 = UnityEngine.Object.Instantiate(temp);

            textMesh = temp3.AddComponent<TextMeshPro>();
            textMesh.outlineColor = Color.black;
            textMesh.outlineWidth = 0.2f;
            textMesh.fontSize = 0.5f;
            textMesh.enableWordWrapping = false;
            textMesh.alignment = TextAlignmentOptions.Bottom;
            textMesh.richText = false;

            if (jet != null) {
                textMesh.transform.parent = jet.transform;
                switch (type) {
                    case VTOLVehicles.AV42C:
                        textMesh.transform.localPosition = new Vector3(-1, 0.4f, 0);
                        break;
                    case VTOLVehicles.FA26B:
                        textMesh.transform.localPosition = new Vector3(-1, 0.5f, 5.5f);
                        break;
                    case VTOLVehicles.F45A:
                        textMesh.transform.localPosition = new Vector3(-1, 0, 6);
                        break;
                    default:
                        textMesh.transform.localPosition = new Vector3(-1, 1, 0);
                        break;
                }
                textMesh.transform.localPosition += Vector3.up * 3;
                textMesh.transform.LookAt(textMesh.transform.position + -jet.transform.right);
            }
            else {
                switch (scene)
                {
                    case VTOLScenes.ReadyRoom:
                        textMesh.transform.position = new Vector3(-3, -0.5f, -0.5f);
                        break;
                    default:
                        textMesh.transform.position = new Vector3(0, 1, 1);
                        break;
                }
                textMesh.transform.position += Vector3.up * 3;
            }
            Update3DChat();
        }

        public void Update3DChat()
        {
            if (textMesh != null) {
                string temp = "";
                for (int i = 0; i < messages.Count; i++)
                {
                    temp += messages[i].user + ": " + messages[i].message + "\n";
                }
                textMesh.text = temp;
            }
        }

        public void DrawChat()
        {
            string temp = "";
            for (int i = 0; i < messages.Count; i++) {
                temp += messages[i].user + ": " + messages[i].message + "\n";
            }
            GUI.TextArea(new Rect(100, 100, 500, 300), temp);
        }
    }
}
