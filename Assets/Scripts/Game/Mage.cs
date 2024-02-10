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
        [SerializeField] private Animator _animator;
        
        private float _rotationSpeed;
        private string _currentAnimationState;
        
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
                            _animator.SetTrigger("TakeDamage");
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
            _animator.SetTrigger("Die");
        }
        
        private void MoveMage()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * ActiveModel.Speed * vertical * Time.deltaTime);
            transform.Rotate(Vector3.up, _rotationSpeed * horizontal * Time.deltaTime);
            
            if (vertical != 0 || horizontal != 0)
            {
                _animator.SetBool("IsWalk", true);
            }
            else
            {
                _animator.SetBool("IsWalk", false);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                _animator.SetTrigger("Attack");
            }
        }
        
        public override float ToDamage() => 
            ActiveModel.Damage;
        
        public void InjectRotationSpeed(float rotationSpeed) 
            => _rotationSpeed = rotationSpeed;
    }
}