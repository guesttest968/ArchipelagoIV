using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net;
using CCL.GTAIV;
using IVSDKDotNet;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static IVSDKDotNet.Native.Natives;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using ArchipelagoIV;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.MessageLog.Messages;
using static System.Net.Mime.MediaTypeNames;
using IVSDKDotNet.Native;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.MessageLog.Parts;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using IVSDKDotNet.Enums;
using System.Globalization;
using System.Security.Cryptography;

public class Main : Script // <- It's very important that we add ": Script" here! This will tell IV-SDK .NET that this class is the entry-point of our script.
{
    public static int PlayerHandle { get; set; }
    public static IVPed PlayerPed { get; set; }
    public static NativeCamera GameCamera { get; set; }
    public static bool IsConnected;
    public static string currentMission;
    public static Int32 soundID;
    static List<string> chatlogList = new List<string>();
    static List<string> chatlog = new List<string>();
    public static long localWeaponMask = 0;
    private static bool hasSyncedInitialWeapons = false;
    public static bool hasSyncedInitalIslandLocks = false;
    public static bool[] lockStatus = new bool[] { false, false, false, false, false, false, false, false };
    /* 
     * Regarding         ^^^^^^^^^^
     * 0 = MELEE
     * 1 = HANDGUN
     * 2 = SHOTGUN
     * 3 = SMG
     * 4 = RIFLE
     * 5 = SNIPER
     * 6 = HEAVY
     * 7 = THROWN
     */
    public static bool[] islandStatus = new bool[] { false, false };
    public static bool Foreverwanted;
    public static int ForeverwantedLevel = 0;
    public static bool hasCompletedStory;
    public static int CurrentEpisodeIndex;
    public static string CurrentEpisodeName;
    public bool ImGuiTestOpen;
    private static bool canScrollToEnd;
    private string TextBuffer = "";
    private string ip = "";
    private string port = "38281";
    private string username = "";
    private string password = "";



    private Dictionary<string, long> GTAIVMission_table = new Dictionary<string, long>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "The Cousins Bellic", 810001 },
            { "Easy Fare", 810002 },
            { "Uncle Vlad", 810003 },
            { "Crime and Punishment", 810004 },
            { "The Master and the Molotov", 810005 },
            { "Russian Revolution", 810006 },
            { "Roman's Sorrow", 810007 },
            { "Luck of the Irish", 810008 },
            { "Blow Your Cover", 810009 },
            { "Three Leaf Clover", 810010 },
            { "The Holland Play", 810011 },
            { "Blood Brothers", 810012 },
            { "Museum Piece", 810013 },
            { "Weekend at Florian's", 810014 },
            { "That Special Someone", 810015 },
            { "One Last Thing", 810016 },

            { "If the Price Is Right", 810017 },
            { "Mr. and Mrs. Bellic (Deal)", 810018 },
            { "A Revenger's Tragedy", 810019 },

            { "A Dish Served Cold", 810020 },
            { "Mr. and Mrs. Bellic (Revenge)", 810021 },
            { "Out of Commission", 810022 }
        };


    private Dictionary<string, long> TBOGTMission_table = new Dictionary<string, long>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "I Luv LC", 820001 },
            { "Chinese Takeout", 820002 },
            { "Momma's Boy", 820003 },
            { "Corner Kids", 820004 },
            { "Clocking Off", 820005 },
            { "Practice Swing", 820006 },
            { "Blog This!...", 820007 },
            { "Bang Bang", 820008 },
            { "Boulevard Baby", 820009 },
            { "Frosting on the Cake", 820010 },
            { "Kibbutz Number One", 820011 },
            { "Sexy Time", 820012 },
            { "High Dive", 820013 },
            { "...Blog This!", 820014 },
            { "This Ain't Checkers", 820015 },
            { "Not So Fast", 820016 },
            { "Ladies' Night", 820017 },
            { "No. 3", 820018 },
            { "Going Deep", 820019 },
            { "Caught with your Pants Down", 820020 },
            { "Dropping In", 820021 },
            { "For the Man Who Has Everything", 820022 },
            { "In The Crosshairs", 820023 },
            { "Ladies Half Price", 820024 },
            { "Party's Over", 820025 },
            { "Departure Time", 820026 },
        };

    enum GTAIVWeapons
    {
        UNARMED = 0,
        BASEBALLBAT = 1,
        POOLCUE = 2,
        KNIFE = 3,
        GRENADE = 4,
        MOLOTOV = 5,
        ROCKET = 6,
        PISTOL = 7,
        UNUSED0 = 8,
        DEAGLE = 9,
        SHOTGUN = 10,
        BARETTA = 11,
        MICRO_UZI = 12,
        MP5 = 13,
        AK47 = 14,
        M4 = 15,
        SNIPERRIFLE = 16,
        M40A1 = 17,
        RLAUNCHER = 18,
        FTHROWER = 19,
        MINIGUN = 20,
        GRENADELAUNCHER = 21,
        ASSAULTSHOTGUN = 22,
        UNUSED = 23,
        BROKENPOOLCUE = 24,
        GRENADELAUNCHERSGRENADE = 25,
        SAWNOFFSHOTGUN = 26,
        AUTOMATICPISTOL = 27,
        PIPEBOMB = 28,
        PISTOL44 = 29,
        AA12EXPLOSIVESHELLS = 30,
        AA12 = 31,
        P90 = 32,
        GOLDENUZI = 33,
        M249 = 34,
        ADVANCEDSNIPER = 35,
        STICKYBOMB = 36,
        BUZZARDSROCKETLAUNCHER = 37,
        BUZZARDSROCKETLAUNCHERROCKETS = 38,
        BUZZARDSMINIGUN = 39,
        APCCANNON = 40,
        PARACHUTE = 41,
        UNUSED1 = 42,
        UNUSED2 = 43,
        UNUSED3 = 44,
        CAMERA = 45,
        OBJECT = 46,
        WEAPONTYPE_LAST_WEAPONTYPE = 47,
        ARMOUR = 48,
        RAMMEDBYCAR = 49,
        RUNOVERBYCAR = 50,
        EXPLOSION = 51,
        UZI_DRIVEBY = 52,
        DROWNING = 53,
        FALL = 54,
        UNIDENTIFIED = 55,
        ANYMELEE = 56,
        ANYWEAPON = 57
    }



    public static void RegisterItemsHandler()
    {

        Global.MyIndex = Global.session.Items.Index;

        Global.session.Items.ItemReceived += (helper) =>
        {
            while (helper.Index > Global.MyIndex)
            {
                var item = helper.AllItemsReceived[Global.MyIndex];

                Global.ItemQueue.Enqueue(item.ItemId);

                Global.MyIndex++;
            }
        };
    }


    public Main()
    {

        Tick += Main_Tick;
        OnImGuiRendering += Main_OnImGuiRendering;
        KeyDown += Main_KeyDown;
    }

    private void Main_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F3)
        {
            // Opens the ImGui test window
            ImGuiTestOpen = true;


        }
    }

    private void Main_OnImGuiRendering(IntPtr devicePtr, ImGuiIV_DrawingContext ctx)
    {
        if (IsConnected && !Global.firstconnected)
        {
            ImGuiIV.OpenPopup("Connected!");
        }
        firstconnected();
        ImGuiTest();

    }

    void firstconnected()
    {


        if (ImGuiIV.BeginPopupModal("Connected!", eImGuiWindowFlags.AlwaysAutoResize | eImGuiWindowFlags.NoResize))
        {
            ImGuiIV.Text("Now that you are connected, you can use !help to list commands to run via the server.\nIf your client supports it, you may have additional local commands you can list with /help.");

            if (ImGuiIV.Button("Ok"))
            {
                Global.firstconnected = true;
                ImGuiIV.CloseCurrentPopup();
            }
            ImGuiIV.EndPopup();
        }
    }

    void ImGuiTest()
    {

        if (!ImGuiTestOpen) return;



        if (ImGuiIV.Begin("ArchipelagoIV", ref ImGuiTestOpen))
        {
            if (ImGuiIV.BeginTabBar("MyTabBar"))
            {
                if (ImGuiIV.BeginTabItem("Connect"))
                {
                    if (Global.session == null || !IsConnected)
                    {
                        ImGuiIV.TextColored(System.Drawing.Color.Red, "Currently not connected to an Archipelago server!");
                    }
                    else
                    {
                        ImGuiIV.TextColored(System.Drawing.Color.Green, "Currently connected to an Archipelago server!");
                    }

                    ImGuiIV.Separator();
                    ImGuiIV.Spacing(2);
                    string gametext = "None";
                    int invalidinputs = 0;

                    if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.IV)
                    {
                        gametext = "Grand Theft Auto IV";
                        ImGuiIV.Text("Episode: " + gametext);
                    }
                    if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TLaD)
                    {
                        gametext = "The Lost and Damned";
                        ImGuiIV.Text("Episode: " + gametext);

                    }
                    if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TBoGT)
                    {
                        gametext = "The Ballad of Gay Tony";
                        ImGuiIV.Text("Episode: " + gametext);
                    }
                    ImGuiIV.Spacing(2);
                    ImGuiIV.Separator();
                    ImGuiIV.Spacing(1);
                    ImGuiIV.BeginDisabled(IsConnected);
                    ImGuiIV.Text("IP");
                    ImGuiIV.SameLine(80.0f, 2);
                    ImGuiIV.SetNextItemWidth(ImGuiIV.GetContentRegionAvail().X * 0.5f);
                    ImGuiIV.InputText("##IPTextBox", ref ip);
                    if (string.IsNullOrEmpty(ip))
                    {
                        invalidinputs++;
                        ImGuiIV.SetNextWindowPos(new Vector2(ImGuiIV.GetItemRectMin().X, ImGuiIV.GetItemRectMax().Y));

                        if (ImGuiIV.BeginTooltip())
                        {
                            ImGuiIV.TextColored(System.Drawing.Color.Red, "IP Cannot be empty!");
                            ImGuiIV.EndTooltip();


                        }
                    }
                    ImGuiIV.Spacing(2);
                    ImGuiIV.Text("Port");
                    ImGuiIV.SameLine(80.0f, 2);
                    ImGuiIV.SetNextItemWidth(Math.Min(Math.Max(ImGuiIV.GetContentRegionAvail().X * 0.2f, ImGuiIV.CalcTextSize(port).X + ImGuiIV.GetStyle().FramePadding.X * 2.0f + 1.0f), ImGuiIV.GetContentRegionAvail().X));
                    ImGuiIV.InputText("##PortTextBox", ref port);
                    if (string.IsNullOrEmpty(port))
                    {
                        invalidinputs++;
                        ImGuiIV.SetNextWindowPos(new Vector2(ImGuiIV.GetItemRectMin().X, ImGuiIV.GetItemRectMax().Y));

                        if (ImGuiIV.BeginTooltip())
                        {
                            ImGuiIV.TextColored(System.Drawing.Color.Red, "Port Cannot be empty!");
                            ImGuiIV.EndTooltip();


                        }
                    }
                    else if (!string.IsNullOrEmpty(port))
                    {
                        if (port.Length > 5)
                        {
                            invalidinputs++;
                            port = port.Remove(5);
                            ImGuiIV.SetNextWindowPos(new Vector2(ImGuiIV.GetItemRectMin().X, ImGuiIV.GetItemRectMax().Y));

                            if (ImGuiIV.BeginTooltip())
                            {
                                ImGuiIV.TextColored(System.Drawing.Color.Red, "Port must not contain 5 numbers");
                                ImGuiIV.EndTooltip();


                            }
                        }
                        if (!string.IsNullOrEmpty(port) && !Regex.IsMatch(port, @"^[0-9]*$"))
                        {
                            invalidinputs++;
                            port = Regex.Replace(port, @"[^0-9]", "");
                            ImGuiIV.SetNextWindowPos(new Vector2(ImGuiIV.GetItemRectMin().X, ImGuiIV.GetItemRectMax().Y));

                            if (ImGuiIV.BeginTooltip())
                            {
                                ImGuiIV.TextColored(System.Drawing.Color.Red, "Port must not contain letters or symbols");
                                ImGuiIV.EndTooltip();


                            }
                        }

                    else
                    {
                        if (invalidinputs < 1)
                        {
                            invalidinputs = 0;
                        }
                    }




                    }
                    ImGuiIV.Spacing(4);

                    ImGuiIV.Text("Username");
                    ImGuiIV.SameLine(80.0f, 2);
                    ImGuiIV.SetNextItemWidth(ImGuiIV.GetContentRegionAvail().X * 0.5f);
                    ImGuiIV.InputText("##UsernameTextBox", ref username);
                    if (string.IsNullOrEmpty(username))
                    {
                        invalidinputs++;
                        ImGuiIV.SetNextWindowPos(new Vector2(ImGuiIV.GetItemRectMin().X, ImGuiIV.GetItemRectMax().Y));

                        if (ImGuiIV.BeginTooltip())
                        {
                            ImGuiIV.TextColored(System.Drawing.Color.Red, "Username Cannot be empty!");
                            ImGuiIV.EndTooltip();


                        }
                    }
                    else
                    {
                        if (invalidinputs < 1)
                        {
                            invalidinputs = 0;
                        }
                    }
                    ImGuiIV.Spacing(2);
                    ImGuiIV.Text("Password");
                    ImGuiIV.SameLine(80.0f, 2);
                    ImGuiIV.SetNextItemWidth(ImGuiIV.GetContentRegionAvail().X * 0.5f);
                    ImGuiIV.InputText("##PasswordTextBox", ref password);
                    ImGuiIV.Spacing(2);
                    ImGuiIV.EndDisabled();
                    ImGuiIV.Separator();

                    ImGuiIV.Spacing(2);
                    ImGuiIV.BeginDisabled(!IsConnected);
                    if (ImGuiIV.Button("Disconnect"))
                    {
                        Global.session.Socket.DisconnectAsync();
                        Global.session = null;
                        Global.chatlog = null;
                        IsConnected = false;
                        CLEAR_HELP();
                    }
                    ImGuiIV.EndDisabled();
                    ImGuiIV.SameLine(100.0f, 2);
                    ImGuiIV.BeginDisabled(IsConnected);
                    ImGuiIV.BeginDisabled(invalidinputs > 0);
                    if (ImGuiIV.Button("Connect"))
                    {
                        Connect(ip, int.Parse(port), username, password);
                    }
                    ImGuiIV.EndDisabled();
                    ImGuiIV.EndDisabled();
                    ImGuiIV.EndTabItem();
                }

                if (ImGuiIV.BeginTabItem("Chat"))
                {
                    if (Global.session == null || !IsConnected)
                    {
                        ImGuiIV.TextColored(System.Drawing.Color.Red, "Currently not connected to an Archipelago server!");
                    }
                    else
                    {
                        ImGuiIV.TextColored(System.Drawing.Color.Green, "Currently connected to an Archipelago server!");
                    }

                    ImGuiIV.Separator();

                    ImGuiIV.BeginDisabled(!IsConnected);

                    if (ImGuiIV.BeginChild("##APChatlog", new Vector2(ImGuiIV.FloatMin, 2f * -ImGuiIV.GetTextLineHeightWithSpacing())))
                    {

                        for (int i = 0; i < chatlog.Count; i++)
                        {
                            string logEntry = chatlog[i].Replace("{", "{{").Replace("}", "}}");

                            System.Drawing.Color displayColor = System.Drawing.Color.White;

                            if (logEntry.StartsWith("!"))
                                displayColor = System.Drawing.Color.Yellow;



                            ImGuiIV.PushStyleColor(eImGuiCol.Text, displayColor);
                            ImGuiIV.TextWrapped(logEntry);
                            ImGuiIV.PopStyleColor();

                        }



                        ImGuiIV.EndChild();
                    }

                    if (ImGuiIV.Button("Send"))
                    {
                        SendSayPacket(TextBuffer);
                        TextBuffer = "";
                    }

                    ImGuiIV.SameLine();

                    ImGuiIV.InputText("##inputTextBox", ref TextBuffer);

                    ImGuiIV.EndDisabled();
                    ImGuiIV.EndTabItem();

                }


                ImGuiIV.EndTabBar();
            }
            ImGuiIV.End();
        }
    }

    private void Main_Tick(object sender, EventArgs e)
    {

        if (Global.ItemQueue.Count > 0)
        {
            long itemID = Global.ItemQueue.Dequeue();
            GiveWeaponByID(itemID);
        }


        PlayerPed = IVPed.FromUIntPtr(IVPlayerInfo.FindThePlayerPed());
        PlayerHandle = PlayerPed.GetHandle();

        WeaponLock();
        IslandLock();
        ForeverWanted();
        RegisterCommands();
        MissionCheck();
    }

    private void RegisterCommands()
    {
        IVGame.Console.RegisterCommand(this, "APDisconnect", (string[] args) => { OnDisconnectCommand(args); });
        IVGame.Console.RegisterCommand(this, "APConnect", (string[] args) => { OnConnectCommand(args); });

        IVGame.Console.RegisterCommand(this, "APSay", (string[] args) =>
        {
            string fullText = string.Join(" ", args);
            SendSayPacket(fullText);
        });


    }

    private void MissionCheck()
    {
        if (hasCompletedStory && ARE_CREDITS_FINISHED() && IS_PLAYER_CONTROL_ON(0) && HAS_CUTSCENE_FINISHED())
        {
            Global.session.SetGoalAchieved();

        }
        Global.ismissioncompleted = IS_MISSION_COMPLETE_PLAYING();
        CurrentEpisodeIndex = Convert.ToInt32(GET_CURRENT_EPISODE());
        if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.IV)
        {
            currentMission = GET_STRING_FROM_TEXT_FILE(IVTheScripts.GetGlobalString(9926));
        }
        if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TLaD)
        {
            currentMission = GET_STRING_FROM_TEXT_FILE(IVTheScripts.GetGlobalString(10987));

        }
        if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TBoGT)
        {
            currentMission = GET_STRING_FROM_TEXT_FILE(IVTheScripts.GetGlobalString(10966));
        }
        if (currentMission != "NULL")
        {
            if (currentMission != Global.missionName)
            {
                Global.hasSentCheck = false;
                Global.missionName = currentMission;
                IVGame.Console.Print("APIV: New Mission Detected: " + currentMission);
                if ((currentMission == "Story Complete") || (currentMission == "TLaD Complete") || (currentMission == "TBoGT Complete"))
                {
                    hasCompletedStory = true;
                }

            }
        }
        if (Global.ismissioncompleted == true && !Global.hasSentCheck && Global.session != null)
        {
            if (IS_PLAYER_PLAYING(0) == true)
            {
                SendMissionCheck(Global.missionName);
                Global.hasSentCheck = true;
            }
        }
    }

    public bool IsPlayerInside(Vector3 Playerpos, Vector3 Floor_Corner, Vector3 Ceiling_Corner)
    {
        bool xCheck = Playerpos.X >= Math.Min(Floor_Corner.X, Ceiling_Corner.X) && Playerpos.X <= Math.Max(Floor_Corner.X, Ceiling_Corner.X);
        bool yCheck = Playerpos.Y >= Math.Min(Floor_Corner.Y, Ceiling_Corner.Y) && Playerpos.Y <= Math.Max(Floor_Corner.Y, Ceiling_Corner.Y);
        bool zCheck = Playerpos.Z >= Math.Min(Floor_Corner.Z, Ceiling_Corner.Z) && Playerpos.Z <= Math.Max(Floor_Corner.Z, Ceiling_Corner.Z);

        return xCheck && yCheck && zCheck;
    }
    private void ForeverWanted()
    {
        if (!IsConnected || !Foreverwanted)
        {
            return;
        }
        if (Foreverwanted && ForeverwantedLevel == 0)
        {
            Random random = new Random();
            ForeverwantedLevel = random.Next(1, 6);
        }
        else if (ForeverwantedLevel != 0)
        {
            ALTER_WANTED_LEVEL_NO_DROP(0, Convert.ToUInt32(ForeverwantedLevel));
            APPLY_WANTED_LEVEL_CHANGE_NOW(0);
        }
    }
    private void IslandLock()
    {
        if (!IsConnected && !hasSyncedInitalIslandLocks && Global.session == null)
        {
            return;
        }
        else
        {
            GET_CHAR_COORDINATES(PlayerHandle, out Vector3 playervector);

            Vector3 Algonquin1_Min = new Vector3(-824f, -885f, -100f);
            Vector3 Algonquin1_Max = new Vector3(620f, 594f, 1975f);

            Vector3 Algonquin2_Min = new Vector3(-678f, 600f, -100f);
            Vector3 Algonquin2_Max = new Vector3(350f, 1383f, 1975f);

            Vector3 Algonquin3_Min = new Vector3(-710f, 1358f, -100f);
            Vector3 Algonquin3_Max = new Vector3(188, 2279f, 1975f);

            Vector3 Alderney1_Min = new Vector3(-894f, -947f, -100f);
            Vector3 Alderney1_Max = new Vector3(-2320f, -107f, 1975f);

            Vector3 Alderney2_Min = new Vector3(-840f, -97f, -100f);
            Vector3 Alderney2_Max = new Vector3(-2489f, 792f, 1975f);

            Vector3 Alderney3_Min = new Vector3(-745f, 736f, -100f);
            Vector3 Alderney3_Max = new Vector3(-2785f, 2199f, 1975f);

            if (IsPlayerInside(playervector, Algonquin1_Min, Algonquin1_Max) && GET_CURRENT_EPISODE() != 2 || IsPlayerInside(playervector, Algonquin2_Min, Algonquin2_Max) && GET_CURRENT_EPISODE() != 2 || IsPlayerInside(playervector, Algonquin3_Min, Algonquin3_Max) && GET_CURRENT_EPISODE() != 2)
            {
                if (islandStatus[0])
                {
                    return;
                }
                else
                {
                    IVTheScripts.SetDummyThread();
                    _TASK_DIE(PlayerHandle);

                    IVTheScripts.RestorePreviousThread();
                }

            }
            else if (IsPlayerInside(playervector, Alderney1_Min, Alderney1_Max) || IsPlayerInside(playervector, Alderney2_Min, Alderney2_Max) || IsPlayerInside(playervector, Alderney3_Min, Alderney3_Max))
            {
                if (islandStatus[1])
                {
                    return;
                }
                else
                {
                    IVTheScripts.SetDummyThread();
                    _TASK_DIE(PlayerHandle);

                    IVTheScripts.RestorePreviousThread();
                }
            }


        }

    }

    private void WeaponLock()
    {
        if (!IsConnected && hasSyncedInitialWeapons && Global.session == null)
        {
            return;
        }
        int currentweapon;
        GET_CURRENT_CHAR_WEAPON(PlayerHandle, out currentweapon);
        if (!lockStatus[0] && (currentweapon == 1 || currentweapon == 2 || currentweapon == 3))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 1);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 2);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 3);
        }
        else if (!lockStatus[1] && (currentweapon == 7 || currentweapon == 9))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 7);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 9);
        }
        else if (!lockStatus[2] && (currentweapon == 10 || currentweapon == 11))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 10);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 11);
        }
        else if (!lockStatus[3] && (currentweapon == 12 || currentweapon == 13))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 12);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 13);
        }
        else if (!lockStatus[4] && (currentweapon == 14 || currentweapon == 15))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 14);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 15);
        }
        else if (!lockStatus[5] && (currentweapon == 16 || currentweapon == 17))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 16);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 17);
        }
        else if (!lockStatus[6] && currentweapon == 18)
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 18);
        }
        else if (!lockStatus[7] && (currentweapon == 4 || currentweapon == 5))
        {
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 4);
            REMOVE_WEAPON_FROM_CHAR(PlayerHandle, 5);
        }
    }

    static void OnMessageReceived(LogMessage message)
    {
        switch (message)
        {
            case ChatLogMessage chatLog:
                string playerName = chatLog.Player.Name;
                string chatText = $"~g~{playerName}~w~: {chatLog.Message}";
                chatlog.Add($"{playerName}: {chatLog.Message}");
                AppendToChatLog(chatText);

                break;

            case ItemSendLogMessage itemSend:
                string itemtext = $"~g~{itemSend.Receiver}~w~ received ~p~{itemSend.Item.ItemName}~w~ by ~g~{itemSend.Sender}~w~ (~y~{itemSend.Item.LocationDisplayName}~w~)";
                chatlog.Add($"{itemSend.Receiver} received {itemSend.Item.ItemName} by {itemSend.Sender} ({itemSend.Item.LocationDisplayName})");
                AppendToChatLog(itemtext);

                break;

            case JoinLogMessage joinLogMessage:
                string joinMessage = $"~g~{joinLogMessage.Player.Alias}~w~ Joined on ~p~{joinLogMessage.Player.Game}~w~";
                chatlog.Add($"{joinLogMessage.Player.Alias} Joined on {joinLogMessage.Player.Game}");
                AppendToChatLog(joinMessage);
                break;

            case LeaveLogMessage leaveLogMessage:
                string leavemessage = $"~g~{leaveLogMessage.Player.Alias}~w~ Left (~p~{leaveLogMessage.Player.Game}~w~)";
                chatlog.Add($"{leaveLogMessage.Player.Alias} Joined on {leaveLogMessage.Player.Game}");
                AppendToChatLog(leavemessage);
                break;


            case ReleaseLogMessage releaseLogMessage:
                string releasemessage = $"~g~{releaseLogMessage.Player.Name}~w~ has released all the remaining items from their world.";
                chatlog.Add($"{releaseLogMessage.Player.Name} has released all the remaining items from their world.");
                AppendToChatLog(releasemessage);
                break;

            case TutorialLogMessage tutorialLog:
                string tuttext = $"~p~{tutorialLog}";
                AppendToChatLog(tuttext);
                break;

            case ServerChatLogMessage serverlog:
                string serverText = $"~r~[Server]~w~: {serverlog.Message}";

                AppendToChatLog(serverText);
                chatlog.Add($"[Server]: {serverlog.Message}");
                break;

            case CountdownLogMessage countdownlog:
                string countdowntext = $"~r~[Server]~w~: ~b~{countdownlog.RemainingSeconds}~w~";
                AppendToChatLog(countdowntext);
                chatlog.Add($"[Server]: {countdownlog.RemainingSeconds}");
                if (countdownlog.RemainingSeconds == 0)
                {
                    chatlog.Add("[Server]: GO!");
                }
                break;

            case GoalLogMessage goallog:
                break;



            case CommandResultLogMessage commandresult:
                IVGame.Console.Print(commandresult.ToString());
                chatlog.Add(commandresult.ToString());
                break;






        }

    }

    static void AppendToChatLog(string newMessage)
    {
        if (Global.chatlog == null) Global.chatlog = "";
        SET_HELP_MESSAGE_BOX_SIZE(0.3f);
        SET_HELP_MESSAGE_BOX_SIZE_F(0.3f);

        string formatted = newMessage + "~n~";

        chatlogList.Add(formatted);

        int maxMessages = 3;
        while (chatlogList.Count > maxMessages)
        {
            chatlogList.RemoveAt(0);
        }
        string fullDisplay = string.Join("~n~", chatlogList);

        ShowChatMessage("~y~Archipelago Chat:~w~~n~" + fullDisplay);

    }

    private void OnConnectCommand(string[] args)
    {
        if (Global.session != null) { IVGame.Console.PrintError("Already Connected!"); return; }

        if (args.Length < 2)
        {
            IVGame.Console.Print("Usage: APConnect <server:port> <user> [password]");
            return;
        }

        string server = args[0];
        int port = 38281;
        if (server.Contains(":"))
        {
            string[] parts = server.Split(':');
            server = parts[0];
            int.TryParse(parts[1], out port);
        }

        string user = args[1];
        string pass = args.Length > 2 ? args[2] : string.Empty;

        IVGame.Console.Print($"APIV: Attempting to connect to {server}:{port} as {user}...");
        Connect(server, port, user, pass);
    }

    private void OnDisconnectCommand(string[] args)
    {
        if (Global.session != null)
        {
            Global.session.Socket.DisconnectAsync();
            Global.session = null;
            Global.chatlog = null;
            IsConnected = false;
            CLEAR_HELP();
        }
        else
        {
            IVGame.Console.PrintError("AP: Not connected to a server");
        }


    }

    public static void SendSayPacket(string text)
    {
        if (Global.session != null)
        {
            Global.session.Socket.SendPacket(new SayPacket { Text = text });

        }
        else
        {
            IVGame.Console.PrintError("APIV: Not connected to a Archipelago server");
        }
    }

    private static void Connect(string server, int port, string user, string pass)
    {

        Global.session = ArchipelagoSessionFactory.CreateSession(server, port);
        Global.session.MessageLog.OnMessageReceived += OnMessageReceived;
        LoginResult result;
        CurrentEpisodeIndex = Convert.ToInt32(GET_CURRENT_EPISODE());
        try
        {
            if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TBoGT)
            {
                CurrentEpisodeName = "Grand Theft Auto IV: The Ballad of Gay Tony";
            }
            if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.TLaD)
            {
                CurrentEpisodeName = "Grand Theft Auto IV: The Lost and Damned";

            }
            if (CurrentEpisodeIndex == (int)CCL.GTAIV.Episode.IV)
            {
                CurrentEpisodeName = "Grand Theft Auto IV";
            }
            result = Global.session.TryConnectAndLogin(
                        CurrentEpisodeName,
                        user,
                        ItemsHandlingFlags.AllItems,
                        password: pass
                    );

        }
        catch (Exception e)
        {
            result = new LoginFailure(e.GetBaseException().Message);
            IVGame.Console.PrintError($"Error: {e.Message}");
            IVGame.Console.PrintError(e.GetBaseException().Message);
            Global.session.Socket.DisconnectAsync();
            Global.session = null;
            Global.chatlog = null;
            IsConnected = false;
            CLEAR_HELP();
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string errorMessage = $"Failed to Connect to {server} as {user}:";
            foreach (string error in failure.Errors)
            {
                errorMessage += $"\n    {error}";
                IVGame.Console.PrintError(errorMessage);
                Global.session.Socket.DisconnectAsync();
                Global.session = null;
                Global.chatlog = null;
                IsConnected = false;
                CLEAR_HELP();
                return;
            }
            foreach (ConnectionRefusedError error in failure.ErrorCodes)
            {
                errorMessage += $"\n    {error}";
                IVGame.Console.PrintError(errorMessage);
                Global.session.Socket.DisconnectAsync();
                Global.session = null;
                Global.chatlog = null;
                IsConnected = false;
                CLEAR_HELP();
                return;
            }

            return;
        }

        RegisterItemsHandler();


        var loginSuccess = (LoginSuccessful)result;

        IsConnected = true;
        Global.chatlog = null;
        Global.session.DataStorage[Scope.Slot, "WeaponLock"].Initialize(new[] { false, false, false, false, false, false, false, false });
        Global.session.DataStorage[Scope.Slot, "IslandLock"].Initialize(new[] { false, false });

        Global.session.DataStorage[Scope.Slot, "WeaponLock"].OnValueChanged += WeaponLock_OnValueChanged;
        Global.session.DataStorage[Scope.Slot, "IslandLock"].OnValueChanged += IslandLock_OnValueChanged;

        lockStatus = Global.session.DataStorage[Scope.Slot, "WeaponLock"].To<bool[]>();
        islandStatus = Global.session.DataStorage[Scope.Slot, "IslandLock"].To<bool[]>();

        hasSyncedInitialWeapons = true;
        hasSyncedInitalIslandLocks = true;
        Global.session.SetClientState(ArchipelagoClientState.ClientPlaying);
    }

    private static void IslandLock_OnValueChanged(Newtonsoft.Json.Linq.JToken originalValue, Newtonsoft.Json.Linq.JToken newValue, Dictionary<string, Newtonsoft.Json.Linq.JToken> additionalArguments)
    {
        islandStatus = newValue.ToObject<bool[]>();

    }

    private static void WeaponLock_OnValueChanged(Newtonsoft.Json.Linq.JToken originalValue, Newtonsoft.Json.Linq.JToken newValue, Dictionary<string, Newtonsoft.Json.Linq.JToken> additionalArguments)
    {
        lockStatus = newValue.ToObject<bool[]>();
    }

    private void SendMissionCheck(string missionName)
    {
        if (string.IsNullOrEmpty(missionName) || missionName == "NULL") return;

        if (Global.session == null)
        {
            IVGame.Console.Print("AP Error: Session not initialized.");
            return;
        }
        switch (GET_CURRENT_EPISODE())
        {
            case 0:
                if (GTAIVMission_table.ContainsKey(missionName))
                {
                    long locationID = GTAIVMission_table[missionName];

                    Global.session.Locations.CompleteLocationChecks(locationID);

                    ShowSubtitleMessage($"Sent Check: {missionName} (ID: {locationID})");

                }
                else
                {
                    IVGame.Console.Print($"AP Warning: Mission '{missionName}' not in location_table.");
                }
                break;

            case 1:
                break;

            case 2:
                if (TBOGTMission_table.ContainsKey(missionName))
                {
                    long locationID = TBOGTMission_table[missionName];

                    Global.session.Locations.CompleteLocationChecks(locationID);

                    ShowSubtitleMessage($"Sent Check: {missionName} (ID: {locationID})");

                }
                else
                {
                    IVGame.Console.Print($"AP Warning: Mission '{missionName}' not in location_table.");
                }
                break;
        }

    }

    private void GiveWeaponByID(long id)
    {


        switch (id)
        {
            case 800001:
                lockStatus[4] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.AK47, 120, true);
                break;

            case 800002:
                lockStatus[0] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.BASEBALLBAT, 120, true);
                break;

            case 800003:
                lockStatus[4] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.M4, 150, true);
                break;

            case 800004:
                lockStatus[1] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.DEAGLE, 45, true);
                break;

            case 800005:
                lockStatus[2] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.BARETTA, 20, true);
                break;

            case 800006:
                lockStatus[5] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.M40A1, 20, true);
                break;

            case 800007:
                lockStatus[7] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.GRENADE, 20, true);
                break;

            case 800008:
                lockStatus[0] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.KNIFE, 20, true);
                break;

            case 800009:
                lockStatus[3] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.MICRO_UZI, 50, true);
                break;

            case 800010:
                lockStatus[7] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.MOLOTOV, 50, true);
                break;

            case 800011:
                lockStatus[1] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.PISTOL, 50, true);
                break;

            case 800012:
                lockStatus[2] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.SHOTGUN, 50, true);
                break;

            case 800013:
                lockStatus[6] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.RLAUNCHER, 5, true);
                break;

            case 800014:
                lockStatus[3] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.MP5, 5, true);
                break;

            case 800015:
                lockStatus[5] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.SNIPERRIFLE, 10, true);
                break;

            case 800016:
                ADD_ARMOUR_TO_CHAR(PlayerHandle, 100);
                break;

            case 800100:
                SET_CHAR_HEALTH(PlayerHandle, 200);
                break;

            case 800101:
                ADD_SCORE(0, 500);
                break;

            case 800102:
                ALTER_WANTED_LEVEL_NO_DROP(0, 1);
                APPLY_WANTED_LEVEL_CHANGE_NOW(0);
                break;

            case 800103:
                ALTER_WANTED_LEVEL_NO_DROP(0, 2);
                APPLY_WANTED_LEVEL_CHANGE_NOW(0);

                break;

            case 800104:
                ALTER_WANTED_LEVEL_NO_DROP(0, 3);
                APPLY_WANTED_LEVEL_CHANGE_NOW(0);

                break;

            case 800105:
                ALTER_WANTED_LEVEL_NO_DROP(0, 4);
                APPLY_WANTED_LEVEL_CHANGE_NOW(0);

                break;

            case 800106:
                ALTER_WANTED_LEVEL_NO_DROP(0, 5);
                APPLY_WANTED_LEVEL_CHANGE_NOW(0);
                break;

            case 800108:
                Foreverwanted = false;
                ForeverwantedLevel = 0;
                CLEAR_WANTED_LEVEL(0);
                break;

            case 800109:
                Random random = new Random();
                int randomammo;
                randomammo = random.Next(0, 500);
                int weapontype;
                GET_CURRENT_CHAR_WEAPON(PlayerHandle, out weapontype);

                SET_CHAR_AMMO(PlayerHandle, 4, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 5, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 6, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 7, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 9, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 10, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 11, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 12, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 13, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 14, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 15, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 16, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 17, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 18, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 19, randomammo);
                SET_CHAR_AMMO(PlayerHandle, 20, randomammo);
                break;

            case 800110:
                GameCamera = NativeCamera.GetGameCam();
                GameCamera.SetDrunkEffect(2, 10000);

                break;

            case 800111:
                Foreverwanted = true;
                ForeverWanted();

                break;

            case 800050:
                islandStatus[0] = true;
                Global.session.DataStorage[Scope.Slot, "IslandLock"] = islandStatus;
                break;

            case 800051:
                islandStatus[1] = true;
                Global.session.DataStorage[Scope.Slot, "IslandLock"] = islandStatus;
                break;

            case 800023:
                lockStatus[1] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.PISTOL44, 45, true);
                break;

            case 800024:
                lockStatus[2] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.AA12EXPLOSIVESHELLS, 45, true);
                break;

            case 800025:
                lockStatus[3] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.GOLDENUZI, 45, true);
                break;

            case 800026:
                lockStatus[3] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.P90, 45, true);
                break;

            case 800027:
                lockStatus[4] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.M249, 45, true);
                break;

            case 800028:
                lockStatus[5] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.ADVANCEDSNIPER, 45, true);
                break;

            case 800029:
                lockStatus[7] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.STICKYBOMB, 45, true);
                break;

            case 800030:
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.PARACHUTE, 45, true);
                break;

            case 800031:
                lockStatus[2] = true;
                Global.session.DataStorage[Scope.Slot, "WeaponLock"] = lockStatus;
                GIVE_WEAPON_TO_CHAR(PlayerHandle, (int)GTAIVWeapons.AA12, 45, true);
                break;
        }

    }

    public static void ShowChatMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            IVTheScripts.SetDummyThread();
            SET_HELP_MESSAGE_BOX_SIZE(1.0f);
            SET_HELP_MESSAGE_BOX_SIZE_F(1.0f);
            IVText.TheIVText.ReplaceTextOfTextLabel("PLACEHOLDER_1", message);
            Natives.PRINT_HELP_FOREVER("PLACEHOLDER_1");
            SET_HELP_MESSAGE_BOX_SIZE(1.0f);
            SET_HELP_MESSAGE_BOX_SIZE_F(1.0f);
            IVTheScripts.RestorePreviousThread();

        }
    }

    public static void ShowMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            IVTheScripts.SetDummyThread();
            SET_HELP_MESSAGE_BOX_SIZE(0.3f);
            SET_HELP_MESSAGE_BOX_SIZE_F(0.3f);
            IVText.TheIVText.ReplaceTextOfTextLabel("PLACEHOLDER_1", message);
            Natives.PRINT_HELP_FOREVER("PLACEHOLDER_1");
            SET_HELP_MESSAGE_BOX_SIZE(0.3f);
            SET_HELP_MESSAGE_BOX_SIZE_F(0.3f);
            IVTheScripts.RestorePreviousThread();

        }
    }

}

