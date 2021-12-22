using UnityEngine;
using UnityEngine.EventSystems;
public class CameraInput : MonoBehaviour, IDragHandler, IEndDragHandler
{
  public bool IsDragging;

  public void OnDrag(PointerEventData eventData)
  {
    IsDragging = true;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    IsDragging = false;
  }
}
