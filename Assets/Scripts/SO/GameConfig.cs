using System;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public CameraConfig Camera;
        
        [Serializable]
        public class CameraConfig
        {
            public Vector3 CameraOffset;
            public float SmoothSpeed;
        }
        
    }
}