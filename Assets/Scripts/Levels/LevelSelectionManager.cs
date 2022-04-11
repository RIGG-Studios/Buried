using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject levelUIPrefab = null;
    [SerializeField] private Transform levelUIGrid = null;

    private void Start()
    {
        Level[] allLevels = GameManager.instance.levels;
        Debug.Log(gameObject.name);
        if(allLevels.Length > 0)
        {
            for(int i = 0; i < allLevels.Length; i++)
            {
                LevelUI newLevel = Instantiate(levelUIPrefab, levelUIGrid).GetComponent<LevelUI>();
                if (newLevel != null)
                    newLevel.Initialize(allLevels[i]);
            }
        }
    }


}
