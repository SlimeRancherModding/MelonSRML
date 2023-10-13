using Il2CppSystem.Collections.Generic;

namespace MelonSRML.Utils.Extensions;

public static class IdentifiableTypeListExtensions
{
    public static void AddIfNotContaining(this IdentifiableTypeGroup @this, IdentifiableType identifiableType)
    {
        if (@this is null) 
            return;

        foreach (IdentifiableType t in new List<IdentifiableType>(@this.GetAllMembers()))
        {
            if (identifiableType == t)
                return;
        }

        @this.memberTypes.Add(identifiableType);
    }
}