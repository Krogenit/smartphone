using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class Button : Object
    {
        public string name;
        private Texture2D lightText;
        private bool isLightText;
        private Vector4 lightColor;
        public Vector2 stringPos;
        private Color lightColorMax;
        public Button(Vector2 pos, bool lightText, string name, Color maxC):base(pos)
        {
            isLightText = lightText;
            this.name = name;
            lightColorMax = maxC;
        }

        public override void LoadText(Microsoft.Xna.Framework.Content.ContentManager content, string put)
        {
            base.LoadText(content, put);
            if (isLightText)
            {
                if (lightText == null)
                    lightText = content.Load<Texture2D>(put+"light");
            }
        }
        public override void Update()
        {
            if (rect.Contains(Core.mNewState.X, Core.mNewState.Y))
            {
                if (lightColor.W < 1)
                {
                    if (lightColorMax == Color.Blue)
                    {
                        lightColor.X += 0.02f;
                        lightColor.Y += 0.075f;
                        lightColor.Z += 0.1f;
                        lightColor.W += 0.1f;
                    }
                    else if (lightColorMax == Color.Red)
                    {
                        lightColor.X += 0.1f;
                        lightColor.Y += 0.03f;
                        lightColor.Z += 0.03f;
                        lightColor.W += 0.1f;
                    }
                }
            }
            else
            {
                if (lightColor.W > 0)
                {
                    if (lightColorMax == Color.Blue)
                    {
                        lightColor.X -= 0.02f;
                        lightColor.Y -= 0.075f;
                        lightColor.Z -= 0.1f;
                        lightColor.W -= 0.1f;
                    }
                    else if(lightColorMax == Color.Red)
                    {
                        lightColor.X -= 0.1f;
                        lightColor.Y -= 0.03f;
                        lightColor.Z -= 0.03f;
                        lightColor.W -= 0.1f;
                    }
                }
            }
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
        public override void Render(SpriteBatch sb)
        {
            base.Render(sb);
            if (lightText != null)
                sb.Draw(lightText, position, null, new Color(lightColor), rotation, origin, size, SpriteEffects.None, 0);
            if(name != "")
            {
                sb.DrawString(TelephoneBase.font, name,position + stringPos, new Color(TelephoneBase.fontColor));
            }
        }
    }
}
