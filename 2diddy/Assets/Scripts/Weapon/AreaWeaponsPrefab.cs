using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AreaWeaponsPrefab : MonoBehaviour
{
    public AreaWeapon weapon;
    private Vector2 targetSize;
    private float timer;
    public List<Enemy> enemiesInRange;
    private float counter;



    void Start()
    {
        weapon = GameObject.Find("Area weapon").GetComponent<AreaWeapon>();
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector3.zero;
        timer = weapon.stats[weapon.weaponLevel].duration; //timer from this file and duration from areaweapon file
    }


    void Update()
    {
        // grow and shrink towards targetSize
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 5);//animation speed just time it by whatever you want
        // shrink and only then destroy
        timer -= Time.deltaTime;
        if (timer <= 0) {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f) {
                Destroy(gameObject);
            }
        }
        counter -= Time.deltaTime;
        if (counter <= 0) {
            counter = weapon.stats[weapon.weaponLevel].speed;
            for (int i = 0; i < enemiesInRange.Count; i++) {
                enemiesInRange[i].TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy")) {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (!enemiesInRange.Contains(enemy)) {
                enemiesInRange.Add(enemy);
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel]. damage); // Immediate damage on entry
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            enemiesInRange.Remove(collider.GetComponent<Enemy>());
        }
    }
}

