using System.Reflection;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    private Animator animator;
    public void AnimEnd()
    {
        Destroy(gameObject);
    }
}
