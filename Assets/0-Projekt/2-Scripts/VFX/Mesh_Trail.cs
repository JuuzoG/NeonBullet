using System.Collections;
using UnityEngine;

public class Mesh_Trail : MonoBehaviour
{
    public float activeTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    private Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    private string shaderVarRef = "_Alpha";
    private float shaderVarRate = 0.5f;
    private float shaderVarRefreshRate = 0.5f;

    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    void Start()
    {
        positionToSpawn = GetComponent<Transform>();
    }

    void Update()
    {
       
        
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        
        
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive  > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int i = 0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

               MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
               MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimatedMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }


            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }
    IEnumerator AnimatedMaterialFloat (Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
