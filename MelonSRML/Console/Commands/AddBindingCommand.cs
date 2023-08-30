using System.Collections.Generic;
using System.Linq;
using MelonSRML.Utils;
using UnityEngine.InputSystem;

namespace MelonSRML.Console.Commands
{
    public class AddBindingCommand : ConsoleCommand
    {
        public override string ID => "bind";

        public override string Usage => "bind <key> <command>";

        public override string Description => "binds a command to a key";

        public override bool Execute(string[] args)
        {
            KeyBindManager.ConsoleBind bind = new KeyBindManager.ConsoleBind(EnumUtils.Parse<Key>(args[0]), args[1]);
            KeyBindManager.ModBindingsV01.consoleBinds.Add(bind);
            
            return true;
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return EnumUtils.GetAllNames<Key>().ToList();
        }
    }
}