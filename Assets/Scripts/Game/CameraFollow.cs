using Core;
using Support;
using UniRx;
using UnityEngine;

namespace Game
{
    public class CameraFollow : DisposableClass
    {
        private readonly Camera _camera;
        private readonly Transform _targetForFollow;
        private readonly Vector3 _offSet;
        private readonly float _smoothSpeed;
        
        public CameraFollow(Camera camera, Transform targetForFollow, Vector3 offSet, float smoothSpeed)
        {
            _camera = camera;
            _targetForFollow = targetForFollow;
            _offSet = offSet;
            _smoothSpeed = smoothSpeed;
        }

        protected override void OnInit()
        {
            base.OnInit();

            Observable
                .EveryLateUpdate()
                .SafeSubscribe(_ => Follow())
                .AddTo(Disposables);
        }

        private void Follow()
        {
            if (_targetForFollow == null) 
                return;
            
            var desiredPosition = _targetForFollow.position + _offSet;
            var smoothedPosition = Vector3.Lerp(_camera.transform.position, desiredPosition, _smoothSpeed);
            _camera.transform.position = smoothedPosition;
        }
    }
}