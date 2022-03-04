using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviourPunCallbacks
{
    public float damage = 1f;
    public float bulletRange = 100f;
    public float fireRate = 15f;
    public Camera playerFPSCamera;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public AudioSource machineGunNoiseEnds;
    public GameObject player;

    public bool isSinglePlayerOverride = false;

    private PhotonView view;



    private float nextGunFireTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        view = player.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view == null && isSinglePlayerOverride)
        {
            ShootHandler();
        }
        else
        {
            if (view.IsMine)
            {
                ShootHandler();
            }
        }


    }
    private void ShootHandler()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextGunFireTime)
        {
            nextGunFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
        else
        {
            if (muzzleFlash.isPlaying && Time.time >= nextGunFireTime)
            {
                muzzleFlash.Stop();
            }
        }
    }

    private void Shoot()
    {
        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        if (!machineGunNoiseEnds.isPlaying)
        {
            machineGunNoiseEnds.Play();
        }
        RaycastHit hit;
        if(Physics.Raycast(playerFPSCamera.transform.position, playerFPSCamera.transform.forward, out hit, bulletRange))
        {
            //If element is hit.
            RaycastTargetScript target = hit.transform.GetComponent<RaycastTargetScript>();
            if(target != null)
            {
                target.takeDamage(damage, isSinglePlayerOverride);
            }

            GameObject impactEffectInstance = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactEffectInstance, 0.5f);
        }

    }

}
