using System;

namespace BaseClass
{
    public interface StatusEffectInterface
    {
        public bool Applied { set; get; }
        public void ApplyEffect(float time);
        public void RemoveEffect();
        public void StackEffect(float time);
    }
}