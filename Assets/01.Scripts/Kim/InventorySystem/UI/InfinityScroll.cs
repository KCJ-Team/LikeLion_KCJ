using UnityEngine;
using UnityEngine.UI;

public class InfinityScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public float scrollSpeed = 0.5f;
    
    private float contentWidth;
    private float viewportWidth;

    private void Start()
    {
        viewportWidth = scrollRect.viewport.rect.width;
        contentWidth = contentPanel.rect.width;

        // Disable vertical scrolling
        scrollRect.vertical = false;
    }

    private void Update()
    {
        // Check if content is wider than viewport
        if (contentWidth > viewportWidth)
        {
            // Get current normalized position
            float normalizedPosition = scrollRect.horizontalNormalizedPosition;

            // If we're at the right edge, wrap to the left
            if (normalizedPosition >= 0.999f)
            {
                scrollRect.horizontalNormalizedPosition = 0.001f;
            }
            // If we're at the left edge, wrap to the right
            else if (normalizedPosition <= 0.001f)
            {
                scrollRect.horizontalNormalizedPosition = 0.999f;
            }

            // Apply continuous scrolling
            scrollRect.horizontalNormalizedPosition += scrollSpeed * Time.deltaTime;
        }
    }
}