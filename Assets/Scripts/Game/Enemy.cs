using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class Enemy : UnitBase<Enemy.EnemyModel>, IAttackable
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
        
        private IAnimationAction _animationAction;

        private bool _isDead;
        private bool _hasAttacked;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            _animationAction = new AnimationAction(_animator);
            
            new EnemyMove(ActiveModel.Speed, ActiveModel.Mage, _animationAction, this)
                .Init()
                .AddTo(Disposables);

            new EnemyCollideHitter(_collider, _animationAction, this)
                .Init()
                .AddTo(Disposables);
        }
        
        protected override void Die()
        {
            _isDead = true;
            
            _animationAction.ResetTrigger(AnimationConsts.AttackState);
            _animationAction.SetTrigger(AnimationConsts.DieState);
        }
        
        public override void TakeDamage(float damage)
        {
            ActiveModel.Health -= damage * ActiveModel.Defence;
   
            if (ActiveModel.Health <= 0)
            {
                Die();
            }
        }

        public void Respawn(Transform respawn)
        {
            _isDead = false;
            transform.position = respawn.position;
        }
        
        public void SetAttacked() =>
            _hasAttacked = true;
        
        public void ResetAttackFlag() =>
            _hasAttacked = false;
        
        public override bool IsDead() => 
            _isDead;

        public bool IsAttackState() =>
            _animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationConsts.AttackState);
        
        public bool HasAttacked() =>
            _hasAttacked;
        
        public float ToDamage() =>
            ActiveModel.Damage;
    }
}