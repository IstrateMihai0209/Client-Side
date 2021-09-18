using UnityEngine;

public class GunManager : MonoBehaviour
{
    [Header("Components and variables references")]
    private Animator animator;
    private PlayerController playerComponent;
    private PlayerShoot playerShootComponent;
    private CameraRecoil camRecoil;
    [SerializeField] private GameObject crosshair;
    private AdjustCrosshair adjustCrosshair;
    private float playerSpeed;

    [Header("Hashes")]
    private int aimHash;
    private int sprintOrVaultHash;
    private int shootHash;

    [Header("Recoil values for every weapon")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private Vector3 recoilRotation;
    [SerializeField] private Vector3 recoilRotationAiming;
    [SerializeField] private float crosshairOffset;

    [Header("Shooting values for every weapon")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform firePoint;

    void Start()
    {
        //Initialize components and variables
        animator = GetComponent<Animator>();
        aimHash = Animator.StringToHash("aim");
        sprintOrVaultHash = Animator.StringToHash("sprintOrVault");
        shootHash = Animator.StringToHash("shoot");
        GameObject player = GameObject.Find("LocalPlayer(Clone)");
        playerComponent = player.GetComponent<PlayerController>();
        playerShootComponent = player.GetComponent<PlayerShoot>();

        playerShootComponent.bullet = bullet;
        playerShootComponent.muzzleFlash = muzzleFlash;
        playerShootComponent.firePoint = firePoint;

        GameObject lookRoot = GameObject.Find("Look Root");
        camRecoil = lookRoot.GetComponent<CameraRecoil>();

        camRecoil.rotationSpeed = rotationSpeed;
        camRecoil.returnSpeed = returnSpeed;
        camRecoil.recoilRotation = recoilRotation;
        camRecoil.recoilRotationAiming = recoilRotationAiming;
    }

    void OnEnable()
    {
        //This function will be called every time the weapon is swapped to change to the weapon's own values
        adjustCrosshair = crosshair.GetComponent<AdjustCrosshair>();

        if (playerShootComponent != null)
        {
            playerShootComponent.bullet = bullet;
            playerShootComponent.muzzleFlash = muzzleFlash;
            playerShootComponent.firePoint = firePoint;
        }

        if (camRecoil != null)
        {
            camRecoil.rotationSpeed = rotationSpeed;
            camRecoil.returnSpeed = returnSpeed;
            camRecoil.recoilRotation = recoilRotation;
            camRecoil.recoilRotationAiming = recoilRotationAiming;
        }

        adjustCrosshair.offset = crosshairOffset;
    }

    void Update()
    {
        //Handling Animations
        /*bool aim = Input.GetButton("Fire2");
        playerSpeed = playerComponent.speed;

        //Aiming Animation
        if (aim && !playerComponent.sprinting)
        {
            animator.SetBool(aimHash, true);
            //camRecoil.aiming = true;
        }
        else
        {
            animator.SetBool(aimHash, false);
            //camRecoil.aiming = false;
        }

        //Holster Animation
        if (playerSpeed > 13f || playerComponent.vaultState)
        {
            animator.SetBool(sprintOrVaultHash, true);
            playerShootComponent.canFire = false;
        }
        else
        {
            animator.SetBool(sprintOrVaultHash, false);

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                playerShootComponent.canFire = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Pickup"))
            {
                playerShootComponent.canFire = false;
            }
        }

        //Shooting Animation
        /*if (playerShootComponent.shoot)
            animator.SetBool(shootHash, true);
        else
            animator.SetBool(shootHash, false);*/

        //Multiply the recoil even further while moving
        if (playerComponent.moving)
            camRecoil.recoilMultiplier = 85f;
        else
            camRecoil.recoilMultiplier = 50f;
    }
}