using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main {
    public class SpawnRooms : MonoBehaviour {

        public LayerMask whatIsRoom;
        public LevelGeneration levelGen;

        void Update() {
            if (!levelGen.stopGeneration) return;
            
            Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
            if (roomDetection == null) {
                int rand = Random.Range(0, levelGen.rooms.Length);
                Instantiate(levelGen.rooms[rand], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}