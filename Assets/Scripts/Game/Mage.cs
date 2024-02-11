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

        private IAnimationAction _animationAction;
        private bool _isDead;
        
        protected override void OnInit()
        {
            base.OnInit();

            _animationAction = new AnimationAction(_animator);
            
            new MageInput(ActiveModel.Speed, 
                          ActiveModel.RotationSpeed, 
                          ActiveModel.Pool, 
                          ActiveModel.Spells, 
                          _animationAction, 
                          this)
                .Init()
                .AddTo(Disposables);

            new MageCollideHitter(_collider, _animationAction, this)
                .Init()
                .AddTo(Disposables);
        }
        
        protected override void Die()
        {
            _isDead = true;
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
        
        public override bool IsDead() => 
            _isDead;
    }
}