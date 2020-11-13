using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pefrab {
    public class LevelGeneration : MonoBehaviour {
        public Transform[] startingPositions;
        public GameObject[] rooms; //0 -- fechada; 1 -- LR; 2 -- LRB; 3 -- LRT; 4 -- LRBT

        private int direction;
        public bool stopGeneration;
        private int downCounter;

        public float moveIncrement;
        private float timeBtwSpawn;
        public float startTimeBtwSpawn;

        public LayerMask whatsIsRoom;

        private void Start() {
            int randStartingPos = Random.Range(0, startingPositions.Length);
            transform.position = startingPositions[randStartingPos].position;

            Instantiate(rooms[1], transform.position, Quaternion.identity);

            direction = Random.Range(1, 6);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (timeBtwSpawn <= 0 && stopGeneration == false) {
                Move();
                timeBtwSpawn = startTimeBtwSpawn;
            } else {
                timeBtwSpawn -= Time.deltaTime;
            }
        }

        private void Move() {
            if (direction == 1 || direction == 2) { // move right
                if (transform.position.x < 35) {
                    downCounter = 0;
                    Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);
                    transform.position = pos;

                    int randRoom = Random.Range(1, 4);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    //garantir que o level generator nao volte para esquerda pois acabou de vir de la
                    direction = Random.Range(1, 6);
                    if (direction == 3) {
                        direction = 1;
                    } else if (direction == 4) {
                        direction = 5;
                    }
                } else {
                    direction = 5;
                }
            } else if (direction == 3 || direction == 4) { // move left
                if (transform.position.x > 5) {
                    downCounter = 0;
                    Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
                    transform.position = pos;

                    int randRoom = Random.Range(1, 4);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(3, 6);
                } else {
                    direction = 5;
                }
            } else if (direction == 5) {
                downCounter++;
                if (transform.position.y > -25) {
                    //agora deve-se alterar a room ANTES de colocar uma room que tem abertura pra baixo, tipo 3 ou 5
                    // Handles.color = Color.red;
                    // Handles.DrawWireDisc(transform.position, Vector3.back, 1);
                    Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatsIsRoom);
                    if (previousRoom != null) {
                        if (previousRoom.GetComponent<Room>().roomType != 4 && previousRoom.GetComponent<Room>().roomType != 2) {
                            previousRoom.GetComponent<Room>().RoomDestruction();
                            if (downCounter >= 2) {
                                Instantiate(rooms[4], transform.position, Quaternion.identity);
                            } else {
                                int randRoomDownOpening = Random.Range(2, 5);
                                if (randRoomDownOpening == 3) {
                                    randRoomDownOpening = 2;
                                }
                                Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
                            }
                        }
                    }

                    Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrement);
                    transform.position = pos;

                    int randRoom = Random.Range(3, 5);
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                    direction = Random.Range(1, 6);
                } else {
                    stopGeneration = true;
                }
            }
        }
    }
}