using UnityEngine;
using System.Collections;

public class TankAi : MonoBehaviour {
    // General state machine variables
    private GameObject player;
    private Animator animator;
    private Ray ray;
    private RaycastHit hit;
    private float maxDistanceToCheck = 6.0f;
    private float currentDistance;
    private Vector3 checkDirection;

    // Patrol state variables
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    float temp;

    private int currentTarget;
    private float distanceFromTarget;
    private Transform[] waypoints = null;

    private void Awake()
    {
        player = GameObject.FindWithTag("Tree");
        animator = gameObject.GetComponent<Animator>();
        pointA = GameObject.Find("Tree").transform;
        pointB = GameObject.Find("House").transform;
        pointC = GameObject.Find("TreeB").transform;
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        waypoints = new Transform[3] 
        {
            pointA,
            pointB,
            pointC
        };
        currentTarget = 0;
        navMeshAgent.SetDestination(waypoints[currentTarget].position);
    }

    private void FixedUpdate()
    {
        //First we check distance from the player 
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
        animator.SetFloat("distanceFromTree", currentDistance);
        
        //Then we check for visibility
        checkDirection = player.transform.position - transform.position;
        ray = new Ray(transform.position, checkDirection);
        if (Physics.Raycast(ray, out hit, maxDistanceToCheck))
        {
            if (hit.collider.gameObject == player)
            {
                animator.SetBool("isTreeVisible", true);
            }
            else
            {
                animator.SetBool("isTreeVisible", false);
            }
        }
        else
        {
            animator.SetBool("isTreeVisible", false);
        }

        //Lastly, we get the distance to the next waypoint target
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);
        animator.SetFloat("distanceFromWaypoint", distanceFromTarget);
        temp = Random.value;
        animator.SetFloat("num", temp);
    }

    public void SetNextPoint()
    {
        distanceFromTarget = Vector3.Distance(waypoints[currentTarget].position, transform.position);
        if (distanceFromTarget <= .5)
        {
            
            switch (currentTarget)
            {
                case 0:
                    currentTarget = 1;
                    break;
                case 1:
                    if (temp > .5)
                    {
                        currentTarget = 2;
                    }
                    else
                    {
                        currentTarget = 0;
                    }
                    
                    break;
                case 2:
                    currentTarget = 1;
                    break;

            }
            navMeshAgent.SetDestination(waypoints[currentTarget].position);
        }
    }
}
