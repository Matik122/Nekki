using Core;

namespace Game
{
    public abstract class UnitBase<T> : DisposableBehaviour<T> where T : UnitBase<T>.UnitBaseModel
    {
        public class UnitBaseModel
        {
            public float Health;
            public float Damage;
            public float Defence;
            public float Speed;

            public UnitBaseModel(float health, float damage, float defence, float speed)
            {
                Health = health;
                Damage = damage;
                Defence = defence;
                Speed = speed;
            }
        }

        /*public abstract void SetTrigger(string triggerName);

        public abstract void SetBool(string triggerName, bool state);*/

        public abstract void TakeDamage(float damage);
        
        protected abstract void Die();
    }
}