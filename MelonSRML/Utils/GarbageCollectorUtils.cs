using Il2CppSystem.Collections.Generic;
using Object = Il2CppSystem.Object;

namespace MelonSRML.Utils
{
    public static class GarbageCollectorUtils
    {
        private static List<Object> AntiGarbageCollectorList = new List<Object>();

        static GarbageCollectorUtils()
        {
        }

        public static Object AddToAntiGC(this Object @this)
        {
            AntiGarbageCollectorList.Add(@this);
            return @this;
        }
    }
}