using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppSystem.Linq;
using MelonSRML.SR2;

namespace MelonSRML.Console.Commands
{
    class KillAllCommand : ConsoleCommand
    {
        public override string ID => "killall";

        public override string Usage => "killall [radius/type]";

        public override string Description => "Kills all of a type in a radius, or just all";

        public override bool Execute(string[] args)
        {
            int radius = -1;
            List<IdentifiableType> toKill = new List<IdentifiableType>(); 
            foreach(var v in args ?? Array.Empty<string>())
            {
                if(uint.TryParse(v,out uint rad))
                {
                    radius = (int)rad;
                    continue;
                }
                else try
                    {
                        toKill.Add(SRLookup.IdentifiableTypes.FirstOrDefault(x => x.ValidatableName.Equals(v)));
                        //toKill.Add((Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), v, true));
                    }
                    catch
                    {

                    }
                    
            }


            List<GameObject> toDestroy = new List<GameObject>();
            var allActors = SceneContext.Instance.GameModel.AllActors();
            foreach (var v in allActors)
            {
                var actorValue = v.Value;
                var actorTransform = actorValue?.transform;
                var actorPosition = actorTransform?.position ?? SceneContext.Instance.PlayerState.model.position;

                if (radius == -1 || Vector3.Distance(actorPosition, SceneContext.Instance.PlayerState.model.position) < radius)
                {
                    if (toKill.Count == 0 || toKill.Contains(actorValue.ident))
                    {
                        if (actorValue.ident.ReferenceId.Equals("IdentifiableType.Player")) 
                            continue;
                        
                        toDestroy.Add(actorTransform?.gameObject);
                    }
                }
            }

            int deletedCount = 0;
            foreach (var v in toDestroy)
            {
                if (v)
                {
                    deletedCount++;
                    DeathHandler.Kill(v, EntryPoint.KillObject);
                }
            }
            MelonLogger.Msg($"Destroyed {deletedCount} actors!");
            return true;
        }

        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            if(argIndex == 0)
            {
                return Resources.FindObjectsOfTypeAll<IdentifiableTypeGroup>().FirstOrDefault(x => x.name.Equals("VaccableNonLiquids"))?.GetAllMembers().ToArray().Select(x => x.ValidatableName).ToList();
                //return IdentifiableTypes.Select(x => x.ValidatableName).ToList();
            }
            return base.GetAutoComplete(argIndex, argText);        
        }
    }
}