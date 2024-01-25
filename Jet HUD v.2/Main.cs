using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using System.Drawing;
using GTA.Native;
using GTA.Math;


namespace JetHUD2
{
    public class Main : Script
    {
        public Main()
        {
            Tick += OnTick;
        }

        #region Pola
        public static Model[] Jets = new Model[]
            {
                "lazer",
                "hydra",
                "besra"
            };
        #endregion

        public static void OnTick(object sender, EventArgs e)
        {
            if (Game.Player.Character.IsInVehicle() && Game.Player.Character.CurrentVehicle.IsAlive)
            {
                if (Jets.Contains(Game.Player.Character.CurrentVehicle.Model))
                {
                    string[] TXT = new string[] { "GPS DTA", $"LON {Math.Round(Game.Player.Character.Position.X)}", $"LAT {Math.Round(Game.Player.Character.Position.Y)}", World.GetZoneLocalizedName(Game.Player.Character.Position) };
                    Draw(TXT, new PointF(3, 5), 18, 0.6F, DefaultColor);

                    TXT = new string[] { $"SPD {Math.Round(Game.Player.Character.CurrentVehicle.Speed * 3.6, 1)}", $"VSPD {Math.Round(Game.Player.Character.CurrentVehicle.Velocity.Z * 3.6, 1)}", $"ROL {Math.Round(Game.Player.Character.CurrentVehicle.Rotation.X)}°", $"PIT {Math.Round(Game.Player.Character.CurrentVehicle.Rotation.Y)}°" };
                    Draw(TXT, new PointF(3, 300), 23, 0.7f, DefaultColor);

                    Draw(new string[] { $"HDG {Math.Round(Game.Player.Character.CurrentVehicle.Rotation.Z)}°" }, new PointF(GTA.UI.Screen.Width / 2, 3), 23, 0.6F, DefaultColor, GTA.UI.Alignment.Center);

                    Draw(new string[] { $"DATE {World.CurrentDate.ToShortDateString()}", $"TIME {World.CurrentDate.TimeOfDay}" }, new PointF(GTA.UI.Screen.Width * 3 / 4, 3), 23, 0.6F, DefaultColor);

                    int dist = 0;
                    string[] weapons = new string[] { "MISL ", "GUN " };
                    if (!(GetVehWeapon() == -494786007)) { weapons[0] = "[MISL]"; dist = 1500; } else { weapons[1] = "[GUN]";dist = 200; }
                    Draw(new string[] { weapons[0]},new PointF(GTA.UI.Screen.Width*3/4, 620),0,0.6f, DefaultColor, GTA.UI.Alignment.Center);
                    Draw(new string[] { weapons[1]},new PointF(GTA.UI.Screen.Width*3/4+40, 620),0,0.6f, DefaultColor, GTA.UI.Alignment.Center);

                    if (Game.GameTime > ShowDelay)
                    {
                        switch (Game.Player.Character.CurrentVehicle.LandingGearState)
                        {
                            case VehicleLandingGearState.Broken:
                                Draw(new string[] { "BRKN" }, new PointF(GTA.UI.Screen.Width * 3 / 4+ 18, 620 + 23*2), 0, 0.6f, Color.DarkRed);
                                if (Game.GameTime > ShowDelay + 800)
                                    ShowDelay = Game.GameTime + 300;
                                break;
                            case VehicleLandingGearState.Deployed:
                                Draw(new string[] { "DWN" }, new PointF(GTA.UI.Screen.Width * 3 / 4+ 18, 620 + 23*2), 0, 0.6f, Color.Green);
                                break;
                            case VehicleLandingGearState.Retracted:
                                Draw(new string[] { "UP" }, new PointF(GTA.UI.Screen.Width * 3 / 4+ 18, 620 + 23*2), 0, 0.6f, Color.Green);
                                break;
                            case VehicleLandingGearState.Retracting:
                                Draw(new string[] { "RTRCTING" }, new PointF(GTA.UI.Screen.Width * 3 / 4+ 18, 620 + 23*2), 0, 0.6f, Color.Green);
                                if(Game.GameTime> ShowDelay + 800)
                                ShowDelay = Game.GameTime + 300;
                                break;
                            case VehicleLandingGearState.Deploying:
                                Draw(new string[] { "DPLING" }, new PointF(GTA.UI.Screen.Width * 3 / 4+18, 620 + 23*2), 0, 0.6f, Color.Green);
                                if (Game.GameTime > ShowDelay + 800)
                                    ShowDelay = Game.GameTime + 300;
                                break;
                        }
                    }
                    string eng="";
                    if(Game.Player.Character.CurrentVehicle.IsEngineRunning) eng = "ON"; else eng = "OFF";

                    TXT = new string[] { $"HGT {Game.Player.Character.HeightAboveGround.ToString()} XD{GetVehWeapon()}", "GEAR ", $"ENG {eng}" };
                    Draw(TXT,new PointF(GTA.UI.Screen.Width*3/4-17,620+23),23,0.6f, DefaultColor);
                    //Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.ForwardVector * 200
                    RaycastResult Raycast;
                    PointF RecPos;
                    if (dist == 200)
                    {
                        Raycast = World.Raycast(Game.Player.Character.CurrentVehicle.Position - new Vector3(0, 0, 0.92f), Game.Player.Character.CurrentVehicle.Position - new Vector3(0, 0, 0.92f) + Game.Player.Character.CurrentVehicle.ForwardVector * dist, IntersectFlags.Everything, Game.Player.Character.CurrentVehicle);
                        if (Raycast.DidHit) RecPos = GTA.UI.Screen.WorldToScreen(Raycast.HitPosition); else RecPos = GTA.UI.Screen.WorldToScreen(Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.ForwardVector * dist);

                    }
                    else
                    {
                        Raycast = World.Raycast(Game.Player.Character.CurrentVehicle.Position, Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.ForwardVector * dist, IntersectFlags.Everything, Game.Player.Character.CurrentVehicle);

                        RaycastResult Raycast1 = World.Raycast(Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.RightVector * 4f, Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.RightVector * 4f + Game.Player.Character.CurrentVehicle.ForwardVector * dist,dist, IntersectFlags.Everything, Game.Player.Character.CurrentVehicle);
                        RaycastResult Raycast2 = World.Raycast(Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.RightVector * -4f, Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.RightVector * -4f + Game.Player.Character.CurrentVehicle.ForwardVector * dist,dist, IntersectFlags.Everything, Game.Player.Character.CurrentVehicle) ;
                        RecPos = GTA.UI.Screen.WorldToScreen(Game.Player.Character.CurrentVehicle.Position + Game.Player.Character.CurrentVehicle.ForwardVector * dist);
                        if (Raycast1.DidHit&&Raycast2.DidHit) RecPos = GTA.UI.Screen.WorldToScreen(Raycast.HitPosition); 
                        //if (Raycast1.DidHit&&!Raycast2.DidHit) RecPos = GTA.UI.Screen.WorldToScreen(Raycast2.HitPosition); 
                        //if (!Raycast1.DidHit&&Raycast2.DidHit) RecPos = GTA.UI.Screen.WorldToScreen(Raycast1.HitPosition); 
                    }
                    new GTA.UI.CustomSprite(".\\scripts\\Jet_HUD\\W.png", new SizeF(420/5, 420/5), RecPos, DefaultColor, -Game.Player.Character.CurrentVehicle.Rotation.Y,true).Draw();

                    Vehicle[] vehs= World.GetAllVehicles();
                    foreach(Vehicle veh in vehs)
                    {
                        if((veh.ClassType== VehicleClass.Helicopters || veh.ClassType== VehicleClass.Planes)&& veh !=Game.Player.Character.CurrentVehicle && veh.IsAlive)
                        {
                            PointF pos = GTA.UI.Screen.WorldToScreen(veh.Position, true);
                            if (pos != new PointF(0, 0))
                            {
                                new GTA.UI.CustomSprite(".\\scripts\\Jet_HUD\\Rectangle.png", new SizeF(420 / 10, 420 / 10), pos, DefaultColor, 45, true).Draw();
                                new GTA.UI.TextElement($"{Math.Round(Game.Player.Character.Position.DistanceTo(veh.Position))}m", pos, 0.6f, DefaultColor, GTA.UI.Font.ChaletComprimeCologne, GTA.UI.Alignment.Center, true, false).Draw();
                            }
                            }
                    }

                }
            }

        }
        static int ShowDelay;
        static Color DefaultColor = Color.FromArgb(72, 165, 0);
        public static void Draw(string[] str, PointF start, int shift, float size, Color col, GTA.UI.Alignment al = GTA.UI.Alignment.Left)
        {
            int it = 0;
            foreach (string st in str)
            {
                new GTA.UI.TextElement(st, new PointF(start.X, start.Y + shift * it), size, Color.FromArgb(72, 165, 0), GTA.UI.Font.ChaletComprimeCologne, al, true, true).Draw();
                it++;
            }
        }

        public static int GetVehWeapon()
        {
           OutputArgument Output = new OutputArgument();
           Function.Call(Hash.GET_CURRENT_PED_VEHICLE_WEAPON, Game.Player.Character, Output);
           return Output.GetResult<int>();
        }
    }
}