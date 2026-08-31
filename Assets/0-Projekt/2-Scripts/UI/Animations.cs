using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    private Animator anim;
    private Player player;
    private WeaponSelector selectedWeapon;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.instance.player;
        selectedWeapon = GameManager.instance.WeaponSelect;
    }

    void Update()
    {
        if (Input.GetKeyDown(player.shot))
        {
            switch (selectedWeapon.CurrentWeaponIndex)
            {
                case 0:
                    anim.SetTrigger("Pistol");
                    break;
                case 1:
                    anim.SetTrigger("Railgun");
                    break;
                case 2:
                    anim.SetTrigger("Rifle");
                    break;
                default:
                    Debug.Log("Oh shit",this);
                    break;
            }
        }
    }
}