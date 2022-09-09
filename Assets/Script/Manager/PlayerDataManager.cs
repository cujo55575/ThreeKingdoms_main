using UnityEngine;
using System;
using Common.Player;
using System.IO;
using System.Collections.Generic;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    const string SF_VOLUME = "SoundEffectVolume";
    const string BGM_VOLUME = "BGMusicVolume";
    const string SF_MUTE = "SoundMute";
    const string BGM_MUTE = "BGMute";
    const string FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY = "ScriptableObject/Players";

    public ulong PlayerSN;
    public string Token;

    private string m_TempPlayerDataFilePath
    {
        get
        {
            //return Path.Combine(Application.persistentDataPath,GLOBALCONST.TEMP_PLAYER_DATA_SAVE_FILE);
            return (Application.persistentDataPath + "/" + GLOBALCONST.TEMP_PLAYER_DATA_SAVE_FILE);
        }
    }

    private string m_TempFakePlayerFilePath
    {
        get
        {
            return (Application.persistentDataPath + "/" + GLOBALCONST.TEMP_FAKE_PLAYER_SAVE_FILE);
        }
    }

    public Action NotifyRefresh;
    //玩家資料
    PlayerData playerData;

    public PlayerData PlayerData
    {
        get { return playerData; }
        set
        {
            playerData = value;
            NotifyRefresh.Invoke();

            //for (int i = 0; i < playerData.OwnedHeros.Count; i++)
            //{
            //	playerData.OwnedHeros[i].ReCheckArmyUnlockConditions();
            //}
            //TransferEquippingItemSNtoID();
        }
    }

    private List<PlayerObject> m_rankPlayers;

    public List<PlayerObject> RankPlayers
    {
        get
        { return m_rankPlayers; }
        set
        { m_rankPlayers = value; }
    }

    private PlayerObject m_myRankData;

    public PlayerObject MyRankData
    {
        get
        { return m_myRankData; }
        set
        { m_myRankData = value; }
    }

    public int MuteSound
    {
        get { return PlayerPrefs.GetInt(SF_MUTE, 0); }
        set { PlayerPrefs.SetInt(SF_MUTE, value); }
    }

    public int MuteMusic
    {
        get { return PlayerPrefs.GetInt(BGM_MUTE, 0); }
        set { PlayerPrefs.SetInt(BGM_MUTE, value); }
    }

    public float SoundVolume
    {
        get { return PlayerPrefs.GetFloat(SF_VOLUME, 1f); }
        set { PlayerPrefs.SetFloat(SF_VOLUME, value); }
    }

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat(BGM_VOLUME, 1f); }
        set { PlayerPrefs.SetFloat(BGM_VOLUME, value); }
    }

    public override void Initialize()
    {
        base.Initialize();
        //LoadPlayerData();
    }

    private bool CheckPlayerDataIntegrity()
    {
        if (playerData == null || playerData.OwnedHeros == null || playerData.PlayerHeroArmies == null)
        {
            return false;
        }
        return true;
    }

    public struct ListContainer
    {
        public List<PlayerObject> dataList;

        public ListContainer(List<PlayerObject> _dataList)
        {
            dataList = _dataList;
        }
    }


    #region OFFLINE_DATA
    public void LoadPlayerData()
    {
#if !OFFLINE_DATA
		Debug.LogError("Calling LoadPlayer when not offline data");
		return;
#endif
        if (File.Exists(m_TempPlayerDataFilePath) && (PlayerPrefs.GetInt(GLOBALCONST.IS_NEW_PLAYER) > 0))
        {
            string dataString = File.ReadAllText(m_TempPlayerDataFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(dataString);
            Debug.Log("File Found");
        }
        else
        {
            playerData = new PlayerData();
            playerData.Name = GLOBALCONST.PLAYER_NAME_TEMP;
            playerData.Level = GLOBALCONST.PLAYER_LEVEL_TEMP;
            playerData.BattlePoint = GLOBALCONST.PLAYER_BATTLE_POINT;
            playerData.BambooRoll = GLOBALCONST.PLAYER_BAMBOO_ROLL_TEMP;
            playerData.BambooFragment = GLOBALCONST.PLAYER_BAMBOO_FRAGMENT_TEMP;
            playerData.PlayerIconName = GLOBALCONST.PLAYER_ICON_NAME_TEMP;
            playerData.WinCount = 0;
            playerData.LoseCount = 0;
            playerData.RankPoint = 0;
            playerData.IsNewPlayer = true;
            PlayerPrefs.SetInt(GLOBALCONST.IS_NEW_PLAYER, 1);
            //playerData.OwnedHeros = TableManager.Instance.PlayerHeroDataTable.Datas;
            playerData.OwnedHeros = new System.Collections.Generic.List<PlayerHeroData>();
            playerData.PlayerHeroArmies = TableManager.Instance.HeroArmyDataTable.Datas;
            Debug.Log("File not found at " + m_TempPlayerDataFilePath);
        }


        if (File.Exists(m_TempFakePlayerFilePath) && (PlayerPrefs.GetInt(GLOBALCONST.IS_NEW_PLAYER) > 0))
        {
            string dataString = File.ReadAllText(m_TempFakePlayerFilePath);
            ListContainer listContainer = JsonUtility.FromJson<ListContainer>(dataString);
            m_rankPlayers = new List<PlayerObject>();
            m_rankPlayers = listContainer.dataList;
            for (int i = 0; i < m_rankPlayers.Count; i++)
            {
                if (m_rankPlayers[i] == null || m_rankPlayers[i].GetInstanceID() == 0)
                {
                    m_rankPlayers = new List<PlayerObject>();
                    PlayerObject[] fakePlayers = Resources.LoadAll<PlayerObject>(FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY);
                    for (int ii = 0; ii < fakePlayers.Length; ii++)
                    {
                        m_rankPlayers.Add(fakePlayers[i]);
                    }
                    break;
                }
            }
            Debug.Log("fake player File Found");
        }
        else
        {
            m_rankPlayers = new List<PlayerObject>();
            PlayerObject[] fakePlayers = Resources.LoadAll<PlayerObject>(FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY);
            for (int i = 0; i < fakePlayers.Length; i++)
            {
                m_rankPlayers.Add(fakePlayers[i]);
            }
            Debug.Log("File not found at " + m_TempFakePlayerFilePath);
        }

        //TempFakePlayerFix
        m_rankPlayers = new List<PlayerObject>();
        PlayerObject[] tempFakes = Resources.LoadAll<PlayerObject>(FAKE_PLAYER_SCRIPTABLE_OBJECT_DIRECTORY);
        for (int i = 0; i < tempFakes.Length; i++)
        {
            m_rankPlayers.Add(tempFakes[i]);
        }
    }

    public void SavePlayerData()
    {
#if !OFFLINE_DATA
		Debug.LogError("Calling SavePlayerData when not offline data");
		return;
#endif

        if (CheckPlayerDataIntegrity())
        {
            string dataString = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(m_TempPlayerDataFilePath, dataString);
        }


        if (m_rankPlayers != null && m_rankPlayers.Count > 0)
        {
            //Delete self data from fake players and null
            for (int i = 0; i < m_rankPlayers.Count; i++)
            {
                if (m_rankPlayers[i] == null || m_rankPlayers[i].GetInstanceID() == 0 || m_rankPlayers[i].Data.IsNewPlayer)
                {
                    m_rankPlayers.RemoveAt(i);
                    continue;
                }
            }

            if (m_rankPlayers.Count < 1)
            {
                return;
            }

            ListContainer listContainer = new ListContainer(m_rankPlayers);
            string fakePlayerString = JsonUtility.ToJson(listContainer, true);

            File.WriteAllText(m_TempFakePlayerFilePath, fakePlayerString);
        }
    }
    public void SignOut()
    {
        // FBAuth.SignOut();
    }
    #endregion
}