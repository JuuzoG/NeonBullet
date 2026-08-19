using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CursorAnim()
    {
        anim.SetBool("shot",true);
    }

    public void CursorEvent()
    {
        anim.SetBool("shot",false);
    }
}
