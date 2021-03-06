﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.Application;

namespace Assets.Scripts.View {
    public class CameraController : MonoBehaviour
    {
        private bool moveLocked;
        private bool zoomLocked;

        public float offset;
        public float speed;
        public float zoomSpeed;
        public float targetOrtho;
        public float smoothSpeed = 2.0f;
        public float minOrtho = 1.0f;
        public float maxOrtho = 20.0f;

        //x - min y - max
        public Vector2 minMaxXPosition;
        public Vector2 minMaxYPosition;

        private float screenWidth;
        private float screenHeight;
        private Vector3 cameraMove;

        private Transform mTarget;
        private bool mIsFollowing;

        // Use this for initialization
        void Start() {
            moveLocked = false;
            zoomLocked = false;
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            targetOrtho = Camera.main.orthographicSize;

            cameraMove.x = transform.position.x;
            cameraMove.y = transform.position.y;
            cameraMove.z = transform.position.z;
        }

        public void FollowGameObject(Transform gameObject) {
            mIsFollowing = true;
            mTarget = gameObject;
        }

        public void StopFollowingGameObject() {
            mIsFollowing = false;
            mTarget = null;
        }

        public void MoveToPosition(Vector3 position) {
            cameraMove.x = position.x;
            cameraMove.y = position.y;
            transform.position = cameraMove;
        }

        public void LockMoveCamera() {
            moveLocked = true;
        }

        public void UnlockMoveCamera() {
            moveLocked = false;
        }

        public void LockZoomCamera() {
            zoomLocked = true;
        }

        public void UnlockZoomCamera() {
            zoomLocked = false;
        }

        // Update is called once per frame
        void Update() {
            if (mIsFollowing) {
                cameraMove.x = mTarget.position.x;
                cameraMove.y = mTarget.position.y;
                
                transform.position = cameraMove;
            }

            if (!moveLocked) {
                if (Input.GetKeyDown(KeyCode.LeftShift)) {
                    speed *= 2;
                } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
                    speed /= 2;
                }
                /*
                //Move camera
                if ((Input.mousePosition.x > screenWidth - offset) && transform.position.x < minMaxXPosition.y) {
                    cameraMove.x += MoveSpeed();
                }

                if ((Input.mousePosition.x < offset) && transform.position.x > minMaxXPosition.x) {
                    cameraMove.x -= MoveSpeed();
                }

                if ((Input.mousePosition.y > screenHeight - offset) && transform.position.y < minMaxYPosition.y) {
                    cameraMove.y += MoveSpeed();
                }

                if ((Input.mousePosition.y < offset) && transform.position.y > minMaxYPosition.x) {
                    cameraMove.y -= MoveSpeed();
                }
                */
                //Move camera with WASD
                if ((Input.GetKey(KeyCode.D))) {
                    cameraMove.x += MoveSpeed();
                }

                if ((Input.GetKey(KeyCode.A))) {
                    cameraMove.x -= MoveSpeed();
                }

                if ((Input.GetKey(KeyCode.W))) {
                    cameraMove.y += MoveSpeed();
                }

                if ((Input.GetKey(KeyCode.S))) {
                    cameraMove.y -= MoveSpeed();
                }

                transform.position = cameraMove;
            }

            if (!zoomLocked) {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    if (scroll != 0.0f) {
                        targetOrtho -= scroll * zoomSpeed;
                        targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
                    }
                    Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
                }
            }
        }

        float MoveSpeed() {
            return speed * Time.deltaTime;
        }
    }
}
