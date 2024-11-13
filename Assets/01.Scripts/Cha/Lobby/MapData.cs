using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapType
{
    Conquest,    // 점령전
    Elimination, // 섬멸전
}

[CreateAssetMenu(fileName = "MapData", menuName = "Map/Map Data")]
public class MapData : ScriptableObject
{
    public int mapId;
    public string mapName;
    public Sprite mapIcon;
    public MapType mapType;
    public GameObject mapObject; // 3d 오브젝트 구체
    public string mapSceneName; // 맵 씬명
}
