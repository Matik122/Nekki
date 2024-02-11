using System;
using System.Collections.Generic;
using Pool;
using SO;
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
            public IGamePool Pool;
            public List<GameConfig.SpellConfig> Spells;

            public MageModel(float health, float damage, float defence, float speed, float rotationSpeed, IGamePool pool, List<GameConfig.SpellConfig> spells)
                : base(health, damage, defence, speed)
            {
                RotationSpeed = rotationSpeed;
                Pool = pool;
                Spells = spells;
            }
        }
        
        [SerializeField] private Collider _collider;
        [SerializeField] private Animator _animator;
        
        private string _currentAnimationState;
        
        protected override void OnInit()
        {
            base.OnInit();
            
            new MageInput(ActiveModel.Speed, ActiveModel.RotationSpeed, ActiveModel.Pool, ActiveModel.Spells, this)
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

        public override void SetTrigger(string triggerName) =>
            _animator.SetTrigger(Animator.StringToHash(triggerName));
        
        public override void SetBool(string triggerName, bool state) =>
            _animator.SetBool(Animator.StringToHash(triggerName), state);
        
    }
}