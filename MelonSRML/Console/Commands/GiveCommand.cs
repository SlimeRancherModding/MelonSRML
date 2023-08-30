using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Linq;
using MelonSRML.SR2;

namespace MelonSRML.Console.Commands
{
    public class GiveCommand : ConsoleCommand
    {
        public override string ID => "give";

        public override string Usage => "give <id> <amount>";

        public override string Description => "Gives an item";

        public override bool Execute(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                MelonLogger.Error("Incorrect number of arguments!");
                return false;
            }

            IdentifiableType id = SRLookup.IdentifiableTypes.FirstOrDefault(x => x.ValidatableName.Equals(args[0]));
            if (id == null)
                throw new ArgumentException("This ID is incorrect");
            int count = 0;

            if (args.Length != 2 || !int.TryParse(args[1], out count)) count = 1;

            for (int i = 0; i < count; i++)
            {
                SceneContext.Instance.PlayerState?.Ammo.MaybeAddToSlot(id, null);
            }
            return true; 
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            if(argIndex == 0)
            {
                var identifiableTypeGroup = Resources.FindObjectsOfTypeAll<IdentifiableTypeGroup>().FirstOrDefault(x => x.name.Equals("VaccableNonLiquids"));
                return identifiableTypeGroup == null ? new List<string>() : identifiableTypeGroup.GetAllMembers().ToArray().Select(x => x.ValidatableName).ToList();
            }
            return base.GetAutoComplete(argIndex, argText);
        }
    }
}