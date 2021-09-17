using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public MeshRenderer model;

    [SerializeField] private GameObject carbineObj;
    [SerializeField] private GameObject smgObj;
    [SerializeField] private GameObject weaponsObj;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
    }

    public void ActivateWeapon(int _weaponId)
    {
        //Deactivate other weapons
        carbineObj.SetActive(false);
        carbineObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);

        smgObj.SetActive(false);
        smgObj.transform.localPosition = new Vector3(0.1f, -0.24f, 0.3f);

        //Activate chosen weapon
        GameObject weapon;

        switch(_weaponId)
        {
            case 0:
                weapon = carbineObj;
                break;
            case 1:
                weapon = smgObj;
                break;
            default:
                weapon = carbineObj;
                break;
        }

        weapon.SetActive(true);
    }

    public void WeaponRotation(Quaternion _rotation)
    {
        weaponsObj.transform.localRotation = _rotation;
    }
}