using Core;
using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class EnemyMove : DisposableClass
    {
        private readonly Mage _mage;
        private readonly float _speed;
        private readonly UnitBase<Enemy.EnemyModel> _enemy;

        public EnemyMove(float speed, Mage mage, UnitBase<Enemy.EnemyModel> enemy)
        {
            _speed = speed;
            _mage = mage;
            _enemy = enemy;
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            Observable
                .EveryUpdate()
                .SafeSubscribe(_ => MoveToMage())
                .AddTo(Disposables);
        }
        
        private void MoveToMage()
        {
            if (_enemy.gameObject.activeSelf && _mage != null)
            {
                float distanceToMage = Vector3.Distance(_enemy.transform.position, _mage.transform.position);

                if (distanceToMage > 0.7f)
                {
                    var moveDirection = (_mage.transform.position - _enemy.transform.position).normalized;
                    _enemy.transform.position += moveDirection * _speed * Time.deltaTime;
                    _enemy.transform.LookAt(_mage.transform);
                }
            }
        }
    }
}