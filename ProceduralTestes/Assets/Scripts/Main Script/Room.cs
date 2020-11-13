using UnityEngine;
using System.Collections;

namespace Main {
    public class Room : MonoBehaviour {
        public RoomType roomType;
        
        public void RoomDestruction() {
            Destroy(gameObject);
        }

        public enum RoomType {
            Esquerda = 0,
            Centro = 1,
            Direita = 2,
            Fazio = 3
        }

        public enum RoomSubType {

        }
    }
}