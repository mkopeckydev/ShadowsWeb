using Shadows.Data;
using Shadows.Data.CustomData;
using Shadows.Data.Facade;
using Shadows.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;

namespace Shadows.Web
{
    [WebService(Namespace = "https://mkopecky.dev/")]
    public class AndroidClient : System.Web.Services.WebService
    {
        private NHibernate.ISession NHibernateSession;

        public AndroidClient()
        {
            NHibernateSession = NHibernateHelper.OpenSession();
        }

        public Character Auth(string securityToken)
        {
            byte[] bytes = Convert.FromBase64String(securityToken);
            string text = System.Text.Encoding.UTF8.GetString(bytes);

            string[] splitted = text.Split(':');
            if (splitted.Length != 2) return null;

            var userName = splitted[0];
            var password = splitted[1];

            CharacterFacade fCharacter = new CharacterFacade(NHibernateSession);
            Character character = fCharacter.Login(userName, password);
            
            return character;
        }

        private void CloseSession()
        {
            if (NHibernateSession != null)
            {
                NHibernateSession.Flush();
                NHibernateSession.Clear();
                NHibernateSession.Close();
                NHibernateSession.Dispose();
                NHibernateSession = null;
            }
        }

        [WebMethod]
        public bool Login(string token)
        {
            var character = Auth(token);

            bool result = false;

            if (character != null)
            {
                result = true;
            }

            CloseSession();

            return result;
        }

        [WebMethod]
        public List<CharacterSkillLevel> SkillList(string securityToken)
        {
            var character = Auth(securityToken);

            List<CharacterSkillLevel> list = new List<CharacterSkillLevel>();

            if (character != null)
            {
                BusinessFacade<CharacterSkillLevel> fCharacterSkillLevel = new BusinessFacade<CharacterSkillLevel>(NHibernateSession);
                list = fCharacterSkillLevel.GetListEntities("Skill", true, String.Format("(IdCharacter = {0})", character.Id), true);
            }
            CloseSession();

            return list;
        }


        [WebMethod]
        public List<Item> ItemSearch(string searchText, int idSkill, string securityToken)
        {
            var character = Auth(securityToken);

            List<Item> list = new List<Item>();

            if (character != null)
            {
                ItemFacade fItem = new ItemFacade(NHibernateSession);
                list = fItem.Search(searchText, idSkill, character);
            }

            CloseSession();

            return list;
        }

        [WebMethod]
        public ItemData ItemDetail(int id, string securityToken)
        {
            var character = Auth(securityToken);

            ItemData result = new ItemData();

            if (character != null)
            {
                ItemFacade fItem = new ItemFacade(NHibernateSession);
                BusinessFacade<ItemNote> fItemNote = new BusinessFacade<ItemNote>(NHibernateSession);

                result.Item = fItem.GetEntity(id);

                GetVisibleItemRowsResult rows = fItem.GetVisibleItemRows(character, result.Item);
                result.ValueSum = rows.ValueSum;
                result.BonusLevel = rows.BonusLevel;
                result.List = rows.List;

                List<ItemNote> notes = fItemNote.GetListEntities(String.Empty, true, String.Format("(IdItem = {0}) and (IdCharacter = {1})", id, character.Id));
                if (notes.Count > 0)
                {
                    result.ItemNote = notes[0].Note;
                }
                else
                {
                    result.ItemNote = String.Empty;
                }

                FilesManager fm = new FilesManager();
                MemoryStream ms = fm.ReadFile(fm.GetImagePath(result.Item.ImageName));
                result.ImageData = ms.ToArray();
            }

            CloseSession();

            return result;
        }

        [WebMethod]
        public void SaveItemNote(int id, string note, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<ItemNote> fItemNote = new BusinessFacade<ItemNote>(NHibernateSession);
                ItemNote itemNote = new ItemNote();

                List<ItemNote> notes = fItemNote.GetListEntities(String.Empty, true, String.Format("(IdItem = {0}) and (IdCharacter = {1})", id, character.Id));
                if (notes.Count > 0)
                {
                    itemNote = notes[0];
                    itemNote.Note = note;
                    fItemNote.SaveEntity(itemNote);
                }
                else if (!String.IsNullOrEmpty(note))
                {
                    itemNote.IdCharacter = character.Id;
                    itemNote.IdItem = id;
                    itemNote.Note = note;
                    fItemNote.SaveEntity(itemNote);
                }
            }

            CloseSession();
        }

        [WebMethod]
        public void SaveCustomCounter(CustomCounter data, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<CustomCounter> fCustomCounter = new BusinessFacade<CustomCounter>(NHibernateSession);
                data.IdCharacter = character.Id;
                fCustomCounter.SaveEntity(data);
            }

            CloseSession();
        }

        [WebMethod]
        public void DeleteCustomCounter(int id, string securityToken)
        {
            var character = Auth(securityToken);

            if ((character != null) && (id > BaseEntity.NEW_OBJECT_ID))
            {
                BusinessFacade<CustomCounter> fCustomCounter = new BusinessFacade<CustomCounter>(NHibernateSession);
                CustomCounter data = fCustomCounter.GetEntity(id);
                fCustomCounter.DeleteEntitiy(data);
            }

            CloseSession();
        }

        [WebMethod]
        public List<CustomCounter> GetCustomCounterList(string securityToken)
        {
            var character = Auth(securityToken);

            List <CustomCounter> data = new List<CustomCounter>();

            if (character != null)
            {
                BusinessFacade<CustomCounter> fCustomCounter = new BusinessFacade<CustomCounter>(NHibernateSession);
                data = fCustomCounter.GetListEntities("Ordering", true, String.Format("(IdCharacter = {0})", character.Id));
            }

            CloseSession();

            return data;
        }


        [WebMethod]
        public void SaveFightNumber(FightNumber data, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<FightNumber> fFightNumber = new BusinessFacade<FightNumber>(NHibernateSession);
                BusinessFacade<FightNumberBonus> fFightNumberBonus = new BusinessFacade<FightNumberBonus>(NHibernateSession);

                data.IdCharacter = character.Id;
                fFightNumber.SaveEntity(data);

                foreach(FightNumberBonus bonus in data.Bonuses)
                {
                    bonus.IdFightNumber = data.Id;

                    if (String.IsNullOrEmpty(bonus.Caption))
                    {
                        fFightNumberBonus.DeleteEntitiy(bonus);
                    }
                    else
                    {
                        fFightNumberBonus.SaveEntity(bonus);
                    }
                }
            }

            CloseSession();
        }

        [WebMethod]
        public void DeleteFightNumber(int id, string securityToken)
        {
            var character = Auth(securityToken);

            if ((character != null) && (id > BaseEntity.NEW_OBJECT_ID))
            {
                BusinessFacade<FightNumber> fFightNumber = new BusinessFacade<FightNumber>(NHibernateSession);
                BusinessFacade<FightNumberBonus> fFightNumberBonus = new BusinessFacade<FightNumberBonus>(NHibernateSession);

                FightNumber data = fFightNumber.GetEntity(id);

                List<FightNumberBonus> listBonus = fFightNumberBonus.GetListEntities("Id", true, String.Format("(IdFightNumber = {0})", data.Id));
                foreach(FightNumberBonus bonus in listBonus)
                {
                    fFightNumberBonus.DeleteEntitiy(bonus);
                }

                fFightNumber.DeleteEntitiy(data);
            }

            CloseSession();
        }

        [WebMethod]
        public List<FightNumber> GetFightNumberList(string securityToken)
        {
            var character = Auth(securityToken);

            List<FightNumber> data = new List<FightNumber>();

            if (character != null)
            {
                BusinessFacade<FightNumber> fFightNumber = new BusinessFacade<FightNumber>(NHibernateSession);
                BusinessFacade<FightNumberBonus> fFightNumberBonus = new BusinessFacade<FightNumberBonus>(NHibernateSession);

                data = fFightNumber.GetListEntities("Ordering", true, String.Format("(IdCharacter = {0})", character.Id));

                foreach(FightNumber fn in data)
                {
                    fn.Bonuses = fFightNumberBonus.GetListEntities("Id", true, String.Format("(IdFightNumber = {0})", fn.Id));
                }
            }

            CloseSession();

            return data;
        }

        [WebMethod]
        public void SaveFightNumberBonusSelected(int idFightNumberBonus, bool isSelected, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<FightNumberBonus> fFightNumberBonus = new BusinessFacade<FightNumberBonus>(NHibernateSession);
                FightNumberBonus b = fFightNumberBonus.GetEntity(idFightNumberBonus);
                b.IsSelected = isSelected;
                fFightNumberBonus.SaveEntity(b);
            }

            CloseSession();
        }






        [WebMethod]
        public List<SpellbookItem> SpellbookItemSearch(string searchText, string securityToken)
        {
            var character = Auth(securityToken);

            List<SpellbookItem> list = new List<SpellbookItem>();

            if (character != null)
            {
                SpellbookItemFacade fItem = new SpellbookItemFacade(NHibernateSession);
                list = fItem.Search(searchText, 0, character);
            }

            CloseSession();

            return list;
        }

        [WebMethod]
        public SpellbookItemData SpellbookItemDetail(int id, string securityToken)
        {
            var character = Auth(securityToken);

            SpellbookItemData result = new SpellbookItemData();

            if (character != null)
            {
                SpellbookItemFacade fSpellbookItem = new SpellbookItemFacade(NHibernateSession);
                BusinessFacade<SpellbookItemNote> fSpellbookItemNote = new BusinessFacade<SpellbookItemNote>(NHibernateSession);

                result.Item = fSpellbookItem.GetEntity(id);

                List<SpellbookItemNote> notes = fSpellbookItemNote.GetListEntities(String.Empty, true, String.Format("(IdSpellbookItem = {0}) and (IdCharacter = {1})", id, character.Id));
                if (notes.Count > 0)
                {
                    result.ItemNote = notes[0].Note;
                }
                else
                {
                    result.ItemNote = String.Empty;
                }
            }

            CloseSession();

            return result;
        }

        [WebMethod]
        public void SaveSpellbookItemNote(int id, string note, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<SpellbookItemNote> fSpellbookItemNote = new BusinessFacade<SpellbookItemNote>(NHibernateSession);
                SpellbookItemNote itemNote = new SpellbookItemNote();

                List<SpellbookItemNote> notes = fSpellbookItemNote.GetListEntities(String.Empty, true, String.Format("(IdSpellbookItem = {0}) and (IdCharacter = {1})", id, character.Id));
                if (notes.Count > 0)
                {
                    itemNote = notes[0];
                    itemNote.Note = note;
                    fSpellbookItemNote.SaveEntity(itemNote);
                }
                else if (!String.IsNullOrEmpty(note))
                {
                    itemNote.IdCharacter = character.Id;
                    itemNote.IdSpellbookItem = id;
                    itemNote.Note = note;
                    fSpellbookItemNote.SaveEntity(itemNote);
                }
            }

            CloseSession();
        }




        [WebMethod]
        public List<CharacterActivity> GetCharacterActivityList(string securityToken)
        {
            var character = Auth(securityToken);

            List<CharacterActivity> data = new List<CharacterActivity>();

            if (character != null)
            {
                BusinessFacade<CharacterActivity> fCharacterActivity = new BusinessFacade<CharacterActivity>(NHibernateSession);
                data = fCharacterActivity.GetListEntities("Caption", true, String.Format("(IdCharacter = {0})", character.Id));
            }

            CloseSession();

            return data;
        }

        [WebMethod]
        public void SaveCharacterActivity(CharacterActivity data, string securityToken)
        {
            var character = Auth(securityToken);

            if (character != null)
            {
                BusinessFacade<CharacterActivity> fCharacterActivity = new BusinessFacade<CharacterActivity>(NHibernateSession);

                data.IdCharacter = character.Id;
                fCharacterActivity.SaveEntity(data);
            }

            CloseSession();
        }

        [WebMethod]
        public void DeleteCharacterActivity(int id, string securityToken)
        {
            var character = Auth(securityToken);

            if ((character != null) && (id > BaseEntity.NEW_OBJECT_ID))
            {
                BusinessFacade<CharacterActivity> fCharacterActivity = new BusinessFacade<CharacterActivity>(NHibernateSession);

                CharacterActivity data = fCharacterActivity.GetEntity(id);

                fCharacterActivity.DeleteEntitiy(data);
            }

            CloseSession();
        }

        [WebMethod]
        public string Message(string securityToken)
        {
            var character = Auth(securityToken);

            string result = String.Empty;

            if (character != null)
            {
                if (!String.IsNullOrEmpty(character.ClientMessage)) 
                { 
                    result = character.ClientMessage;

                    character.ClientMessage = String.Empty;

                    CharacterFacade fCharacter = new CharacterFacade(NHibernateSession);
                    fCharacter.SaveEntity(character);
                }
            }

            CloseSession();

            return result;
        }
    }
}
