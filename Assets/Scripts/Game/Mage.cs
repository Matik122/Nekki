using System.Collections.Generic;
using Windows;
using Pool;
using Services.WindowService;
using SO;
using UniRx;
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
            public ReactiveProperty<float> OnHealthChange;
            public WindowsService WindowsService;
            public FailWindow.Model FailWindowModel;

            public MageModel(float health, 
                             float damage, 
                             float defence, 
                             float speed, 
                             float rotationSpeed, 
                             IGamePool pool, 
                             List<GameConfig.SpellConfig> spells,
                             ReactiveProperty<float> onHealthChange,
                             WindowsService windowsService,
                             FailWindow.Model failWindowModel)
                : base(health, damage, defence, speed)
            {
                RotationSpeed = rotationSpeed;
                Pool = pool;
                Spells = spells;
                OnHealthChange = onHealthChange;
                WindowsService = windowsService;
                FailWindowModel = failWindowModel;
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
            
            ActiveModel.WindowsService.Open(ActiveModel.FailWindowModel, false);
        }
        
        public override void TakeDamage(float damage)
        {
            ActiveModel.Health -= damage * ActiveModel.Defence;
            ActiveModel.OnHealthChange.Value = ActiveModel.Health;
   
            if (ActiveModel.Health <= 0)
            {
                Die();
            }
        }
        
        public override bool IsDead() => 
            _isDead;
    }
}