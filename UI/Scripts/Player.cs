using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    //public FixedJoystick MovejoyStick; //左画面JoyStick
    public InputGame TouchRun;
    //public FixedButton SitButton;
    //public FixedTouchField TouchField;
    public EnemyAI Enemy;
    public float speed;
    public Take take;
    [HideInInspector]
    public bool isSelecting;
    [HideInInspector]
    public GameObject PlayerView;
    [HideInInspector]
    public Vector3 PlayerView_origin;
    [HideInInspector]
    public RigidbodyFirstPersonController fps;
    protected AudioSource TraceSound;
    protected AudioSource FootStep;
    private Rigidbody rb;
    private float runStepLengthen = 0.7f;
    private float stepInterval = 10f;
    private float stepCycle;
    private float nextStep;
    private List<string> CollisionObs = new List<string>();
    private Vector3 StepHitPosition;
    [SerializeField] AudioClip[] clips;
    [SerializeField] bool randomizePitch = true;
    [SerializeField] float pitchRange = 0.1f;


    //[SerializeField] AudioClip[] clips;


    void Start()
    {
        PlayerView = gameObject.transform.Find("MainCamera").gameObject;
        PlayerView_origin = PlayerView.transform.localPosition;
        TraceSound = PlayerView.GetComponents<AudioSource>()[0];
        rb = GetComponent<Rigidbody>();
        FootStep = GetComponents<AudioSource>()[1];
        fps = GetComponent<RigidbodyFirstPersonController>(); // これをスタートに
    }

    void Update()
    {
        //var fps = GetComponent<RigidbodyFirstPersonController>(); // これをスタートに
        //fps.RunAxis = TouchRun.Direction;
        //fps.mouseLook.LookAxis = TouchField.TouchDist;
        ProgressStepCycle(speed);
        TraceAudioPlay();
        TraceAudioStop();
        //transform.position += (isStep && TouchRun.Pressed ? 1f : 0f)*Stepheight*Vector3.up;


    }

    void OnCollisionEnter(Collision collision)
    {
        string ObjectName = collision.gameObject.name;
        CollisionObs.Add(ObjectName);
        if(ObjectName == "enemy"){
            SceneManager.LoadScene("GameOver");
          }
    }

    void OnCollisionExit(Collision collision)
    {
      string ObjectName = collision.gameObject.name;
      CollisionObs.Remove(ObjectName);
    }
    //public void Sitting(){
      //if (SitButton.Pressed){
        //PlayerView.transform.localPosition = Vector3.zero;
        //Debug.Log("PlayerView:" + PlayerView.transform.localPosition);
      //}
      //else{
        //Debug.Log("Sitting else");
        //PlayerView.transform.localPosition = PlayerView_origin;
      //}
    //}

    public void TraceAudioPlay(){
      if (!TraceSound.isPlaying){
        if(Enemy && Enemy.traceMode){
          TraceSound.Play();
        }
      }
    }
    public void TraceAudioStop(){
      if (TraceSound.isPlaying){
        if(!Enemy.traceMode){
          TraceSound.Stop();
        }
      }
    }
    void PlayStepSound()
    {
        if(CollisionObs.Count > 0){
          if(randomizePitch){
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
        if(rb.velocity.sqrMagnitude > 0)
        {
            // 三項演算子（プログラミングテクニック）
            //stepCycle += (rb.velocity.magnitude + (speed * (isWalking ? 1f : runStepLengthen))) * Time.deltaTime;
            stepCycle += rb.velocity.magnitude * speed *  Time.deltaTime ;
        }

        if(!(stepCycle > nextStep))
        {
            return;
        }
        // 足音の発生間隔・・・＞歩く時は間隔が長い。走る時は短い。
        nextStep = stepCycle + stepInterval;
        PlayStepSound();
    }

    public void OnTriggerEnter(Collider other){
      switch(other.tag){
        case "Item":
          isSelecting = true;
          take.item = other.gameObject;
          break;
      }

    }

    public void OnTriggerExit(Collider other){
      isSelecting = false;

    }

    public bool RayCastfromCenter(string ObjectName){
      Vector2 center = new Vector2 (Screen.width/2, Screen.height/2);
      Ray ray = Camera.main.ScreenPointToRay(center);
      RaycastHit hit;
      return Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == ObjectName;

    }
}
