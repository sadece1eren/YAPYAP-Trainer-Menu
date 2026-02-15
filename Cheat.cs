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
        private bool dropprops = false;
        private bool spawnvoid = false;
        private bool gold = false;
        private bool stamina = false;
        private bool menushow = true;
        private int selectedTab = 0;
        private Rect windowRect = new Rect(Screen.width / 2 - 340f, Screen.height / 2 - 250f, 680f, 500f);
        private int mainWID = 1024;
        private bool isDragging1 = false;
        private Vector2 offset1 = Vector2.zero;
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 creditsScrollPosition = Vector2.zero;
        private CursorLockMode previousCursorLockMode;
        private bool previousCursorVisible;
        private float animationTime = 0f;
        private float pulseAnimation = 0f;
        private float tabSwitchAnimation = 0f;
        private GUIStyle windowStyle;
        private GUIStyle headerStyle;
        private GUIStyle tabButtonStyle;
        private GUIStyle tabButtonActiveStyle;
        private GUIStyle modernButtonStyle;
        private GUIStyle toggleStyle;
        private GUIStyle playerBoxStyle;
        private GUIStyle titleStyle;
        private GUIStyle subtitleStyle;
        private GUIStyle textStyle;
        private bool stylesInitialized = false;
        private Texture2D bgTexture;
        private Texture2D buttonTexture;
        private Texture2D buttonHoverTexture;
        private Texture2D buttonActiveTexture;
        private Texture2D toggleOnTexture;
        private Texture2D toggleOffTexture;
        private Texture2D panelTexture;
        private Texture2D headerTexture;

        void Start()
        {
            InitializeTextures();
        }

        void InitializeTextures()
        {
            bgTexture = CreateGradientTexture(
                new Color(0.08f, 0.08f, 0.12f, 0.98f),
                new Color(0.05f, 0.05f, 0.08f, 0.98f)
            );
            buttonTexture = CreateRoundedGradientTexture(
                new Color(0.15f, 0.15f, 0.22f, 1f),
                new Color(0.12f, 0.12f, 0.18f, 1f),
                32, 32
            );
            buttonHoverTexture = CreateRoundedGradientTexture(
                new Color(0.25f, 0.25f, 0.35f, 1f),
                new Color(0.20f, 0.20f, 0.28f, 1f),
                32, 32
            );
            buttonActiveTexture = CreateRoundedGradientTexture(
                new Color(0.2f, 0.4f, 0.8f, 1f),
                new Color(0.15f, 0.3f, 0.6f, 1f),
                32, 32
            );
            toggleOnTexture = CreateRoundedGradientTexture(
                new Color(0.2f, 0.8f, 0.4f, 1f),
                new Color(0.15f, 0.6f, 0.3f, 1f),
                32, 32
            );
            toggleOffTexture = CreateRoundedGradientTexture(
                new Color(0.2f, 0.2f, 0.25f, 1f),
                new Color(0.15f, 0.15f, 0.2f, 1f),
                32, 32
            );
            panelTexture = CreateGradientTexture(
                new Color(0.1f, 0.1f, 0.15f, 0.9f),
                new Color(0.08f, 0.08f, 0.12f, 0.9f)
            );
            headerTexture = CreateGradientTexture(
                new Color(0.1f, 0.3f, 0.5f, 1f),
                new Color(0.05f, 0.15f, 0.3f, 1f)
            );
        }

        Texture2D CreateRoundedGradientTexture(Color topColor, Color bottomColor, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height);
            float cornerRadius = 8f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float t = y / (float)height;
                    Color gradientColor = Color.Lerp(bottomColor, topColor, t);
                    float distToCorner = 0f;
                    if (x < cornerRadius && y > height - cornerRadius)
                    {
                        float dx = cornerRadius - x;
                        float dy = (height - cornerRadius) - y;
                        distToCorner = Mathf.Sqrt(dx * dx + dy * dy);
                        if (distToCorner > cornerRadius)
                        {
                            gradientColor.a = 0f;
                        }
                        else
                        {
                            gradientColor.a *= 1f - (distToCorner / cornerRadius) * 0.3f;
                        }
                    }
                    else if (x > width - cornerRadius && y > height - cornerRadius)
                    {
                        float dx = x - (width - cornerRadius);
                        float dy = (height - cornerRadius) - y;
                        distToCorner = Mathf.Sqrt(dx * dx + dy * dy);
                        if (distToCorner > cornerRadius)
                        {
                            gradientColor.a = 0f;
                        }
                        else
                        {
                            gradientColor.a *= 1f - (distToCorner / cornerRadius) * 0.3f;
                        }
                    }
                    else if (x < cornerRadius && y < cornerRadius)
                    {
                        float dx = cornerRadius - x;
                        float dy = cornerRadius - y;
                        distToCorner = Mathf.Sqrt(dx * dx + dy * dy);
                        if (distToCorner > cornerRadius)
                        {
                            gradientColor.a = 0f;
                        }
                        else
                        {
                            gradientColor.a *= 1f - (distToCorner / cornerRadius) * 0.3f;
                        }
                    }
                    else if (x > width - cornerRadius && y < cornerRadius)
                    {
                        float dx = x - (width - cornerRadius);
                        float dy = cornerRadius - y;
                        distToCorner = Mathf.Sqrt(dx * dx + dy * dy);
                        if (distToCorner > cornerRadius)
                        {
                            gradientColor.a = 0f;
                        }
                        else
                        {
                            gradientColor.a *= 1f - (distToCorner / cornerRadius) * 0.3f;
                        }
                    }

                    texture.SetPixel(x, y, gradientColor);
                }
            }
            texture.Apply();
            return texture;
        }

        Texture2D CreateGradientTexture(Color topColor, Color bottomColor)
        {
            Texture2D texture = new Texture2D(1, 32);
            for (int i = 0; i < 32; i++)
            {
                float t = i / 31f;
                texture.SetPixel(0, i, Color.Lerp(bottomColor, topColor, t));
            }
            texture.Apply();
            return texture;
        }

        Texture2D CreateSolidTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        void InitializeStyles()
        {
            if (stylesInitialized) return;
            windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.normal.background = bgTexture;
            windowStyle.onNormal.background = bgTexture;
            windowStyle.hover.background = bgTexture;
            windowStyle.onHover.background = bgTexture;
            windowStyle.focused.background = bgTexture;
            windowStyle.onFocused.background = bgTexture;
            windowStyle.active.background = bgTexture;
            windowStyle.onActive.background = bgTexture;
            windowStyle.border = new RectOffset(12, 12, 24, 12);
            windowStyle.padding = new RectOffset(15, 15, 30, 15);
            windowStyle.normal.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.hover.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.onNormal.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.onHover.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.focused.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.onFocused.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.active.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.onActive.textColor = new Color(0.9f, 0.9f, 1f);
            windowStyle.fontSize = 18;
            windowStyle.fontStyle = FontStyle.Bold;
            windowStyle.alignment = TextAnchor.UpperCenter;
            headerStyle = new GUIStyle(GUI.skin.label);
            headerStyle.normal.background = headerTexture;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 22;
            headerStyle.normal.textColor = new Color(0.5f, 0.9f, 1f);
            headerStyle.padding = new RectOffset(10, 10, 12, 12);
            tabButtonStyle = new GUIStyle(GUI.skin.button);
            tabButtonStyle.normal.background = buttonTexture;
            tabButtonStyle.hover.background = buttonHoverTexture;
            tabButtonStyle.active.background = buttonActiveTexture;
            tabButtonStyle.normal.textColor = new Color(0.7f, 0.7f, 0.8f);
            tabButtonStyle.hover.textColor = new Color(0.9f, 0.9f, 1f);
            tabButtonStyle.active.textColor = Color.white;
            tabButtonStyle.fontSize = 14;
            tabButtonStyle.fontStyle = FontStyle.Bold;
            tabButtonStyle.border = new RectOffset(10, 10, 10, 10);
            tabButtonStyle.margin = new RectOffset(2, 2, 2, 2);
            tabButtonStyle.padding = new RectOffset(12, 12, 10, 10);
            tabButtonActiveStyle = new GUIStyle(tabButtonStyle);
            tabButtonActiveStyle.normal.background = buttonActiveTexture;
            tabButtonActiveStyle.normal.textColor = Color.white;
            modernButtonStyle = new GUIStyle(GUI.skin.button);
            modernButtonStyle.normal.background = buttonTexture;
            modernButtonStyle.hover.background = buttonHoverTexture;
            modernButtonStyle.active.background = buttonActiveTexture;
            modernButtonStyle.normal.textColor = new Color(0.8f, 0.8f, 0.9f);
            modernButtonStyle.hover.textColor = new Color(1f, 1f, 1f);
            modernButtonStyle.active.textColor = new Color(0.5f, 0.9f, 1f);
            modernButtonStyle.fontSize = 13;
            modernButtonStyle.fontStyle = FontStyle.Bold;
            modernButtonStyle.border = new RectOffset(10, 10, 10, 10);
            modernButtonStyle.padding = new RectOffset(10, 10, 8, 8);
            modernButtonStyle.alignment = TextAnchor.MiddleCenter;
            toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.normal.background = toggleOffTexture;
            toggleStyle.onNormal.background = toggleOnTexture;
            toggleStyle.hover.background = buttonHoverTexture;
            toggleStyle.onHover.background = CreateRoundedGradientTexture(
                new Color(0.25f, 0.9f, 0.5f, 1f),
                new Color(0.2f, 0.7f, 0.4f, 1f),
                32, 32
            );
            toggleStyle.normal.textColor = new Color(0.7f, 0.7f, 0.8f);
            toggleStyle.onNormal.textColor = Color.white;
            toggleStyle.hover.textColor = new Color(0.9f, 0.9f, 1f);
            toggleStyle.onHover.textColor = Color.white;
            toggleStyle.fontSize = 13;
            toggleStyle.fontStyle = FontStyle.Bold;
            toggleStyle.padding = new RectOffset(28, 10, 7, 7);
            toggleStyle.border = new RectOffset(10, 10, 10, 10);
            toggleStyle.alignment = TextAnchor.MiddleLeft;
            toggleStyle.contentOffset = new Vector2(0, 0);
            toggleStyle.fixedHeight = 30;
            playerBoxStyle = new GUIStyle(GUI.skin.box);
            playerBoxStyle.normal.background = panelTexture;
            playerBoxStyle.border = new RectOffset(8, 8, 8, 8);
            playerBoxStyle.padding = new RectOffset(10, 10, 10, 10);
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontSize = 28;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = new Color(0.5f, 0.9f, 1f);
            subtitleStyle = new GUIStyle(GUI.skin.label);
            subtitleStyle.alignment = TextAnchor.MiddleCenter;
            subtitleStyle.fontSize = 16;
            subtitleStyle.fontStyle = FontStyle.Bold;
            subtitleStyle.normal.textColor = new Color(0.6f, 0.8f, 1f);
            textStyle = new GUIStyle(GUI.skin.label);
            textStyle.alignment = TextAnchor.MiddleCenter;
            textStyle.fontSize = 13;
            textStyle.wordWrap = true;
            textStyle.normal.textColor = new Color(0.7f, 0.7f, 0.8f);
            stylesInitialized = true;
        }

        void KeyBoardStuff()
        {
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                menushow = !menushow;
                animationTime = 0f;

                if (menushow)
                {
                    previousCursorLockMode = UnityEngine.Cursor.lockState;
                    previousCursorVisible = UnityEngine.Cursor.visible;
                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;
                }
                else
                {
                    UnityEngine.Cursor.lockState = previousCursorLockMode;
                    UnityEngine.Cursor.visible = previousCursorVisible;
                }
            }
            if (Input.GetKeyDown(KeyCode.End))
            {
                Loader.UnLoad();
            }
        }

        void Update()
        {
            KeyBoardStuff();
            animationTime += Time.deltaTime * 2f;
            pulseAnimation = Mathf.Sin(animationTime) * 0.5f + 0.5f;

            if (tabSwitchAnimation > 0)
            {
                tabSwitchAnimation -= Time.deltaTime * 4f;
            }

            if (godmode)
            {
                var playerhealthstuff = GameObject.FindObjectsOfType<PawnHurtbox>();
                foreach (YAPYAP.PawnHurtbox healththing in playerhealthstuff)
                {
                    healththing.SvSetHealth(100);
                }
            }
            if (stamina)
            {
                var staminastuff = GameObject.FindObjectsOfType<PawnBlackboard>();
                foreach (YAPYAP.PawnBlackboard staminathing in staminastuff)
                {
                    staminathing.Stamina = 90f;
                }
            }
            if (ghost)
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.NetworkghostSpawned = true;
                }
            }
            if (gold)
            {
                var goldstuff = GameObject.FindObjectsOfType<YAPYAP.UIGameScore>();
                foreach (YAPYAP.UIGameScore goldata in goldstuff)
                {
                    goldata.UpdateGold(999999, 9999999);
                }
            }

            if (dropprops)
            {
                var propstuff = GameObject.FindObjectsOfType<YAPYAP.Pawn>();
                foreach (YAPYAP.Pawn propcontrol in propstuff)
                {
                    propcontrol.SvDropAllProps();
                }
            }
            if (spawnvoid)
            {
                Vector3 voidPosition = new Vector3(999999f, 999999f, 999999f);
                var propstuff = GameObject.FindObjectsOfType<YAPYAP.Pawn>();
                foreach (YAPYAP.Pawn propcontrol in propstuff)
                {
                    propcontrol.SvTeleport(voidPosition, playFx: false, roundStartedPrevent: false);
                }
            }
        }

        void OnGUI()
        {
            if (!menushow)
                return;

            InitializeStyles();

            switch (selectedTab)
            {
                case 0:
                    windowRect = GUI.Window(0, windowRect, menu, "◢ YAPYAP TRAINER V1.0 ◣", windowStyle);
                    break;

                case 1:
                    windowRect = GUI.Window(0, windowRect, menu2, "◢ YAPYAP TRAINER V1.0 ◣", windowStyle);
                    break;
                default:
                    Debug.LogError("Invalid tab index!");
                    break;
            }

            HandleDragging(ref windowRect, ref isDragging1, ref offset1);
        }

        private void menu(int id)
        {
            windowRect.width = 715f;
            windowRect.height = 515f;
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Self And Server Trolls", selectedTab == 0 ? tabButtonActiveStyle : tabButtonStyle, GUILayout.Width(295f), GUILayout.Height(32)))
            {
                selectedTab = 0;
                tabSwitchAnimation = 1f;
            }
            GUILayout.Space(15);
            if (GUILayout.Button("Credits", selectedTab == 1 ? tabButtonActiveStyle : tabButtonStyle, GUILayout.Width(295f), GUILayout.Height(32)))
            {
                selectedTab = 1;
                tabSwitchAnimation = 1f;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            DrawColoredRect(new Rect(15, 67, windowRect.width - 30, 1), new Color(0.3f, 0.3f, 0.4f));
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUIStyle.none, GUIStyle.none, GUILayout.Width(640), GUILayout.Height(405));
            GUILayout.BeginVertical(playerBoxStyle, GUILayout.Width(610));
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical(GUILayout.Width(600));
            GUILayout.Label("━━━ PLAYER CONTROLS ━━━", headerStyle);
            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Kill Player", modernButtonStyle, GUILayout.Width(145), GUILayout.Height(40)))
            {
                var healthstuff = GameObject.FindObjectsOfType<YAPYAP.PawnHurtbox>();
                foreach (YAPYAP.PawnHurtbox healthdata in healthstuff)
                {
                    healthdata.SvDoKill();
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Revive Player", modernButtonStyle, GUILayout.Width(145), GUILayout.Height(40)))
            {
                var healthstuff = GameObject.FindObjectsOfType<YAPYAP.PawnHurtbox>();
                foreach (YAPYAP.PawnHurtbox healthdata in healthstuff)
                {
                    healthdata.SvDoRevive();
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Ragdoll Players", modernButtonStyle, GUILayout.Width(145), GUILayout.Height(40)))
            {
                var players = GameObject.FindObjectsOfType<YAPYAP.Pawn>();
                foreach (YAPYAP.Pawn playerss in players)
                {
                    playerss.SvSetRagdoll(-playerss.transform.forward * 15f + Vector3.up * 5f);
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("All Drop Items", modernButtonStyle, GUILayout.Width(145), GUILayout.Height(40)))
            {
                dropprops = !dropprops;
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("━━━ GAME CONTROLS ━━━", headerStyle);
            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Round", modernButtonStyle, GUILayout.Width(195), GUILayout.Height(40)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SvStartRound();
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("End Round", modernButtonStyle, GUILayout.Width(195), GUILayout.Height(40)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SvEndRound();
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Add Inf Points", modernButtonStyle, GUILayout.Width(195), GUILayout.Height(40)))
            {
                var gamestuff = GameObject.FindObjectsOfType<YAPYAP.GameManager>();
                foreach (YAPYAP.GameManager gamedata in gamestuff)
                {
                    gamedata.SetTotalScore(9999999);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("━━━ MODIFIERS ━━━", headerStyle);
            GUILayout.Space(6);

            GUILayout.BeginHorizontal();
            godmode = GUILayout.Toggle(godmode, "God Mode", toggleStyle, GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            stamina = GUILayout.Toggle(stamina, "Inf Stamina", toggleStyle, GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            ghost = GUILayout.Toggle(ghost, "Spawn Ghost", toggleStyle, GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            gold = GUILayout.Toggle(gold, "Inf Gold", toggleStyle, GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            GUILayout.BeginHorizontal();
            spawnvoid = GUILayout.Toggle(spawnvoid, "All Spawn Void", toggleStyle, GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            GUILayout.Label("", GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            GUILayout.Label("", GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.Space(5);
            GUILayout.Label("", GUILayout.Width(145), GUILayout.Height(30));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("━━━ CONNECTED PLAYERS ━━━", headerStyle);
            GUILayout.Space(5);

            var allPawns = UnityEngine.Object.FindObjectsOfType<YAPYAP.Pawn>();

            if (allPawns.Length == 0)
            {
                GUILayout.Label("No players found", textStyle);
                GUILayout.Space(10);
            }
            else
            {
                foreach (var pawn in allPawns)
                {
                    if (pawn == null) continue;
                    string playerId = pawn.PlayerId ?? pawn.GetInstanceID().ToString();

                    GUILayout.BeginHorizontal(playerBoxStyle);
                    GUILayout.Label($"Player: {playerId}", textStyle, GUILayout.Width(375), GUILayout.Height(30));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Kill", modernButtonStyle, GUILayout.Width(98), GUILayout.Height(30)))
                        pawn.Hurtbox?.SvDoKill();
                    GUILayout.Space(5);
                    if (GUILayout.Button("Revive", modernButtonStyle, GUILayout.Width(98), GUILayout.Height(30)))
                        pawn.Hurtbox?.SvDoRevive();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                }
            }

            GUILayout.Space(10);

            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.EndScrollView();
            GUILayout.Space(20);
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        private void menu2(int id)
        {
            windowRect.width = 715f;
            windowRect.height = 515f;

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Self And Server Trolls", selectedTab == 0 ? tabButtonActiveStyle : tabButtonStyle, GUILayout.Width(295f), GUILayout.Height(32)))
            {
                selectedTab = 0;
                tabSwitchAnimation = 1f;
            }
            GUILayout.Space(15);
            if (GUILayout.Button("Credits", selectedTab == 1 ? tabButtonActiveStyle : tabButtonStyle, GUILayout.Width(295f), GUILayout.Height(32)))
            {
                selectedTab = 1;
                tabSwitchAnimation = 1f;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            DrawColoredRect(new Rect(15, 67, windowRect.width - 30, 1), new Color(0.3f, 0.3f, 0.4f));
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            creditsScrollPosition = GUILayout.BeginScrollView(creditsScrollPosition, false, false, GUIStyle.none, GUIStyle.none, GUIStyle.none, GUILayout.Width(640), GUILayout.Height(405));
            GUILayout.BeginVertical(playerBoxStyle, GUILayout.Width(610));
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginVertical(GUILayout.Width(600));
            GUILayout.Space(10);
            GUILayout.Label("━━━━━━━━━━━━━━━", titleStyle);
            GUILayout.Label("YAPYAP TRAINER", titleStyle);
            GUILayout.Label("━━━━━━━━━━━━━━━", titleStyle);
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Width(570), GUILayout.Height(2));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            GUILayout.Label("DEVELOPED BY", subtitleStyle);
            GUILayout.Space(8);
            GUILayout.Label("sadece1eren", titleStyle);
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Width(570), GUILayout.Height(2));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            GUILayout.Label("TECHNOLOGIES USED", subtitleStyle);
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width(570));
            GUILayout.Label("Unity Engine (IMGUI)", textStyle);
            GUILayout.Label("Mirror Networking", textStyle);
            GUILayout.Label("Server-side RPC Control", textStyle);
            GUILayout.Label("NetworkBehaviour Hooks", textStyle);
            GUILayout.Label("Runtime Object Manipulation", textStyle);
            GUILayout.Label("Custom Admin / Debug Tools", textStyle);
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(12);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box("", GUILayout.Width(570), GUILayout.Height(2));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            GUILayout.Label("CONTROLS", subtitleStyle);
            GUILayout.Space(8);
            GUILayout.BeginVertical(playerBoxStyle);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("PAGE UP", subtitleStyle, GUILayout.Width(140));
            GUILayout.Label(">", textStyle, GUILayout.Width(30));
            GUILayout.Label("Show / Hide Menu", textStyle, GUILayout.Width(180));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("END", subtitleStyle, GUILayout.Width(140));
            GUILayout.Label(">", textStyle, GUILayout.Width(30));
            GUILayout.Label("Terminate Process", textStyle, GUILayout.Width(180));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(12);
            GUIStyle disclaimerStyle = new GUIStyle(textStyle);
            disclaimerStyle.fontSize = 11;
            disclaimerStyle.fontStyle = FontStyle.Italic;
            disclaimerStyle.normal.textColor = new Color(0.5f, 0.5f, 0.6f);
            GUILayout.Space(5);
            GUILayout.Label("Experimental DLL Made by me idk if i can do v2 lol.", disclaimerStyle);
            GUILayout.Label("Not intended for fair play.", disclaimerStyle);
            GUILayout.Label("Use responsibly... or don't ;P", disclaimerStyle);
            GUILayout.Space(10);
            GUILayout.EndVertical();
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.Space(20);
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        void DrawColoredRect(Rect rect, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            GUI.DrawTexture(rect, texture);
            UnityEngine.Object.Destroy(texture);
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
