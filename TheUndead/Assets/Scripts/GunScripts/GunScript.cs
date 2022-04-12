using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

   
    // Start is called before the first frame update
    void Start()
    {
        ammoCountText = (Text)GameObject.FindWithTag("Player Ammo Text HUD").GetComponent<Text>() as Text;
        view = player.GetComponent<PhotonView>();
        currentAmmo = ammoCapacity;
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

    private void updateText()
    {
        if(isReloading) ammoCountText.text = "Ammo: Reloading";
        else ammoCountText.text = "Ammo: " + currentAmmo + "/" + ammoCapacity;
    }

    IEnumerator ReloadTime()
    {
        if (!isReloading) yield break;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        currentAmmo = ammoCapacity;
        updateText();
    }

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
