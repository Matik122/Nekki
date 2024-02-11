using Core;
using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class MageCollideHitter : DisposableClass
    {
        private readonly Collider _collider;
        private readonly IAnimationAction _animationAction;
        private readonly UnitBase<Mage.MageModel> _mage;
        
        public MageCollideHitter(Collider collider,  IAnimationAction animationAction, UnitBase<Mage.MageModel> mage)
        {
            _collider = collider;
            _animationAction = animationAction;
            _mage = mage;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            _collider.OnTriggerStayAsObservable()
                .Where(trigger => trigger.LayerValidation("Enemy") && !_mage.IsDead())
                .SafeSubscribe(trigger =>
                {
                    var enemy = trigger.gameObject.GetComponent<IAttackable>();

                    if (enemy.IsAttackState())
                    {
                        if (!enemy.HasAttacked()) 
                        {
                            _animationAction.SetTrigger(AnimationConsts.DamageState);
                            _mage.TakeDamage(enemy.ToDamage());
                            enemy.SetAttacked();
                        }
                    } 
                    else
                    {
                        enemy.ResetAttackFlag();
                    }

                }).AddTo(Disposables);
        }
    }
}