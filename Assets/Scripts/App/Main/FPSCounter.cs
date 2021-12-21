using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
  [SerializeField] Text fpsText;
  [SerializeField] Image fpsBG;

  int currentFPS;

  void Start()
  {
    StartCoroutine(CountFPS());
  }

  IEnumerator CountFPS()
  {
    while (true)
    {
      currentFPS = (int)(1f / Time.unscaledDeltaTime);
      fpsText.text = currentFPS.ToString() + " fps";

      fpsBG.color = currentFPS >= 50 ? Color.green : Color.red;

      yield return new WaitForSeconds(0.25f);
    }
  }
}
