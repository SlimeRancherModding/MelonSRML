using Il2CppMonomiPark.SlimeRancher.Damage;
using Il2CppMonomiPark.SlimeRancher.World;

namespace MelonSRML.Console.Commands
{
    class KillCommand : ConsoleCommand
    {
        public override string ID => "kill";

        public override string Usage => "kill";

        public override string Description => "Kills what you're looking at";
        
        public override bool Execute(string[] args)
        {
            if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out var hit))
            {
                var gameobject = hit.collider.gameObject;
                if (gameobject.GetComponent<Identifiable>())
                {
                    DeathHandler.Kill(gameobject, EntryPoint.KillObject);
                }
                else if (gameobject.GetComponentInParent<Gadget>())
                {
                    gameobject.GetComponentInParent<Gadget>().RequestDestroy("ok");
                }
                else if (gameobject.GetComponentInParent<LandPlot>())
                {
                    gameobject.GetComponentInParent<LandPlotLocation>().Replace(gameobject.GetComponentInParent<LandPlot>(), GameContext.Instance.LookupDirector.GetPlotPrefab(LandPlot.Id.EMPTY));
                }
            }
            MelonLogger.Error("Not looking at a valid object!");
            return false;
        }
    }
}