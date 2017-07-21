using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssembleObject
{

    public class ObjectMain : MonoBehaviour
    {

        public string Name;

        [SerializeField]
        private float rotateSpeed = 5f;
        [SerializeField]
        private float doubleTapRecoil = 0.5f;
        private float doubleTapCurrentRecoil;
        private int tapCount = 0;
        private bool assembled = true;

        private List<ObjectPart> parts;

        private void Start()
        {
            parts = new List<ObjectPart>();
            SetChildren();
        }

        public void Disassemble()
        {
            transform.rotation = Quaternion.identity;
            foreach (var c in parts)
            {
                c.Disassemble();
            }
            assembled = false;
        }

        public void Assemble()
        {
            foreach (var c in parts)
            {
                c.Assemble();
            }
            assembled = true;
        }

        private void Update()
        {
            HandleTouch();
            if (assembled)
            {
                transform.RotateAround(transform.position, transform.up, .1f);
            }
        }

        public void SetChildren()
        {
            foreach (Transform c in transform)
            {
                parts.Add(c.gameObject.GetComponent<ObjectPart>());
            }
        }

        void HandleTouch()
        {
            if (Input.touchCount > 0 && assembled)
            {
                Touch firstTouch = Input.GetTouch(0);
                if (firstTouch.phase == TouchPhase.Began)
                {
                    if (doubleTapCurrentRecoil > 0 && tapCount == 1)
                    {
                        Disassemble();
                    }
                    else
                    {
                        doubleTapCurrentRecoil = doubleTapRecoil;
                        tapCount++;
                    }
                }


                if (Input.touchCount == 1 && firstTouch.phase == TouchPhase.Moved)
                {
                    if (Mathf.Abs(firstTouch.deltaPosition.x) > Mathf.Abs(firstTouch.deltaPosition.y))
                    {
                        transform.RotateAround(transform.position, transform.up, -firstTouch.deltaPosition.x * rotateSpeed * Time.deltaTime);
                        //Debug.Log("Touch0.x: " + firstTouch.deltaPosition.x);
                    }
                }

                if (Input.touchCount == 2)
                {
                    if (firstTouch.phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        Disassemble();
                    }
                }

            }
            if (doubleTapCurrentRecoil > 0)
            {
                doubleTapCurrentRecoil -= Time.deltaTime;
            }
            else
            {
                tapCount = 0;
            }
        }

    }
}
