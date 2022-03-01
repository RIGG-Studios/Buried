using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Room : ScriptableObject
{
    [System.Serializable]
    public class RoomEnemySpawnProperties
    {
        public int enemyCount;
    }

    public enum RoomType
    {
        RegularRoom,
        SecretRoom,
        Hallway
    }

    public enum OxygenIntensity
    {
        Low,
        Med,
        High
    }

    public RoomType roomType;

    //regular room
    public bool breathable;
    public OxygenIntensity oxygenIntensity;

    public bool spawnEnemies;
    public RoomEnemySpawnProperties[] enemies;

    public bool restrictVision;
    public float vignetteIntensity;

    public bool tripOut;
    public float tripIntensity;

    public GameObject roomPrefab;
}
