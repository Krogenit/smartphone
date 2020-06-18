using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public enum TextBoxType
    {
        Number = 0, All = 1, AllWhitoutProbel = 2
    }
    public class TextBox : Object
    {
        private SpriteFont font;
        public bool isSelected;
        private Keys[] lastKeys;
        private double timer;
        public String stringDate;
        private TextBoxType boxType;
        private String name;
        private int curTimer;
        private KeyboardState keyNewState;
        private int maxSize = 16;
        private Vector4 lightColor;
        public TextBox(Vector2 pos, TelephoneBase tel, TextBoxType tbt, String text, int maxStringSize):base(pos)
        {
            this.name = text;
            boxType = tbt;
            font = TelephoneBase.font;
            maxSize = maxStringSize-1;
        }

        public override void Update()
        {
            if (rect.Contains(Core.mNewState.X, Core.mNewState.Y) || isSelected)
            {
                if (lightColor.W < 1)
                {
                    lightColor.X += 0.02f;
                    lightColor.Y += 0.075f;
                    lightColor.Z += 0.1f;
                    lightColor.W += 0.1f;
                }
            }
            else
            {
                if (lightColor.W > 0)
                {
                    lightColor.X -= 0.02f;
                    lightColor.Y -= 0.075f;
                    lightColor.Z -= 0.1f;
                    lightColor.W -= 0.1f;
                }
            }
            keyNewState = Keyboard.GetState();
            if (--curTimer <= 0)
                curTimer = 60;
            if(isSelected)
            {
                Keys[] keys = keyNewState.GetPressedKeys();

                foreach (Keys currentKey in keys)
                {
                    if (currentKey != Keys.None)
                    {
                        if (lastKeys.Contains(currentKey))
                        {
                            if ((Core.gt.TotalGameTime.TotalMilliseconds - timer > 125))
                            {
                                if(boxType == TextBoxType.All)
                                {
                                    HandleKey(Core.gt, currentKey);
                                }
                                else if(boxType == TextBoxType.Number)
                                {
                                    HandleNumber(Core.gt, currentKey);
                                }
                            }
                        }
                        else if (!lastKeys.Contains(currentKey))
                        {
                            if (boxType == TextBoxType.All)
                            {
                                HandleKey(Core.gt, currentKey);
                            }
                            else if (boxType == TextBoxType.Number)
                            {
                                HandleNumber(Core.gt, currentKey);
                            }
                        }
                    }
                }
                lastKeys = keys;
            }
        }
        private void HandleKey(GameTime gameTime, Keys currentKey)
        {
            string keyString = currentKey.ToString();
            if (currentKey == Keys.Space && boxType != TextBoxType.AllWhitoutProbel && (stringDate == null || stringDate.Length <= maxSize))
                stringDate += " ";
            else if ((currentKey == Keys.Back || currentKey == Keys.Delete) && stringDate.Length > 0)
                stringDate = stringDate.Remove(stringDate.Length - 1);
            else if (stringDate== null || stringDate.Length <= maxSize)
                stringDate += GetKey(currentKey);
            timer = gameTime.TotalGameTime.TotalMilliseconds;
        }
        private void HandleNumber(GameTime gameTime, Keys currentKey)
        {
            string keyString = currentKey.ToString();
            if (currentKey == Keys.Space && boxType != TextBoxType.AllWhitoutProbel && (stringDate == null || stringDate.Length <= maxSize))
                stringDate += " ";
            else if ((currentKey == Keys.Back || currentKey == Keys.Delete) && stringDate.Length > 0)
                stringDate = stringDate.Remove(stringDate.Length - 1);
            else if (stringDate == null || stringDate.Length <= maxSize)
                stringDate += GetNumFromKey(currentKey);
            timer = gameTime.TotalGameTime.TotalMilliseconds;
        }
        private string GetKey(Keys keys)
        {
            if (keys != Keys.LeftShift && keys != Keys.Back && keys != Keys.Delete)
            {
                string num = GetNumFromKey(keys);
                if (num != "")
                    return num;
                else
                {
                    if (keyNewState.IsKeyDown(Keys.LeftShift))
                        return keys.ToString();
                    else
                        return keys.ToString().ToLower();
                }
            }
            else
            {
                return "";
            }
        }
        private string GetNumFromKey(Keys keys)
        {
            if (keys != Keys.LeftShift && keys != Keys.Back && keys != Keys.Delete)
            {
                switch (keys)
                {
                    case Keys.D0:
                        return "0";
                    case Keys.D1:
                        return "1";
                    case Keys.D2:
                        return "2";
                    case Keys.D3:
                        return "3";
                    case Keys.D4:
                        return "4";
                    case Keys.D5:
                        return "5";
                    case Keys.D6:
                        return "6";
                    case Keys.D7:
                        return "7";
                    case Keys.D8:
                        return "8";
                    case Keys.D9:
                        return "9";
                    case Keys.OemPeriod:
                        return ".";
                    case Keys.OemQuestion:
                        return ".";
                    default:
                        return "";
                }
            }
            else
            {
                return "";
            }
        }
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
            spriteBatch.Draw(text, position, null, new Color(lightColor), rotation, origin, size, SpriteEffects.None, 0);
            if(stringDate != null)
                spriteBatch.DrawString(font, stringDate, new Vector2(position.X - origin.X + 2, position.Y - 12), new Color(TelephoneBase.fontColor));
            if (curTimer > 30 && isSelected)
            {
                Vector2 p;
                String s;
                if (stringDate != null)
                {
                    p = new Vector2(position.X - origin.X   + stringDate.Length * 11, position.Y - 15);
                    s = stringDate + "|";
                }
                else
                {
                    p = new Vector2(position.X - origin.X , position.Y - 15);
                    s = "|";
                }
                spriteBatch.DrawString(font, "|",p, new Color(TelephoneBase.fontColor));
            }
            else if (!isSelected && text != null && (stringDate == null || stringDate.Length <= 0))
                spriteBatch.DrawString(font, name, new Vector2(position.X - name.Length * 5 - 12, position.Y - 12), new Color(TelephoneBase.fontColor));
        }
    }
}
