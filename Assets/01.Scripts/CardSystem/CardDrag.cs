using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Vector2 originalScale;

    [SerializeField] private float dragScale = 1.1f;
    [SerializeField] private float dragAlpha = 0.6f;

    private GameObject cloneCard;
    private RectTransform cloneRectTransform;
    private CanvasGroup cloneCanvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;

        CreateCloneCard();
        UpdateClonePosition(eventData);

        // 원본 카드의 상호작용 비활성화
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateClonePosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 원본 카드의 상호작용 재활성화
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        bool isValidDropLocation = CheckValidDropLocation(eventData.position);

        if (isValidDropLocation)
        {
            PlaceCardAtValidLocation(eventData.position);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        Destroy(cloneCard);
    }

    private void CreateCloneCard()
    {
        cloneCard = Instantiate(gameObject, canvas.transform);
        cloneRectTransform = cloneCard.GetComponent<RectTransform>();
        cloneCanvasGroup = cloneCard.GetComponent<CanvasGroup>();

        // 클론 카드 설정
        cloneRectTransform.SetAsLastSibling();
        cloneRectTransform.localScale = originalScale * dragScale;
        cloneCanvasGroup.alpha = dragAlpha;
        cloneCanvasGroup.blocksRaycasts = false;

        // 클론 카드의 드래그 컴포넌트 제거
        Destroy(cloneCard.GetComponent<CardDrag>());
    }

    private void UpdateClonePosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        cloneRectTransform.anchoredPosition = localPoint;
    }

    private bool CheckValidDropLocation(Vector2 dropPosition)
    {
        // 여기에 카드를 놓을 수 있는 유효한 위치인지 확인하는 로직을 구현합니다.
        // 예: 레이캐스트를 사용하여 덱 영역이나 필드 영역을 확인
        return true; // 임시로 항상 true를 반환
    }

    private void PlaceCardAtValidLocation(Vector2 dropPosition)
    {
        // 여기에 카드를 유효한 위치에 놓는 로직을 구현합니다.
        // 예: 덱에 추가, 필드에 배치 등
        rectTransform.anchoredPosition = cloneRectTransform.anchoredPosition;
        Debug.Log("Card placed at valid location: " + dropPosition);
    }
}