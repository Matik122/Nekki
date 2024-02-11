using Core;
using Support;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class MageCollideHitter : DisposableClass
    {
        private readonly Collider _collider;
        private readonly UnitBase<Mage.MageModel> _mage;
        
        public MageCollideHitter(Collider collider, UnitBase<Mage.MageModel> mage)
        {
            _collider = collider;
            _mage = mage;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            _collider.OnTriggerStayAsObservable()
                .Where(trigger => trigger.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                .SafeSubscribe(trigger =>
                {
                    var enemy = trigger.gameObject.GetComponent<Enemy>();

                    if (enemy.IsAttackState())
                    {
                        if (!enemy.HasAttacked()) 
                        {
                            Debug.LogError("Attack");
                            _mage.SetTrigger("TakeDamage");
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