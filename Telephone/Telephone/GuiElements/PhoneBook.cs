using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class PhoneBook:AppIcon
    {
        public PhoneBook(TelephoneBase tel, Vector2 pos):base(pos, tel)
        {
            LoadText(phone.content, "GuiElements\\icon_phonebook");
        }
        public override void Click()
        {
            phone.currentScreen = new PhoneBookScreen(phone);
        }
        public override void Update()
        {
            base.Update();
        }

        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
        }
    }
}
