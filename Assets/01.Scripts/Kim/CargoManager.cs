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

    private void Start()
    {
        if (!ValidatePathNodes()) return;
        
        InitializeCargoPosition();
    }

    private void Update()
    {
        var (player, otherPlayer) = FindPlayers();
        UpdateMovementState(player, otherPlayer);
        if (isMoving) MoveCargo();
    }

    private bool ValidatePathNodes()
    {
        if (pathNodes == null || pathNodes.Count < 2)
        {
            return false;
        }
        return true;
    }

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

    private (GameObject player, GameObject otherPlayer) FindPlayers()
    {
        return (
            FindObjectOfType<Player>()?.gameObject,
            FindObjectOfType<OtherPlayer>()?.gameObject
        );
    }

    private void UpdateMovementState(GameObject player, GameObject otherPlayer)
    {
        bool playerNear = IsObjectNear(player);
        bool otherPlayerNear = IsObjectNear(otherPlayer);

        if (playerNear && !otherPlayerNear)
        {
            MoveTowardsNextNode();
        }
        else if (!playerNear && otherPlayerNear)
        {
            MoveTowardsCurrentNode();
        }
        else
        {
            isMoving = false;
        }
    }

    private bool IsObjectNear(GameObject obj)
    {
        return obj != null && Vector3.Distance(transform.position, obj.transform.position) <= playerDetectionDistance;
    }

    private void MoveTowardsNextNode()
    {
        if (currentNodeIndex < pathNodes.Count - 1)
        {
            isMoving = true;
            targetPosition = pathNodes[currentNodeIndex + 1].position;
        }
    }

    private void MoveTowardsCurrentNode()
    {
        if (currentNodeIndex > -1)
        {
            isMoving = true;
            targetPosition = pathNodes[currentNodeIndex].position;
        }
    }

    private void MoveCargo()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            UpdateNodeIndex();
            CheckMovementCompletion();
        }
    }

    private void UpdateNodeIndex()
    {
        int direction = (targetPosition == pathNodes[currentNodeIndex + 1].position) ? 1 : -1;
        currentNodeIndex += direction;
    }

    private void CheckMovementCompletion()
    {
        if (currentNodeIndex == -1 || currentNodeIndex == pathNodes.Count - 1)
        {
            isMoving = false;
        }
    }
}