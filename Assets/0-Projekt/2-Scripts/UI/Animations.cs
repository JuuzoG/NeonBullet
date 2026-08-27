using UnityEngine;

public class Animations : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void CursorAnim(string animName)
    {
        anim.SetTrigger(animName);
    }
}
