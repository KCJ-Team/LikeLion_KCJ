using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoint : MonoBehaviour
{
    public float captureTime; // 카운트 시간
    public Slider CPSlider; // 슬라이더 참조

    public LayerMask playerLayer; // 플레이어 레이어 설정
    public LayerMask enemyLayer; // 적 레이어 설정
    public Vector3 boxSize = new Vector3(5f, 5f, 5f); // 감지 박스 크기

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
        // 감지 상태를 주기적으로 업데이트
        DetectEntities();

        // 플레이어가 존에 있고 적이 없을 때만 카운트가 감소
        if (playerInZone && enemyCount == 0)
        {
            captureTime -= Time.deltaTime;

            // 슬라이더 값 갱신
            CPSlider.value = captureTime;

            if (captureTime <= 0)
            {
                Debug.Log("Point Captured!");
            }
        }
    }

    void DetectEntities()
    {
        // 현재 박스 영역 내의 모든 플레이어 감지
        Collider[] playerColliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, playerLayer);
        playerInZone = playerColliders.Length > 0; // 플레이어가 있으면 true, 없으면 false

        // 현재 박스 영역 내의 모든 적 감지
        Collider[] enemyColliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.identity, enemyLayer);
        enemyCount = enemyColliders.Length; // 적의 수를 업데이트
    }

    // 구체적인 감지 영역을 시각적으로 확인하기 위해 Gizmos를 사용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}