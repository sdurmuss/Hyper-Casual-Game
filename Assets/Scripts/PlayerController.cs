using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    public float currentRunningSpeed = 5f;
    float maxRunningSpeed = 5f;
    public float xSpeed;
    float xClamp = 0;
    float touchXDelta = 0;

    public GameObject ridingCylinder;
    public List<RidingCylinder> listCylinders;
    public Animator animator;
    public AudioSource cylinderAudioSound, triggerAudioSource, itemAudioSource;
    public AudioClip gatherAudioClip, dropAudioClip, coinAudioClip, buyAudioClip, equipAudioClip, unequipAudioClip;

    private float dropSoundTimer;
    private float lastTouchedX;
    private float scoreTimer = 0;
    private bool finished;
    private bool spawningBridge;
    public GameObject bridgePiecePrefab;
    private BridgeSpawner bridgeSpawner;
    private float creatingBridgeTimer;

    public List<GameObject> wearSpots;

    private void Start()
    {
        current = this;
    }
    void Update()
    {
        if (LevelController.current == null || !LevelController.current.gameActive)
        {
            return;
        }
        if (Input.touchCount > 0)// touchCount telefona dokundumu diye kontrol eder ve parmaðýný sürüklüyorsa demek. ilk dokunuþ da denilebilir.
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lastTouchedX = Input.GetTouch(0).position.x;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchXDelta = 5 * (Input.GetTouch(0).position.x - lastTouchedX) / Screen.width;//delta position kullanýcýnýn ekraný ne kadar kaydýrdýgýný kontrol eder.ilk nokta ile son noktaya bakarak.Delta fark demek.
                lastTouchedX = Input.GetTouch(0).position.x;
            }// 5 ile çarpmamýzýn sebebi daha hassas dokunuþ için.
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }

        xClamp = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        Vector3 movementVector = new Vector3(Mathf.Clamp(xClamp, -3.8f, 3.8f), transform.position.y, transform.position.z + currentRunningSpeed * Time.deltaTime);
        transform.position = movementVector;

        if (spawningBridge)
        {
            PlayDropSound();
            creatingBridgeTimer -= Time.deltaTime;
            if (creatingBridgeTimer < 0) 
            {
                creatingBridgeTimer = 0.01f;
                IncrementCylinderVolumeP(-0.01f);
                GameObject createdBridgePiece = Instantiate(bridgePiecePrefab, this.transform);
                createdBridgePiece.transform.SetParent(null);
                Vector3 direction = bridgeSpawner.endReferance.transform.position - bridgeSpawner.startReferance.transform.position; 
                float distance = direction.magnitude; 
                direction = direction.normalized;
                createdBridgePiece.transform.forward = direction;
                float characterDistance = transform.position.z - bridgeSpawner.startReferance.transform.position.z;
                characterDistance = Mathf.Clamp(characterDistance, 0, distance);
                Vector3 newPiecePosition = bridgeSpawner.startReferance.transform.position + direction * characterDistance; //oluþan parça uzaklýðý
                newPiecePosition.x = transform.position.x;
                createdBridgePiece.transform.position = newPiecePosition;
                if (finished)
                {
                    scoreTimer -= Time.deltaTime;
                    if (scoreTimer < 0)
                    {
                        scoreTimer = 0.1f;
                        LevelController.current.ChangeScore(1);
                    }
                }
            }
        }
    }
    public void ChangeSpeed(float value)
    {
        currentRunningSpeed = value;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AddCylinder")
        {
            cylinderAudioSound.PlayOneShot(gatherAudioClip, 0.3f);
            IncrementCylinderVolumeP(0.1f);
            Destroy(other.gameObject);
        }
        else if (other.tag == "StartBridgeSpawn")
        {
            StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());//köprü yaratma fonk
        }
        else if (other.tag == "StopBridgeSpawn")
        {
            StopSpawningBridge();
            if (finished)
            {
                LevelController.current.FinishGame();
            }
        }
        else if (other.tag == "Finish")
        {
            finished = true;
            StartSpawningBridge(other.transform.parent.GetComponent<BridgeSpawner>());
        }
        else if (other.tag == "Coin")
        {
            triggerAudioSource.PlayOneShot(coinAudioClip, 0.3f);
            other.tag = "Untagged";
            LevelController.current.ChangeScore(5);
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (LevelController.current.gameActive)
        {
            if (other.tag == "Trap")
            {
                PlayDropSound();
                IncrementCylinderVolumeP(-Time.fixedDeltaTime);
            }
        }        
    }
    public void IncrementCylinderVolumeP(float value)
    {
        if (listCylinders.Count == 0)
        {
            if (value > 0)
            {
                CreateCylinder(value);
            }
            else
            {
                if (finished)
                {
                    LevelController.current.FinishGame();
                }
                else
                {
                    Die();
                }
            }
        }
        else
        {
            listCylinders[listCylinders.Count - 1].IncrementCylinderVolumeR(value);
        }
    }
    public void Die()
    {
        animator.SetBool("dead", true);
        gameObject.layer = 6;
        Camera.main.transform.SetParent(null);
        LevelController.current.GameOver();
    }
    public void CreateCylinder(float value)
    {
        RidingCylinder createdCylinder = Instantiate(ridingCylinder, transform).GetComponent<RidingCylinder>();// intantiate ye transfor yapmak parentýna child yapmak oluyor.
        listCylinders.Add(createdCylinder);
        createdCylinder.IncrementCylinderVolumeR(value);
    }
    public void DestroyCylinder(RidingCylinder cylinder)
    {
        listCylinders.Remove(cylinder);
        Destroy(cylinder.gameObject);
    }

    public void StartSpawningBridge(BridgeSpawner spawner)
    {
        bridgeSpawner = spawner;
        spawningBridge = true;
    }
    public void StopSpawningBridge()
    {
        spawningBridge = false;
    }
    public void PlayDropSound()
    {
        dropSoundTimer -= Time.deltaTime;
        if (dropSoundTimer < 0)//Ayný anda çalýp ses karýþýklýðý yaramasýn diye.
        {
            dropSoundTimer = 0.15f;
            cylinderAudioSound.PlayOneShot(dropAudioClip, 0.3f);
        }
    }
}
