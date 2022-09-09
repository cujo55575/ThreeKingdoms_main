using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MathOperationObject : ScriptableObject
{
	public E_AttributeOwner AttributeOwner;
	public E_AttributeType AttributToModify;
	public E_Sign Modifier;
	public float ModifierValue;
	public E_AffectValueType ModifierType;
	public E_Sign NextModifier;
}
