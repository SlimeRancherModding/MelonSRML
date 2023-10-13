using System;
using System.Collections.Generic;
using System.Linq;

namespace MelonSRML.Console.Commands
{
    public class GiveGadgetCommand : ConsoleCommand
    {
        public override string ID => "givegadget";

        public override string Usage => "givegadget <id> <amount>";

        public override string Description => "Gives an gadget";

        public override bool Execute(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                MelonLogger.Error("Incorrect number of arguments!");
                return false;
            }

            GadgetDefinition id = Resources.FindObjectsOfTypeAll<GadgetDefinition>().FirstOrDefault(x => x.ValidatableName.Equals(args[0]));
            if (id == null)
                throw new ArgumentException("This ID is incorrect");
            int count = 0;

            if (args.Length != 2 || !int.TryParse(args[1], out count)) count = 1;

            for (int i = 0; i < count; i++)
            {
                SRSingleton<SceneContext>.Instance.GadgetDirector.AddItem(id, count);
                //SceneContext.Instance.PlayerState?.Ammo.MaybeAddToSlot(id, null);
            }

            return true;
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            if (argIndex == 0)
            {
                var identifiableTypeGroup =
                    Resources.FindObjectsOfTypeAll<GadgetDefinition>().Select(x => x.ValidatableName);
                return identifiableTypeGroup.ToList();
            }

            return null;
        }
    }
}
            