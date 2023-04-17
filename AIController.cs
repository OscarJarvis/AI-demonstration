using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public Transform otherPlayer;

    public float timer, wanderTime;

    public enum State
    {
        FindCharacter,
        Wander,
        Idle
    }

    private State currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = State.FindCharacter;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.FindCharacter:
                
                SetAITargetLocation(otherPlayer.position);

                if (_navMeshAgent.remainingDistance < 1f && _navMeshAgent.remainingDistance > 0.5f)
                {
                    currentState = State.Wander;
                }

                break;
            
            case State.Wander:
                
                timer += Time.deltaTime;
                Wander();

                break;
            
            case State.Idle:
                
                SetAITargetLocation(transform.position);

                break;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // SetAITargetLocation(hit.point);
            }
        }

        
    }
    
    //Look! a new comment!

    private void SetAITargetLocation(Vector3 targetLocation)
    {
        _navMeshAgent.SetDestination(targetLocation);
    }


    private void Wander()
    {
        if (timer >= wanderTime)
        {
            Vector2 wanderTarget = Random.insideUnitCircle * wanderTime;
            Vector3 wanderPos3D = new Vector3(transform.position.x + wanderTarget.x, transform.position.y,
                transform.position.z + wanderTarget.y);
            SetAITargetLocation(wanderPos3D);
            timer = 0;
        }
    }

    public void SetState(string newState)
    {
        currentState = (State) Enum.Parse(typeof(State), newState);
    }
}