using System;

namespace MelonSRML.EnumPatcher
{
    public class IdentifiableTypeHolderAttribute : Attribute
    {
        public bool shouldCategorize = false;
        public bool shouldRegister = true;

        public IdentifiableTypeHolderAttribute(bool shouldCategorize = true, bool shouldRegister = true)
        {
            this.shouldCategorize = shouldCategorize;
            this.shouldRegister = shouldRegister;
        }
    }
}
