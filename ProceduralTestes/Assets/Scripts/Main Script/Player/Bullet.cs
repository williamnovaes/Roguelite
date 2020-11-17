using UnityEngine;

public class Bullet : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.tag.Equals("Player")) {
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }
    }
}