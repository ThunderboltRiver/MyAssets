using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //public FixedJoystick MovejoyStick; //左画面JoyStick
    //public FixedButton SitButton;
    //public FixedTouchField TouchField;
    public EnemyAI Enemy;
    public float speed;
    // [HideInInspector]
    // public bool isSelecting;
    [HideInInspector]
    public GameObject selectingitem;
    [HideInInspector]
    public bool isSitting = false;
    // [HideInInspector]
    // public GameObject PlayerView;
    // [HideInInspector]
    // public Vector3 PlayerView_origin;
    [HideInInspector]
    protected AudioSource TraceSound;
    protected AudioSource FootStep;
    [HideInInspector]
    public RigidbodyFirstPersonController fps;
    private Rigidbody rb;
    private float runStepLengthen = 0.7f;
    private float stepInterval = 10f;
    private float stepCycle;
    private float nextStep;
    private List<string> CollisionObs = new List<string>();
    private Vector3 StepHitPosition;
    private AudioSource[] sounds;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;
    [SerializeField] AudioClip[] clips;


    void Start()
    {
        // PlayerView = gameObject.transform.Find("MainCamera").gameObject;
        // PlayerView_origin = PlayerView.transform.localPosition;
        //TraceSound = PlayerView.GetComponents<AudioSource>()[0];
        sounds = GetComponents<AudioSource>();
        FootStep = sounds[1];
        TraceSound = sounds[2];
        fps = GetComponent<RigidbodyFirstPersonController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProgressStepCycle(speed);
        TraceAudioPlay();
        TraceAudioStop();
    }

    //Model
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("GameOver");
            return;
        }
        string ObjectName = collision.gameObject.name;
        CollisionObs.Add(ObjectName);
    }

    void OnCollisionExit(Collision collision)
    {
        string ObjectName = collision.gameObject.name;
        CollisionObs.Remove(ObjectName);
    }

    //Views
    public void TraceAudioPlay()
    {
        if (!TraceSound.isPlaying && Enemy.traceMode) TraceSound.Play();
    }
    public void TraceAudioStop()
    {
        if (TraceSound.isPlaying && !Enemy.traceMode) TraceSound.Stop();
    }
    void PlayStepSound()
    {
        if (CollisionObs.Count > 0)
        {
            if (randomizePitch)
            {
                FootStep.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);
            }
            FootStep.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }
        // ジャンプ中は足音を発生させない。

        //FootStep.clip = StepSound;
        //FootStep.PlayOneShot(FootStep.clip);
        //FootStep.Play();
    }

    // ★追加（足音）
    // ステップサイクル
    void ProgressStepCycle(float speed)
    {
        if (rb.velocity.sqrMagnitude > 0)
        {

            //stepCycle += (rb.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) * Time.deltaTime;
            stepCycle += rb.velocity.magnitude * speed * Time.deltaTime;
        }

        if (!(stepCycle > nextStep))
        {
            return;
        }
        // 足音の発生間隔・・・＞歩く時は間隔が長い。走る時は短い。
        nextStep = stepCycle + stepInterval;
        PlayStepSound();
    }

    // public void OnTriggerEnter(Collider other)
    // {
    //     switch (other.tag)
    //     {
    //         case "Item":
    //             selectingitem = other.gameObject;
    //             break;
    //     }

    // }

    // public void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Item"))
    //     {
    //         selectingitem = null;
    //     }
    // }

    // public bool RayCastfromCenter(string ObjectName){
    //   Vector2 center = new Vector2 (Screen.width/2, Screen.height/2);
    //   Ray ray = Camera.main.ScreenPointToRay(center);
    //   RaycastHit hit;
    //   return Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == ObjectName;

    // }
}
