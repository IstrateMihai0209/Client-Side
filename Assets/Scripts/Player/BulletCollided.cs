using UnityEngine;

public class BulletCollided : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "LocalPlayer" && col.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }
}
