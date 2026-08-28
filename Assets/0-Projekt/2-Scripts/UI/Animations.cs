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

    public void CursorAnim(string animName)
    {
        if(animName == "Rifle") anim.SetBool(animName,true);
        else anim.SetTrigger(animName);
    }

    void Update()
    {
        if (Input.GetKeyDown(player.shot)) //Schuss-Animation abspielen
        {
            switch (selectedWeapon.CurrentWeaponIndex)
            {
                case 0:
                    CursorAnim("Pistol");
                    break;
                case 1:
                    CursorAnim("Railgun");
                    break;
                case 2:
                    CursorAnim("Rifle");
                    break;
                default:
                    Debug.Log("Oh shit",this);
                    break;
            }
        }
    }
}