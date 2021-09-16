using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private GameObject carbineObj;
    [SerializeField] private GameObject smgObj;

    void Start()
    {
        carbineObj.SetActive(false);
        smgObj.SetActive(false);
    }

    void Update()
    {
        //Listen for input
        bool carbine = Input.GetKeyDown("1");
        bool smg = Input.GetKeyDown("2");

        //Swap between weapons
        if (smgObj != null && carbineObj != null)
        {
            if (smg && !smgObj.activeSelf)
            {
                ActivateWeapon(smgObj);
            }

            if (carbine && !carbineObj.activeSelf)
            {
                ActivateWeapon(carbineObj);
            }
        }
    }

    void ActivateWeapon(GameObject _weapon)
    {
        //Deactivate other weapons
        carbineObj.SetActive(false);
        carbineObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);

        smgObj.SetActive(false);
        smgObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);

        //Activate chosen weapon
        _weapon.SetActive(true);
    }

    /*void ActivateSmg()
    {
        smgObj.SetActive(true);

        carbineObj.SetActive(false);
        carbineObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);
    }

    void ActivateCarbine()
    {
        carbineObj.SetActive(true);

        smgObj.SetActive(false);
        smgObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);
    }*/
}
