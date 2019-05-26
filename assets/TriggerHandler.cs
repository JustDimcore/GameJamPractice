using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
  public Blender Target;
  
  private void OnTriggerEnter(Collider other)
  {
    Target.OnTriggerEnter(other);
  }

  private void OnTriggerExit(Collider other)
  {
    Target.OnTriggerExit(other);
  }
}
