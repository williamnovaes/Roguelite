using UnityEngine;

namespace Pefrab {
    public class Room : MonoBehaviour {
        public int roomType;

        public void RoomDestruction() {
            Destroy(gameObject);
        }
    }
}