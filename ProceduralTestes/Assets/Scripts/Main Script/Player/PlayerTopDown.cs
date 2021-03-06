﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main {
    public class PlayerTopDown : MonoBehaviour {
        CharacterController controller;
        SpriteRenderer spriteRenderer;
        Vector2 moveVector;

        float bulletSpawnGap;
        float timeNextShoot;

        public Transform weaponTransform;
        public Transform aim;
        public bool spriteOriginallyFacesLeft = false;
        public float moveSpeed;
        public float bulletSpeed = 3f;
        public float bulletsPerSecond = 2f;

        public GameObject bullet;

        private void Start() {
            controller = GetComponent<CharacterController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            bulletSpawnGap = 1f / bulletsPerSecond;
            timeNextShoot = 0f;
        }

        private void Update() {
            SetHorizontalMovement(PlayerInput.Instance.Horizontal.Value * moveSpeed);
            SetVerticalMovement(PlayerInput.Instance.Vertical.Value * moveSpeed);
            if (CheckForShootInput()) {
                Shoot();
            }
            UpdateAimPosition();
        }

        private bool CheckForGrounded() {
            return controller.IsGrounded;
        }

        private void FixedUpdate() {
            NextMove();
        }
        
        void NextMove() {
            controller.Move(moveVector * Time.deltaTime);
        }

        void SetHorizontalMovement(float horizontalMovement) {
            moveVector.x = horizontalMovement;
        }

        void SetVerticalMovement(float verticalMovement) {
            moveVector.y = verticalMovement;
        }

        void IncrementMoveVector(Vector2 move) {
            moveVector += move;
        }

        void IncrementHorizontalMovement(float horizontalMovement) {
            moveVector.x += horizontalMovement;
        }

        void IncrementVerticalMovement(float verticalMovement) {
            moveVector.y += verticalMovement;
        }

        void UpdateAimPosition() {
            float desiredLocalPosX = PlayerInput.Instance.AimHorizontal.Value;
            float desiredLocalPosY = PlayerInput.Instance.AimVertical.Value;

            if (PlayerInput.Instance.AimHorizontal.RealValue != 0f) {
                desiredLocalPosX = PlayerInput.Instance.AimHorizontal.RealValue;
            }

            if (PlayerInput.Instance.AimVertical.RealValue != 0f) {
                desiredLocalPosY = PlayerInput.Instance.AimVertical.RealValue;
            }

            aim.localPosition = new Vector2(desiredLocalPosX, desiredLocalPosY);

            Vector2 dir = aim.position - weaponTransform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
     
            // if ((angle >= 0.0 && angle <= 90.0) || (angle <= 0.0 && angle >= -90.0))
            weaponTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            UpdateFacing();
        }

        bool CheckForShootInput() {
            return PlayerInput.Instance.Shoot.Held;
        }

        void Shoot() {
            if (Time.time >= timeNextShoot) {
                GameObject bulletInst = Instantiate(bullet, weaponTransform.position, weaponTransform.rotation) as GameObject;
                Rigidbody2D bulletRigidbody = bulletInst.GetComponent<Rigidbody2D>();
                Vector2 dir = aim.position - weaponTransform.position;
                if (dir == Vector2.zero) {
                    dir = Vector2.right * GetFacing();
                }
                bulletRigidbody.velocity = dir.normalized * bulletSpeed;
                timeNextShoot = Time.time + bulletSpawnGap;
            }
        }

        int GetFacing() {
            return spriteRenderer.flipX != spriteOriginallyFacesLeft ? 1 : -1;
        }

        void UpdateFacing() {
            float dir = PlayerInput.Instance.AimHorizontal.Value;
            if (dir != 0f) {
                spriteRenderer.flipX = dir >= 0 ? false : true;
            }
        }
    }
}