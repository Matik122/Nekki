using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Enemy : UnitBase<Enemy.EnemyModel>
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
        
        private bool _hasAttacked;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            new EnemyMove(ActiveModel.Speed, ActiveModel.Mage, this)
                .Init()
                .AddTo(Disposables);
        }
        
        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Die()
        {
            throw new System.NotImplementedException();
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
        
        public override float ToDamage() =>
            ActiveModel.Damage;
    }
}