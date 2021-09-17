using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting Variables")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;

    private float timeToFire;

    [Header("Prefabs")] 
    //[SerializeField] private GameObject bulletImpact;
    //[SerializeField] private GameObject bulletHole;
    public GameObject bullet;
    public ParticleSystem muzzleFlash;
    public Transform firePoint;

    [Header("Public animation variables")]
    public bool canFire = true;
    public bool shoot;

    void Update()
    {
        if (muzzleFlash != null)
        {
            //Firing triggers
            bool fire = Input.GetButton("Fire1") && canFire;
            bool stopFiring = Input.GetButtonUp("Fire1") || !canFire;

            if (fire && Time.time >= timeToFire)
            {
                //Shoot at a fire rate
                timeToFire = Time.time + 1 / fireRate;
                shoot = true;
                Shoot();
            }
            else if (stopFiring)
            {
                //Stop the muzzleFlash and the shooting animation when releasing the mouse button
                muzzleFlash.Stop(true);
                shoot = false;
            }
        }
    }

    void Shoot()
    {
        //Create projectiles
        GameObject bulletTrail = Instantiate(bullet, firePoint.position, Quaternion.identity);
        bulletTrail.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * projectileSpeed;

        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play(true);
        }
    }
}