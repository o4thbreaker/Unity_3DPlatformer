using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class JarType : ScriptableObject
{
    private bool isAbilityAllowed;
    public Collectible.CollectibleType collectibleType;
    public bool IsAbilityAllowed { get { return isAbilityAllowed; } set { isAbilityAllowed = value; } }
}
