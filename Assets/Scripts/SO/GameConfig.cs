using System;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public CameraConfig Camera;
        public MageConfig Mage;
        
        [Serializable]
        public class CameraConfig
        {
            public Vector3 CameraOffset;
            public float SmoothSpeed;
        }
        
        [Serializable]
        public class MageConfig
        {
            public int Health;
            public int Damage;
            public int Defence;
            public int Speed;
        }
        
    }
}