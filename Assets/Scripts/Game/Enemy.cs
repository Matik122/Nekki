using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class Enemy : UnitBase<Enemy.EnemyModel>, IDamageble
    {
        public class EnemyModel : UnitBaseModel
        {
            public Mage Mage;

            public EnemyModel(float health, float damage, float defence, float speed, Mage mage)
                : base(health, damage, defence, speed)
            {
                Mage = mage;
            }
        }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _collider;
        
        private bool _hasAttacked;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            new EnemyMove(ActiveModel.Speed, ActiveModel.Mage, this)
                .Init()
                .AddTo(Disposables);

            new EnemyCollideHitter(_collider, this)
                .Init()
                .AddTo(Disposables);
        }
        
        public override void TakeDamage(float damage)
        {
            ActiveModel.Health -= damage * ActiveModel.Defence;
   
            if (ActiveModel.Health <= 0)
            {
                Die();
            }
        }
        
        protected override void Die()
        {
            Debug.LogError("DEAD");
        }
        
        public override void SetTrigger(string triggerName) =>
            _animator.SetTrigger(Animator.StringToHash(triggerName));
        
        public override void SetBool(string triggerName, bool state) =>
            _animator.SetBool(Animator.StringToHash(triggerName), state);
        
        public bool IsAttackState() =>
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        
        public void ResetAttackFlag() =>
            _hasAttacked = false;
        
        public bool HasAttacked() =>
            _hasAttacked;
        
        public void SetAttacked() =>
            _hasAttacked = true;
        
        public float ToDamage() =>
            ActiveModel.Damage;
    }
}