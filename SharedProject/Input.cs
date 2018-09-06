using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SharedProject
{
    public class Input
    {
        private static KeyboardState oldKeyState;
        private static KeyboardState nowKeyState;

        private static TouchCollection oldTouchLocations;
        private static TouchCollection nowTouchLocations;        

        public Input() { }

        public static void Initialize()
        {
            Update();
        }

        public static void Update()
        {
            oldKeyState = nowKeyState;
            nowKeyState = Keyboard.GetState();

            oldTouchLocations = nowTouchLocations;
            nowTouchLocations = TouchPanel.GetState();
        }

        public static bool IsJustKeyDown(Keys key)
        {
            bool down = false;

            if(oldKeyState.IsKeyUp(key) && nowKeyState.IsKeyDown(key))
            {
                down = true;
            }

            return down;
        }

        public static bool IsJustKeyUp(Keys key)
        {
            bool up = false;

            if (oldKeyState.IsKeyDown(key) && nowKeyState.IsKeyUp(key))
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

        public static bool IsTouch()
        {
            bool touch = false;

            if(nowTouchLocations.Count > 0)
            {
                touch = true;
            }

            return touch;
        }

        public static bool IsTap()
        {
            bool tap = false;

            if (nowTouchLocations.Count == 0)
            {
                return tap;
            }

            foreach (TouchLocation nowTouch in nowTouchLocations)
            {
                foreach(TouchLocation oldTouch in oldTouchLocations)
                {
                    if(nowTouch.State != TouchLocationState.Moved)
                    {
                        break;
                    }
                        
                    if(nowTouch.Id == oldTouch.Id && IsPositionCheck(nowTouch.Position, oldTouch.Position))
                    {
                        tap = true;
                    }
                }

            }

            return tap;
        }

        private static bool IsPositionCheck(Vector2 nowPosition, Vector2 oldPosition)
        {
            //ちょっと範囲を設ける
            float offset = 10.0f;

            if(oldPosition.X - offset <= nowPosition.X && nowPosition.X <= oldPosition.X + offset &&
               oldPosition.Y - offset <= nowPosition.Y && nowPosition.Y <= oldPosition.Y + offset)
            {
                return true;
            }

            return false;
        }
    }
}
