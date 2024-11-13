using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMap : MonoBehaviour, IMap
{
    [SerializeField]
    protected MapData mapData;
    
    public MapData GetMapData()
    {
        return mapData;
    }

    public virtual void Initialize()
    {
    }

    public virtual void StartGame()
    {
    }
} // end class
