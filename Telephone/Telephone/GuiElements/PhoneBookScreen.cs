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
    public struct ContactsList
    {
        public string[] name;
        public string[] number;
        public bool fStart;
    }
    public class PhoneBookScreen : PhoneScreen
    {
        private Button addContact, acceptAdd;
        private TextBox nameBox, phoneBox, searchBox;
        private List<Contact> contacts = new List<Contact>();
        private List<Contact> contactsSearched = new List<Contact>();
        private Object screenFonAdd, search;
        private Vector2 camPos;
        private bool isMovingScreen;
        private int editableContact;
        private bool firstStart = true;
        private enum GuiType
        {
            Main = 0, Add = 1, Edit = 2
        }
        private GuiType guiType;
        public PhoneBookScreen(TelephoneBase tel):base(tel)
        {
            Load();
            base.LoadText(tel.content, "GuiElements\\screen_phonebook");
            addContact = new Button(new Vector2(30, 64), true, "Новый", Color.Blue);
            addContact.LoadText(tel.content, "GuiElements\\gui_add");
            addContact.size = 0.75f;
            addContact.stringPos = new Vector2(10, 0);
            acceptAdd = new Button(new Vector2(125, 386), true, "Добавить", Color.Blue);
            acceptAdd.LoadText(phone.content, "GuiElements\\gui_button");
            acceptAdd.stringPos = new Vector2(-50, -10);
            guiType = GuiType.Main;
            nameBox = new TextBox(new Vector2(105, 180), phone, TextBoxType.All, "Имя",16);
            nameBox.LoadText(phone.content, "GuiElements\\gui_inputbox");
            phoneBox = new TextBox(new Vector2(105, 240), phone, TextBoxType.Number, "Номер", 11);
            phoneBox.LoadText(phone.content, "GuiElements\\gui_inputbox");
            screenFonAdd = new Object(new Vector2( Core.screenWidth / 2, 66));
            screenFonAdd.LoadText(phone.content, "GuiElements\\screen_phonebookadd");
            search = new Object(new Vector2(75, 64));
            search.LoadText(phone.content, "GuiElements\\gui_search");
            //search.color = new Vector4(0.2f, 0.75f, 1, 1);
            searchBox = new TextBox(new Vector2(170, 64), phone, TextBoxType.All, "Поиск",8);
            searchBox.LoadText(phone.content, "GuiElements\\gui_inputsearch");
            search.position = new Vector2(120, 64);
            searchBox.position = new Vector2(190, 64);
            if (firstStart)
            {
                Contact c = new Contact(phone, "Balance", "104");
                contacts.Add(c);
                c = new Contact(phone, "Mother", "89181112233");
                contacts.Add(c);
                c = new Contact(phone, "Father", "89184433121");
                contacts.Add(c);
                c = new Contact(phone, "Brother", "89187711889");
                contacts.Add(c);
                c = new Contact(phone, "Friend", "89187722889");
                contacts.Add(c);
                c = new Contact(phone, "Antoha", "89003475712");
                contacts.Add(c);
                c = new Contact(phone, "My number", "89237643712");
                contacts.Add(c);
                c = new Contact(phone, "OLD NUMBER", "89648527592");
                contacts.Add(c);
                c = new Contact(phone, "Matvey", "8964245314");
                contacts.Add(c);
            }
            firstStart = false;
            Save();
        }
        private void MoveScreen(Vector2 newPos)
        {
            camPos.Y = newPos.Y;
            if (phone.raznostMousePos.Y > 5 || phone.raznostMousePos.Y < -5)
                isMovingScreen = true;
            else
                isMovingScreen = false;
        }
        public override void Update()
        {
            if(guiType == GuiType.Main)
            {
                if (phone.isMousePress)
                {
                    MoveScreen(addPos + phone.raznostMousePos);
                }
                else
                {
                    addPos = camPos;
                }
                addContact.Update();
                searchBox.Update();
                int count=0;
                if (searchBox.stringDate != null && searchBox.stringDate.Length > 0)
                {
                    count = (contactsSearched.Count);
                }
                else
                {
                    count = (contacts.Count);
                }
                if (camPos.Y < (count > 5 ? (count-5) * -54 : 0))
                    camPos.Y = (count > 5 ? (count-5) * -54 : 0);
                else if (camPos.Y > 0)
                    camPos.Y = 0;
                if (searchBox.stringDate != null && searchBox.stringDate.Length > 0)
                {
                    contactsSearched.Clear();
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        bool searched = true;
                        for(int j=0;j<searchBox.stringDate.Length;j++)
                        {
                            if (j < contacts[i].name.Length)
                            {
                                if (contacts[i].name[j] == searchBox.stringDate[j])
                                {

                                }
                                else
                                {
                                    searched = false;
                                    break;
                                }
                            }
                        }
                        if(searched)
                        {
                            contactsSearched.Add(contacts[i]);
                        }
                    }
                    for (int i = 0; i < contactsSearched.Count; i++)
                    {
                        contactsSearched[i].Update(new Vector2(28, 120 + i * 54) + camPos);
                    }
                }
                else
                {
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        contacts[i].Update(new Vector2(28, 120 + i * 54) + camPos);
                    }
                }
            }
            else if(guiType == GuiType.Add)
            {
                nameBox.Update();
                phoneBox.Update();
                acceptAdd.name = "Добавить";
                acceptAdd.Update();
            }
            else if(guiType == GuiType.Edit)
            {
                nameBox.Update();
                phoneBox.Update();
                acceptAdd.name = "Изменить";
                acceptAdd.Update();
            }
            base.Update();
        }
        public override void Back()
        {
            Save();
            if (guiType == GuiType.Main)
            {
                phone.currentScreen = new MainScreen(phone);
            }
            else if (guiType == GuiType.Add || guiType == GuiType.Edit)
            {
                guiType = GuiType.Main;
            }
        }
        public void RemoveContact(int num)
        {
            contacts.RemoveAt(num);
        }
        public void EditContact(int num)
        {
            guiType = GuiType.Edit;
            editableContact = num;
            nameBox.stringDate = contacts[editableContact].name;
            phoneBox.stringDate = contacts[editableContact].number;
        }
        public override void Click()
        {
            if (!isMovingScreen)
            {
                nameBox.isSelected = false;
                phoneBox.isSelected = false;
                searchBox.isSelected = false;
                if (guiType == GuiType.Main)
                {
                    if (searchBox.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        searchBox.isSelected = true;
                    }
                    else if (addContact.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        nameBox.stringDate = null;
                        phoneBox.stringDate = null;
                        guiType = GuiType.Add;
                        return;
                    }
                    else if (searchBox.stringDate != null && searchBox.stringDate.Length > 0)
                    {
                        for (int i = 0; i < contactsSearched.Count; i++)
                        {
                            if (contactsSearched[i].fon.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                            {
                                contactsSearched[i].Click(i);
                                return;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < contacts.Count; i++)
                        {
                            if (contacts[i].fon.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                            {
                                contacts[i].Click(i);
                                return;
                            }
                        }
                    }
                }
                else if (guiType == GuiType.Add)
                {
                    if (nameBox.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        nameBox.isSelected = true;
                    }
                    else if (phoneBox.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        phoneBox.isSelected = true;
                    }
                    else if (acceptAdd.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        if (nameBox.stringDate != null && nameBox.stringDate.Length > 0 &&
                            phoneBox.stringDate != null && phoneBox.stringDate.Length > 0)
                        {
                            Contact c = new Contact(phone, nameBox.stringDate, phoneBox.stringDate);
                            contacts.Add(c);
                            guiType = GuiType.Main;
                        }
                    }
                }
                else if (guiType == GuiType.Edit)
                {
                    if (nameBox.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        nameBox.isSelected = true;
                    }
                    else if (phoneBox.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        phoneBox.isSelected = true;
                    }
                    else if (acceptAdd.rect.Contains(Core.mNewState.X, Core.mNewState.Y))
                    {
                        if (nameBox.stringDate != null && nameBox.stringDate.Length > 0 &&
                            phoneBox.stringDate != null && phoneBox.stringDate.Length > 0)
                        {
                            contacts[editableContact].name = nameBox.stringDate;
                            contacts[editableContact].number = phoneBox.stringDate;
                            guiType = GuiType.Main;
                            editableContact = -1;
                        }
                    }
                }
            }
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

            string filename = "phoneBookContacts.sav";
            if (container.FileExists(filename))
                container.DeleteFile(filename);
            Stream stream = container.CreateFile(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(ContactsList));
            ContactsList contactsList = new ContactsList();
            contactsList.name = new string[contacts.Count];
            contactsList.number = new string[contacts.Count];
            for (int i = 0; i < contacts.Count; i++)
            {
                contactsList.name[i] = contacts[i].name;
                contactsList.number[i] = contacts[i].number;
            }
            contactsList.fStart = firstStart;
            serializer.Serialize(stream, contactsList);
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
            string filename = "phoneBookContacts.sav";
            if (!container.FileExists(filename))
            {
                container.Dispose();
                return;
            }
            Stream stream = container.OpenFile(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(ContactsList));
            ContactsList contactsList = (ContactsList)serializer.Deserialize(stream);
            for (int i = 0; i < contactsList.name.Length; i++)
            {
                Contact c = new Contact(phone, contactsList.name[i], contactsList.number[i]);
                contacts.Add(c);
            }
            firstStart = contactsList.fStart;
            stream.Close();

            container.Dispose();
        }
        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Render(sb);
            if (guiType == GuiType.Main)
            {
                if (searchBox.stringDate != null && searchBox.stringDate.Length > 0)
                {
                    for (int i = 0; i < contactsSearched.Count; i++)
                    {
                        contactsSearched[i].Render(sb);
                    }
                }
                else
                {
                    for (int i = 0; i < contacts.Count; i++)
                    {
                        contacts[i].Render(sb);
                    }
                }
                screenFonAdd.Render(sb);
                addContact.Render(sb);
                searchBox.Render(sb);
                search.Render(sb);
            }
            else if(guiType == GuiType.Add)
            {
                sb.DrawString(TelephoneBase.font, "Добавить контакт", position + new Vector2(-100,-170), new Color(TelephoneBase.fontColor));
                nameBox.Render(sb);
                phoneBox.Render(sb);
                acceptAdd.Render(sb);
            }
            else if(guiType == GuiType.Edit)
            {
                sb.DrawString(TelephoneBase.font, "Изменить контакт", position + new Vector2(-100, -170), new Color(TelephoneBase.fontColor));
                nameBox.Render(sb);
                phoneBox.Render(sb);
                acceptAdd.Render(sb);
            }
        }
    }
}
