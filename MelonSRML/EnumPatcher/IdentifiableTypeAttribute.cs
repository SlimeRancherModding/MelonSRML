using System;

namespace MelonSRML.EnumPatcher
{
    public class IdentifiableTypeHolderAttribute : Attribute
    {
        public bool shouldCategorize = false;
        public IdentifiableTypeHolderAttribute(bool shouldCategorize = true)
        {
            this.shouldCategorize = shouldCategorize;
        }
    }
}
