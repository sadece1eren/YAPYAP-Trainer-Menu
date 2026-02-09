using Mirror.Examples.CCU;
using Photon.Pun;
using Photon.Realtime;
using Pooling;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utilities;
using YAPYAP;
using static Unity.Burst.Intrinsics.Arm;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace YapYapTrainer
{
    class Cheat : MonoBehaviour
    {
        private string messageInput = "";
        private bool godmode;
        private bool ghost = false;
        private bool stamina = false;
        private bool menushow = true;
        private int selectedTab = 0;
        private Rect windowRect = new Rect(Screen.width / 2 - 300f, Screen.height / 2 - 175f, 600f, 350f);
        private int mainWID = 1024;
        private bool isDragging1 = true;
        private Vector2 offset1 = Vector2.zero;
        private Color selectedColor = Color.white;
        private Texture2D CreateTextureFromColor(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        void KeyBoardStuff()
        {
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                menushow = !menushow;
            }
            if (Input.GetKeyDown(KeyCode.End))
            {
                Loader.UnLoad();
            }
        }

        void Update()
        {
            KeyBoardStuff();
            if (godmode)
            {
               
            }
            if (stamina)
            {
                var staminastuff = GameObject.FindObjectsOfType<PawnBlackboard>();
                foreach (YAPYAP.PawnBlackboard staminathing in staminastuff)
                {
                    staminathing.Stamina = 90f;
                }
            }
        }

        void OnGUI()
        {
            if (!menushow)
                return;

            switch (selectedTab)
            {
                case 0:
                    windowRect = GUI.Window(0, windowRect, menu, "YAPYAP Menu V 1.0");
                    break;

                case 1:
                    windowRect = GUI.Window(0, windowRect, menu2, "YAPYAP Menu V 1.0");
                    break;
                default:
                    Debug.LogError("Invalid tab index!");
                    break;
            }

            HandleDragging(ref windowRect, ref isDragging1, ref offset1);
        }

        private void menu(int id)
        {
            windowRect.width = 480f;
            windowRect.height = 430f;
            GUIStyle windowStyle = new GUIStyle(GUI.skin.window);
            Color hoverColor = new Color(0f, 51f / 255f, 102f / 255f);
            Texture2D hoverTexture = new Texture2D(1, 1);
            hoverTexture.SetPixel(0, 0, hoverColor);
            hoverTexture.Apply();
            windowStyle.normal.background = hoverTexture;
            windowStyle.onNormal.background = hoverTexture;
            windowStyle.onHover.background = hoverTexture;
            GUI.skin.window = windowStyle;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Self And Server Trolls", GUILayout.Width(200f)))
                selectedTab = 0;

            GUILayout.Space(56);

            if (GUILayout.Button("Credits", GUILayout.Width(200f)))
                selectedTab = 1;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill  Player", GUILayout.Width(145), GUILayout.Height(52)))
            {
                var healthstuff = GameObject.FindObjectsOfType<YAPYAP.PawnHurtbox>();
                foreach (YAPYAP.PawnHurtbox healthdata in healthstuff)
                {
                    healthdata.SvDoKill();
                }
            }
            if (GUILayout.Button("Revive  Player", GUILayout.Width(145), GUILayout.Height(52)))
            {
                var healthstuff = GameObject.FindObjectsOfType<YAPYAP.PawnHurtbox>();
                foreach (YAPYAP.PawnHurtbox healthdata in healthstuff)
                {
                    healthdata.SvDoRevive();
                }
            }
            if (GUILayout.Button("End Round", GUILayout.Width(155), GUILayout.Height(52)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SvEndRound();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Round", GUILayout.Width(145), GUILayout.Height(52)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SvStartRound();
                }
            }
            ghost = GUILayout.Toggle(ghost, "Spawn Ghost", GUILayout.Width(145), GUILayout.Height(52));

            if (ghost)
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.NetworkghostSpawned = true;
                }
            }
            else
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.NetworkghostSpawned = false;
                }
            }

            stamina = GUILayout.Toggle(stamina, "Inf Stamina", GUILayout.Width(145), GUILayout.Height(52));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Inf Points", GUILayout.Width(145), GUILayout.Height(52)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SetTotalScore(9999999);
                }
            }
            if (GUILayout.Button("Ragdoll Players", GUILayout.Width(145), GUILayout.Height(52)))
            {
                var players = GameObject.FindObjectsOfType<YAPYAP.Pawn>();
                foreach (YAPYAP.Pawn playerss in players)
                {
                    playerss.SvSetRagdoll(-playerss.transform.forward * 15f + Vector3.up * 5f);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(400));

            GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 16;
            GUILayout.Label("Players", headerStyle);
            GUILayout.Space(5);
            Vector2 scroll = Vector2.zero;
            scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(200));
            var allPawns = UnityEngine.Object.FindObjectsOfType<YAPYAP.Pawn>();
            foreach (var pawn in allPawns)
            {
                if (pawn == null) continue;
                string playerId = pawn.PlayerId ?? pawn.GetInstanceID().ToString();
                GUILayout.BeginHorizontal();
                GUIStyle playerLabelStyle = new GUIStyle(GUI.skin.label);
                playerLabelStyle.alignment = TextAnchor.MiddleCenter;
                GUILayout.Label($"ID: {playerId}", playerLabelStyle, GUILayout.Width(200), GUILayout.Height(50));
                if (GUILayout.Button("Kill", GUILayout.Width(65), GUILayout.Height(50)))
                    pawn.Hurtbox?.SvDoKill();
                if (GUILayout.Button("Revive", GUILayout.Width(65), GUILayout.Height(50)))
                    pawn.Hurtbox?.SvDoRevive();
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();


        }

        private void menu2(int id)
        {
            windowRect.width = 480f;
            windowRect.height = 400f;
            GUIStyle windowStyle = new GUIStyle(GUI.skin.window);
            Color hoverColor = new Color(0f, 51f / 255f, 102f / 255f);
            Texture2D hoverTexture = new Texture2D(1, 1);
            hoverTexture.SetPixel(0, 0, hoverColor);
            hoverTexture.Apply();
            windowStyle.normal.background = hoverTexture;
            windowStyle.onNormal.background = hoverTexture;
            windowStyle.onHover.background = hoverTexture;
            GUI.skin.window = windowStyle;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Self And Server Trolls", GUILayout.Width(200f)))
                selectedTab = 0;

            GUILayout.Space(56);

            if (GUILayout.Button("Credits", GUILayout.Width(200f)))
                selectedTab = 1;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            //i will code this part later lol
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        void HandleDragging(ref Rect window, ref bool isDragging, ref Vector2 offset)
        {
            if (Event.current.type == EventType.MouseDown &&
                Event.current.button == 0 &&
                window.Contains(Event.current.mousePosition))
            {
                isDragging = true;
                offset = Event.current.mousePosition - new Vector2(window.x, window.y);
            }

            if (isDragging && Event.current.type == EventType.MouseDrag)
            {
                window.position = Event.current.mousePosition - offset;
            }

            if (Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
            }
        }
    }
}
