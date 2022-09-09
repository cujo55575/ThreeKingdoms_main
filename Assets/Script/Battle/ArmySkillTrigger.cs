using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArmySkillTrigger : MonoBehaviour
{
	public bool IsTestHero = false;
	public Leader leader;
	public void Awake()
	{
		leader = GetComponent<Leader>();
		Buffs = new List<Buff>();
	}
	public void OnEachAttack()
    {
        Debug.Log("OnEachAttack"); 
		TryTriggerSpell(E_JudgingTiming.EachAttack);
    }
    public void OnAlways()
    {
		ArmySkillUpdateLoop();
    }
    public void OnGetDeadlyDamage()
    {
        Debug.Log("OnGetDeadlyDamage");
		TryTriggerSpell(E_JudgingTiming.OnGetDeadlyDamage);
	}
    public void OnNotAttack()
    {
        Debug.Log("NotAttacking");
		TryTriggerSpell(E_JudgingTiming.OnNotAttack);
	}
    public void OnAllyFailed(Leader leader)
    {
        Debug.Log("OnAllyFailed");

		DispelBuffWhenProviderDie(leader);

		TryTriggerSpell(E_JudgingTiming.OnAllyFailed);
	}
    public void OnUnderAttack()
    {
        Debug.Log("OnUnderAttack");
		TryTriggerSpell(E_JudgingTiming.OnUnderAttack);
	}
	public void OnGameStart()
	{
		if (IsTestHero)
		{
			ArmySkill armySkill = new ArmySkill();
			armySkill.SkillName = "QuickShot";
			armySkill.skillType = E_SkillType.Passive;
			armySkill.judgeTiming = E_JudgingTiming.OnAlways;
			armySkill.targetType = E_TargetType.Self;
			armySkill.NormalCD = 1f;
			armySkill.TriggerChance = 25;
			ArmySpell spell = new ArmySpell(armySkill);
			spell.spell.skillEffect1 = new SkillEffectData();
			spell.spell.skillEffect1.ID = 10;
			spell.spell.skillEffect1.AffectAttribute =(byte)E_AttributeType.ShaftPunch;
			spell.spell.skillEffect1.AffectValue = 0f;
			spell.spell.skillEffect1.CooldownTime = 5f;
			spell.spell.skillEffect1.Target =(byte)E_TargetType.Self;
			spell.spell.skillEffect1.EffectNameID = "ShaftPunch";
			ArmySkills.Add(spell);
		}
		for (int i = 0; i < ArmySkills.Count; i++)
		{
			if (ArmySkills[i].spell.skillType != E_SkillType.Passive)
			{
				continue;
			}
			if (ArmySkills[i].spell.skillEffect1.ID != 0)
			{
				TriggerBuff(ArmySkills[i].spell.skillEffect1);
			}
			if (ArmySkills[i].spell.skillEffect2.ID != 0)
			{
				TriggerBuff(ArmySkills[i].spell.skillEffect2);
			}
			if (ArmySkills[i].spell.skillEffect3.ID != 0)
			{
				TriggerBuff(ArmySkills[i].spell.skillEffect3);
			}
		}

	}
	public List<ArmySpell> ArmySkills = new List<ArmySpell>();
	public List<Buff> Buffs = new List<Buff>();
	public void ArmySkillUpdateLoop()
	{
		if (ArmySkills.Count >0)
		{
			for (int i = 0; i < ArmySkills.Count; i++)
			{
				ArmySkills[i].CurrentCD -= Time.deltaTime;
				if (ArmySkills[i].CurrentCD <= 0)
				{
					ArmySkills[i].CurrentCD = 0;
				}
			}
		}
		if (Buffs.Count > 0)
		{
			for (int i = 0; i < Buffs.Count; i++)
			{
				if (Buffs[i].skillData.CooldownTime != 0)
				{
					Buffs[i].CD -= Time.deltaTime;
					if (Buffs[i].CD <= 0)
					{
						Buffs[i].CD = 0;
						if (Buffs[i].Effect != null)
						{
							Destroy(Buffs[i].Effect);
						}
						Buffs.RemoveAt(i);
						OnBuffChange();
					}
				}
			}
		}
	}
	public void OnBuffChange()
	{
		leader.HandleBuff(Buffs);
	}
	public void TryTriggerSpell(E_JudgingTiming judgingTiming)
	{
		List<ArmySpell> triggerSkills = ArmySkills.FindAll(x => x.spell.judgeTiming == judgingTiming && x.CurrentCD == 0);
		if (triggerSkills.Count > 0)
		{
			for (int i = 0; i < triggerSkills.Count; i++)
			{
				int rand = Random.Range(0,100);
				if (rand < triggerSkills[i].spell.TriggerChance)
				{
					TriggerSpell(triggerSkills[i].spell);
					triggerSkills[i].CurrentCD = triggerSkills[i].spell.NormalCD;
					return;
				}
			}
		}
	}
	public void TriggerSpell(ArmySkill skillData)
	{
		if (skillData.skillEffect1.ID != 0)
		{
			this.leader.TriggerSkill(skillData.skillEffect1);
		}

		if (skillData.skillEffect2.ID != 0)
		{
			this.leader.TriggerSkill(skillData.skillEffect2);
		}

		if (skillData.skillEffect3.ID != 0)
		{
			this.leader.TriggerSkill(skillData.skillEffect3);
		}
	}
	public void TriggerBuff(SkillEffectData skillData,Leader provider=null)
	{
		if ((E_TargetType)skillData.Target == E_TargetType.Allies && provider==null)
		{
			List<Leader> mates= leader.battleManager.GetTeamates(leader.team.team);
			for (int i = 0; i < mates.Count; i++)
			{
				if (mates[i] == leader)
				{
					continue;
				}
				mates[i].skillTriggerScript.TriggerBuff(skillData,leader);
			}
			provider = this.leader;
		}
		Buff buff = new Buff();
		buff.CD = skillData.CooldownTime;
		buff.skillData = skillData;
		buff.Provider = provider;

		GameObject vfx = (GameObject)Resources.Load("Effects/" + skillData.EffectNameID);
		if (vfx != null)
		{
			vfx = Instantiate(vfx,transform.position,transform.rotation);
			vfx.SetActive(true);
			vfx.transform.SetParent(this.transform,true);
			buff.Effect = vfx;
		}
		Buffs.Add(buff);
		OnBuffChange();
	}
	public bool IsThereBuff(E_AttributeType BuffType)
	{
		List<Buff> availableBuffs = Buffs.FindAll(x => x.skillData.AffectAttribute==(byte)BuffType);
		if (availableBuffs.Count > 0)
		{
			return true;
		}
		return false;
	}
	public void DispelBuff(E_AttributeType BuffType)
	{
		List<Buff> availableBuffs = Buffs.FindAll(x => x.skillData.AffectAttribute == (byte)BuffType);
		for (int i = 0; i < availableBuffs.Count; i++)
		{
			if (availableBuffs[i].Effect != null)
			{
				Destroy(availableBuffs[i].Effect);
			}
			Buffs.Remove(availableBuffs[i]);
		}
		OnBuffChange();
	}
	public void DispelBuffWhenProviderDie(Leader provider)
	{
		List<Buff> availableBuffs = Buffs.FindAll(x => x.Provider==provider);
		for (int i = 0; i < availableBuffs.Count; i++)
		{
			if (availableBuffs[i].Effect != null)
			{
				Destroy(availableBuffs[i].Effect);
			}
			Buffs.Remove(availableBuffs[i]);
		}
		OnBuffChange();
	}
}
[System.Serializable]
public class ArmySpell
{
	public ArmySkill spell;
	public float CurrentCD;
	public ArmySpell(ArmySkill skill)
	{
		spell = skill;
	}
}
[System.Serializable]
public class ArmySkill
{
	public string SkillName;
	public E_SkillType skillType;
	public E_JudgingTiming judgeTiming;
	public E_TargetType targetType;
	public int TriggerChance;
	public float StartCD;
	public float NormalCD;
	public float duration;
	public SkillEffectData skillEffect1;
	public SkillEffectData skillEffect2;
	public SkillEffectData skillEffect3;
}
[System.Serializable]
public class Buff
{
	public SkillEffectData skillData;
	public float CD;
	public Leader Provider;
	public GameObject Effect;
}


