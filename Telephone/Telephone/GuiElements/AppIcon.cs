using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class AppIcon:Object
    {
        public TelephoneBase phone;
        public Vector2 addPos;
        public string name;
        public AppIcon(Vector2 pos, TelephoneBase tel):base(pos)
        {
            phone = tel;
            addPos = pos;
        }
        public override void Update()
        {
            base.Update();
            if(rect.Contains(Core.mNewState.X,Core.mNewState.Y))
            {
                if(size < 1.1f)
                {
                    size+=0.024f;
                }
            }
            else
            {
                if(size > 1)
                {
                    size -= 0.024f;
                }
            }
        }
        public void UpdatePos()
        {
            addPos += velocity;
            position = phone.currentScreen.position + addPos;
            FixAppPos();
            Update();
        }
        private void FixAppPos()
        {
            for (int i = 1; i < 10; i++)
            {
                if (position.X - phone.currentScreen.position.X < -62*i && position.X - phone.currentScreen.position.X >= -62 * (2 + i))
                {
                    if (position.X - phone.currentScreen.position.X != -62 * i - 31)
                        MoveTo(new Vector2(-62 * i - 31,0));
                    else
                        velocity.X = 0;
                }
                else if (position.X - phone.currentScreen.position.X >= -62 * i && position.X - phone.currentScreen.position.X < (i == 1 ? 0 : -62 * i))
                {
                    if (position.X - phone.currentScreen.position.X != -62 * i + 31)
                        MoveTo(new Vector2(-62 * i + 31,0));
                    else
                        velocity.X = 0;
                }
                else if (position.X - phone.currentScreen.position.X >= (i == 1 ? 0 : 62*i) && position.X - phone.currentScreen.position.X < 62 * i)
                {
                    if (position.X - phone.currentScreen.position.X != 62 * i - 31)
                        MoveTo(new Vector2( 62 * i - 31,0));
                    else
                        velocity.X = 0;
                }
                else if (position.X - phone.currentScreen.position.X >= 62 * i && position.X - phone.currentScreen.position.X < 62 * (2 + i))
                {
                    if (position.X - phone.currentScreen.position.X != 62 * i + 31)
                        MoveTo(new Vector2(62 * i + 31, 0));
                    else
                        velocity.X = 0;
                }
            }
            int Ypos = 62;

            if (position.Y - phone.currentScreen.position.Y < -Ypos*2)
            {
                if (position.Y - phone.currentScreen.position.Y != -Ypos - Ypos/2)
                    MoveTo(new Vector2(0, -Ypos*2 - Ypos/2));
                else
                    velocity.Y = 0;
            }
            else if (position.Y - phone.currentScreen.position.Y < -Ypos && position.Y - phone.currentScreen.position.Y >= -Ypos*2)
            {
                if (position.Y - phone.currentScreen.position.Y != -Ypos  - Ypos/2)
                    MoveTo(new Vector2(0,-Ypos  - Ypos/2));
                else
                    velocity.Y = 0;
            }
            else if (position.Y - phone.currentScreen.position.Y >= -Ypos  && position.Y - phone.currentScreen.position.Y < 0)
            {
                if (position.Y - phone.currentScreen.position.Y != -Ypos  + Ypos/2)
                    MoveTo(new Vector2(0,-Ypos  + Ypos/2));
                else
                    velocity.Y = 0;
            }
            else if (position.Y - phone.currentScreen.position.Y >= 0 && position.Y - phone.currentScreen.position.Y < Ypos )
            {
                if (position.Y - phone.currentScreen.position.Y != Ypos  - Ypos/2)
                    MoveTo(new Vector2(0,Ypos  - Ypos/2));
                else
                    velocity.Y = 0;
            }
            else if (position.Y - phone.currentScreen.position.Y >= Ypos)
            {
                if (position.Y - phone.currentScreen.position.Y != Ypos  + Ypos/2)
                    MoveTo(new Vector2(0,Ypos  + Ypos/2));
                else
                    velocity.Y = 0;
            }
        }
        public virtual void Click()
        {

        }
        private void MoveTo(Vector2 newPos)
        {
            if(newPos.Y == 0)
                velocity.X = (newPos.X - (position.X - phone.currentScreen.position.X)) / 20f;
            else
                velocity.Y = (newPos.Y - (position.Y - phone.currentScreen.position.Y)) / 20f;
        }
        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
            //sb.DrawString(phone.font, "Pos: " + position, new Vector2(0, 70), new Color(phone.fontColor));
        }
    }
}
