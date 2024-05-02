using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasePunchesConfig", menuName = "Combat/Base Punches Config", order = 0)]
public class BasePunchesConfig : ScriptableObject
{
    public float basePunchDamage = 30f;
    public float basePunchCooldown = 0.5f;
    public float basePunchComboCooldown = 2f;
    public float basePunchComboTime = 1f;
    public int basePunchesNumber = 5;
    public float basePunchForce = 20f;
    public bool stopRunning = true;
}
