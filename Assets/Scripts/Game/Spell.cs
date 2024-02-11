using Core;

namespace Game
{
    public class Spell : DisposableBehaviour<Spell.Model>
    {
        public class Model
        {
            public float Damage;
            
            public Model(float damage)
            {
                Damage = damage;
            }
        }
        
        public float ToDamage() => 
            ActiveModel.Damage;
    }
}