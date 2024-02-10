using Core;

namespace Game
{
    public abstract class UnitBase : DisposableBehaviour<UnitBase.BaseModel>
    {
        public class BaseModel
        {
            public float Health;
            public float Damage;
            public float Defence;
            public float Speed;

            public BaseModel(float health, float damage, float defence, float speed)
            {
                Health = health;
                Damage = damage;
                Defence = defence;
                Speed = speed;
            }
        }
        
        public abstract float ToDamage();

        protected abstract void TakeDamage(float damage);
        
        protected abstract void Die();
    }
}