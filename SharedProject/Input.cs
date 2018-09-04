using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SharedProject
{
    static class Input
    {
        private static KeyboardState oldKeyState;
        private static KeyboardState newKeyState;

        public static void Init()
        {
            Update();
        }

        public static void Update()
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
        }

        public static bool IsJustKeyDown(Keys key)
        {
            bool down = false;

            if(oldKeyState.IsKeyUp(key) && newKeyState.IsKeyDown(key))
            {
                down = true;
            }

            return down;
        }

        public static bool IsJustKeyUp(Keys key)
        {
            bool up = false;

            if (oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key))
            {
                up = true;
            }

            return up;
        }

        public static bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }
    }
}
