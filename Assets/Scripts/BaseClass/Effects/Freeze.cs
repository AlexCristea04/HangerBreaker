namespace BaseClass
{
    public class Freeze : StatusEffectInterface
    {
        public bool Applied { get; set; }

        public void ApplyEffect(float time)
        {
            
        }

        public void RemoveEffect()
        {
            throw new System.NotImplementedException();
        }

        public void StackEffect(float time)
        {
            throw new System.NotImplementedException();
        }
    }
}