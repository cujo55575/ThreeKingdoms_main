using System;

public enum E_DebugLogType
{
	None = 0,
	Warning,
	Error,
	Protocol,   //協議種類
}

public enum E_GameStatus
{
	None = 0,
	Init,
	Version,
	Login,
	Play,
}

public enum E_PlayLevel
{
	Menu,
	Play,
	GameOver,
}

[Serializable]
public enum E_TerrainType
{
    None,
    Ground,
    Road,
    Water,
    Grass,
}

public enum E_Player
{
	None,
	One,
	Two
}

public enum E_TileStatus : int 
{
    Free = -1,
    Avaliable = -2,
    NotAvaliable = -3,
}

public enum E_TileDirection
{
    Top,
    Right,
    Buttom,
    Left,
}
public enum E_MessageBox
{
	YesNo,
	Yes
}
public enum E_RewardType
{
	Type1,
	Type2,
	Type3
}

public enum E_CardType
{
    CardTypeA,
    CardTypeB,
    CardTypeC
}

public enum E_SystemLanguage
{
	None = 0,
	Traditional_Chinese = 59027,
	Simplified_Chinese = 59028,
	Japanese = 59029,
	English = 59030,
}

public enum E_Platform : byte
{
	None = 0,
	FB,
	GPlus,
}

public enum E_ResourceKind : byte
{
	Model = 0,
	Action = 1,
	Material = 2,
	Texture = 3,//image,
	Sound = 4,
	UI = 5,
	Effect = 6,
	Scene = 7,
	Map = 8,
	Table = 9,
	Video = 10,
}

public enum E_PlayerType
{
	Local,  //本地玩家
	Other,  //連線其他玩家
	AI,     //AI玩家
}

public enum E_Gender
{
	None = 0,
	Male = 1,
	Female = 2,
}

public enum E_ActionType
{
	None = 0,
	Idle,
	Run,
	NormalAttack,
	MagicAttack,
	SpecialAttack,
	Ready,
	Hit,
	Liedown,
	Dodge,
	Defense,
	Counter,
	Dizzy,
	Courage,
	Hit_C_p, //Hit Card disabled cards
	Hit_C_s, //Hit card damage
	Max,
}

public enum E_MoneyType
{
	None = 0,
	Gold = 1,
	Diamond = 2,
	Energy = 3,
	Money = 4
}

public enum E_MessageBoxType : byte
{
	Yes,
	YesNo,
}

// Dokapon Kingdom Characters part
public enum E_Occupation : byte
{
	Thief = 1
}

public enum E_CharacterType : byte
{
	Player = 1,
	Network = 2,
	AI = 3
}

public enum E_Restriction : byte
{
	None = 0,
	CharacterLevel = 1,
	JobLevel = 2,
	SexLevel = 3,
	AccountLevel = 4
}

public enum E_ItemType : byte
{
	None = 0,
	Necklace = 1,
	Ring = 2,
	Earring = 3,
	Bracelet = 4,
	Weapon = 5,
	Head = 6,
	Shirt = 7,
	Pants = 8,
	Wings = 9,
	Fragment = 10,
	LuckyBag = 11,
	Giftbox = 12,
	Consumables = 13,
	ExpMaterials = 14,
	Card = 15
}

public enum E_AbilityType
{
	None = 0,
	HP = 1,
	ATK = 2,
	DEF = 3,
	DEX = 4,
	AGI = 5,
	CRI = 6,
	DCR = 7,
	SkillPlus1 = 8,
	SkillPlus2 = 9,
	SkillPlus3 = 10
}

public enum E_SkillEffectType : byte
{
	None = 0,
	Atk = 1,
	Def = 2,
	Dex = 3,
	Agi = 4,
	Cri = 5,
	Dcr = 6,
	EarthDamage = 7,
	WaterDamage = 8,
	FireDamage = 9,
	WindDamage = 10,
	EarthDefense = 11,
	WaterDefense = 12,
	FireDefense = 13,
	WindDefense = 14,
	IncHitHP = 15, // increase hp while hit
	IncHitHPEarth = 16, // increase hp while earth skill hit
	IncHitHPWater = 17,
	IncHitHPFire = 18,
	IncHitHPWind = 19,
	IncHitAtk = 20,
	IncHitDef = 21,
	IncHitDex = 22,
	IncHitAgi = 23,
	IncHitCri = 24,
	IncHitDcr = 25,
	AddDamageAtk = 26, // Gain additional damage by atk
	ChgSpAtkPo = 27 // Change Special Attack power
}

public enum E_ValueType : byte
{
	Percentage = 0,
	Value = 1
}

public enum E_Target : byte
{
	Self = 0,
	Enemy = 1
}

public enum E_DefualtOutWard : byte
{
	None = 0,
	Weapon = 1,
	Head = 2,
	Shirt = 3,
	Pants = 4,
	Wings = 5,
	Body = 6,
	Body2 = 7,
	Max
}

//For Objective Type of map in Editor
public enum E_MapObjective : byte
{
	MoneyLimit = 1,
	KillBoss = 2,
	KillMonsters = 3,
}

//For Block Type of map in Editor
public enum E_BlockType : byte
{
	GameHall = 1,
	Village = 2,
	NPC = 3,
	Treasure = 4
}

public enum E_VillageEvent : byte
{
	None = 0,
	Fight = 1,
	PayRest = 2,
	Upgrade = 3
}

public enum E_NextBlockInvalidType : byte
{
	Duplicate = 1,
	SelfIndicate,
	DoesNotExist,
	None
}

public enum E_PlayerInBattle : byte
{
	None = 0,
	Is_Battle_Player,
	Non_Battle_Player,
}

public enum E_BattlePlayerRole : byte
{
	//Battle_Player,
	//Non_Battle_Player,
	None = 0,
	Attacker,
	Defender,
}
public enum E_BattleGameState : byte
{
	None = 0,
	ResourceLoading,
	Spawn,
	TurnChoice,
	TurnResult,
	BattleOptionChoice,
	OptionChosenResult,
	AtkerSkillShow,
	ActiveAttack,
	ActiveDefense,
	AttackerAttackDefender,
	AttackerAttackDefenderStatusShow,
	TurnChange,
	BattleResult,
	BattleFinishStatus,
	SendCallbackToBoardManager,
}

public enum E_BattleType : byte
{
	None = 0,
	PVP,
	PVBlockNPC,
	PVVillageNPC,
}

public enum E_BattleOptionsType : byte
{
	Run = 0,
	Special,
	Normal,
	Magic,
	None,
}

public enum E_SkillElement : byte
{
	None = 0,
	Earth,
	Water,
	Fire,
	Wind
}

public enum E_BattleResult : byte
{
	Draw = 0,
	AttackerWin,
	AttackerLose,
	None
}

public enum E_UIPositions : byte
{
	CenterTop = 0,
	CenterBottom,
	LeftTop,
	RightTop
}

public enum E_SkillType : byte
{
	Active = 0,//主動技
	Passive = 1,//被動技
}

public enum E_CharacterSkillIndex : int
{
	None = -1,
	SpecialAttack,
	NormalAttack,
	MagicAttack,
	SpecialDefense,
	NormalDefense,
	MagicDefense,
	MaxNumber
}

public enum E_SkillIndex : int
{
	Skill1 = 0,
	Skill2,
	Skill3,
	Skill4,
	Skill5,
	Skill6,
	Skill7,
	Skill8,
	Skill9,
	Skill10,
	Skill11,
	Skill12,
	Skill13,
	Skill14,
	Skill15,
	Skill16,
	Skill17,
	Skill18,
	Skill19,
	Skill20,
	MaxNumber,
}

public enum E_StatusTextMovement
{
	Static,
	LeftToRight,
	RightToLeft,
}


public enum E_WeaponType : byte
{
	None = 0,
	Sword = 1,
	Skateboard = 2,
	TwinBlade = 3,
	Orb = 4,
	Shuriken = 5,
	TwinGun = 6
}

public enum E_EventType : byte
{
	None = 0,
	IncreaseHP = 1,
	IncreaseMoney = 2,
	FreeRest = 3,
	BlockBadEffect = 4,
	MultipleDice = 5,
	ReduceHP = 6,
	ReduceMoney = 7,
	HighRest = 8,
	SpecialDice = 9,
	Dizzy = 10,
	MoveToLocation = 11,
	MoveToCategory = 12
}

public enum E_TreasureRewardType : byte
{
	Item = 0,//道具
	Gold = 1,//令牌
	MagicStone = 2,
	Character = 3//角色
}

public enum E_Emoji : byte
{
	Angry = 0,
	Bad = 1,
	Fun = 2,
	Good = 3,
	Happy = 4,
	Note = 5,
	Silence = 6,
	Sorrow = 7,
	Surprised = 8,
	Max = 9,
}

public enum E_Currency : byte
{
	Gold = 1,//令牌
	MagicStone = 2,//靈幣
	Cash = 3,//現金(目前無使用到)
	ItemFragments = 4,//竹片
	CharacterFragments = 5//武將卡牌
}

public enum E_Channel : byte
{
	World = 1,
	Family = 2,
	Personal = 3,
	System = 4
}

public enum E_ConsumableRewards : byte
{
	None = 0,
	Gold = 1,
	MagicStone = 2,
	AccountExp = 3,
	VIPExp = 4
}

public enum E_QuestType : byte
{
	None = 0,
	DailyLogin = 1,
	UpgradeLevel = 11,
	Collecting = 12,
	Cost = 13,
	CompleteMode = 14,
	AnyModeWin = 15,
	DefeatAnyTarget = 16,
}

public enum E_QuestSubtype : byte
{
	None = 0,
	One = 1,
	Two = 2,
	Three = 3
}

public enum E_DailyRewardState
{
	NotAvailable = 0,
	Available,
	Received,
	AvailableOnNextDay
}

public enum E_QuestState
{
	NotComplete = 0,
	CompletedNotGetReward,
	AlreadyGetReward,
}

public enum E_FriendType
{
	None,
	Add,
	Block,
	Application,
	Waiting,
	Played,
	Delete
}

//Used in RESOURCE_BLOCK_MATERIAL_FORMAT. *DONOT CHANGE* to upper case
public enum E_VillageOwner : int
{
	noowner = 0,
	player1 = 1,
	player2 = 2,
	player3 = 3,
}

//used in AttackJudgementController and UIBattleHPBarAndStatus
public enum E_AttackJudgementMoveType : int
{
	None,
	LeftToCenter,
	LeftToRun,
	RightToCenter,
	RightToRun,
	MoveCenterTogether,
}

public enum E_ButtonType : byte
{
    None,
    Ok,
    Cancel,
}

public enum E_EquipableStatus : byte
{
	Locked = 0,
	Unlocked = 1,
    Equipped=2,
    EquippedAt1 = 3,
    EquippedAt2 = 4,
    EquippedAt3 = 5,
	Unlockable = 6,
}

public enum E_ElementType : byte
{
	None = 0,
	Wind = 1,//風屬
	Poison = 2,//毒屬
	Water = 3,//水屬
	Fire = 4,//火屬
	Earth = 5//土屬
}

public enum E_TargetType : byte//技能影響目標
{
	Self = 0,//自身軍隊
	Allies = 1,//友軍
	Enemy = 2,//敵軍
	MostFarEnemy=3
}

public enum E_AffectType : byte//技能形式
{
	AOE = 0,//範圍技
	SingleTarget = 1,//單體目標
}

public enum E_AffectValueType : byte//技能數值的種類
{
	Numeric = 0,//數字
	Percentage = 1,//百分比
}

public enum E_ArmyType : byte
{    
	 
	BasicFootman = 1, //長槍兵
	Footman = 2, //重裝長槍兵
	QingzhouSoldiers = 3, //青州兵
	HeavyFootman = 4, //大戟士
	FuryForce = 5, //虎賁義從
	Jiman = 6, //虎魏軍
	Kuseforce = 21, //白耳兵
	ShieldFootman = 8, //陷陣營
	DeathVanguard = 30, //先登死士
	DanyangSoldier = 23, //丹陽兵
	ShieldLancer = 24, //泰山兵
	// RattanSoldiers = 12, //藤甲兵
	HookSwordsmen = 22, //夜叉行
	ForestLegion = 14, //羽林軍
	Archer = 15, //短弓兵
	HeavyBowman = 16, //重弓兵
	Xianbeibowman = 17, //長弓兵
	YulinArcher = 18, //錦帆軍
	// FlitterVanguard = 19, //無當非軍
	RattanSoldier = 19,
	PoisonFangRanger = 32, //毒齒材官
	// CrossBowman = 21, //弩兵
	LianNuShooter = 31, //元戎弩
	// SkySickleCavalry = 23, //烏丸飛鐮騎
	// Bowrider = 24, //漁陽突騎
	// WarWagon = 25, //羌鐵車兵
	Horseman = 26, //輕騎兵
	// XiongNuRider = 27, //匈奴輕騎兵
	SaberKnight = 25, //驃刀騎
	// WhiteHorseKnight = 29, //白馬義從
	FlyingBearForces = 20, //飛熊軍
	WolfForceRider = 27, //并州狼騎
	SilverPlumedKnight = 32, //銀翎飛騎
	HeavyRider = 40, //重騎兵
	Xilianrider = 34, //西涼鐵騎
	ScimitarRider = 29, //武衛騎
	FormationBreaker = 36, //折衝彪騎
	Jrider = 37, //宿衛虎騎
	EleRider = 33, //象兵
	PantherForce = 28, //虎豹騎
	LongGuardian = 40 //龍飛衛
}

public enum E_ArmyJobType: byte
{
	Assasin = 0,//奇襲型軍種
	Attacker = 1,//攻擊型軍種
	Defender = 2,//防禦型軍種
}

public enum E_ArmyMoveType : byte
{
	Infantry = 0,//用走的，步兵種
	Cavalry = 1,//騎馬的，騎兵種
}

public enum E_ArmyAttackType : byte
{
	Meelee = 0,//近戰型
	Range = 1,//遠攻型
}

public enum E_ResourceType : byte
{
	BattlePoint = 0,//戰鬥點數
}

public enum E_AttributeType : byte
{
	Hp = 0,//軍隊血量
	Atk = 1,//攻擊力
	Def = 2,//防禦力
	AtkSpeed = 3,//攻擊速度
	Crit = 4,//爆擊機率
	CritDef = 5,//爆擊防禦
	MoveSpeed = 6,//移動速度
	AtkRange = 7,//射程、攻擊距離
	ShaftPunch=8,
	Invisible=9,
	StunResist=10,
	KnockResist=11,
	RootResist=12,
	MultiShot=13
}

public enum E_KingdomType : byte
{
    //陣營種類
    Shu =0,//蜀漢
    Wei=1,//曹魏
    Wu=2,//東吳
    Other=3//其他群雄
}
public enum E_SkillTrigger : int
{
    Thump=0,//槌擊
    HammerShooting=1,//穿心箭(附擊退效果)
    LegShooting=2,//致殘射擊
    FireScheme=3,//火計陣
    AllyHealing=4,//治癒友軍
    SelfHealing=5,//自體治癒
    BloodlySucking=6,//吸血狀態
    Ambush=7,//(重生)伏襲
    StunPunch=8,//暈眩重擊
	PunctureAttack=9,
	QuickShot=10
}

public enum E_MatchMode : int
{
	FriendlyMatch = 0,//友誼戰
	RankedMatch = 1,//排位配對
	CampaignMatch = 2,
	TowerMatch = 3,
}

public enum E_Sign : int
{
	Plus,
	Minus,
	Multiply,
	Divide,
	Remainder,
	None
}


public enum E_AttributeOwner : int
{
	PlayerHero,
	Army
}

public enum E_GachaType : int
{
	None = 0,
	Fixed = 1,
	BambooRoll1X = 2,
	BambooFragment1X = 3,
	BambooRoll10X = 4,
	BambooFragment10X = 5,
	BattleWin = 6,
	BattleLose = 7,
}

public enum E_JudgingTiming
{
	EachAttack=0,
	OnAlways=1,
	OnGetDeadlyDamage=2,
	OnNotAttack=3,
	OnAllyFailed=4,
	OnUnderAttack=5
}

public enum E_ShaderType
{
	Normal,
	SemiTransparent,
	Invisible,
    ModelGreyScale,
}


public enum E_DialougeTriggerType : int
{ 
	BeforeBattle = 0,
	AfterBattleWin = 1,
	AfterBattleLose = 2,
	EnemyHeroDie = 3,
	PlayerSupportAppear = 4,
	EnemySupportAppear = 5,
	Weather = 6
}

public enum E_DialogueUIType : int
{
	ImageLeft = 0,
	ImageRight = 1,
	ImageEmpty = 2,
}

public enum E_Color : int
{
	Red = 0,
	Green = 1,
	Blue = 2,
}

public enum E_DamageType
{
	StrikeDamage=0,
	SpikeDamage=1,
	SlashingDamage=2
}

