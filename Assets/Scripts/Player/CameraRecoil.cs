using UnityEngine;
using System.Collections;

public class CameraRecoil : MonoBehaviour
{
    [Header("Recoil settings")]
    public float rotationSpeed;
    public float returnSpeed;
    public float recoilMultiplier = 50f;

    [Header("Hipfire")]
    public Vector3 recoilRotation;

    private Vector3 currentRotation;
    private Vector3 rotation;

    [Header("Aiming")]
    public Vector3 recoilRotationAiming;

    [Header("Others")]
    private PlayerShoot playerShoot;
    private GameObject lookRoot;
    public bool aiming;

    void Start()
    {
        playerShoot = GameObject.Find("LocalPlayer(Clone)").GetComponent<PlayerShoot>();
        lookRoot = GameObject.Find("Look Root");
    }

    void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.fixedDeltaTime); //return to Vector3.zero when player is not shooting
        rotation = Vector3.Lerp(rotation, currentRotation, rotationSpeed * Time.fixedDeltaTime); //rotate when the currentRotation variable isn't in place
        transform.localRotation = Quaternion.Euler(rotation);

        if (Input.GetButton("Fire1") && playerShoot.canFire)
        {
            Fire(); 
        }

        if (lookRoot.transform.localRotation != Quaternion.Euler(Vector3.zero))
            ClientSend.CameraRecoil(rotation);

    }

    private void Fire()
    {
        //Add force to the rotation
        if (!aiming)
        {
            currentRotation += new Vector3(-recoilRotation.x,
                Random.Range(-recoilRotation.y, recoilRotation.y) * recoilMultiplier * 2f * Time.deltaTime,
                Random.Range(-recoilRotation.z, recoilRotation.z)) * recoilMultiplier * Time.deltaTime;
        }
        else
        {
            currentRotation += new Vector3(-recoilRotationAiming.x,
                Random.Range(-recoilRotationAiming.y, recoilRotationAiming.y) * recoilMultiplier * 2f * Time.deltaTime,
                Random.Range(-recoilRotationAiming.z, recoilRotationAiming.z)) * recoilMultiplier * Time.deltaTime;
        }
    }
}