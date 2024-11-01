using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NetworkConfig", menuName = "Configurations/NetworkConfig")]
public class NetworkConfig : ScriptableObject
{
    public enum IPType
    {
        Local,
        Release
    }

    public IPType selectedIPType; // 현재 선택된 IP 타입
    public string localServerIP; // 로컬
    public string releaseServerIP; // 배포
    public string serverIP; // 현재 사용 중인 IP
    public int serverPort;
}