using System.Collections.Generic;
using Core;
using Pool;
using SO;
using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class MageInput : DisposableClass
    {
        private readonly float _speed;
        private readonly float _rotationSpeed;
        private readonly IGamePool _pool;
        private readonly List<GameConfig.SpellConfig> _spells;
        private readonly IAnimationAction _animationAction;
        private readonly UnitBase<Mage.MageModel> _mage;

        private int _currentSpellIndex;

        public MageInput(float speed, 
                         float rotationSpeed,
                         IGamePool pool, 
                         List<GameConfig.SpellConfig> spells,
                         IAnimationAction animationAction,
                         UnitBase<Mage.MageModel> mage)
        {
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _pool = pool;
            _spells = spells;
            _animationAction = animationAction;
            _mage = mage;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            Observable
                .EveryUpdate()
                .Where(_ => !_mage.IsDead())
                .SafeSubscribe(_ => InputActions())
                .AddTo(Disposables);
        }
        
        private void InputActions()
        {
            MoveInput();
            SpellInput();
        }

        private void MoveInput()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            _mage.transform.Translate(Vector3.forward * _speed * vertical * Time.deltaTime);
            _mage.transform.Rotate(Vector3.up, _rotationSpeed * horizontal * Time.deltaTime);
            
            _animationAction.SetBool(AnimationConsts.IsWalk, vertical != 0 || horizontal != 0);
        }

        private void SpellInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SelectSpell(-1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SelectSpell(1);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                CastSpell();
            }
            
        }

        private void CastSpell()
        {
            _animationAction.SetTrigger(AnimationConsts.AttackState);
                
            var spell = _pool.Spawn(_spells[_currentSpellIndex].Spell, _mage.transform);
                
            spell
                .Init(new Spell.Model(_spells[_currentSpellIndex].Damage))
                .AddTo(Disposables);
        }
        
        private void SelectSpell(int direction) =>
            _currentSpellIndex = (_currentSpellIndex + direction + _spells.Count) % _spells.Count;
    }
}