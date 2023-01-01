using System;

namespace MelonSRML
{
    public static class MSRModLoader
    {
        public static Step CurrentLoadingStep { internal set; get; } = Step.None;
        public static SRMLMelonMod CurrentMod { internal set; get; } = null;
        [Flags]
        public enum Step
        {
            None = 0,
            OnSystemContext = 1,
            PreRegister = 2,
            OnGameContext = 3,
            OnSceneContext = 4
        }
    }
}