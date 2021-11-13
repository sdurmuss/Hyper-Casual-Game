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
        float distance = direction.magnitude; // iki nokta arasý uzaklýk
        direction = direction.normalized;   // yön vektörünü birim vektörünü dönüþtürdük. iþlemlerde kullanabilmek için.
        hiddenPlatform.transform.forward = direction; //oluþacak platformun yönünü belirledik.
        hiddenPlatform.size = new Vector3(hiddenPlatform.size.x, hiddenPlatform.size.y, distance);// platformun uzunluðu
        hiddenPlatform.transform.position = startReferance.transform.position + (direction * distance / 2) + (new Vector3(0, -direction.z, direction.y) * hiddenPlatform.size.y / 2);// objeyi büyütürken 2 tarafa büyüyeceði için oluþucak platformun ortasýna getirdik.

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
