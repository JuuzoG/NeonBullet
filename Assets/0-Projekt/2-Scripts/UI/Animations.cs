using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    private Animator anim;
    public Image image;
    public Sprite[] sprites;
    public int bitch;

    void Start()
    {
        anim = GetComponent<Animator>();
        //image = GetComponent<Image>();
    }

    public void CursorAnim(string animName)
    {
        if(animName == "Rifle") anim.SetBool(animName,true);
        else anim.SetTrigger(animName);
    }

    public void eventRifle()
    {
        anim.SetBool("Rifle",false);
    }

    void Update()
    {
        bitch=GameManager.instance.WeaponSelect.CurrentWeaponIndex;
        image.sprite = sprites[bitch];
        
    }
}