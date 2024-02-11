using System;
using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class Mage : UnitBase<Mage.MageModel>
    {
        public class MageModel : UnitBaseModel
        {
            public float RotationSpeed;

            public MageModel(float health, float damage, float defence, float speed, float rotationSpeed)
                : base(health, damage, defence, speed)
            {
                RotationSpeed = rotationSpeed;
            }
        }
        
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        
        private string _currentAnimationState;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            new MageInput(ActiveModel.Speed, ActiveModel.RotationSpeed, this)
                .Init()
                .AddTo(Disposables);

            new MageCollideHitter(_collider, this)
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
            SetTrigger("Die");
        }
        
        public override float ToDamage() => 
            ActiveModel.Damage;

        public override void SetTrigger(string triggerName) =>
            _animator.SetTrigger(Animator.StringToHash(triggerName));
        
        public override void SetBool(string triggerName, bool state) =>
            _animator.SetBool(Animator.StringToHash(triggerName), state);
        
    }
}