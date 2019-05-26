using System.Collections.Generic;
using UnityEngine;

namespace MeatResources
{
  public static class Meats
  {
    public static void Spawn(int count, GameObject prefab,  Transform parent, List<MobController> mobs, List<Meat> meats)
    {
      for (int i = 0; i < count; i++)
      {
        for (var j = 0; j < mobs.Count; j++)
        {
          var go = Object.Instantiate(prefab, parent);
          go.transform.position = mobs[j].transform.position;
          var meat = go.GetComponent<Meat>();
          var rigidbody = meat.GetComponent<Rigidbody>();
          rigidbody.AddForce(RandomDirection * Force, ForceMode.VelocityChange);
          meats.Add(meat);
        }
      }
    }

    public static void Clear(List<Meat> meats)
    {
      foreach (var meat in meats)
        Object.Destroy(meat.gameObject);

      meats.Clear();
    }

    private static float Force
    {
      get { return GameController.Instance.MeatForce; }
    }

    private static Vector3 RandomDirection
    {
      get
      {
        var x = Random.Range(-0.3f, 0.3f);
        var z = Random.Range(-0.3f, 0.3f);
        return new Vector3(x, 1f, z);
      }
    }
  }
}
