using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Leader : MonoBehaviour
{
	private const float SPEED_MODIFIER = 0.7f;
	//Attributes
    public float HP;
	public float Evasion = 0.0f;
	public float Lifesteal = 0.0f;
	public int DamageBlockCount = 0;
	public float Speed = 1.0f;
	public float Range = 30f;
	public float AttackTime = 1f;
	public float CombinedTotalAtk;
	public float CombinedTotalDef;
	public float CombinedTotalCrit;
	public float CombinedTotalCritDef;
	public float CombinedTotalAtkSpeed;
	public float DmgPcntCorrection;
	//-----End of Attributes


	//Status
	private bool ClosestTargetFinder = true;

	public bool Pause;
    public bool IsKnocking = false;

    public bool IsRooting = false;
    private float RootingDuration=0.0f;

    public bool IsStunning = false;
    private float StunningDuration=0.0f;

    private float LifestealDuration=0.0f;

    //Store data
    public float m_AgentSpeed;
    public float m_AgentAngularSpeed;

    public string MovingParam;
    public string AttackingParam;
    public string SpeedParam = "Speed";
   

    public Transform Target;
	public Transform SecondTarget;
	public Transform ThirdTarget;
    public Animator anim;
    private NavMeshAgent agent;
    public Formator formator;
    private AlternativeMaterials suits;
    public Team team;

    public bool IsMoving;
    public bool IsAttacking;

	
    public float TotalHP;
    public float SkillTriggerPoints;


    public float Timer = 0f;

    public BattleManager battleManager;
    public List<Team> enemies;

    public Transform UI;
    public GameObject UIPrefab;
    public GameObject Text3D;

    public GameObject EffectPrefab;
    //public HeroGraphCanvas HeroGraph;
    private LineRenderer line;

    public GameObject FormatorDetector;
    public GameObject m_formatorDetector;

    public GameObject UICard;
    public bool GameAlreadyStarted = false;
    public bool AlreadyAttack = false;

    //UI data
    public Texture FaceSprite;
    public string HeroName;
    public string ArmyName;
    public List<string> AttackSoundName;

    //Data Class
    public PlayerHeroData m_PlayerHeroData;
    private AttributeData m_HeroAttributeData;
    public HeroArmyData m_ArmyData;
	private AttributeData m_ArmyAttributeData;

	private NpcData m_NpcData;
	public NpcData NpcData
	{
		get 
		{
			return m_NpcData;
		}
		set
		{
			m_NpcData = value;
		}
	}

	List<SkillData> m_Skills=new List<SkillData>();
    List<SkillData> m_ArmySkills = new List<SkillData>();

	public E_ArmyMoveType ArmyMoveType;
	public E_ArmyAttackType ArmyAttackType;
	public E_ArmyJobType ArmyJobType;

	//NegativeBuff
	public List<NegativeBuff> ngBuffs=new List<NegativeBuff>();

    public ArmySkillTrigger skillTriggerScript;

	public bool IsAggresive=false;
	public bool IsInvisible = false;
	private E_ShaderType CurrentShader = E_ShaderType.Normal;

	public string ArmyModelString;
	public void FillNpcData(NpcData npcData,ArmyData armyData)
	{
		NpcData = npcData;
		m_Skills = new List<SkillData>();

		SkillData skill1 = TableManager.Instance.SkillDataTable.GetData(npcData.NpcSkill1);
		if (skill1 != null)
		{
			m_Skills.Add(skill1);
		}
		SkillData skill2 = TableManager.Instance.SkillDataTable.GetData(npcData.NpcSkill2);
		if (skill2 != null)
		{
			m_Skills.Add(skill2);
		}
		SkillData skill3 = TableManager.Instance.SkillDataTable.GetData(npcData.NpcSkill3);
		if (skill3 != null)
		{
			m_Skills.Add(skill3);
		}
		m_HeroAttributeData = npcData.GetNPCAttribute();
		m_ArmyAttributeData = npcData.GetArmyData().GetArmyAttribute(NpcData.ArmyLevel);
		if (m_ArmyAttributeData != null && m_HeroAttributeData!=null)
		{
			float heroTotalAtk = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_HeroAttributeData.Atk);
			float armyTotalAtk = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Atk);
			CombinedTotalAtk = GLOBALFUNCTION.FORMULA.GetCombinedTotalAtk(heroTotalAtk,armyTotalAtk);

			float heroTotalDef = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_HeroAttributeData.Def);
			float armyTotalDef = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Def);
			CombinedTotalDef = GLOBALFUNCTION.FORMULA.GetCombinedTotalDef(heroTotalDef,armyTotalDef);

			CombinedTotalCrit = m_ArmyAttributeData.Crit;
			CombinedTotalCritDef = m_ArmyAttributeData.CritDef;
			CombinedTotalAtkSpeed = m_ArmyAttributeData.AtkSpeed;

			DmgPcntCorrection = GLOBALFUNCTION.FORMULA.GetDmgPcntCorrection();
			Range = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.AtkRange);
			HP = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Hp);
			Speed = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.MoveSpeed) * SPEED_MODIFIER * 50f;
			m_AgentSpeed = Speed;

			ArmyMoveType = (E_ArmyMoveType)npcData.GetArmyData().ArmyMoveType;
			ArmyAttackType = (E_ArmyAttackType)npcData.GetArmyData().ArmyAttackType;
			ArmyJobType = (E_ArmyJobType)npcData.GetArmyData().ArmyJobType;

			TotalAttack = CombinedTotalAtk;
			TotalDef = CombinedTotalDef;
			TotalSpeed = m_AgentSpeed;
			TotalRange = Range;
			TotalAtkSpeed = CombinedTotalAtkSpeed;

			if (TotalAtkSpeed != 0)
			{
				AttackTime = 1f / TotalAtkSpeed;
			}
		}
		else
		{
			HP = 10f;
			Debug.LogError("Wrong Attribute ID for NPC");
			
		}
	}
    public void RegisterAttributes(PlayerHeroData data,HeroArmyData heroArmyData=null,bool isNPC=false)
    {
       // FormulaObject formulaObject = GameObject.FindObjectOfType<HeroPlacementManager>().formulaObject;

        m_PlayerHeroData = data;
        m_HeroAttributeData = data.GetHeroAttribute();
		

		if (isNPC)
        {
            m_Skills = m_PlayerHeroData.GetEquippedSkills();
			m_ArmyData = heroArmyData;
			 ArmyName = TableManager.Instance.LocaleStringDataTable.GetString(m_ArmyData.GetArmyData().ArmyNameID);
            ArmyModelString = m_ArmyData.GetArmyData().ModelName;

            int level = m_ArmyData.ArmyLevel;
            Range = m_ArmyData.GetArmyAttribute(level).AtkRange;

			m_ArmySkills = m_PlayerHeroData.GetEquippedArmySelf().GetEquipedSkills();
		}
        else
        {
            m_Skills = m_PlayerHeroData.GetEquippedSkills();
            ArmyName = TableManager.Instance.LocaleStringDataTable.GetString(m_PlayerHeroData.GetEquippedArmySelf().GetArmyData().ArmyNameID);
            m_ArmyData = data.GetEquippedArmySelf();

            ArmyModelString=m_ArmyData.GetArmyData().ModelName;

            int level = m_ArmyData.ArmyLevel;
            Range = m_ArmyData.GetArmyAttribute(level).AtkRange;

			m_ArmySkills = m_PlayerHeroData.GetEquippedArmySelf().GetEquipedSkills();
		}
		m_ArmyAttributeData = m_ArmyData.GetArmyAttribute(m_ArmyData.ArmyLevel);
		
		float heroTotalAtk = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_HeroAttributeData.Atk);
		float armyTotalAtk = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Atk);
		CombinedTotalAtk = GLOBALFUNCTION.FORMULA.GetCombinedTotalAtk(heroTotalAtk,armyTotalAtk);

		float heroTotalDef = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_HeroAttributeData.Def);
		float armyTotalDef = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Def);
		CombinedTotalDef = GLOBALFUNCTION.FORMULA.GetCombinedTotalDef(heroTotalDef,armyTotalDef);

		CombinedTotalCrit = m_ArmyAttributeData.Crit;
		CombinedTotalCritDef = m_ArmyAttributeData.CritDef;
		CombinedTotalAtkSpeed = m_ArmyAttributeData.AtkSpeed;

		DmgPcntCorrection = GLOBALFUNCTION.FORMULA.GetDmgPcntCorrection();
		Range = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.AtkRange);
		HP = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.Hp);
		Speed = GLOBALFUNCTION.FORMULA.GetTotalAttribute(m_ArmyAttributeData.MoveSpeed) * SPEED_MODIFIER * 50f;
		m_AgentSpeed = Speed;

		ArmyMoveType=(E_ArmyMoveType)m_ArmyData.GetArmyData().ArmyMoveType;
		ArmyAttackType = (E_ArmyAttackType)m_ArmyData.GetArmyData().ArmyAttackType;
		ArmyJobType=(E_ArmyJobType)m_ArmyData.GetArmyData().ArmyJobType;

		TotalAttack = CombinedTotalAtk;
		TotalDef = CombinedTotalDef;
		TotalSpeed = m_AgentSpeed;
		TotalRange = Range;
		TotalAtkSpeed = CombinedTotalAtkSpeed;

		if (TotalAtkSpeed != 0)
		{
			AttackTime = 1f / TotalAtkSpeed;
		}
	}
	public float TotalAttack;
	public float TotalDef;
	public float TotalSpeed;
	public float TotalRange;
	public float TotalAtkSpeed;
	public void HandleBuff(List<Buff> Buffs)
	{
		float Atk = 0;
		float Def=0;
		float MSpd = 0;
		float M_Range=0;
		float AtkSpeed = 0;
		for (int i = 0; i <Buffs.Count; i++)
		{
			switch ((E_AttributeType)Buffs[i].skillData.AffectAttribute)
			{
				case E_AttributeType.Atk:
				Atk += (CombinedTotalAtk * (Buffs[i].skillData.AffectValue / 100f));
				break;
				case E_AttributeType.Def:
				Def += (CombinedTotalDef * (Buffs[i].skillData.AffectValue / 100f));
				break;
				case E_AttributeType.MoveSpeed:
				MSpd+= (m_AgentSpeed * (Buffs[i].skillData.AffectValue / 100f));
				break;
				case E_AttributeType.AtkRange:
				M_Range+= (Range * (Buffs[i].skillData.AffectValue / 100f));
				break;
				case E_AttributeType.AtkSpeed:
				AtkSpeed += (CombinedTotalAtkSpeed * (Buffs[i].skillData.AffectValue / 100f));
				break;
			}
		}
		TotalAttack = CombinedTotalAtk + Atk;
		TotalDef =CombinedTotalDef+Def;
		TotalSpeed = m_AgentSpeed + MSpd;
		TotalRange = Range + M_Range;
		TotalAtkSpeed =CombinedTotalAtkSpeed+ AtkSpeed;

		if (Speed != 0)
		{
			Speed = TotalSpeed;
			agent.speed = Speed;
		}
		if (TotalAtkSpeed != 0)
		{
			AttackTime = 1f / TotalAtkSpeed;
		}
	}
	public void TriggerSkill(SkillEffectData skillEffect)
	{
		switch ((E_SkillTrigger)skillEffect.AffectAttribute)
		{
			case E_SkillTrigger.PunctureAttack:
				if (Target != null)
				{
					Target.GetComponent<Leader>().Hit(TotalAttack*skillEffect.AffectValue,this,false,true);
				}
			break;
			case E_SkillTrigger.QuickShot:
				if (Target != null)
				{
					GameObject vfx = (GameObject)Resources.Load("Effects/"+skillEffect.EffectNameID);
					vfx=Instantiate(vfx,Target.transform.position,Target.transform.rotation);
					vfx.SetActive(true);
					StartCoroutine(TriggerQuickShot(Target.gameObject));
				}
			break;
		}
	}
    private void Start()
	{
		m_formatorDetector = Instantiate(FormatorDetector,transform.position,Quaternion.identity);
		m_formatorDetector.transform.SetParent(transform);

		agent.radius = 0.2f;
	}
	public void GameStart()
	{
		IsAggresive = true;

		battleManager = GameObject.FindObjectOfType<BattleManager>();

		agent.speed = TotalSpeed;
        m_AgentAngularSpeed = agent.angularSpeed;

        AttackSoundName = formator.GetAttackSFXNames();

        FindTargets();

        TotalHP = HP;
        GameObject ins= Instantiate(UIPrefab);
        ins.SetActive(true);
		UI = ins.transform;
        UI.GetComponent<HeroBattleStatus>().ChangeIcon(FaceSprite);
        if (team.team == Team.TeamType.TeamTwo)
		{
			UI.GetComponent<HeroBattleStatus>().ChangeColor();     
		}
		line = UI.GetComponent<LineRenderer>();

		for (int i = 0; i < formator.Units.Count; i++)
		{
			formator.Units[i].transform.parent = null;
			formator.Units[i].GetComponent<NavMeshAgent>().enabled=true;
		}
		List<Team> visibleEnemies = new List<Team>();
		for (int i = 0; i < enemies.Count; i++)
		{
			if (!enemies[i].leader.IsInvisible)
			{
				visibleEnemies.Add(enemies[i]);
			}
		}
		GameObject newTarget = GLOBALFUNCTION.FindTargetToAttack(visibleEnemies,ArmyMoveType,ArmyAttackType,ArmyJobType,transform.position);
		if (newTarget != null)
		{
			Target = newTarget.transform;
		}

		if (ArmyMoveType == E_ArmyMoveType.Cavalry)
		{
			GameObject sandVFX = (GameObject) Resources.Load("BuffEffects/Sand");

			for (int i = 0; i < formator.Units.Count; i++)
			{
				GameObject effect=Instantiate(sandVFX,formator.Units[i].transform.position,Quaternion.identity);
				effect.transform.SetParent(formator.Units[i].transform,true);
				effect.transform.localScale = Vector3.one;
			}
		}

		Invoke("DelayGameStart",2f);
	}
	public void DelayGameStart()
	{
		GameAlreadyStarted = true;
		skillTriggerScript.OnGameStart();
	}
    public void FindTargets()
    {
        Team[] AllGroups = GameObject.FindObjectsOfType<Team>();

        enemies = new List<Team>();
        for (int i = 0; i < AllGroups.Length; i++)
        {
            if (AllGroups[i].team != team.team)
            {
                enemies.Add(AllGroups[i]);
            }
        }
    }
	public void Attack()
	{
		if (Target != null)
		{
            skillTriggerScript.OnEachAttack();
			Leader Enemy = Target.GetComponent<Leader>();
			int CritChance = Random.Range(1,101);
			if ((CombinedTotalCrit-Enemy.CombinedTotalCritDef)>= CritChance)
			{
				Enemy.Hit(TotalAttack*2f,this,false,true);
			}
			else
			{
				Enemy.Hit(TotalAttack,this);
			}
			skillTriggerScript.DispelBuff(E_AttributeType.Invisible);
            for(int i=0;i<AttackSoundName.Count;i++)
            {
                SoundManager.Instance.PlaySound(AttackSoundName[i]);
            }
			SecondTarget = null;
			ThirdTarget = null;

			if (skillTriggerScript.IsThereBuff(E_AttributeType.MultiShot))
			{
				List<Team> visibleEnemies = new List<Team>();
				for (int i = 0; i < enemies.Count; i++)
				{
					if (!enemies[i].leader.IsInvisible)
					{
						visibleEnemies.Add(enemies[i]);
					}
				}
				visibleEnemies.Remove(Target.GetComponent<Team>());

				if (visibleEnemies.Count != 0)
				{
					SecondTarget = GLOBALFUNCTION.GetClosetEnemy(visibleEnemies,transform.position).transform;
					if (Vector3.Distance(transform.position,SecondTarget.position) > TotalRange)
					{
						SecondTarget = null;
					}
				}

				if (SecondTarget != null)
				{
					visibleEnemies.Remove(SecondTarget.GetComponent<Team>());
					if (visibleEnemies.Count != 0)
					{
						ThirdTarget = GLOBALFUNCTION.GetClosetEnemy(visibleEnemies,transform.position).transform;
					}
				}

				if (SecondTarget != null)
				{
					Enemy = SecondTarget.GetComponent<Leader>();
					CritChance = Random.Range(1,101);
					if ((CombinedTotalCrit - Enemy.CombinedTotalCritDef) >= CritChance)
					{
						Enemy.Hit(TotalAttack * 2f,this,false,true);
					}
					else
					{
						Enemy.Hit(TotalAttack,this);
					}
				}

				if (ThirdTarget != null)
				{
					Enemy = ThirdTarget.GetComponent<Leader>();
					CritChance = Random.Range(1,101);
					if ((CombinedTotalCrit - Enemy.CombinedTotalCritDef) >= CritChance)
					{
						Enemy.Hit(TotalAttack * 2f,this,false,true);
					}
					else
					{
						Enemy.Hit(TotalAttack,this);
					}
				}
			}
			//SkillTriggerPoints += Damage;
		}
		if (!AlreadyAttack)
		{
			GameObject.FindObjectOfType<CameraController>().StartControl();
			AlreadyAttack = true;
		}
	}
    public void LifestealCallback(float DmgPoints)
    {
        SkillTriggerPoints += TotalAttack;

        float LifestealPoints = Lifesteal * DmgPoints*0.01f;
        if(LifestealPoints>0)
        {
            HealWithPoints(LifestealPoints);
        }
    }
	public void Hit(float dmg,Leader DmgDealer=null,bool IsPoison=false,bool isCrit=false)
	{
		if (skillTriggerScript.IsThereBuff(E_AttributeType.ShaftPunch) && DmgDealer.ArmyAttackType==E_ArmyAttackType.Meelee)
		{
			DmgDealer.StartCoroutine("ShaftPunch");
		}
		dmg -= TotalDef;
		if (dmg <= 0)
		{
			dmg = 0;
		}

        float rndEvasion = Random.Range(1f,100f);
        if(rndEvasion<Evasion)
        {
            int rnd1 = Random.Range(0, formator.Units.Count);
            TextMesh tm1 = Instantiate(Text3D, formator.Units[rnd1].transform.position+Vector3.up*2, transform.rotation).GetComponent<TextMesh>();
            tm1.text = "Evade";
            tm1.color = Color.yellow;
            return;
        }
        if(DamageBlockCount>0)
        {
            DamageBlockCount--;
            int rnd1 = Random.Range(0, formator.Units.Count);
            TextMesh tm1 = Instantiate(Text3D, formator.Units[rnd1].transform.position+ Vector3.up * 2, transform.rotation).GetComponent<TextMesh>();
            tm1.text = "Blocked";
            tm1.color = Color.red;
            return;
        }
		if (dmg > 0)
		{
			skillTriggerScript.DispelBuff(E_AttributeType.Invisible);
		}
        HP -= dmg;
        SkillTriggerPoints += dmg;
        if (DmgDealer != null)
        {
            DmgDealer.LifestealCallback(dmg);
        }
		if (HP <= 0.0f)
		{
            skillTriggerScript.OnGetDeadlyDamage();

			this.gameObject.SetActive(false);
			UI.gameObject.SetActive(false);

			for (int i = 0; i < formator.Units.Count; i++)
			{
				if (formator.Units[i].activeSelf)
				{
					formator.Units[i].GetComponent<Unit>().Die();
				}
			}
            List<Leader> teamates=new List<Leader>();
            if (team.team==Team.TeamType.TeamOne)
            {
                teamates= battleManager.teamOne;
            }
            else if(team.team==Team.TeamType.TeamTwo)
            {
                teamates = battleManager.teamTwo;
            }
            for(int i=0;i<teamates.Count;i++)
            {
                if(teamates[i]!=this)
                {
                    teamates[i].skillTriggerScript.OnAllyFailed(this);
                }
            }
		}
		if (HP <= 60 && HP >= 50)
		{
			int randomChance = Random.Range(0,100);
			if (randomChance<30)
			{
                if (m_ArmySkills.Count > 0)
                {
                    int rand = Random.Range(0, m_ArmySkills.Count);
                    UI.GetComponent<HeroBattleStatus>().ShowDialogue(m_ArmySkills[rand].GetSkillName());
                }      
			}
		}
		int rnd = Random.Range(0,formator.Units.Count);
		TextMesh tm = Instantiate(Text3D,formator.Units[rnd].transform.position+ Vector3.up * 2,transform.rotation).GetComponent<TextMesh>();
		tm.text = "-"+(int)dmg;
		if (IsPoison)
		{
			tm.color = new Color32(128,0,255,255);
		}
		else if (isCrit)
		{
			tm.color = new Color32(255,255,0,255);
		}
		else if (team.team == Team.TeamType.TeamOne)
		{
			tm.color = Color.red;
		}
		float hpPercentage = HP / TotalHP;
		UI.GetComponent<HeroBattleStatus>().UpdateHP(hpPercentage);
        skillTriggerScript.OnUnderAttack();

		if (DmgDealer != null && !IsAttacking)
		{
			if (Target != DmgDealer.transform)
			{
				Target = DmgDealer.transform;

				for (int i = 0; i < formator.Units.Count; i++)
				{
					formator.Units[i].GetComponent<Unit>().AttackTarget=null;
				}
			}		
		}
	}
    private GameObject Effect;
    private string SfxName;
    private E_SkillTrigger e_skillTrigger;
	public void TriggerEffect(SkillData skill)
	{
        e_skillTrigger = (E_SkillTrigger)skill.SkillTriggerID;
        Effect = skill.GetVFX();
        SfxName = skill.GetSFXName();
        if(Effect==null)
        {
            return;
        }
		CameraController camController=GameObject.FindObjectOfType<CameraController>();
        if(camController.SystemControl)
        {
            return;
        }
        SkillTriggerPoints = 0;
        camController.SystemControl = true;
		camController.Target.position = GetCeneterPoint();

		Leader[] leaders = GameObject.FindObjectsOfType<Leader>();
		for (int i = 0; i < leaders.Length; i++)
		{
			leaders[i].Pause = true;
			leaders[i].agent.enabled = false;
		}
        UIBattleSkillTrigger.UIBatttleSkillTriggerParams para= new UIBattleSkillTrigger.UIBatttleSkillTriggerParams();
        para.texture = FaceSprite;
        para.SkillName = skill.GetSkillName();
		para.teamType = team.team;
        UIManager.Instance.ShowUI("UIBattleSkillTrigger",para);
		Camera.main.GetComponent<Animator>().ResetTrigger("FadeIn");
		Camera.main.GetComponent<Animator>().SetTrigger("FadeIn");
		Invoke("ShowEffect",1.5f);
		Invoke("RefreshPause",3f);
	}
	void StopShaking()
	{
		Camera.main.GetComponent<Animator>().SetBool("IsShaking",false);
	}
	void ShowEffect()
	{
		battleManager.Vibrate(0.3f);
		Camera.main.GetComponent<Animator>().SetBool("IsShaking",true);
		Invoke("StopShaking",0.3f);
        UIManager.Instance.CloseUI("UIBattleSkillTrigger");
        GameObject effect=Instantiate(Effect,formator.Units[0].transform.position,transform.rotation);
		effect.SetActive(true);
        Debug.Log(SfxName);
        Leader enemyLeader = Target.GetComponent<Leader>();
        if(enemyLeader!=null)
        {
            switch (e_skillTrigger)
            {
                case E_SkillTrigger.Thump:
                case E_SkillTrigger.HammerShooting:
                    Target.GetComponent<Leader>().StartCoroutine("TriggerKnockback"); //Thump or Hammer Shooting
                    break;
                case E_SkillTrigger.LegShooting:
                    Target.GetComponent<Leader>().Root(5f); //Leg Shooting
                    break;
                case E_SkillTrigger.FireScheme:
                    TriggerFlame(); //Fire scheme
                    break;
                case E_SkillTrigger.AllyHealing:
                    HealAllAllies(); //ally healing
                    break;
                case E_SkillTrigger.SelfHealing:
                    HealWithPercentage(10f); //Self Healing
                    break;
                case E_SkillTrigger.BloodlySucking:
                    TriggerLifesteal(60, 7); //Bloodly sucking
                    break;
                case E_SkillTrigger.Ambush:
                    Ambush(); //ambush
                    break;
                case E_SkillTrigger.StunPunch:
                    Target.GetComponent<Leader>().Stun(5f);
                    break;
            }
        }   
        SoundManager.Instance.PlaySound(SfxName);
    }
	void RefreshPause()
	{
		Leader[] leaders = GameObject.FindObjectsOfType<Leader>();
		for (int i = 0; i < leaders.Length; i++)
		{
            leaders[i].Pause = false;
			leaders[i].agent.enabled = true;
		}

        CameraController camController = GameObject.FindObjectOfType<CameraController>();
		camController.SystemControl = false;
	}
	void CheckDead()
	{
		if (formator.Units.Count == 0)
		{
			return;
		}
		int TotalCount = formator.Units.Count + formator.Deads.Count;
		int AliveCount = 0;
		float HPpercentage = HP / TotalHP;
		AliveCount = Mathf.CeilToInt(HPpercentage * TotalCount);

		if(formator.Units.Count > AliveCount)
		{
				int rnd = Random.Range(0,formator.Units.Count);

				formator.Units[rnd].GetComponent<Unit>().Die();
		
				formator.Deads.Add(formator.Units[rnd]);
				formator.Units.RemoveAt(rnd);
		}
	}
	public void OnDisable()
	{
		if (IsInvoking("RefreshPause"))
		{
			CancelInvoke();
			Camera.main.GetComponent<Animator>().ResetTrigger("FadeIn");
			Camera.main.GetComponent<Animator>().SetBool("IsShaking",false);
			Camera.main.GetComponent<Animator>().CrossFade("idle",0);
			RefreshPause();
			UIManager.Instance.CloseUI("UIBattleSkillTrigger");
		}
	}
	private void Awake()
	{
		anim = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		formator = GetComponentInChildren<Formator>();
		team = GetComponent<Team>();
        AttackSoundName = new List<string>();

        skillTriggerScript = this.gameObject.AddComponent<ArmySkillTrigger>();
    }
    private void SpellTriggers()
    {
        if (SkillTriggerPoints >= 300 && !IsStunning)
        {
            if (m_Skills.Count > 0 && Target!=null && Target.gameObject.activeSelf)
            {
                int rand = Random.Range(0, m_Skills.Count);
                TriggerEffect(m_Skills[rand]);
            }
        }
    }
	private void Update()
	{
		if (!GameAlreadyStarted)
		{
			return;
		}
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x,-250f,250f),
			transform.position.y,
			Mathf.Clamp(transform.position.z,-455f,455f)
		);
		IsInvisible = skillTriggerScript.IsThereBuff(E_AttributeType.Invisible);
		if (IsInvisible)
		{
			if (team.team == Team.TeamType.TeamOne)
			{
				ChangeShader(E_ShaderType.SemiTransparent);
			}
			else
			{
				ChangeShader(E_ShaderType.Invisible);
				UI.gameObject.SetActive(false);
			}
		}
		else
		{
			ChangeShader(E_ShaderType.Normal);
			UI.gameObject.SetActive(true);
		}
		if (UI != null)
		{
			UI.transform.position = transform.position + Vector3.up * 50 + transform.forward * -20;
		}
        if (formator.Units.Count != 0)
        {
            line.SetPosition(0, UI.transform.position - Vector3.up * 10);
            line.SetPosition(1, GetCeneterPoint() + Vector3.up * 5);
        }
        if (Pause)
        {
            return;
        }
        if(Lifesteal>0)
        {
            LifestealDuration -= Time.deltaTime;
            if(LifestealDuration<=0)
            {
                LifestealDuration = 0;
                Lifesteal = 0;
            }
        }
        if(ngBuffs.Count>0)
        {
            for(int i=0;i<ngBuffs.Count;i++)
            {
                NegativeBuff buff = ngBuffs[i];
                buff.timer += Time.deltaTime;
                if(buff.timer>=buff.TriggerInterval)
                {
                    buff.timer -= buff.TriggerInterval;
                    Hit(buff.Param1,null,true);
                    buff.Duration -= buff.TriggerInterval;
                }
                if(buff.Duration-buff.timer<=0)
                {
                    ngBuffs.RemoveAt(i);
                }
            }
        }
        if(IsRooting)
        {
            agent.speed = 0;
            RootingDuration -= Time.deltaTime;
            if(RootingDuration<=0)
            {
                RootingDuration = 0;
                IsRooting = false;
            }
        }
        else if(IsStunning)
        {
            agent.speed = 0;
            StunningDuration -= Time.deltaTime;
            if (StunningDuration <= 0)
            {
                StunningDuration = 0;
                IsStunning = false;
            }
        }
        else
        {
            agent.speed = TotalSpeed;
        }
        CheckDead();
		if (IsAttacking)
		{
			SpellTriggers();
		}

        skillTriggerScript.OnAlways();

		if (!IsAttacking && IsAggresive)
		{
			List<Team> visibleEnemies = new List<Team>();
			for (int i = 0; i < enemies.Count; i++)
			{
				if (!enemies[i].leader.IsInvisible)
				{
					visibleEnemies.Add(enemies[i]);
				}
			}
			GameObject newTarget = GLOBALFUNCTION.GetClosetVisible(visibleEnemies,transform,100f,30f);
			if (newTarget != null)
			{
				if (newTarget != Target)
				{
					Target = newTarget.transform;
					Target.GetComponent<Leader>().TriggerToDuel(transform);
				}
			}
		}

        for (int i = 0; i < enemies.Count; i++)
		{
			if (!enemies[i].gameObject.activeSelf)
			{
				enemies.Remove(enemies[i]);
			}
		}
		if (enemies.Count == 0)
		{
			IsMoving = false;
			IsAttacking = false;
			anim.SetBool(MovingParam, IsMoving);
			anim.SetBool(AttackingParam, IsAttacking);
			agent.SetDestination(transform.position);
			return;
		}
		if (Target == null)
		{
			IsMoving = false;
			IsAttacking = false;
			Timer = 0;
			anim.SetBool(MovingParam, IsMoving);
			anim.SetBool(AttackingParam, IsAttacking);
			agent.SetDestination(transform.position);

			if (enemies.Count > 0)
			{
				List<Team> visibleEnemies = new List<Team>();
				for (int i = 0; i < enemies.Count; i++)
				{
					if (!enemies[i].leader.IsInvisible)
					{
						visibleEnemies.Add(enemies[i]);
					}
				}
				GameObject newTarget = GLOBALFUNCTION.GetClosetEnemy(visibleEnemies,transform.position);
				if (newTarget != null)
				{
					Target = newTarget.transform;
				}
				
			}
		}
        if(IsAttacking)
        {
            if(Vector3.Distance(transform.position,Target.position)>=TotalRange*1.5f)
            {
                Target = null;
                IsAttacking = false;
                for(int i=0;i<formator.Units.Count;i++)
                {
                    formator.Units[i].GetComponent<Unit>().DispelTarget();
                }
                return;
            }
        }
		if (Target!=null && !Target.gameObject.activeSelf)
		{
			Target = null;
			return;
		}
		if (Target!=null && Target.GetComponent<Leader>().IsInvisible)
		{
			Target = null;
			return;
		}
		if (Target != null)
		{
			if (Vector3.Distance(transform.position,Target.position) <= TotalRange)
			{
				agent.SetDestination(transform.position);
				//agent.Warp(transform.position);
				IsMoving = false;
				IsAttacking = true;
				//transform.LookAt(Target.position);
			}
			else
			{
				agent.SetDestination(Target.position);
				//agent.Warp(Target.position);
				IsMoving = true;
				IsAttacking = false;
				Timer = 0;
			}
		}
	
        if(IsStunning)
        {
            IsAttacking = false;
        }
        if(!IsAttacking && !IsStunning)
        {
            skillTriggerScript.OnNotAttack();
        }
		anim.SetBool(MovingParam,IsMoving);
		anim.SetBool(AttackingParam,IsAttacking);

		if (IsAttacking)
		{
			Timer += Time.deltaTime;
			if (Timer >= AttackTime)
			{
				Invoke("Attack",0.02f);
				Timer = 0;
			}
		}
	}
	public void Shoot()
	{
		//do nothing anymore
	}
	public void TriggerToDuel(Transform target)
	{
		if (!IsAttacking && Target!=target)
		{
			Target = target;
		}
	}
    public void TriggerLifesteal(float buffPercentage,float duration)
    {
        Lifesteal = buffPercentage;
        LifestealDuration = duration;
    }
    public void Ambush()
    {
        Leader tempLeader=battleManager.GetDeadLeader(team.team);
        if(tempLeader!=null)
        {
            tempLeader.Revive();
        }
        battleManager.RefreshEnemies();
    }
    public void Revive()
    {
        this.gameObject.SetActive(true);
        this.UI.gameObject.SetActive(true);

        HP = TotalHP * 0.2f;

        float hpPercentage = HP / TotalHP;
        UI.GetComponent<HeroBattleStatus>().UpdateHP(hpPercentage);

        int TotalCount = formator.Units.Count + formator.Deads.Count;
        int AliveCount = 0;
        float HPpercentage = HP / TotalHP;
        AliveCount = Mathf.CeilToInt(HPpercentage * TotalCount);

        for(int i=0;i<AliveCount;i++)
        {
            formator.Units[i].GetComponent<Unit>().Revive();

            formator.Units.Add(formator.Deads[i]);
            formator.Deads.RemoveAt(i);
        }
    }
    public void Root(float duration)
    {
		if (skillTriggerScript.IsThereBuff(E_AttributeType.RootResist))
		{
			return;
		}
		RootingDuration = duration;
        IsRooting = true;
    }
    public void Stun(float duration)
    {
		if (skillTriggerScript.IsThereBuff(E_AttributeType.StunResist))
		{
			return;
		}
        StunningDuration = duration;
        IsStunning = true;
    }
    public GameObject FlameObject;
    public void TriggerFlame()
    {
        Flame flame = Instantiate(FlameObject, transform.position, Quaternion.identity).GetComponent<Flame>();
        flame.StartFlame(team.team,10,5f);
    }
    public IEnumerator TriggerKnockback()
    {
		if (!skillTriggerScript.IsThereBuff(E_AttributeType.KnockResist))
		{
			IsKnocking = true;
			float time = 0.0f;
			Vector3 startPos = transform.position;
			Vector3 desiredPos = transform.position + transform.forward * -100f;
			desiredPos = new Vector3(
			Mathf.Clamp(desiredPos.x,-250f,250f),
			desiredPos.y,
			Mathf.Clamp(desiredPos.z,-455f,455f)
			);
			while (IsKnocking)
			{
				IsAttacking = false;
				time += 0.1f;
				transform.position = Vector3.Lerp(startPos,desiredPos,time);
				yield return new WaitForSeconds(0.02f);
				if (time >= 1)
				{
					IsKnocking = false;
				}
			}
			yield return new WaitForSeconds(0.0f);
		}
    }
	public IEnumerator ShaftPunch()
	{
		if (!skillTriggerScript.IsThereBuff(E_AttributeType.KnockResist))
		{
			IsKnocking = true;
			float time = 0.0f;
			Vector3 startPos = transform.position;
			Vector3 desiredPos = transform.position + transform.forward * -50f;
			desiredPos = new Vector3(
			Mathf.Clamp(desiredPos.x,-250f,250f),
			desiredPos.y,
			Mathf.Clamp(desiredPos.z,-455f,455f)
			);

			while (IsKnocking)
			{
				IsAttacking = false;
				time += 0.1f;
				transform.position = Vector3.Lerp(startPos,desiredPos,time);
				yield return new WaitForSeconds(0.02f);
				if (time >= 1)
				{
					IsKnocking = false;
				}
			}
			yield return new WaitForSeconds(0.0f);
		}
	}
	public IEnumerator TriggerQuickShot(GameObject enemy)
	{
		yield return new WaitForSeconds(0.1f);
		if (enemy.activeSelf)
		{
			enemy.GetComponent<Leader>().Hit(TotalAttack);
		}
		yield return new WaitForSeconds(0.1f);
		if (enemy.activeSelf)
		{
			enemy.GetComponent<Leader>().Hit(TotalAttack);
		}
	}
	public void HealAllAllies()
    {
        if(team.team==Team.TeamType.TeamOne)
        {
            List<Leader> mates=battleManager.teamOne;
            for(int i=0;i<mates.Count;i++)
            {
                if(mates[i]!=null)
                {
                    mates[i].HealWithPercentage(25);
                }      
            }
        }
        else if(team.team == Team.TeamType.TeamTwo)
        {
            List<Leader> mates = battleManager.teamTwo;
            for (int i = 0; i < mates.Count; i++)
            {
                if (mates[i] != null)
                {
                    mates[i].HealWithPercentage(25);
                }
            }
        }
    }
    public void HealWithPercentage(float percentage)
    {
        float points = percentage * TotalHP*0.01f;
        HealWithPoints(points);
    }
    public void HealWithPoints(float points)
    {
        if(HP<=0)
        {
            return;
        }
        HP += points;
        if(HP>=TotalHP)
        {
            HP = TotalHP;
        }
        float hpPercentage = HP / TotalHP;
        UI.GetComponent<HeroBattleStatus>().UpdateHP(hpPercentage);
    }
	public Vector3 GetCeneterPoint()
	{
		Vector3 center=Vector3.zero;

		for (int i = 0; i < formator.Units.Count; i++)
		{
			center += formator.Units[i].transform.position;
		}
		center = center / formator.Units.Count;

		return center;
	}
    public GameObject getGrid()
    {
        Collider[] cols = Physics.OverlapBox(transform.position,new Vector3(1,200,1), Quaternion.identity, LayerMask.GetMask("Grid"));
        if(cols!=null)
        {
            return cols[0].gameObject;
        }
        return null;
    }
	public void ChangeShader(E_ShaderType shaderType)
	{
		if (CurrentShader == shaderType)
		{
			return;
		}
		for (int i = 0; i < formator.Units.Count; i++)
		{
			formator.Units[i].GetComponent<AlternativeMaterials>().ChangeShaderWithType(shaderType);
		}
		for (int i = 0; i < formator.Deads.Count; i++)
		{
			formator.Deads[i].GetComponent<AlternativeMaterials>().ChangeShaderWithType(shaderType);
		}
		CurrentShader = shaderType;
	}
}
public class NegativeBuff
{
    public float TotalDuration;
    public float Duration;
    public float TriggerInterval;
    public float Param1;
    public float timer;
}
