using UnityEngine;

public class Blender : MonoBehaviour
{
  public void OnTriggerEnter(Collider other)
  {
    Debug.Log("OnTriggerEnter");
  }

  public void OnTriggerExit(Collider other)
  {
    Debug.Log("OnTriggerExit");
  }
}
