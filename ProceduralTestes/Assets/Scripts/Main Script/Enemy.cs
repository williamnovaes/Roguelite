using UnityEngine;
using UnityEngine.AI;

namespace Main {
    public class Enemy : MonoBehaviour {
        NavMeshAgent agent;
        public Transform target;

        private void Start() {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        private void Update() {
            agent.SetDestination(target.position);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.tag.Equals("Bullet")) {
                Destroy(gameObject);
            }
        }
    }
}