using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    private Animator anim;
    private Image image;
    public Sprite[] sprites;

    void Start()
    {
        GameManager.instance.AnimationCS = this;
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void CursorAnim(string animName)
    {
        if(animName == "Rifle") anim.SetBool(animName,true);
        else anim.SetTrigger(animName);
    }

    void Update()
    {
        image.sprite = sprites[2];
    }
}