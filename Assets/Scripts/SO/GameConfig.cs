using System;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        public CameraConfig Camera;
        public MageConfig MainPlayer;
        public EnemyPoolConfig EnemyPool;
        public List<EnemyConfig> Enemies;
        public List<SpellConfig> Spells;
        
        [Serializable]
        public class CameraConfig
        {
            public Vector3 CameraOffset;
            public float SmoothSpeed;
        }
        
        [Serializable]
        public class UnitBaseConfig
        {
            public float Health;
            public float Damage;
            public float Defence;
            public float Speed;
        }
        
        [Serializable]
        public class EnemyPoolConfig
        {
            public int IntervalForPool;
            public int StartRandomIndex;
            public int LimitPoolCount;
        }
        
        [Serializable]
        public class MageConfig
        {
            public UnitBaseConfig Mage;
            public float RotationSpeed;
        }
        
        [Serializable]
        public class EnemyConfig
        {
            public Enemy EnemyPrefab;
            public UnitBaseConfig Enemy;
        }
        
        [Serializable]
        public class SpellConfig
        {
            public Spell Spell;
            public float Damage;
        }
        
    }
}