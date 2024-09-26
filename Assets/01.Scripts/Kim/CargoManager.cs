using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CargoManager : MonoBehaviour
{
    [SerializeField] private List<Transform> pathNodes;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float playerDetectionDistance = 3f;

    private int currentNodeIndex;
    private bool isMoving;
    private Vector3 targetPosition;

    // 시작 시 경로 노드를 검증하고 화물의 초기 위치를 설정한다.
    private void Start()
    {
        if (!ValidatePathNodes()) return;
        
        InitializeCargoPosition();
    }

    // 매 프레임마다 플레이어를 찾고, 움직임 상태를 업데이트하며, 필요시 화물을 이동시킨다.
    private void Update()
    {
        var (player, otherPlayer) = FindPlayers();
        UpdateMovementState(player, otherPlayer);
        if (isMoving) MoveCargo();
    }

    // 경로 노드가 유효한지 확인한다. 노드가 2개 이상 있어야 유효하다.
    private bool ValidatePathNodes()
    {
        if (pathNodes == null || pathNodes.Count < 2)
        {
            return false;
        }
        return true;
    }

    // 화물의 초기 위치를 경로의 중간 지점으로 설정한다.
    private void InitializeCargoPosition()
    {
        currentNodeIndex = (pathNodes.Count - 1) / 2;
        transform.position = Vector3.Lerp(
            pathNodes[currentNodeIndex].position,
            pathNodes[currentNodeIndex + 1].position,
            0.5f
        );
        targetPosition = transform.position;
    }

    // 씬에서 Player_kim과 OtherPlayer_kim 객체를 찾아 반환한다.
    private (GameObject player, GameObject otherPlayer) FindPlayers()
    {
        return (
            FindObjectOfType<Player_kim>()?.gameObject,
            FindObjectOfType<OtherPlayer_kim>()?.gameObject
        );
    }

    // 플레이어들의 위치에 따라 화물의 이동 상태를 업데이트한다.
    private void UpdateMovementState(GameObject player, GameObject otherPlayer)
    {
        bool playerNear = IsObjectNear(player);
        bool otherPlayerNear = IsObjectNear(otherPlayer);

        // Player_kim만 가까이 있으면 다음 노드로 이동한다.
        if (playerNear && !otherPlayerNear)
        {
            MoveTowardsNextNode();
        }
        // OtherPlayer_kim만 가까이 있으면 현재 노드로 이동한다.
        else if (!playerNear && otherPlayerNear)
        {
            MoveTowardsCurrentNode();
        }
        // 둘 다 가까이 있거나 둘 다 멀리 있으면 이동을 멈춘다.
        else
        {
            isMoving = false;
        }
    }

    // 주어진 객체가 화물 근처에 있는지 확인한다.
    private bool IsObjectNear(GameObject obj)
    {
        return obj != null && Vector3.Distance(transform.position, obj.transform.position) <= playerDetectionDistance;
    }

    // 다음 노드로 이동하도록 설정한다.
    private void MoveTowardsNextNode()
    {
        if (currentNodeIndex < pathNodes.Count - 1)
        {
            isMoving = true;
            targetPosition = pathNodes[currentNodeIndex + 1].position;
        }
    }

    // 현재 노드로 이동하도록 설정한다.
    private void MoveTowardsCurrentNode()
    {
        if (currentNodeIndex > -1)
        {
            isMoving = true;
            targetPosition = pathNodes[currentNodeIndex].position;
        }
    }

    // 화물을 목표 위치로 이동시킨다.
    private void MoveCargo()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 목표 위치에 도달하면 노드 인덱스를 업데이트하고 이동 완료 여부를 확인한다.
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            UpdateNodeIndex();
            CheckMovementCompletion();
        }
    }

    // 현재 노드 인덱스를 업데이트한다.
    private void UpdateNodeIndex()
    {
        int direction = (targetPosition == pathNodes[currentNodeIndex + 1].position) ? 1 : -1;
        currentNodeIndex += direction;
    }

    // 화물이 경로의 끝에 도달했는지 확인하고, 도달했다면 이동을 멈춘다.
    private void CheckMovementCompletion()
    {
        if (currentNodeIndex == -1 || currentNodeIndex == pathNodes.Count - 1)
        {
            isMoving = false;
        }
    }
}