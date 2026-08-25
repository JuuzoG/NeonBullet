using System.Collections;
using UnityEngine;

public class Mesh_Trail : MonoBehaviour
{
    public float activeTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;

    private bool isTrailActive;
    void Update()
    {
        if(Input.GetKeyDown (KeyCode.Space) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
        
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive  > 0)
        {
            timeActive -= meshRefreshRate;
            yield return new WaitForSeconds(meshRefreshRate);

        }
    }
}
