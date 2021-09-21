using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float smoothness;

    private Quaternion origin;

    void Start()
    {
        origin = transform.localRotation;
    }

    void FixedUpdate()
    {
        Quaternion XAdjust = Quaternion.AngleAxis(-intensity * Input.GetAxis("Mouse X"), Vector3.up);
        Quaternion YAdjust = Quaternion.AngleAxis(intensity * Input.GetAxis("Mouse Y"), Vector3.right);

        Quaternion targetRotation = origin * XAdjust * YAdjust;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, smoothness * Time.deltaTime);
    }
}
