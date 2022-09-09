using System.Collections.Generic;
using Common.Player;

namespace Common.Engine
{
    class Tool
    {
        public static List<string> Infomation = new List<string>();

        public static void ShowInfo(string info)
        {
            if (Infomation.Count >= 500)
                Infomation.RemoveRange(0 , 100);
            Infomation.Add(info);
        }

        public static bool CheckDirtyWord(string message)
        {
            return string.IsNullOrEmpty(message);
        }      
        
        /*
        public static PlayerInfo GetAIPlayerInfo(ulong sn , int npcID)
        {
            NPCData NPC = TableManager.Instance.NPCDataTable.GetData(npcID);
            if (NPC == null)
                return null;
            AIDecisionData AI = TableManager.Instance.AIDecisionDataTable.GetData(NPC.AIID);
            if (AI == null)
                return null;
            PlayerInfo Data = new PlayerInfo(sn, npcID)
            {
                Name = TableManager.Instance.LocaleStringDataTable.GetString(NPC.NameID)
            };

            Data.Character.Level = 1; // TODO NPC.Level;

            for (int j = (int)E_SkillIndex.Skill1; j < (int)E_SkillIndex.MaxNumber; j++)
            {
                int SkillID = NPC.SkillID((E_SkillIndex)j);
                for (int k = (int)E_CharacterSkillIndex.NormalAttack; k < (int)E_CharacterSkillIndex.MaxNumber; k++)
                {
                    int UsingSkillID = AI.GetSkillID((E_CharacterSkillIndex)k);

                    if (SkillID != UsingSkillID)
                        continue;

                    Data.Character.UsingSkills[k] = (byte)j;
                    Data.Character.SkillLevels[j] = AI.GetSkillLv((E_CharacterSkillIndex)k);
                    break;
                }
            }

            return Data;
        }
        */
          
    }
}
