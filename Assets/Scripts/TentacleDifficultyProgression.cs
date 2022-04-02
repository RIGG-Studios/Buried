using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TentacleDifficultyProgression : ScriptableObject
{
    public List<DifficultyProgressionProperties> difficultyProgression = new List<DifficultyProgressionProperties>();

    public DifficultyProgressionProperties FindProperty(int generatorCount)
    {
        for(int  i = 0; i < difficultyProgression.Count; i++)
        {
            if (generatorCount == difficultyProgression[i].generatorCount)
                return difficultyProgression[i];
        }

        return null;
    }
}

[System.Serializable]
public class DifficultyProgressionProperties
{
    public int generatorCount;
    public int tentaclesToSpawn;
}
