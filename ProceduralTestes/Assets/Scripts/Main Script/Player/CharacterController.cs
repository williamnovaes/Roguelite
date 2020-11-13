using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CharacterController : MonoBehaviour {

        Rigidbody2D characterRb;
        CapsuleCollider2D capsuleCollider;
        ContactFilter2D m_ContactFilter;
        RaycastHit2D[] m_HitBuffer = new RaycastHit2D[5];
        RaycastHit2D[] m_FoundHits = new RaycastHit2D[1];
        Collider2D[] m_GroundColliders = new Collider2D[1];
        Vector2[] m_RaycastPositions = new Vector2[1];

        Vector2 nextMovement;
        Vector2 currentPosition;
        Vector2 previousPosition;

        public Vector2 Velocity { get; protected set; }
        public bool IsGrounded { get; protected set; }

        public float groundedRaycastDistance;
        public LayerMask groundedLayerMask;

        void Awake() {
            characterRb = GetComponent<Rigidbody2D>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();

            currentPosition = characterRb.position;
            previousPosition = characterRb.position;

            m_ContactFilter.layerMask = groundedLayerMask;
            m_ContactFilter.useLayerMask = true;
            m_ContactFilter.useTriggers = false;

            Physics2D.queriesStartInColliders = false;
        }

        private void FixedUpdate() {
            ApplyMovement();
            CheckCapsuleEndCollisions();
        }

        private void ApplyMovement() {
            Vector2 previousPosition = characterRb.position;
            Vector2 currentPosition = previousPosition + nextMovement;
            Velocity = (currentPosition - previousPosition) / Time.deltaTime;

            characterRb.MovePosition(currentPosition);
            nextMovement = Vector2.zero;
        }
        
        public void Move(Vector2 movement) {
            nextMovement = movement;
        }

        private void CheckCapsuleEndCollisions() {
            Vector2 raycastDirection;
            Vector2 raycastStart;
            float raycastDistance;

            raycastStart = characterRb.position + capsuleCollider.offset;
            raycastDistance = capsuleCollider.size.x * 0.5f + groundedRaycastDistance * 2f;

            raycastDirection = Vector2.down;
            Vector2 raycastStartCenter = raycastStart + Vector2.down * (capsuleCollider.size.y * 0.5f - capsuleCollider.size.x * 0.5f);
            m_RaycastPositions[0] = raycastStartCenter;

            for (int i = 0; i < m_RaycastPositions.Length; i++) {
                int count = Physics2D.Raycast(m_RaycastPositions[i], raycastDirection, m_ContactFilter, m_HitBuffer, raycastDistance);

                m_FoundHits[i] = count > 0 ? m_HitBuffer[0] : new RaycastHit2D();
                m_GroundColliders[i] = m_FoundHits[i].collider;
            }

            
            Vector2 groundNormal = Vector2.zero;
            int hitCount = 0;

            for (int i = 0; i < m_FoundHits.Length; i++) {
                if (m_FoundHits[i].collider != null) {
                    groundNormal += m_FoundHits[i].normal;
                    hitCount++;
                }
            }

            if (hitCount > 0) {
                groundNormal.Normalize();
            }

            Vector2 relativeVelocity = Velocity;
            for (int i = 0; i < m_GroundColliders.Length; i++) {
                if (m_GroundColliders[i] == null) {
                    continue;
                }
            }

            if (Mathf.Approximately(groundNormal.x, 0f) && Mathf.Approximately(groundNormal.y, 0f)) {
                IsGrounded = false;
            } else {
                IsGrounded = relativeVelocity.y <= 0;

                if (capsuleCollider != null) {
                    if (m_GroundColliders[0] != null) {
                        float capsuleBottomHeight = characterRb.position.y + capsuleCollider.offset.y - capsuleCollider.size.y * 0.5f;
                        float middleHitHeight = m_FoundHits[0].point.y;
                        IsGrounded &= middleHitHeight < capsuleBottomHeight + groundedRaycastDistance;
                    }
                }
            }

            for (int i = 0; i < m_HitBuffer.Length; i++) {
                m_HitBuffer[i] = new RaycastHit2D();
            }
        }
    }
}