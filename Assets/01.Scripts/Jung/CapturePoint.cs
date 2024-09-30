using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public float captureTime = 10f; // 카운트 시간
    public Slider CPSlider; // 슬라이더 참조
    private bool playerInZone = false;
    private int enemyCount = 0;

    void Start()
    {
        // 슬라이더의 최소값과 최대값을 설정
        CPSlider.maxValue = captureTime;
        CPSlider.value = captureTime;
    }

    void Update()
    {
        // 플레이어가 존에 있고 적이 없을 때만 카운트가 감소
        if (playerInZone && enemyCount == 0)
        {
            captureTime -= Time.deltaTime;
            captureTime = Mathf.Clamp(captureTime, 0f, 10f);

            // 슬라이더 값 갱신
            CPSlider.value = captureTime;

            if (captureTime == 0)
            {
                Debug.Log("Point Captured!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
        else if (other.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }
}