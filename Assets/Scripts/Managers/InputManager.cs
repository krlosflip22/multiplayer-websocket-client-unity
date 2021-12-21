using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  private static InputManager instance;

  [SerializeField] MobileInputController mobileInput;
  [SerializeField] CameraInput cameraInput;

  public static bool IsDragging => instance.cameraInput.IsDragging;
  public static float Vertical => instance.mobileInput.Vertical;
  public static float Horizontal => instance.mobileInput.Horizontal;

  void Awake()
  {
    instance = this;
  }

  void OnDestroy()
  {
    instance = null;
  }
}
