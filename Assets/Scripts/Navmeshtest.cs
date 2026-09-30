using UnityEngine;
using UnityEngine.AI;
public class Navmeshtest : MonoBehaviour
{
    private NavMeshAgent agent;
    public  Transform destination;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
