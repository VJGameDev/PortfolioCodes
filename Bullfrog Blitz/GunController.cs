using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public bool isFiring;

    public BulletController bullet;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;

    public int maxMagCapacity = 10; // Max ammo in magazine
    [SerializeField] private int currentMagAmmo;
    public int maxReserveAmmo = 50; // Max ammo in reserve
    [SerializeField] private int currentReserveAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;

    private Gamemanager _gameManager;
    [SerializeField] private PauseMenu _pauseMenu;
    private int _infiniteAmmo = 9999999;

    [SerializeField] private AudioSource pistolSound;
    [SerializeField] private AudioSource pistolReloadSound;

    // Start is called before the first frame update
    void Start() {
        _gameManager = FindObjectOfType<Gamemanager>();

        // Set Ammo count
        currentMagAmmo = maxMagCapacity;
        currentReserveAmmo = maxReserveAmmo;
    }


    // Update is called once per frame
    void Update() {
        if (_gameManager.InfinityModeActive == true)
        {
            currentMagAmmo = _infiniteAmmo;
        }

        if (isReloading) {
            return;
        }    

        if (currentMagAmmo <= 0) {
            StartCoroutine(Reload());
            return;
        } else if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(Reload());
            return;
        }

        if (isFiring && _pauseMenu.IsPauseActive == false) {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0) {
                pistolSound.Play();
                currentMagAmmo--;
                _gameManager.AmmoText.text = ": " + currentMagAmmo.ToString();
                shotCounter = timeBetweenShots;
                BulletController newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as BulletController;
                newBullet.Speed = bulletSpeed;
            }
        } else {
            shotCounter = 0;
        }
    }

    public void SetAmmo(int newAmmo) {
        currentReserveAmmo = Mathf.Clamp(newAmmo, 0, maxReserveAmmo);
    }

    public int GetMaxAmmo() {
        return maxReserveAmmo;
    }
    IEnumerator Reload() {
        if (currentReserveAmmo <= 0 || currentMagAmmo == maxMagCapacity)
            yield break; // Cannot reload if no reserve ammo or magazine is already full

        isReloading = true;
        pistolReloadSound.Play();

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxMagCapacity - currentMagAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, currentReserveAmmo); // Reload as much ammo as possible
        currentReserveAmmo -= ammoToReload;
        currentMagAmmo += ammoToReload;

        _gameManager.AmmoText.text = ": " + currentMagAmmo.ToString();
        isReloading = false;
    }
}
