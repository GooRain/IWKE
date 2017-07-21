using System.Collections;
using UnityEngine;

namespace AssembleObject
{

    public class ObjectPart : MonoBehaviour
    {

        [SerializeField]
        private string partName;
        [SerializeField]
        private Vector3 disassembleCoordinates;
        [SerializeField]
        private AudioSource sound;

        private PartHistory history;
        private bool disassembled = false;
        private BoxCollider2D myCollider;

        private void Start()
        {
            history = new PartHistory();
            if (GetComponent<BoxCollider2D>())
                myCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            HandleTouch();
        }

        private void HandleTouch()
        {
            if (disassembled && myCollider != null)
            {
                if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        Vector2 touchPos = new Vector2(wp.x, wp.y);
                        if (myCollider == Physics2D.OverlapPoint(touchPos))
                        {
                            //  Part touch
                        }
                    }
                }
            }
        }

        public void Disassemble()
        {
            history.SaveState(new PartMemento(transform.localPosition));
            StartDisassemble();
            disassembled = true;
        }

        public void Assemble()
        {
            if (history.IsEmpty())
                return;
            StartCoroutine(Move(transform.localPosition, history.GetState(), 1f));
            disassembled = false;
        }

        private void StartDisassemble()
        {
            //StartCoroutine(Move(transform.position, new Vector3(Random.Range(-15, 5), Random.Range(-15, 5), transform.position.z), 1f));
            StartCoroutine(Move(transform.localPosition, disassembleCoordinates, 1f));
        }

        IEnumerator Move(Vector3 startPos, Vector3 endPos, float during)
        {
            float startTime = Time.time;
            while (Time.time < startTime + during)
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, (Time.time - startTime) / during);
                yield return null;
            }
            transform.localPosition = endPos;
        }

    }
}