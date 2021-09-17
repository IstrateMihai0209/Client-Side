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
            if (carbine && !carbineObj.activeSelf)
            {
                ClientSend.CurrentWeapon(0);
            }

            if (smg && !smgObj.activeSelf)
            {
                ClientSend.CurrentWeapon(1);
            }
        }
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
