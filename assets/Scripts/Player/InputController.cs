using UnityEngine;

namespace Player
{
    public static class InputController
    {
        public static float Vertical(PlayerId playerId)
        {
            return Input.GetAxis($"{(int)playerId}_Vertical");
        }

        public static float Horizontal(PlayerId playerId)
        {
            return Input.GetAxis($"{(int)playerId}_Horizontal");
        }

        public static bool Jump(PlayerId playerId)
        {
            return Input.GetAxis($"{(int)playerId}_Jump") > 0f;
        }
    }
}
