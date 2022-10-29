using MelonLoader;
using System;

namespace MelonSRML
{
    internal struct LoadingError
    {
        public Exception Exception;
        public string ModName;
        public Step LoadingStep;


        public static void CreateLoadingError(MelonMod mod, Step loadingStep, Exception e)
        {
            EntryPoint.error = new LoadingError
            {
                Exception = e,
                LoadingStep = loadingStep,
                ModName = mod.Info.Name
            };

            if (loadingStep == Step.OnSceneContext)
                EntryPoint.interruptGameLoad = true;
            else
                EntryPoint.interruptMenuLoad = true;
        }

        public enum Step
        {
            PreRegister,
            OnSystemContext,
            OnGameContext,
            OnSceneContext
        }
    }
}
