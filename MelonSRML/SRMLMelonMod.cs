﻿namespace MelonSRML
{
    public abstract class SRMLMelonMod : MelonMod
    {
        public virtual void PreRegister(LookupDirector lookupDirector)
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
