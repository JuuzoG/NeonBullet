using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public enum WeaponType
    {
        Pistol = 0,
        Shotgun = 1,
        Rifle = 2,
        Melee = 3
    }

    [SerializeField] private WeaponType currentWeapon = WeaponType.Pistol;
    private int weaponCount;

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

            case WeaponType.Shotgun:
                EquipShotgun();
                break;

            case WeaponType.Rifle:
                EquipRifle();
                break;

            case WeaponType.Melee:
                EquipMelee();
                break;

            default:
                Debug.LogWarning($"WeaponSelector: No case handled for index {weaponIndex}");
                break;
        }
    }

    private void EquipPistol()
    {
        Debug.Log("Pistol equipped");
    }

    private void EquipShotgun()
    {
        Debug.Log("Shotgun equipped");
    }

    private void EquipRifle()
    {
        Debug.Log("Rifle equipped");
    }

    private void EquipMelee()
    {
        Debug.Log("Melee equipped");
    }
}