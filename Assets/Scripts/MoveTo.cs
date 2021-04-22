using UnityEngine;
using UnityEngine.AI;

namespace Gemmeleg
{
    public class MoveTo : MonoBehaviour
    {
#pragma warning disable 0649
        [Header("VR")]
        [SerializeField] private Transform goal;
#pragma warning restore 0649

        void Start()
        {
            UnityEngine.AI.NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = goal.position;
        }
    }
}