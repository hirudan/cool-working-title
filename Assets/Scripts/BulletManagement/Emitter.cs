﻿using UnityEngine;

/**
 * Emits Bullet types. Each Emitter dictates the bullet speed.
 * Each bullet is responsible for knowing what speed it should go
 * as each GameObject must be updated by Unity so this is the most effective.
 */
public class Emitter : MonoBehaviour
{
    // We encode speed as a Vector so that some bullets are faster
    // one way than another.
    public Vector3 bulletSpeedMultiplier = new Vector3(1, 1, 1);
    public BulletPattern bulletPattern;
    public GameObject bulletPrefab;

    // Number of bullets to emit each emit cycle.
    public int emitBulletCount;

    // Number of seconds to wait until next emit
    // set to 0 to emit on instantiation.
    public float emitFrequency;

    // Bullet decay, can be set to 0f for no decay
    public float bulletDecayTime = 0f;

    // Generates bullets
    // Pattern is defined by bulletPattern
    void EmitBullets() {
        // Instantiate the bulletPrefab at emitter position
        for (int id = 0; id < this.emitBulletCount; ++id)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            bullet.SetData(this, this.bulletPattern, id);
            bullet.decayTime = bulletDecayTime;
        }
    }

    void Start()
    {
        if (emitFrequency != 0)
        {
            InvokeRepeating("EmitBullets", this.emitFrequency, this.emitFrequency);
        }
        EmitBullets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}