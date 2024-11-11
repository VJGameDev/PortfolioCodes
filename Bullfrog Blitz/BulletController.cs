using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float Speed;
    public float LifeTime;

    private PlayerHealth _playerHealth;
    private float distanceTraveled;

    public float DistanceTraveled {
        get { return distanceTraveled; }
    }

    void Start() {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update() {
        // Move Bullet forward
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        distanceTraveled += Speed * Time.deltaTime;

        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) {
            Destroy(gameObject);
        }
    }
}
    