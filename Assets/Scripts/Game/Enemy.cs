using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class Enemy : UnitBase
    {
        [SerializeField] private Animator _animator;
        
        private Mage _mage;

        private bool _hasAttacked;
        
        protected override void OnInit()
        {
            base.OnInit();

            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveToMage())
                .AddTo(Disposables);
        }
        
        protected override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
        
        protected override void Die()
        {
            throw new System.NotImplementedException();
        }

        private void MoveToMage()
        {
            if (gameObject.activeSelf && _mage != null)
            {
                float distanceToMage = Vector3.Distance(transform.position, _mage.transform.position);

                if (distanceToMage > 0.7f)
                {
                    var moveDirection = (_mage.transform.position - transform.position).normalized;
                    transform.position += moveDirection * ActiveModel.Speed * Time.deltaTime;
                    transform.LookAt(_mage.transform);
                }
            }
        }
        
        public bool IsAttackState() =>
            _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");

        public override float ToDamage() =>
            ActiveModel.Damage;

        public void InjectMage(Mage mage) => 
            _mage = mage;
        
        public void ResetAttackFlag() =>
            _hasAttacked = false;
        
        public bool HasAttacked() =>
         _hasAttacked;
        
        public void SetAttacked() =>
            _hasAttacked = true;
        
    }
}