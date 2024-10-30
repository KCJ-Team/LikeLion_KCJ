using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkConfig", menuName = "Configurations/NetworkConfig")]
public class NetworkConfig : ScriptableObject
{
    public string localServerIP;
    public string releaseServerIP;
    public int serverPort;
}