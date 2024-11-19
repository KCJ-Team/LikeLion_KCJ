using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public Spawner _spawner;

    private void Start()
    {
        _spawner.StartCoroutine("SpawnWave");
    }
}
