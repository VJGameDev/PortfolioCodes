using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour {
    public Image knifeImage;
    public Image pistolImage;
    public Image shotgunImage;
    public Image smgImage;

    public Gamemanager gameManager; // Reference to the GameManager script
    public WeaponManager weaponManager;

    public TextMeshProUGUI[] AmmoTypes;

    private void Update() {
        switch(weaponManager.currentWeapon) {
            case WeaponType.Knife:
                break;
            case WeaponType.Pistol:
                gameManager.AmmoText = AmmoTypes[0];
                break;
            case WeaponType.Shotgun:
                gameManager.AmmoText = AmmoTypes[1];
                break;
            case WeaponType.SMG:
                gameManager.AmmoText = AmmoTypes[2];
                break;
        }
    }

    // Activate the corresponding image and deactivate others based on the weapon type
    public void UpdateWeaponUI(WeaponType newWeaponType) {
        knifeImage.gameObject.SetActive(newWeaponType == WeaponType.Knife);
        pistolImage.gameObject.SetActive(newWeaponType == WeaponType.Pistol);
        shotgunImage.gameObject.SetActive(newWeaponType == WeaponType.Shotgun);
        smgImage.gameObject.SetActive(newWeaponType == WeaponType.SMG);
    }
}
