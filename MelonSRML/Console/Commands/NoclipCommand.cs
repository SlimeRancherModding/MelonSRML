﻿using Il2CppMonomiPark.SlimeRancher.Player;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelonSRML.Console.Commands
{
    internal class GravityCommand : ConsoleCommand
    {
        public SRCharacterController Player;

        public static T Get<T>(string name) where T : UnityEngine.Object
        {
            return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(x => x.name == name);
        }

        public override string ID => "gravity";

        public override string Usage => "gravity <active>";

        public override string Description => "Toggles gravity";

        public override bool Execute(string[] args)
        {
            if (args.Length < 1 || args.Length >= 2)
            {
                MelonLogger.Error("Incorrect number of arguments!");
                return false;
            }

            if (Get<SRCharacterController>("PlayerControllerKCC") != null)
            {
                Player = Get<SRCharacterController>("PlayerControllerKCC");
                try
                {
                    Player.BypassGravity = !bool.Parse(args[0].ToLower());
                    return true;
                }
                catch
                {
                    MelonLogger.Error("Invalid arguments? Try using true/false.");
                    return false;
                }
            }
            else return false;
        }
    }
    internal class NoclipCommand : ConsoleCommand
    {
        public static T Get<T>(string name) where T : UnityEngine.Object
        {
            return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(x => x.name == name);
        }

        public override string ID => "noclip";

        public override string Usage => "noclip";

        public override string Description => "Toggles noclip";

        public override bool Execute(string[] args)
        {
            if (args != null)
            {
                return false;
            }
            try
            {
                var cam = Get<GameObject>("PlayerCameraKCC");
                if (cam.GetComponent<NoclipComponent>() == null)
                {
                    cam.AddComponent<NoclipComponent>();
                }
                else
                {
                    UnityEngine.Object.Destroy(cam.GetComponent<NoclipComponent>());
                }
                return true;
            }
            catch { return false; }
        }
    }
    internal class NoclipSpeedCommand : ConsoleCommand
    {
        public static T Get<T>(string name) where T : UnityEngine.Object
        {
            return Resources.FindObjectsOfTypeAll<T>().FirstOrDefault(x => x.name == name);
        }

        public override string ID => "noclipadjustspeed";

        public override string Usage => "noclipadjustspeed <float>";

        public override string Description => "Sets the adjust speed of noclip (default 235)";

        public override bool Execute(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                return false;
            }
            try
            {
                NoclipComponent.speedAdjust = float.Parse(args[0]);
                return true;
            }
            catch { return false; }
        }
    }
}
