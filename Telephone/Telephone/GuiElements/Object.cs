using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telephone.GuiElements
{
    public class Object
    {
        public Vector2 position, origin, velocity;
        public float size = 1,rotation;
        public Texture2D text;
        public Vector4 color = new Vector4(1,1,1,1);
        public Rectangle rect;
        public Object(Vector2 pos)
        {
            position = pos;
        }
        public virtual void LoadText(ContentManager content, string put)
        {
            if (text == null)
            text = content.Load<Texture2D>(put);
            Create();
        }
        public void Create()
        {
            origin = new Vector2(text.Width / 2, text.Height / 2);
            rect = new Rectangle((int)position.X - (int)(text.Width / 2 * size), (int)position.Y - (int)(text.Height / 2 * size), (int)(text.Width * size), (int)(text.Height * size));
        }
        public virtual void Update()
        {
            rect = new Rectangle((int)position.X - (int)(text.Width / 2 * size), (int)position.Y - (int)(text.Height / 2 * size), (int)(text.Width * size), (int)(text.Height * size));
        }
        public virtual void Render(SpriteBatch sb)
        {
            if(text != null)
            sb.Draw(text, position, null, new Color(color), rotation, origin, size, SpriteEffects.None, 0);
        }
    }
}
