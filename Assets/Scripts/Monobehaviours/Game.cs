using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;

    public MonsterController monster { get; private set; }  
    public Player player { get; private set; }

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();
        monster = FindObjectOfType<MonsterController>();
    }
}
