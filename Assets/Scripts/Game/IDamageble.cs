namespace Game
{
    public interface IDamageble
    {
        public float ToDamage();
    }

    public interface IAttackable : IDamageble
    {
        public bool IsAttackState();
        public void ResetAttackFlag();
        public bool HasAttacked();
        public void SetAttacked();
    }
}