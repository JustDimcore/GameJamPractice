using UnityEngine;

namespace Player
{
    public static class PlayerInput
    {
        public static float Vertical(PlayerId playerId) => AxisFor($"{(int)playerId}_Vertical");
        public static float Horizontal(PlayerId playerId) => AxisFor($"{(int)playerId}_Horizontal");
        public static bool Jump(PlayerId playerId) => ButtonFor($"{(int)playerId}_Jump");

        private static float AxisFor(string key) => KeyboardAxis(key).Or(JoystickAxis(key));        
        private static float KeyboardAxis(string key) => Axis(key);
        private static float JoystickAxis(string key) => Axis($"{key}_Joystick");
        private static float Axis(string key) => Input.GetAxis(key);

        private static bool ButtonFor(string key) => KeyboardButton(key) || JoystickButton(key);
        private static bool KeyboardButton(string key) => ButtonDown(key);
        private static bool JoystickButton(string key) => ButtonDown($"{key}_Joystick");
        private static bool ButtonDown(string key) => Input.GetButtonDown(key);

        private static float Or(this float axis, float another) => Mathf.Approximately(axis, 0f) ? another : axis;        
    }
}