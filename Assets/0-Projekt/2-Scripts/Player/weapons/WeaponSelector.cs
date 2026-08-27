using Unity.VisualScripting;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol = 0,
        Railgun = 1,
        Rifle = 2,
    }

    [SerializeField] private WeaponType currentWeapon = WeaponType.Pistol;
    public int weaponCount;
    public int CurrentWeaponIndex;

    void Awake()
    {
        GameManager.instance.WeaponSelect = this;
    }

    void Start()
    {
        weaponCount = System.Enum.GetValues(typeof(WeaponType)).Length;
        SelectWeapon((int)currentWeapon);
    }

    void Update()
    {
        if (GameManager.instance.state == GameStates.GameOver) return;
        if (GameManager.instance.state == GameStates.paused) return;
        if (GameManager.instance.state == GameStates.hacking) return;

        CurrentWeaponIndex = (int)currentWeapon;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            int next = ((int)currentWeapon + 1) % weaponCount;
            SelectWeapon(next);
        }
        else if (scroll < 0f)
        {
            int prev = ((int)currentWeapon - 1 + weaponCount) % weaponCount;
            SelectWeapon(prev);
        }
    }

    public void SelectWeapon(int weaponIndex)
    {
        currentWeapon = (WeaponType)weaponIndex;

        switch (currentWeapon)
        {
            case WeaponType.Pistol:
                EquipPistol();
                break;

            case WeaponType.Railgun:
                EquipRailgun();
                break;
            case WeaponType.Rifle:
                EquipRifle();
                break;
        }
    }

    private void EquipPistol()
    {
        Debug.Log("Pistol equipped");
    }

    private void EquipRailgun()
    {
        Debug.Log("Railgun equipped");
    }

    private void EquipRifle()
    {
        Debug.Log("Rifle equipped");
    }
}