using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
public class SkillTreeDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform content; // content holder
    public RectTransform viewport; // content parent
    private Vector2 lastMousePos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePos = eventData.position;
        Debug.Log("BEGAN DRAGGING");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastMousePos;
        content.anchoredPosition += delta;
        lastMousePos = eventData.position;
        Debug.Log("DRAGGING");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 contentSize = content.rect.size;
        Vector2 viewportSize = viewport.rect.size;
        Debug.Log("END DRAG");
        Vector2 minPos = new Vector2(
            Mathf.Min(0, viewportSize.x - contentSize.x),
            Mathf.Min(0, viewportSize.y - contentSize.y)
        );

        Vector2 maxPos = new Vector2(1000, 1000); // Top left limit(supposedly)

        Vector2 clampedPos = content.anchoredPosition;

        clampedPos.x = Mathf.Clamp(clampedPos.x, minPos.x, maxPos.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minPos.y, maxPos.y);

        content.anchoredPosition = clampedPos;

    }
}
