using Core;

namespace Game
{
    public abstract class UnitBase : DisposableBehaviour<UnitBase.BaseModel>
    {
        public class BaseModel
        {
            public int Health;
            public int Damage;
            public int Defence;
            public int Speed;

            public BaseModel(int health, int damage, int defence, int speed)
            {
                Health = health;
                Damage = damage;
                Defence = defence;
                Speed = speed;
            }
        }

        protected abstract void TakeDamage();
        
        protected abstract void ToDamage();
    }
}