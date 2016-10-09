using UnityEngine;
using System.Collections;
[CreateAssetMenu(fileName = "Mod", menuName = "Weapon/Mod", order = 1)]
public class WeaponModification : ScriptableObject {

	public enum weaponType {GATLING, SHOT, RAIL, SAW};
	public weaponType myType;

	public float fireRateMod, damageMod;
}
