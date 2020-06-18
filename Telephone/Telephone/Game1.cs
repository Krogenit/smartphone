using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Telephone.GuiElements;
using Microsoft.Xna.Framework.Storage;

namespace Telephone
{
    public class Core : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private TelephoneBase tel;
        public static int screenWidth, screenHeight;
        public static MouseState mOldState, mNewState;
        public static int boundX, boundY;
        public static Random rand = new Random();
        public static GameTime gt;
        public static bool firstStart = true;
        public static StorageDevice sDev;
        public static IAsyncResult res;
        public Core()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 250;
            graphics.PreferredBackBufferHeight = 450; 
            screenWidth = graphics.PreferredBackBufferWidth;
            screenHeight = graphics.PreferredBackBufferHeight;
        }
        protected override void Initialize()
        {
            
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Create();
        }
        private void Create()
        {
            tel = new TelephoneBase(new Vector2(screenWidth/2, screenHeight/2));
            tel.LoadText(Content, "GuiElements\\phone");
        }
        protected override void Update(GameTime gameTime)
        {
            gt = gameTime;
            boundX = Window.ClientBounds.X;
            boundY = Window.ClientBounds.Y;
            mNewState = Mouse.GetState();
            tel.Update();
            mOldState = mNewState;
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            tel.Render(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
