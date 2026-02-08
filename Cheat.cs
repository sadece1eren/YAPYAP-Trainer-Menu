using Photon.Pun;
using Photon.Realtime;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.Arm;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace YapYapTrainer
{
    class Cheat : MonoBehaviour
    {
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
            if (Input.GetKeyDown(KeyCode.Insert))
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

                case 2:
                    windowRect = GUI.Window(0, windowRect, menu3, "YAPYAP Menu V 1.0");
                    break;

                default:
                    Debug.LogError("Invalid tab index!");
                    break;
            }

            HandleDragging(ref windowRect, ref isDragging1, ref offset1);
        }

        private void menu(int id)
        {
            windowRect.width = 600f;
            windowRect.height = 555f;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("TAB 1", GUILayout.Width(100f)))
                selectedTab = 0;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 2", GUILayout.Width(100f)))
                selectedTab = 1;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 3", GUILayout.Width(100f)))
                selectedTab = 2;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Button("TEST 1", GUILayout.Width(500f));
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        private void menu2(int id)
        {
            windowRect.width = 600f;
            windowRect.height = 555f;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("TAB 1", GUILayout.Width(100f)))
                selectedTab = 0;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 2", GUILayout.Width(100f)))
                selectedTab = 1;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 3", GUILayout.Width(100f)))
                selectedTab = 2;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Button("TEST 2", GUILayout.Width(500f));
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        private void menu3(int id)
        {
            windowRect.width = 600f;
            windowRect.height = 555f;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("TAB 1", GUILayout.Width(100f)))
                selectedTab = 0;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 2", GUILayout.Width(100f)))
                selectedTab = 1;

            GUILayout.Space(56);

            if (GUILayout.Button("TAB 3", GUILayout.Width(100f)))
                selectedTab = 2;

            GUILayout.Space(56);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Button("TEST 3", GUILayout.Width(500f));
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
