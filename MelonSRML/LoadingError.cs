using MelonLoader;
using System;

namespace MelonSRML
{
    internal struct LoadingError
    {
        public Exception Exception;
        public string ModName;
        public MSRModLoader.Step LoadingStep;

        public static void CreateLoadingError(MelonMod mod, MSRModLoader.Step loadingStep, Exception e)
        {

            EntryPoint.error = new LoadingError
            {
                Exception = e,
                LoadingStep = loadingStep,
                ModName =  mod.Info == null ? "mSRML Mod Test" : mod.Info.Name,
            };

            if (loadingStep == MSRModLoader.Step.OnSceneContext)
                EntryPoint.interruptGameLoad = true;
            else
                EntryPoint.interruptMenuLoad = true;
        }

       
    }
}
