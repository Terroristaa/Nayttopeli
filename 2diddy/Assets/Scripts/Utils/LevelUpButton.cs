using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButton : MonoBehaviour
{
    public TMP_Text weaponName;
    public TMP_Text weaponDescription;
    public Image weaponIcon;

    private Weapon assignedWeapon;

    public void ActivateButton(Weapon weapon) {
        if (weapon.gameObject.activeSelf == true) {
            weaponName.text = weapon.name;
            weaponDescription.text = weapon.stats[weapon.weaponLevel].description;
        } else {
            weaponName.text = "NEW " + weapon.name;
            weaponDescription.text = weapon.basicDescription;
        }

        weaponIcon.sprite = weapon.weaponImage;

        assignedWeapon = weapon;
    }

    public void SelectUpgrade()
    {
        assignedWeapon.LevelUp();
        UIController.Instance.LevelUpPanelClose();
        AudioController.Instance.PlaySound(AudioController.Instance.selectUpgrade);

    }
} 