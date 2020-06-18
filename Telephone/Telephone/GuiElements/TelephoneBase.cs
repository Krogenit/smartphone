using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class TelephoneBase : Object
    {
        private Button buttonBack , buttonHome, buttonSettings;
        public PhoneScreen currentScreen;
        public static SpriteFont font;
        public static Vector4 fontColor = new Vector4(0.5f, 0.5f, 0.5f, 1);
        public ContentManager content;
        public Vector2 mousePressPos, raznostMousePos;
        public bool isMousePress;
        public int screenWidth = 248, screenHeight = 370;
        public TelephoneBase(Vector2 pos):base(pos)
        {
            buttonBack = new Button(pos + new Vector2(-85, 197), true, "", Color.Blue);
            buttonBack.size = 0.75f;
            buttonHome = new Button(pos + new Vector2(0, 197), true, "", Color.Blue);
            buttonHome.size = 0.75f;
            buttonSettings = new Button(pos + new Vector2(85, 197), true, "", Color.Blue);
            buttonSettings.size = 0.75f;
        }
        public override void LoadText(ContentManager content, string put)
        {
            base.LoadText(content, put);
            buttonBack.LoadText(content, "GuiElements\\button_back");
            buttonHome.LoadText(content, "GuiElements\\button_home");
            buttonSettings.LoadText(content, "GuiElements\\button_settings");
            font = content.Load<SpriteFont>("Fonts\\GameFont");
            this.content = content;
            currentScreen = new LoadingScreen(this);
        }
         public void ChangeScreen(PhoneScreen screen)
        {
            currentScreen = screen;
        }
        public override void Update()
        {
            currentScreen.Update();
            if (Core.mNewState.LeftButton == ButtonState.Pressed && Core.mNewState.Y > position.Y - screenHeight / 2 && Core.mNewState.Y < position.Y + screenHeight / 2
               && Core.mNewState.X > 0 && Core.mNewState.X < Core.screenWidth)
            {
                if (mousePressPos == Vector2.Zero)
                mousePressPos = new Vector2(Core.mNewState.X, Core.mNewState.Y);
                raznostMousePos = new Vector2(Core.mNewState.X - mousePressPos.X, Core.mNewState.Y - mousePressPos.Y);
                isMousePress = true;
            }
            else
            {
                mousePressPos = Vector2.Zero;
                isMousePress = false;
            }
            if (Core.mNewState.LeftButton == ButtonState.Released && Core.mOldState.LeftButton == ButtonState.Pressed)
            {
                if(buttonBack.rect.Contains(Core.mNewState.X,Core.mNewState.Y))
                {
                    currentScreen.Back();
                }
                else if (buttonHome.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                {
                    if(currentScreen.GetType() == typeof(MainScreen))
                    {

                    }
                    else
                    currentScreen = new MainScreen(this);
                }
                else if (buttonSettings.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                {

                }
                else
                currentScreen.Click();
            }
            buttonBack.Update();
            buttonHome.Update();
            buttonSettings.Update();
        }

        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (currentScreen != null)
                currentScreen.Render(sb);
            base.Render(sb);
            buttonBack.Render(sb);
            buttonHome.Render(sb);
            buttonSettings.Render(sb);
        }
    }
}
