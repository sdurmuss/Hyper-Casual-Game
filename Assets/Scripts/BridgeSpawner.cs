using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{
    public GameObject startReferance, endReferance;
    public BoxCollider hiddenPlatform;
    void Start()
    {
        Vector3 direction = endReferance.transform.position - startReferance.transform.position; // vectorun yonu
        float distance = direction.magnitude; // iki nokta aras� uzakl�k
        direction = direction.normalized;   // y�n vekt�r�n� birim vekt�r�n� d�n��t�rd�k. i�lemlerde kullanabilmek i�in.
        hiddenPlatform.transform.forward = direction; //olu�acak platformun y�n�n� belirledik.
        hiddenPlatform.size = new Vector3(hiddenPlatform.size.x, hiddenPlatform.size.y, distance);// platformun uzunlu�u
        hiddenPlatform.transform.position = startReferance.transform.position + (direction * distance / 2) + (new Vector3(0, -direction.z, direction.y) * hiddenPlatform.size.y / 2);// objeyi b�y�t�rken 2 tarafa b�y�yece�i i�in olu�ucak platformun ortas�na getirdik.

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
