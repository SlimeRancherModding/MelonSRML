public static class IdentifiableTypeListExtensions
{
    public static void IfDontContainsAdd(this IdentifiableTypeGroup @this, IdentifiableType identifiableType)
    {
        if (@this is null) return;
        bool found = false;
        @this.memberTypes.ForEach(new System.Action<IdentifiableType>(type =>
        {

            var b = type.name == @identifiableType.name;
            if (b)
            {
                found = true;
            }
        }));
        if (!found)
            @this.memberTypes.Add(identifiableType);
        /*if (!@this.memberTypes.Contains(identifiableType))
            @this.memberTypes.Add(identifiableType);
            */
    }
}