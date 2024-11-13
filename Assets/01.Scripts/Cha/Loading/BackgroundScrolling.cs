using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [Header("Background Settings")]
    [Tooltip("왼쪽 배경 이미지")]
    public Transform leftBackground;
    
    [Tooltip("오른쪽 배경 이미지")]
    public Transform rightBackground;
    
    [Tooltip("스크롤링 속도")]
    public float scrollSpeed = 5f;

    private float backgroundWidth;

    private void Start()
    {
        // 배경 이미지의 실제 너비 계산
        SpriteRenderer sprite = leftBackground.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            backgroundWidth = sprite.bounds.size.x;
        }
        else
        {
            backgroundWidth = Mathf.Abs(rightBackground.position.x - leftBackground.position.x);
        }

        EnsureProperInitialPosition();
    }

    private void Update()
    {
        // 각 배경 이미지를 개별적으로 이동
        leftBackground.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        rightBackground.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 왼쪽 배경이 화면 밖으로 완전히 나갔는지 체크
        if (leftBackground.position.x <= -backgroundWidth)
        {
            // 왼쪽 배경을 오른쪽 배경의 오른쪽 끝에 재배치
            Vector3 newPosition = rightBackground.position + Vector3.right * backgroundWidth;
            leftBackground.position = newPosition;

            // 배경 참조 교체
            SwapBackgrounds();
        }
    }

    private void SwapBackgrounds()
    {
        Transform temp = leftBackground;
        leftBackground = rightBackground;
        rightBackground = temp;
    }

    private void EnsureProperInitialPosition()
    {
        float currentDistance = rightBackground.position.x - leftBackground.position.x;
        if (Mathf.Abs(currentDistance - backgroundWidth) > 0.1f)
        {
            Debug.LogWarning("배경 이미지들이 올바르게 배치되지 않았습니다. 자동으로 조정합니다.");
            rightBackground.position = new Vector3(
                leftBackground.position.x + backgroundWidth,
                leftBackground.position.y,
                leftBackground.position.z
            );
        }
    }

    private void OnDrawGizmos()
    {
        if (leftBackground != null && rightBackground != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(leftBackground.position, rightBackground.position);
        }
    }
}