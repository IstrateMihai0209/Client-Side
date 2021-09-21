using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Reference Points")]
    [SerializeField] private Transform gunPosition;

    [Header("Speed settings")]
    [SerializeField] private float positionalRecoilSpeed;
    [SerializeField] private float rotationalRecoilSpeed;

    [SerializeField] private float positionalReturnSpeed;
    [SerializeField] private float rotationalReturnSpeed;

    [Header("Amount settings")]
    [SerializeField] private Vector3 recoilRotation;
    [SerializeField] private Vector3 recoilKickback;

    [SerializeField] private Vector3 recoilRotationAim;
    [SerializeField] private Vector3 recoilKickbackAim;

    private Vector3 rotationalRecoil;
    private Vector3 positionalRecoil;
    private Vector3 rotation;
    private float recoilMultiplier = 150f;

    private PlayerShoot playerShoot;

    void Start()
    {
        playerShoot = GameObject.Find("LocalPlayer(Clone)").GetComponent<PlayerShoot>();
    }

    void FixedUpdate()
    {
        //Returning to default position
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.fixedDeltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.fixedDeltaTime);

        gunPosition.localPosition = Vector3.Lerp(gunPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        rotation = Vector3.Lerp(rotation, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        gunPosition.localRotation = Quaternion.Euler(rotation);

        if (Input.GetButton("Fire1") && playerShoot.canFire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        //Add force
        rotationalRecoil += new Vector3(-recoilRotationAim.x, Random.Range(-recoilRotationAim.y, recoilRotationAim.y), Random.Range(-recoilRotationAim.z, recoilRotationAim.z)) * recoilMultiplier * Time.deltaTime;
        positionalRecoil += new Vector3(Random.Range(-recoilKickbackAim.x, recoilKickbackAim.x), Random.Range(-recoilKickbackAim.y, recoilKickbackAim.y), recoilKickbackAim.z) * recoilMultiplier * Time.deltaTime;
    }
}
