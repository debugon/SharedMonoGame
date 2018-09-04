using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SharedProject
{
    class Input
    {
        private KeyboardState oldKeyState;
        private KeyboardState newKeyState;

        public void Init()
        {
            Update();
        }

        public void Update()
        {
            oldKeyState = newKeyState;
            newKeyState = Keyboard.GetState();
        }

        public bool IsJustKeyDown(Keys key)
        {
            bool down = false;

            if(oldKeyState.IsKeyUp(key) && newKeyState.IsKeyDown(key))
            {
                down = true;
            }

            return down;
        }

        public bool IsJustKeyUp(Keys key)
        {
            bool up = false;

            if (oldKeyState.IsKeyDown(key) && newKeyState.IsKeyUp(key))
            {
                up = true;
            }

            return up;
        }

        public bool IsKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }
    }
}
