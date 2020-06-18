using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class Contact:Object
    {
        public string name, number;
        private Button remove;
        private Vector4 lightColor;
        public Object fon;
        private TelephoneBase phone;
        public Contact(TelephoneBase tel,string name,string number):base(Vector2.Zero)
        {
            phone = tel;
            LoadText(tel.content, "GuiElements\\gui_human");
            this.name = name;
            this.number = number;
            remove= new Button(Vector2.Zero,true,"",Color.Red);
            remove.LoadText(tel.content, "GuiElements\\gui_remove");
            remove.size = 0.65f;
            fon = new Object(Vector2.Zero);
            fon.LoadText(tel.content, "GuiElements\\gui_contact");
        }
        public void Update(Vector2 pos)
        {
            position = pos;
            if (fon.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
            {
                if (lightColor.W < 0.4f)
                {
                    lightColor.X += 0.005f;
                    lightColor.Y += 0.01875f;
                    lightColor.Z += 0.025f;
                    lightColor.W += 0.025f;
                }
            }
            else
            {
                if (lightColor.W > 0)
                {
                    lightColor.X -= 0.005f;
                    lightColor.Y -= 0.01875f;
                    lightColor.Z -= 0.025f;
                    lightColor.W -= 0.025f;
                }
            }
            fon.position = position + new Vector2(100, 0);
            fon.color = lightColor;
            fon.Update();
            remove.Update();
            remove.position = position + new Vector2(200, 0);
            FixColor();
            base.Update();
        }
        private void FixColor()
        {
            if (lightColor.X > 1)
            {
                lightColor.X = 1;
            }
            else if (lightColor.X < 0)
            {
                lightColor.X = 0;
            }
            if (lightColor.Y > 1)
            {
                lightColor.Y = 1;
            }
            else if (lightColor.Y < 0)
            {
                lightColor.Y = 0;
            }
            if (lightColor.Z > 1)
            {
                lightColor.Z = 1;
            }
            else if (lightColor.Z < 0)
            {
                lightColor.Z = 0;
            }
            if (lightColor.W > 1)
            {
                lightColor.W = 1;
            }
            else if (lightColor.W < 0)
            {
                lightColor.W = 0;
            }
        }
        public void Click(int contactnum)
        {
            if (phone.currentScreen.GetType() == typeof(PhoneBookScreen))
            {
                PhoneBookScreen pbs = (PhoneBookScreen)phone.currentScreen;
                if (remove.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                {
                    pbs.RemoveContact(contactnum);
                }
                else
                {
                    pbs.EditContact(contactnum);
                }
            }
        }
        public void Render(SpriteBatch sb)
        {
            fon.Render(sb);
            base.Render(sb);
            remove.Render(sb);
            sb.DrawString(TelephoneBase.font, name, position + new Vector2(30,-26), new Color(TelephoneBase.fontColor));
            sb.DrawString(TelephoneBase.font, number, position + new Vector2(30, 0), new Color(TelephoneBase.fontColor));
        }
    }
}
