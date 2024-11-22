using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SceneSingleton<GameManager>
{
    public GameObject Player;
    public PlayerData playerData;
    public CardDatabase CardDatabase;

    protected override void Awake()
    {
        base.Awake();
    }
}
