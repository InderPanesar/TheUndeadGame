using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script which defines the behaviour of the players gun. 
/// </summary>
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

    public int reloadTime = 5;
    public int currentAmmo;
    public int ammoCapacity = 100;


    private float nextGunFireTime = 0f;

    private bool isReloading = false;
    private Text ammoCountText; 

   
    void Start()
    {
        ammoCountText = (Text)GameObject.FindWithTag("Player Ammo Text HUD").GetComponent<Text>() as Text;
        view = player.GetComponent<PhotonView>();
        currentAmmo = ammoCapacity;
    }

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

    /// <summary>
    /// Handles what happens when a gun is shot. 
    /// </summary>
    private void ShootHandler()
    {
       
        if (Input.GetButton("Fire1") && Time.time >= nextGunFireTime && !isReloading)
        {
            nextGunFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
        else
        {
            if (muzzleFlash.isPlaying && Time.time >= nextGunFireTime && !isReloading)
            {
                muzzleFlash.Stop();
            }
        }
    }

    /// <summary>
    /// Handles the animations the raycasts and FX's when a gun is shot. 
    /// </summary>
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
        currentAmmo--;
        updateText();
        if (currentAmmo <= 0)
        {
            isReloading = true;
            updateText();
            muzzleFlash.Stop();
            StartCoroutine(ReloadTime());
        }
    }


    /// <summary>
    /// Updates the ammo count of the user on the UI. 
    /// </summary>
    private void updateText()
    {
        if(isReloading) ammoCountText.text = "Ammo: Reloading";
        else ammoCountText.text = "Ammo: " + currentAmmo + "/" + ammoCapacity;
    }

    /// <summary>
    /// Handles the ammo being reloaded in the game. 
    /// </summary>
    IEnumerator ReloadTime()
    {
        if (!isReloading) yield break;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = ammoCapacity;
        updateText();
    }

    /// <summary>
    /// Method called when a to reload the state of the gun from a Save File. 
    /// </summary>
    public void loadSaveFile(int _currentAmmo, int _ammoCapacity)
    {
        currentAmmo = _currentAmmo;
        ammoCapacity = _ammoCapacity;
        updateText();
        if (currentAmmo <= 0)
        {
            isReloading = true;
            updateText();
            muzzleFlash.Stop();
            StartCoroutine(ReloadTime());
        }
    }

}
