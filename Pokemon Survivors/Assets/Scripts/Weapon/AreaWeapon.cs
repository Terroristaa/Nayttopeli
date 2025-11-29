using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AreaWeapon : Weapon
{
    [SerializeField] private GameObject prefab;
    private float spawnCounter;

    public float baseDuration = 1f;
    public float durationPerLevel = 0.5f;
    public int level = 1;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0) {
            spawnCounter = stats[weaponLevel].cooldown;
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }
    }
}
