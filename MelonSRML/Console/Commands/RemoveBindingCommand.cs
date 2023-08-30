using System.Collections.Generic;
using System.Linq;
using MelonSRML.Utils;
using UnityEngine.InputSystem;

namespace MelonSRML.Console.Commands
{
    public class RemoveBindingCommand : ConsoleCommand
    {
        public override string ID => "unbind";

        public override string Usage => "unbind <key>";

        public override string Description => "removes all binds from a key";

        public override bool Execute(string[] args)
        {
            KeyBindManager.ConsoleBind bind = KeyBindManager.ModBindingsV01.consoleBinds.FirstOrDefault(x => x.Key == EnumUtils.Parse<Key>(args[0]));
            if (bind == null)
            {
                MelonLogger.Error("Invalid key");
                return false;
            }

            KeyBindManager.ModBindingsV01.consoleBinds.Remove(bind);
            return true;
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            if (argIndex != 0)
                return base.GetAutoComplete(argIndex, argText);

            return KeyBindManager.ModBindingsV01.consoleBinds.Select(x => x.Key.ToString()).ToList();
        }
    }
}