using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float wanderRange;
    private NavMeshAgent navMeshAgent;
    private NavMeshHit navMeshHit;
    [HideInInspector]
    public bool traceMode = false;
    private RaycastHit hit;
    private float traceStartTime;
    private float tracingTime;
    public float MaxtraceTime = 10.0f;


    void Start() {
    	navMeshAgent = GetComponent < NavMeshAgent > ();
      navMeshAgent.autoBraking = false;

    }


    void FixedUpdate() {
      bool isPathPending = navMeshAgent.pathPending;
      bool hasReached = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
      bool isHavingPath = navMeshAgent.hasPath;

      bool canSetRoot = !isPathPending && hasReached && !isHavingPath;


      if (canSetRoot){
        traceMode = false;
        SetDestination();
      }
    }

    bool rootchecking() {
      bool isPathPending = navMeshAgent.pathPending;
      bool hasReached = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
      bool isHavingPath = navMeshAgent.hasPath;

      bool canSetRoot = !isPathPending && hasReached && !isHavingPath;

      return canSetRoot;
    }

    void SetDestination(){
        //Vector3 randomPos = new Vector3(0, 0, 0);
        Vector3 randomPos = new Vector3(Random.Range( - wanderRange, wanderRange), 0, Random.Range( - wanderRange, wanderRange));
        //SamplePositionは設定した場所から5の範囲で最も近い距離のBakeされた場所を探す。
        Debug.Log(randomPos);
        NavMesh.SamplePosition(randomPos, out navMeshHit, 5, 1);
        Vector3 destination = navMeshHit.position;
        if (destination != Vector3.positiveInfinity & destination != Vector3.negativeInfinity){
          transform.LookAt(destination);
          navMeshAgent.destination = destination;
          Debug.Log("randomPos is" + navMeshAgent.destination);
        }

      }

    void OnTriggerEnter(Collider collider){
      if (collider.gameObject.transform == player){
        if(player_Raycast_hit() || traceMode){
          navMeshAgent.isStopped = true;
          SetTrace();
        }
      }
    }

    void OnTriggerStay(Collider collider){
      if (collider.gameObject.transform == player){
        if(player_Raycast_hit() || traceMode){
          navMeshAgent.isStopped = true;
          SetTrace();
        }
      }
    }

    void OnTriggerExit(Collider collider){
      //if (collider.gameObject.transform == player){
        //playerInView = false;
      //}
      if (collider.gameObject.transform == player){
        if(player_Raycast_hit() || traceMode){
          navMeshAgent.isStopped = true;
          SetTrace();
        }
      }
    }

    bool player_Raycast_hit(){
      var diff = player.position - transform.position;
      var distance = diff.magnitude;
      var direction = diff.normalized;
      return Physics.Raycast(transform.position, direction, out hit, distance) && hit.transform == player;
    }

    void SetTrace(){
      if(!traceMode){
        traceMode = true;
        traceStartTime = Time.time;

      }
      Vector3 playerPos = new Vector3(player.position.x, 0.0f, player.position.z);
      if(navMeshAgent.destination != playerPos){
        transform.LookAt(playerPos);
        navMeshAgent.SetDestination(playerPos);
        navMeshAgent.avoidancePriority = 0;
        navMeshAgent.isStopped = false;
        tracingTime = Time.time;
      }
      TraceStop();
    }

    void TraceStop(){
      if(tracingTime - traceStartTime >= MaxtraceTime){
        traceMode = false;
      }
    }


}
