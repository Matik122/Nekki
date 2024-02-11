using Core;
using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class EnemyCollideHitter : DisposableClass
    {
        private readonly Collider _collider;
        private readonly IAnimationAction _animationAction;
        private readonly UnitBase<Enemy.EnemyModel> _enemy;

        public EnemyCollideHitter(Collider collider, IAnimationAction animationAction, UnitBase<Enemy.EnemyModel> enemy)
        {
            _collider = collider;
            _animationAction = animationAction;
            _enemy = enemy;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            Observable.Merge(
                    _collider.OnCollisionEnterAsObservable()
                        .Select(collision => collision.collider)
                        .Where(trigger => trigger.LayerValidation("Spell") && !_enemy.IsDead())
                        .Do(OnCollisionWithSpell),
                    _collider.OnTriggerStayAsObservable()
                        .Where(trigger => trigger.LayerValidation("Mage") && !_enemy.IsDead())
                        .Do(_ => _animationAction.SetTrigger(AnimationConsts.AttackState)))
                .EmptySubscribe()
                .AddTo(Disposables);
        }

        private void OnCollisionWithSpell(Collider collider)
        {
            _animationAction.SetTrigger(AnimationConsts.DamageState);
            
            var spell = collider.gameObject.GetComponent<IDamageble>();
            _enemy.TakeDamage(spell.ToDamage());
        }
        
    }
}