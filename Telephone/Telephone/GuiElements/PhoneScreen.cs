using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class PhoneScreen : Object
    {
        public TelephoneBase phone;
        public Vector2 addPos;
        public PhoneScreen(TelephoneBase tel):base(tel.position)
        {
            phone = tel;
        }
        public override void LoadText(Microsoft.Xna.Framework.Content.ContentManager content, string put)
        {
            base.LoadText(content, put);
        }
        public override void Update()
        {

        }
        public virtual void Click()
        {

        }
        public virtual void Back()
        {

        }
        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
            //sb.DrawString(phone.font, "Pos: " + position.X, new Vector2(0,35), new Color(phone.fontColor));
        }
    }
}
