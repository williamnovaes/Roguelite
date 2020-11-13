using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Main {
    public class LevelGeneration : MonoBehaviour {
        // public Transform[] borderPositions;
        public GameObject[] rooms;

        // private int direction;
        public bool stopGeneration;
        // private int downCounter;

        // public float moveIncrement;
        // private float timeBtwSpawn;
        // public float startTimeBtwSpawn;

        // public LayerMask whatsIsRoom;

        // private void Start() {
        //     int randStartingPos = 0;

        //     for (int i = 0; i < borderPositions.Length; i++) {

        //         transform.position = borderPositions[i].position;
        //         Instantiate(rooms[1], transform.position, Quaternion.identity);

        //     }
        //     direction = Random.Range(1, 6);
        // }

        // private void Update() {
        //     if (Input.GetKeyDown(KeyCode.Space)) {
        //         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //     }

        //     if (timeBtwSpawn <= 0 && stopGeneration == false) {
        //         Move();
        //         timeBtwSpawn = startTimeBtwSpawn;
        //     } else {
        //         timeBtwSpawn -= Time.deltaTime;
        //     }
        // }

        // private void Move() {
        //     if (direction == 1 || direction == 2) { // move right
        //         if (transform.position.x < 35) {
        //             downCounter = 0;
        //             Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);
        //             transform.position = pos;

        //             int randRoom = Random.Range(1, 4);
        //             Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

        //             //garantir que o level generator nao volte para esquerda pois acabou de vir de la
        //             direction = Random.Range(1, 6);
        //             if (direction == 3) {
        //                 direction = 1;
        //             } else if (direction == 4) {
        //                 direction = 5;
        //             }
        //         } else {
        //             direction = 5;
        //         }
        //     } else if (direction == 3 || direction == 4) { // move left
        //         if (transform.position.x > 5) {
        //             downCounter = 0;
        //             Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
        //             transform.position = pos;

        //             int randRoom = Random.Range(1, 4);
        //             Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

        //             direction = Random.Range(3, 6);
        //         } else {
        //             direction = 5;
        //         }
        //     } else if (direction == 5) {
        //         downCounter++;
        //         if (transform.position.y > -25) {
        //             //agora deve-se alterar a room ANTES de colocar uma room que tem abertura pra baixo, tipo 3 ou 5
        //             Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatsIsRoom);
        //             if (previousRoom != null) {
        //                 if (previousRoom.GetComponent<Room>().roomType != Room.RoomType.Esquerda && previousRoom.GetComponent<Room>().roomType != Room.RoomType.Esquerda) {
        //                     previousRoom.GetComponent<Room>().RoomDestruction();
        //                     if (downCounter >= 2) {
        //                         Instantiate(rooms[4], transform.position, Quaternion.identity);
        //                     } else {
        //                         int randRoomDownOpening = Random.Range(2, 5);
        //                         if (randRoomDownOpening == 3) {
        //                             randRoomDownOpening = 2;
        //                         }
        //                         Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
        //                     }
        //                 }
        //             }

        //             Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrement);
        //             transform.position = pos;

        //             int randRoom = Random.Range(3, 5);
        //             Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

        //             direction = Random.Range(1, 6);
        //         } else {
        //             stopGeneration = true;
        //         }
        //     }
        // }

        public int width = 5, height = 5;
        public Transform[] posicoes;

        private void Start() {
            Iniciar();
        }

        private void Iniciar() {
            // posicoes = new Transform[width * height];

            for (int column = 0; column < width; column++) {
                for (int row = 0; row < height; row++) {
                    int index = column + (row * width);
                    SetTipo(index);
                }
            }
        }

        void SetTipo(int index) {
            Vector3 position = posicoes[index].position;
            Room.RoomType tipo = GetTipo(index % width);
            string log = "";
            switch (tipo) {
                case Room.RoomType.Esquerda:
                    if (index == 0) {
                        // log = "Canto Superior Esquerdo";
                        Instantiate(rooms[0], position, Quaternion.identity);
                    } else if ((width * height) - width == index) {
                        Instantiate(rooms[2], position, Quaternion.identity);
                        // log = "Canto Inferior Esquerdo";
                    } else {
                        Instantiate(rooms[1], position, Quaternion.identity);
                        // log = "Canto Centro Esquerdo";
                    }
                    break;
                case Room.RoomType.Centro:
                    if (width - index > 0) {
                        Instantiate(rooms[3], position, Quaternion.identity);
                        // log = "Centro Superior";
                    } else if (index % width == 0) {
                        Instantiate(rooms[1], position, Quaternion.identity);
                        // log = "Centro Esquerdo";
                    } else if (index % width == width - 1) {
                        Instantiate(rooms[6], position, Quaternion.identity);
                        // log = "Centro Direito";
                    } else if (index > width * height - width) {
                        Instantiate(rooms[4], position, Quaternion.identity);
                        // log = "Centro Baixo";
                    } else {
                        Instantiate(rooms[8], position, Quaternion.identity);
                        // log = "Centro";
                    }
                    break;
                case Room.RoomType.Direita:
                    if (width - 1 == index) {
                        Instantiate(rooms[5], position, Quaternion.identity);
                        // log = "Canto Superior Direito";
                    } else if (width * height - 1 == index) {
                        Instantiate(rooms[7], position, Quaternion.identity);
                        // log = "Canto Inferior Direito";
                    } else {
                        Instantiate(rooms[6], position, Quaternion.identity);
                        // log = "Canto Centro Direito";
                    }
                    break;
                default:
                    log = "Error! Combinacao nao encontrada";
                    break;
            }
            // Debug.Log(log + ", index: " + index);
        }

        public Room.RoomType GetTipo(int mod) {
            Room.RoomType tipo;
            if (mod == 0) {
                tipo = Room.RoomType.Esquerda;
            }
            else if (mod == width - 1) {
                tipo = Room.RoomType.Direita;
            }
            else {
                tipo = Room.RoomType.Centro;
            }
            return tipo;
        }
    }
}