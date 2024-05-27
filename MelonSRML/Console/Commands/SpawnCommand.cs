using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Linq;
using MelonSRML.SR2;

namespace MelonSRML.Console.Commands
{
    public class SpawnCommand : ConsoleCommand
    {
        public override string ID => "spawn";

        public override string Usage => "spawn <id> [count]";

        public override string Description => "Spawns actor";

        public override bool Execute(string[] args)
        {
            if(args.Length < 1 || args.Length > 2)
            {
                MelonLogger.Error("Incorrect number of arguments!");
                return false;
            }

            GameObject prefab;
            try
            {
                var id = SRLookup.IdentifiableTypes.FirstOrDefault(x => x.ValidatableName.Equals(args[0]));
                prefab = id.prefab;
            }
            catch
            {
                MelonLogger.Error("Invalid ID!");
                return false;
            }

            int count = 0;

            if (args.Length != 2 || !int.TryParse(args[1], out count)) count = 1;

            for(int i = 0; i < count; i++)
            {
                if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out var hit))
                {
                    var spawned = InstantiationHelpers.InstantiateActor(prefab, SceneContext.Instance.RegionRegistry.CurrentSceneGroup, Vector3.zero, Quaternion.identity);
                    if (spawned.GetComponent<Collider>() != null)
                        spawned.transform.position = hit.point+hit.normal*PhysicsUtil.CalcRad(spawned.GetComponent<Collider>());
                    var delta = -(hit.point - Camera.main.transform.position).normalized;
                    spawned.transform.rotation = Quaternion.LookRotation(delta, hit.normal);
                }
            }
            return true;

        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            
            if(argIndex == 0)
            {
                return Resources.FindObjectsOfTypeAll<IdentifiableTypeGroup>().FirstOrDefault(x => x.name.Equals("VaccableNonLiquids"))?.GetAllMembers().ToArray().Select(x => x.ValidatableName).ToList();
            }
            return base.GetAutoComplete(argIndex, argText);
        }
    }
}