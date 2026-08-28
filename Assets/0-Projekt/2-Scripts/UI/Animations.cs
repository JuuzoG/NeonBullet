using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        GameManager.instance.AnimationCS = this;
        anim = GetComponent<Animator>();
    }

    public void CursorAnim(string animName)
    {
        if(animName == "Rifle") anim.SetBool(animName,true);
        else anim.SetTrigger(animName);
    }
}