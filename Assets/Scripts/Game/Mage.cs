using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Mage : UnitBase
    {
        [SerializeField] private float _rotationSpeed = 180f;
        
        protected override void OnInit()
        {
            base.OnInit();

            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveMage())
                .AddTo(Disposables);
        }

        protected override void TakeDamage()
        {
            throw new System.NotImplementedException();
        }

        protected override void ToDamage()
        {
            throw new System.NotImplementedException();
        }

        private void MoveMage()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            transform.Translate(Vector3.forward * ActiveModel.Speed * vertical * Time.deltaTime);
            transform.Rotate(Vector3.up, _rotationSpeed * horizontal * Time.deltaTime);
        }
    }
}