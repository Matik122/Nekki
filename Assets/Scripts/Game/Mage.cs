using System;
using Support;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class Mage : UnitBase
    {
        [SerializeField] private Collider _collider;
        
        private float _rotationSpeed;
        
        protected override void OnInit()
        {
            base.OnInit();

            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveMage())
                .AddTo(Disposables);

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
                            TakeDamage(enemy.ToDamage());
                            enemy.SetAttacked();
                        }
                    } 
                    else
                    {
                        enemy.ResetAttackFlag();
                    }

                }).AddTo(Disposables);
            
        }
        
        protected override void TakeDamage(float damage)
        {
            ActiveModel.Health -= damage * ActiveModel.Defence;
   
            if (ActiveModel.Health <= 0)
            {
                Die();
            }
        }

        protected override void Die()
        {
            gameObject.SetActive(false);
        }
        
        private void MoveMage()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * ActiveModel.Speed * vertical * Time.deltaTime);
            transform.Rotate(Vector3.up, _rotationSpeed * horizontal * Time.deltaTime);
        }
        
        public override float ToDamage() => 
            ActiveModel.Damage;
        
        public void InjectRotationSpeed(float rotationSpeed) 
            => _rotationSpeed = rotationSpeed;
    }
}