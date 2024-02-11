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
        private readonly IAnimationAction _animationAction;
        private readonly UnitBase<Enemy.EnemyModel> _enemy;

        private const float DistanceToMage = 0.7f;

        public EnemyMove(float speed, Mage mage, IAnimationAction animationAction, UnitBase<Enemy.EnemyModel> enemy)
        {
            _speed = speed;
            _mage = mage;
            _animationAction = animationAction;
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
            var enemyValidation = _enemy.gameObject.activeSelf && !_enemy.IsDead();
            
            if (enemyValidation)
            {
                var distanceToMage = Vector3.Distance(_enemy.transform.position, _mage.transform.position);
                var isDistanceReached = distanceToMage > DistanceToMage;

                if (isDistanceReached)
                {
                    var moveDirection = (_mage.transform.position - _enemy.transform.position).normalized;
                    _enemy.transform.position += moveDirection * _speed * Time.deltaTime;
                    _enemy.transform.LookAt(_mage.transform);
                }
            }
            
            _animationAction.SetBool(AnimationConsts.IsWalk, enemyValidation);
        }
    }
}