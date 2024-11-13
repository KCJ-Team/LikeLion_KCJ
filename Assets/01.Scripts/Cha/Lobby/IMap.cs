using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap 
{
    MapData GetMapData();
    void Initialize();
    void StartGame();
}
