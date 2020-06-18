using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telephone.GuiElements
{
    [Serializable]
    public struct AppIcons
    {
        public Vector2[] position;
        public bool fStart;
    }
    public class MainScreen : PhoneScreen
    {
        private List<AppIcon> icons = new List<AppIcon>();
        private Object line,lines;
        private AppIcon allApps;
        private bool isAppToMouse, isMovingScreen;
        private int timerToTake, appPutNum;
        public MainScreen(TelephoneBase tel)
            : base(tel)
        {
            Load();
            this.LoadText(tel.content, "GuiElements\\screen_mainfone");
            if (Core.firstStart)
            {
                PhoneBook pb = new PhoneBook(phone, new Vector2(-70, 100));
                icons.Add(pb);
            }
            line = new Object(phone.position + new Vector2(0, 124));
            line.LoadText(phone.content, "GuiElements\\gui_line");
            lines = new Object(phone.position + new Vector2(0, 125));
            lines.LoadText(phone.content, "GuiElements\\gui_lines");
            lines.color = new Vector4(0.2f, 0.75f, 1f, 1);

            allApps = new AppIcon(phone.position + new Vector2(0, 155), phone);
            allApps.LoadText(phone.content, "GuiElements\\icon_allapps");
            Core.firstStart = false;
            Save();
        }
        public override void LoadText(Microsoft.Xna.Framework.Content.ContentManager content, string put)
        {
            base.LoadText(content, put);
        }
        private void MouseDown()
        {
            if(!isAppToMouse && !isMovingScreen)
            for (int i = 0; i < icons.Count; i++)
            {
                if (icons[i].rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                {
                    timerToTake++;
                    if (timerToTake > 10)
                    {
                        isAppToMouse = true;
                        appPutNum = i;
                        timerToTake = 0;
                    }
                    return;
                }
            }
            MoveScreen(addPos + phone.raznostMousePos);
        }
        public override void Update()
        {
            if (phone.isMousePress && !isAppToMouse)
            {
                MouseDown();
            }
            else
            {
                addPos = position;
                FindScreenPos();
                if(!phone.isMousePress)
                {
                    if (isAppToMouse)
                    {
                        icons[appPutNum].addPos = new Vector2(Core.mNewState.X - position.X, Core.mNewState.Y - position.Y);
                        isAppToMouse = false;
                        appPutNum = -1;
                    }
                }
            }
            if(isAppToMouse)
            {
                if(Core.mNewState.X > Core.screenWidth - 25 && velocity.X > -1f)
                {
                    velocity.X = -4;
                }
                else if (Core.mNewState.X < 25 && velocity.X < 1f)
                {
                    velocity.X = 4;
                }
            }
            position.X += velocity.X;
            FixPos();
            UpdateIcons();
            lines.position.X = Core.screenWidth/2 - (position.X-124)/5.55f;
            allApps.Update();
            base.Update();
        }
        public override void Click()
        {
            Save();
            if (!isMovingScreen && !isAppToMouse)
            {
                for (int i = 0; i < icons.Count; i++)
                {
                    if (icons[i].rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        icons[i].Click();
                        return;
                    }
                }
            }
        }
        private void UpdateIcons()
        {
            for (int i = 0; i < icons.Count; i++)
            {
                if ((isAppToMouse && i != appPutNum) || !isAppToMouse)
                {
                    icons[i].UpdatePos();
                }
            }
            if(isAppToMouse)
            {
                icons[appPutNum].position = new Vector2(Core.mNewState.X, Core.mNewState.Y);
                if (icons[appPutNum].position.X > Core.screenWidth)
                    icons[appPutNum].position.X = Core.screenWidth;
                else if (icons[appPutNum].position.X < 0)
                    icons[appPutNum].position.X = 0;
                else if (icons[appPutNum].position.Y > phone.screenHeight+38)
                    icons[appPutNum].position.Y = phone.screenHeight+38;
                else if (icons[appPutNum].position.Y < 38)
                    icons[appPutNum].position.Y = 38;
            }
        }
        private void MoveScreen(Vector2 newPos)
        {
            position.X = newPos.X;
            if (phone.raznostMousePos.X > 5 || phone.raznostMousePos.X < -5)
                isMovingScreen = true;
            else
                isMovingScreen = false;
        }
        private void FixPos()
        {
            if (position.X > text.Width / 2)
            {
                position.X = text.Width / 2;
            }
            else if (position.X < -(text.Width / 2) + Core.screenWidth)
            {
                position.X = -(text.Width / 2) + Core.screenWidth;
            }
        }
        private void FindScreenPos()
        {
            if (position.X < 248 && position.X >= 0)
            {
                if (position.X != 248 - 124)
                    MoveTo(248 - 124);
                else
                    velocity.X = 0;
            }
            else if (position.X >= 248 && position.X < 496)
            {
                if (position.X != 496 - 124)
                    MoveTo(496 - 124);
                else
                    velocity.X = 0;
            }
            else if (position.X >= 496)
            {
                if (position.X != 744 - 124)
                    MoveTo(744 - 124);
                else
                    velocity.X = 0;
            }
            else if (position.X >= -248 && position.X < 0)
            {
                if (position.X != -124)
                    MoveTo(-124);
                else
                    velocity.X = 0;
            }
            else if (position.X < -248)
            {
                if (position.X != -248 - 124)
                    MoveTo(-248 - 124);
                else
                    velocity.X = 0;
            }
        }
        private void MoveTo(int newPos)
        {
            velocity.X = (newPos - position.X)/20f;
        }
        public void Save()
        {
            Core.res = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            Core.sDev = StorageDevice.EndShowSelector(Core.res);
            IAsyncResult result =
                Core.sDev.BeginOpenContainer("Phone", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = Core.sDev.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            string filename = "appIcons.sav";
            if (container.FileExists(filename))
                container.DeleteFile(filename);
            Stream stream = container.CreateFile(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(AppIcons));
            AppIcons appsData = new AppIcons();
            appsData.position = new Vector2[icons.Count];
            for (int i = 0; i < icons.Count; i++)
            {
                appsData.position[i] = icons[i].addPos;
            }
            appsData.fStart = Core.firstStart;
            serializer.Serialize(stream, appsData);
            stream.Close();

            container.Dispose();
        }
        public void Load()
        {
            Core.res = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            Core.sDev = StorageDevice.EndShowSelector(Core.res);
            IAsyncResult result =
                Core.sDev.BeginOpenContainer("Phone", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = Core.sDev.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            string filename = "appIcons.sav";
            if (!container.FileExists(filename))
            {
                container.Dispose();
                return;
            }
            Stream stream = container.OpenFile(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(AppIcons));
            AppIcons appsData = (AppIcons)serializer.Deserialize(stream);
            for (int i = 0; i < appsData.position.Length; i++)
            {
                PhoneBook icon = new PhoneBook(phone, appsData.position[i]);
                icons.Add(icon);
            }
            Core.firstStart = appsData.fStart;
            stream.Close();

            container.Dispose();
        }
        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
            for(int i=0;i<icons.Count;i++)
            {
                icons[i].Render(sb);
            }
            allApps.Render(sb);
            line.Render(sb);
            lines.Render(sb);
        }
    }
}
