using MelonLoader;

namespace MelonSRML
{
    public abstract class SRMLMelonMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            EntryPoint.preRegister += PreRegister;
            EntryPoint.onSystemContext += OnSystemContext;
            EntryPoint.onGameContext += OnGameContext;
            EntryPoint.onSceneContext += OnSceneContext;
        }

        public virtual void PreRegister(AutoSaveDirector autoSaveDirector)
        {
        }

        public virtual void OnSystemContext(SystemContext context)
        {
        }

        public virtual void OnGameContext(GameContext context)
        {
        }

        public virtual void OnSceneContext(SceneContext context)
        {
        }
    }
}
