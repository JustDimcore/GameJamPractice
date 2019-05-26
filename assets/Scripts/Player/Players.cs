using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player
{
    public static class Players
    {
        public static void Spawn(int count, GameObject prefab, List<Transform> spawnPoints, List<PlayerControl> controls)
        {
            for (var i = 0; i < count; i++)
            {
                var player = Object.Instantiate(prefab, spawnPoints[i]);
                var control = player.GetComponent<PlayerControl>();
                control.playerId = (PlayerId) (i + 1);
                controls.Add(control);
            }
        }

        public static void Clear(List<PlayerControl> controls)
        {
            foreach (var control in controls)
                Object.Destroy(control.gameObject);

            controls.Clear();
        }
    }
}
