using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraInput : MonoBehaviour, IDragHandler, IEndDragHandler
{
  public bool IsDragging;
  // Update is called once per frame
  public void OnDrag(PointerEventData eventData)
  {
    IsDragging = true;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    IsDragging = false;
  }
}
