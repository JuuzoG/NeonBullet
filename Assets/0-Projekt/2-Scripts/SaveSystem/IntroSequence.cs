using TMPro;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    public void Continue()
    {
        SaveManager.instance.Load(SaveManager.instance.currentSlot);
    }
}