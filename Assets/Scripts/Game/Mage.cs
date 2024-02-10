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

            _collider.OnTriggerEnterAsObservable()
                .Where(arg => arg.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                .SafeSubscribe(arg =>
                {
                    var enemy = arg.gameObject.GetComponent<Enemy>();
                    TakeDamage(enemy.ToDamage());
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