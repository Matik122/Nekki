using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Enemy : UnitBase
    {
        private Mage _mage;
        
        protected override void OnInit()
        {
            base.OnInit();

            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveToMage())
                .AddTo(Disposables);
        }

        public void InjectMage(Mage mage)
        {
            _mage = mage;
        }
        
        protected override void TakeDamage()
        {
            throw new System.NotImplementedException();
        }

        protected override void ToDamage()
        {
            throw new System.NotImplementedException();
        }
        
        private void MoveToMage()
        {
            if (gameObject.activeSelf && _mage.transform != null)
            {
                var moveDirection = (_mage.transform .position - transform.position).normalized;
                transform.position += moveDirection * ActiveModel.Speed * Time.deltaTime;
                transform.LookAt(_mage.transform);
            }
        }
    }
}