using UnityEngine;

public class camerajuice : MonoBehaviour
{
    public Transform spielerTransform; 

    public float neigStaerke = 2f;      
    public float neigGeschwindigkeit = 5f;       

    public float verschiebeStaerke = 0.5f;   
    public float verschiebeGeschwindigkeit = 4f;      

    private Vector3 letzteSpielerPosition;
    private float aktuelleDrehungZ = 0f;

    void Start()
    {
        if (spielerTransform == null && GameManager.instance != null && GameManager.instance.player != null)
        {
            spielerTransform = GameManager.instance.player.transform;
        }

        if (spielerTransform != null)
        {
            letzteSpielerPosition = spielerTransform.position;
        }
    }

    void LateUpdate()
    {
        if (spielerTransform == null) return;

        if (GameManager.instance != null)
        {
            if (GameManager.instance.state == GameStates.paused || GameManager.instance.state == GameStates.GameOver)
            {
                letzteSpielerPosition = spielerTransform.position;
                return; 
            }
        }

        Vector3 spielerBewegung = spielerTransform.position - letzteSpielerPosition;

        Vector3 lokaleBewegung = transform.InverseTransformDirection(spielerBewegung);


        float zielDrehungZ = -lokaleBewegung.x * neigStaerke * 10f;
        
        aktuelleDrehungZ = Mathf.Lerp(aktuelleDrehungZ, zielDrehungZ, Time.deltaTime * neigGeschwindigkeit);

        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, aktuelleDrehungZ);


        Vector3 zielVerschiebung = -spielerBewegung * verschiebeStaerke;
        
        transform.position = Vector3.Lerp(transform.position, transform.position + zielVerschiebung, Time.deltaTime * verschiebeGeschwindigkeit);


        
        letzteSpielerPosition = spielerTransform.position;
    }
}