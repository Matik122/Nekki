using Core;
using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class MageInput : DisposableClass
    {
        private readonly float _speed;
        private readonly float _rotationSpeed;
        private readonly UnitBase<Mage.MageModel> _mage;

        public MageInput(float speed, float rotationSpeed, UnitBase<Mage.MageModel> mage)
        {
            _speed = speed;
            _rotationSpeed = rotationSpeed;
            _mage = mage;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveMage())
                .AddTo(Disposables);
        }
        
        private void MoveMage()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            _mage.transform.Translate(Vector3.forward * _speed * vertical * Time.deltaTime);
            _mage.transform.Rotate(Vector3.up, _rotationSpeed * horizontal * Time.deltaTime);
            
            if (vertical != 0 || horizontal != 0)
            {
                _mage.SetBool("IsWalk", true);
            }
            else
            {
                _mage.SetBool("IsWalk", false);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                _mage.SetTrigger("Attack");
            }
        }
    }
}