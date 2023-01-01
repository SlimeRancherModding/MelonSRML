using System;

namespace MelonSRML.SR2.Slime
{
    public abstract class ModdedSlimeSubbehavior : RockSlimeRoll
    {
        
        public ModdedSlimeSubbehavior(IntPtr ptr) : base(ptr) { }

        public abstract float ModdedRelevancy(bool isGrounded);

        public abstract void ModdedSelected();

        public abstract void ModdedAction();

        public virtual void ModdedDeselected()
        {
        }

        public virtual bool ModdedCanRethink() => true;

        public virtual void ModdedAwake()
        {
        }
    }
}
