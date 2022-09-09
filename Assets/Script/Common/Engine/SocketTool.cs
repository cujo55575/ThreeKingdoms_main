using System;
using System.Collections.Generic;
using System.Text;
using NetWork;
using Common.Player;

namespace Common.Engine
{
    class SocketTool
    {
        public static void SetData_PlayerData(ref ClientSocket socket , PlayerData data)
        {
            socket.SetData(data.PlayerSN);
            socket.SetData(data.Name);
            socket.SetData(data.Level);
            socket.SetData(data.Exp);

            socket.SetData(data.BattlePoint);
            socket.SetData(data.BambooRoll);
            socket.SetData(data.BambooFragment);

            socket.SetData(data.RankPoint);
            socket.SetData(data.WinCount);
            socket.SetData(data.LoseCount);
            socket.SetData(data.PlayerIconName);
            
            SetData_OwnedHeros(ref socket, data.OwnedHeros);
            SetData_HeroArmies(ref socket, data.PlayerHeroArmies);
            socket.SetData(data.IsNewPlayer);
            SetData_ArmyFormation(ref socket, data.SavedArmyFormation);
            
        }

        public static void SetData_OwnedHeros(ref ClientSocket socket , List<PlayerHeroData> aList)
        {
            if (socket == null)
                return;
            if (aList == null)
                return;
            socket.SetData(aList.Count);
            for (int i = 0; i <aList.Count; i++) {
                socket.SetData(aList[i].ID);
                socket.SetData(aList[i].PlayerTableID);
                socket.SetData(aList[i].HeroTableID);
                socket.SetData(aList[i].HeroLevel);
                socket.SetData(aList[i].FragmentCount);
                socket.SetData(aList[i].Skill1StatusType);
                socket.SetData(aList[i].Skill2StatusType);
                socket.SetData(aList[i].Skill3StatusType);
                socket.SetData(aList[i].Skill4StatusType);
                socket.SetData(aList[i].Skill5StatusType);
                socket.SetData(aList[i].Skill6StatusType);
                socket.SetData(aList[i].HeroArmy1);
                socket.SetData(aList[i].HeroArmy2);
                socket.SetData(aList[i].HeroArmy3);
                socket.SetData(aList[i].HeroArmy4);
                socket.SetData(aList[i].HeroArmy5);
                socket.SetData(aList[i].HeroArmy6);
                socket.SetData(aList[i].HeroArmy7);
                socket.SetData(aList[i].HeroArmy8);
                socket.SetData(aList[i].HeroArmy9);
                socket.SetData(aList[i].HeroArmy10);
                socket.SetData(aList[i].HeroArmy11);
                socket.SetData(aList[i].HeroArmy12);
                socket.SetData(aList[i].HeroArmy13);
                socket.SetData(aList[i].HeroArmy14);
                socket.SetData(aList[i].HeroArmy15);
                socket.SetData(aList[i].HeroArmy16);
                socket.SetData(aList[i].HeroArmy17);
                socket.SetData(aList[i].HeroArmy18);
                socket.SetData(aList[i].HeroArmy19);
                socket.SetData(aList[i].HeroArmy20);
            }
        }

        public static void SetData_HeroArmies(ref ClientSocket socket , List<HeroArmyData> aList)
        {
            if (socket == null)
                return;
            if (aList == null)
                return;
            socket.SetData(aList.Count);
            for (int i = 0; i <aList.Count; i++) {
                socket.SetData(aList[i].ID);
                socket.SetData(aList[i].HeroTableID);
                socket.SetData(aList[i].ArmyTableID);
                socket.SetData(aList[i].ArmyLevel);
                socket.SetData(aList[i].HeroArmyStatusType);
                socket.SetData(aList[i].Skill1StatusType);
                socket.SetData(aList[i].Skill2StatusType);
                socket.SetData(aList[i].Skill3StatusType);
                socket.SetData(aList[i].Skill4StatusType);
                socket.SetData(aList[i].Skill5StatusType);
                socket.SetData(aList[i].Skill6StatusType);
            }
        }
        
        public static void SetData_ArmyFormation(ref ClientSocket socket , List<ArmyBattleFormation> aList)
        {
            if (socket == null)
                return;
            if (aList == null)
                return;
            
            socket.SetData(aList.Count);
            for (int i = 0; i <aList.Count; i++) {
                socket.SetData(aList[i].PlayerHeroID);
                socket.SetData(aList[i].GridX);
                socket.SetData(aList[i].GridY);
            }
        }
        

        public static List<PlayerHeroData> GetData_OwnedHeros(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;
            int Count = buffer.ReadInt();
            List<PlayerHeroData> plist = new List<PlayerHeroData>();
            for(int i = 0; i < Count; i ++)
            {
                PlayerHeroData pdata = new PlayerHeroData();
                pdata.ID = buffer.ReadInt();
                pdata.PlayerTableID = buffer.ReadInt();
                pdata.HeroTableID = buffer.ReadInt();
                pdata.HeroLevel = buffer.ReadInt();
                pdata.FragmentCount = buffer.ReadInt();
                pdata.Skill1StatusType = buffer.ReadByte();
                pdata.Skill2StatusType = buffer.ReadByte();
                pdata.Skill3StatusType = buffer.ReadByte();
                pdata.Skill4StatusType = buffer.ReadByte();
                pdata.Skill5StatusType = buffer.ReadByte();
                pdata.Skill6StatusType = buffer.ReadByte();
                pdata.HeroArmy1 = buffer.ReadInt();
                pdata.HeroArmy2 = buffer.ReadInt();
                pdata.HeroArmy3 = buffer.ReadInt();
                pdata.HeroArmy4 = buffer.ReadInt();
                pdata.HeroArmy5 = buffer.ReadInt();
                pdata.HeroArmy6 = buffer.ReadInt();
                pdata.HeroArmy7 = buffer.ReadInt();
                pdata.HeroArmy8 = buffer.ReadInt();
                pdata.HeroArmy9 = buffer.ReadInt();
                pdata.HeroArmy10 = buffer.ReadInt();
                pdata.HeroArmy11 = buffer.ReadInt();
                pdata.HeroArmy12 = buffer.ReadInt();
                pdata.HeroArmy13 = buffer.ReadInt();
                pdata.HeroArmy14 = buffer.ReadInt();
                pdata.HeroArmy15 = buffer.ReadInt();
                pdata.HeroArmy16 = buffer.ReadInt();
                pdata.HeroArmy17 = buffer.ReadInt();
                pdata.HeroArmy18 = buffer.ReadInt();
                pdata.HeroArmy19 = buffer.ReadInt();
                pdata.HeroArmy20 = buffer.ReadInt();

                plist.Add(pdata);
            }

            return plist;
        }
        
        public static List<HeroArmyData> GetData_HeroArmies(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;
            int Count = buffer.ReadInt();
            List<HeroArmyData> plist = new List<HeroArmyData>();
            for(int i = 0; i < Count; i ++)
            {
                HeroArmyData pdata = new HeroArmyData();
                pdata.ID = buffer.ReadInt();
 
                pdata.HeroTableID = buffer.ReadInt();
                pdata.ArmyTableID = buffer.ReadInt();
                pdata.ArmyLevel = buffer.ReadInt();
                pdata.HeroArmyStatusType = buffer.ReadByte();
                pdata.Skill1StatusType = buffer.ReadByte();
                pdata.Skill2StatusType = buffer.ReadByte();
                pdata.Skill3StatusType = buffer.ReadByte();
                pdata.Skill4StatusType = buffer.ReadByte();
                pdata.Skill5StatusType = buffer.ReadByte();
                pdata.Skill6StatusType = buffer.ReadByte();

                plist.Add(pdata);
            }

            return plist;
        }
        
        public static List<ArmyBattleFormation> GetData_ArmyFormation(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;
            int Count = buffer.ReadInt();
            List<ArmyBattleFormation> plist = new List<ArmyBattleFormation>();
            for(int i = 0; i < Count; i ++)
            {
                ArmyBattleFormation pdata = new ArmyBattleFormation();
                pdata.PlayerHeroID = buffer.ReadInt();
                pdata.GridX = buffer.ReadInt();
                pdata.GridY = buffer.ReadInt();

                plist.Add(pdata);
            }

            return plist;
        }
        
        /*
        public static void SetData_CharacterBackpack(ref ClientSocket socket , CharacterBackpack data)
        {
            if (socket == null)
                return;
            if (data == null)
                return;
            
            socket.SetData(data.UsingCharacterID);
            //Set Character Data
            socket.SetData(data.Characters.Count);
            for (int i = 0; i < data.Characters.Count; i++)
            {
                socket.SetData(data.Characters[i].CharacterID);
                socket.SetData(data.Characters[i].Level);
                socket.SetData(data.Characters[i].Exp);
                socket.SetData(data.Characters[i].Star);
                for (int j = 0; j < data.Characters[i].SkillLevels.Length; j++)
                {
                    socket.SetData(data.Characters[i].SkillLevels[j]);
                }
                for (int j = 0; j < data.Characters[i].UsingSkills.Length; j++)
                {
                    socket.SetData(data.Characters[i].UsingSkills[j]);
                }
				socket.SetData(data.Characters[i].EquippingItemIDs.Count);
				for (int j = 0; j < data.Characters[i].EquippingItemIDs.Count; j++)
				{
					socket.SetData(data.Characters[i].EquippingItemIDs[j]);
				}

            }
            
        }

        public static CharacterBackpack GetData_CharacterBackpack(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;

            CharacterBackpack Data = new CharacterBackpack();
            /*
            Data.UsingCharacterID = buffer.ReadInt();
            int Count = buffer.ReadInt();
            for(int i = 0; i < Count; i ++)
            {
                Character Char = new Character();
                Char.CharacterID = buffer.ReadInt();
                Char.Level = buffer.ReadByte();
                Char.Exp = buffer.ReadUint();
                Char.Star = buffer.ReadByte();

                for (int j = 0; j < Char.SkillLevels.Length; j++)
                {
                    Char.SkillLevels[j] = buffer.ReadByte();                    
                }
                for (int j = 0; j < Char.UsingSkills.Length; j++)
                {
                    Char.UsingSkills[j] = buffer.ReadByte();                    
                }
				int cc = buffer.ReadInt();
				for (int j = 0; j < cc; j++)
				{
					Char.EquippingItemSNs.Add(buffer.ReadInt());                    
				}

                Data.Characters.Add(Char);
            }
            
            return Data;
        }

        public static void SetData_ItemBackpack(ref ClientSocket socket , ItemBackpack data)
        {
			if (socket == null)
				return;
			if (data == null)
				return;
            
			//Set ItemBackpack data		
			socket.SetData(data.EquipCardSN.Count);
			for (int i = 0; i < data.EquipCardSN.Count; i++)
			{
				socket.SetData(data.EquipCardSN[i]);
			}
			socket.SetData(data.EquipSet1.Count);
			for (int i = 0; i<data.EquipSet1.Count; i++)
			{
				socket.SetData(data.EquipSet1[i]);
			}
			socket.SetData(data.EquipSet2.Count);
			for (int i = 0; i<data.EquipSet2.Count; i++)
			{
				socket.SetData(data.EquipSet2[i]);
			}
			socket.SetData(data.EquipSet3.Count);
			for (int i = 0; i<data.EquipSet3.Count; i++)
			{
				socket.SetData(data.EquipSet3[i]);
			}
			socket.SetData(data.Items.Count);
			for(int i = 0; i<data.Items.Count; i++)
			{
				socket.SetData(data.Items[i].SN);
				socket.SetData(data.Items[i].ItemID);
				socket.SetData(data.Items[i].Numbers);
			}
            

        }
        */
        /*
        public static ItemBackpack GetData_ItemBackpack(ref NetBuffer buffer)
        {
			if (buffer == null)
				return null;
			ItemBackpack Data = new ItemBackpack();

			int Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				uint SN = buffer.ReadUint();
				Data.EquipCardSN.Add(SN);
			}
			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				uint SN = buffer.ReadUint();
				Data.EquipSet1.Add(SN);
			}
			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				uint SN = buffer.ReadUint();
				Data.EquipSet2.Add(SN);
			}
			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				uint SN = buffer.ReadUint();
				Data.EquipSet3.Add(SN);
			}
			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				Item item = new Item();
				item.SN = buffer.ReadUint();
				item.ItemID = buffer.ReadInt();
				item.Numbers = buffer.ReadUshort();
				Data.Items.Add(item);
			}

			return Data;
        }

        public static void SetData_PlayerInfo(ref ClientSocket socket, PlayerInfo data)
        {
			if (socket == null)
				return;
			if (data == null)
				return;
			socket.SetData(data.PlayerSN);
			socket.SetData(data.Name);

			socket.SetData(data.Character.CharacterID);
			socket.SetData(data.Character.Level);
			socket.SetData(data.Character.Exp);
			socket.SetData(data.Character.Star);
			for (int j = 0; j < data.Character.SkillLevels.Length; j++)
			{
				socket.SetData(data.Character.SkillLevels[j]);
			}
			for (int j = 0; j < data.Character.UsingSkills.Length; j++)
			{
				socket.SetData(data.Character.UsingSkills[j]);
			}

			socket.SetData(data.Equips.Count);
			for (int i = 0; i < data.Equips.Count; i++)
			{
				socket.SetData(data.Equips[i].SN);
				socket.SetData(data.Equips[i].ItemID);
				socket.SetData(data.Equips[i].Numbers);
			}

			socket.SetData(data.EquipCards.Count);
			for (int i = 0; i < data.EquipCards.Count; i++)
			{
				socket.SetData(data.EquipCards[i].SN);
				socket.SetData(data.EquipCards[i].ItemID);
				socket.SetData(data.EquipCards[i].Numbers);
			}

			socket.SetData(data.AllCards.Count);
			for (int i = 0; i < data.AllCards.Count; i++)
			{
				socket.SetData(data.AllCards[i].SN);
				socket.SetData(data.AllCards[i].ItemID);
				socket.SetData(data.AllCards[i].Numbers);
			}
        }

        public static PlayerInfo GetData_PlayerInfo(ref NetBuffer buffer)
        {
			if (buffer == null)
				return null;
			PlayerInfo Data = new PlayerInfo(0,0);

			Data.PlayerSN = buffer.ReadUlong();
			Data.Name = buffer.ReadString();

			Character Char = new Character();
			Char.CharacterID = buffer.ReadInt();
			Char.Level = buffer.ReadByte();
			Char.Exp = buffer.ReadUint();
			Char.Star = buffer.ReadByte();

			for (int j = 0; j < Char.SkillLevels.Length; j++)
			{
				Char.SkillLevels[j] = buffer.ReadByte();
			}
			for (int j = 0; j < Char.UsingSkills.Length; j++)
			{
				Char.UsingSkills[j] = buffer.ReadByte();
			}

			Data.Character = Char;

			int Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				Item item = new Item();
				item.SN = buffer.ReadUint();
				item.ItemID = buffer.ReadInt();
				item.Numbers = buffer.ReadUshort();
				Data.Equips.Add(item);
			}

			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				Item item = new Item();
				item.SN = buffer.ReadUint();
				item.ItemID = buffer.ReadInt();
				item.Numbers = buffer.ReadUshort();
				Data.EquipCards.Add(item);
			}

			Count = buffer.ReadInt();
			for (int i = 0; i < Count; i++)
			{
				Item item = new Item();
				item.SN = buffer.ReadUint();
				item.ItemID = buffer.ReadInt();
				item.Numbers = buffer.ReadUshort();
				Data.AllCards.Add(item);
			}

			return Data;
        }    
        */
        /*
        public static void SetData_Character(ref ClientSocket socket, Character data)
        {
            if (socket == null)
                return;

            if (data == null)
                return;

            socket.SetData(data.CharacterID);
            socket.SetData(data.Level);
            socket.SetData(data.Exp);
            socket.SetData(data.Star);

            for (int j = 0; j < data.SkillLevels.Length; j++)
            {
                socket.SetData(data.SkillLevels[j]);
            }
            for (int j = 0; j < data.UsingSkills.Length; j++)
            {
                socket.SetData(data.UsingSkills[j]);
            }

			socket.SetData(data.EquippingItemIDs.Count);
			for(int i = 0; i<data.EquippingItemIDs.Count; i++)
			{
				socket.SetData(data.EquippingItemIDs[i]);
			}
        }

        public static Character GetData_Character(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;

            Character Char = new Character();
            Char.CharacterID = buffer.ReadInt();
            Char.Level = buffer.ReadByte();
            Char.Exp = buffer.ReadUint();
            Char.Star = buffer.ReadByte();

            for (int j = 0; j < Char.SkillLevels.Length; j++)
            {
                Char.SkillLevels[j] = buffer.ReadByte();
            }
            for (int j = 0; j < Char.UsingSkills.Length; j++)
            {
                Char.UsingSkills[j] = buffer.ReadByte();
            }

			int Count = buffer.ReadInt();
			for(int i = 0; i < Count; i++)
			{
				int ItemID = buffer.ReadInt();
				Char.EquippingItemIDs.Add(ItemID);
			}
            return Char;
        }

        public static void SetData_EquipItems(ref ClientSocket socket, List<Item> equips)
        {
            if (socket == null)
                return;
            if (equips == null)
                return;

            socket.SetData(equips.Count);
            for (int i = 0; i < equips.Count; i++)
            {
                socket.SetData(equips[i].SN);
                socket.SetData(equips[i].ItemID);
                socket.SetData(equips[i].Numbers);
            }
        }

        public static List<Item> GetData_EquipItems(ref NetBuffer buffer)
        {
            if (buffer == null)
                return null;
            List<Item> Data = new List<Item>();
            int Count = buffer.ReadInt();
            for (int i = 0; i < Count; i++)
            {
                Item item = new Item();
                item.SN = buffer.ReadUint();
                item.ItemID = buffer.ReadInt();
                item.Numbers = buffer.ReadUshort();
                Data.Add(item);
            }
            return Data;
        }

		public static void SetData_DailyLogin(ref ClientSocket iSocket , DailyLoginSave iData)
        {
            if(iSocket == null)
                return;
            if(iData == null)
                return;

            iSocket.SetData((string)iData.ToString());
            iSocket.SetData((byte)iData.RewardCount);           
        }

        public static DailyLoginSave GetData_DailyLogin(ref NetBuffer iBuf)
        {
            if(iBuf == null)
                return null;

            DailyLoginSave aSave = new DailyLoginSave();

            aSave.CanRewardTime = Convert.ToDateTime(iBuf.ReadString());
            aSave.RewardCount = iBuf.ReadByte();

            return aSave;
        }

        public static void SetData_ItemMail(ref ClientSocket iSocket , List<ItemMailSave> iData)
        {
            if(iSocket == null)
                return;
            if(iData == null)
                return;

            iSocket.SetData((int)iData.Count);

            for(int i = 0; i < iData.Count; i++)
            {
                ItemMailSave aSave = iData[i];

                iSocket.SetData((string)aSave.DeleteTime.ToString());
                iSocket.SetData((int)aSave.ItemID);
                iSocket.SetData((byte)aSave.Num);
            }
        }

        public static List<ItemMailSave> GetData_ItemMail(ref NetBuffer iBuf)
        {
            if(iBuf == null)
                return null;

            List<ItemMailSave> aList = new List<ItemMailSave>();

            int aCount = iBuf.ReadInt();

            for(int i = 0; i < aCount; i++)
            {
                ItemMailSave aSave = new ItemMailSave();
                aSave.DeleteTime = Convert.ToDateTime(iBuf.ReadString());
                aSave.ItemID = iBuf.ReadInt();
                aSave.Num = iBuf.ReadByte();

                aList.Add(aSave);
            }

            return aList;
        }
        

        public static void SetData_Friend(ref ClientSocket iSocket , List<FriendSave> iData)
        {
            if(iSocket == null)
                return;
            if(iData == null)
                return;

            iSocket.SetData((int)iData.Count);

            for(int i = 0; i < iData.Count; i++)
            {
                FriendSave aSave = iData[i];

                iSocket.SetData((ulong)aSave.SN);
                iSocket.SetData((string)aSave.Name);
                iSocket.SetData((int)aSave.RoleID);
                iSocket.SetData((int)aSave.LV);
                iSocket.SetData((string)aSave.LastLoginTime.ToString());
                iSocket.SetData((string)aSave.CanHelloTime.ToString());
            }
        }        

        public static List<FriendSave> GetData_Friend(ref NetBuffer iBuf)
        {
            if(iBuf == null)
                return null;

            List<FriendSave> aList = new List<FriendSave>();

            int aCount = iBuf.ReadInt();

            for(int i = 0; i < aCount; i++)
            {
                FriendSave aSave = new FriendSave();
                aSave.SN = iBuf.ReadUlong();
                aSave.Name = iBuf.ReadString();
                aSave.RoleID = iBuf.ReadInt();
                aSave.LV = iBuf.ReadInt();
                aSave.LastLoginTime = Convert.ToDateTime(iBuf.ReadString());
                aSave.CanHelloTime = Convert.ToDateTime(iBuf.ReadString());

                aList.Add(aSave);
            }

            return aList;
        }
        */
    }

}
