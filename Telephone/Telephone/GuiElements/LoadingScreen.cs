using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    class LoadingScreen : PhoneScreen
    {
        private int loadingTimer = 0, advanceTimer;
        private int maxLoadingTimer = 600, timeToCreateNewRing;
        private List<Object> rings = new List<Object>();
        public LoadingScreen(TelephoneBase tel) : base(tel)
        {
            base.LoadText(tel.content, "GuiElements\\screen_loading");
        }
        public override void Update()
        {
            if (loadingTimer < maxLoadingTimer)
            {
                if (--advanceTimer <= 0)
                {
                    loadingTimer += Core.rand.Next(25);
                    advanceTimer = Core.rand.Next(10);
                }
                if(rings.Count <= 6 && --timeToCreateNewRing <= 0)
                {
                    Object ring = new Object(new Vector2(-50, Core.screenHeight / 2));
                    ring.LoadText(phone.content, "GuiElements\\ring");
                    rings.Add(ring);
                    timeToCreateNewRing = 15;
                }
                for(int i=0;i<rings.Count;i++)
                {
                    if (rings[i].position.X < Core.screenWidth / 2 - 30)
                        rings[i].velocity.X = ((Core.screenWidth / 2 - 30) - rings[i].position.X) / 15f;
                    else if (rings[i].position.X >= Core.screenWidth / 2 - 30 && rings[i].position.X < Core.screenWidth / 2 + 30)
                        rings[i].velocity.X = 1.5f;
                    else if (rings[i].position.X > Core.screenWidth / 2 + 30)
                        rings[i].velocity.X = (rings[i].position.X - (Core.screenWidth / 2 + 30))/15f;
                    if (rings[i].position.X > Core.screenWidth + 5000)
                        rings[i].position.X = -5000;
                    if (rings[i].velocity.X < 1.5f)
                        rings[i].velocity.X = 1.5f;
                    rings[i].position.X += rings[i].velocity.X;
                }
            }
            else
            {
                phone.ChangeScreen(new MainScreen(phone));
            }
            base.Update();
        }
        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
            for (int i = 0; i < rings.Count; i++)
            {
                rings[i].Render(sb);
            }
            sb.DrawString(TelephoneBase.font, "" + (int)(((float)loadingTimer / (float)maxLoadingTimer) * 100) + "%",
                phone.position + new Vector2(-3 * 6, 75), Color.Gray);
        }
    }
}
