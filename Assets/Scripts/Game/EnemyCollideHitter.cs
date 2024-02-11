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
        private readonly UnitBase<Enemy.EnemyModel> _enemy;

        public EnemyCollideHitter(Collider collider, UnitBase<Enemy.EnemyModel> enemy)
        {
            _collider = collider;
            _enemy = enemy;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            _collider.OnCollisionEnterAsObservable()
                .Where(trigger => trigger.gameObject.layer == LayerMask.NameToLayer("Spell"))
                .SafeSubscribe(trigger =>
                {
                    var spell = trigger.gameObject.GetComponent<IDamageble>();
                    _enemy.TakeDamage(spell.ToDamage());
                }).AddTo(Disposables);
        }
    }
}