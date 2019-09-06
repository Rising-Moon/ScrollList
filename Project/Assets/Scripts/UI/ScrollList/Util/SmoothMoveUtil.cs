using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ScrollList.UI
{
    public class SmoothMoveUtil : MonoBehaviour, IBeginDragHandler
    {
        private IEnumerator smoothMoveCoroutine;
        private Scrollbar scrollbar;

        private void Start()
        {
            scrollbar = gameObject.GetComponent<Scrollbar>();
        }

        public void SmoothMoveTo(float target, float time)
        {
            if (target > 1)
                target = 1;
            if (target < 0)
                target = 0;

            if (smoothMoveCoroutine == null)
            {
                smoothMoveCoroutine = SmoothMove(target, time);
                StartCoroutine(smoothMoveCoroutine);
            }
            else
            {
                StopCoroutine(smoothMoveCoroutine);
                smoothMoveCoroutine = SmoothMove(target, time);
                StartCoroutine(smoothMoveCoroutine);
            }
        }

        public void StopMove()
        {
            if (smoothMoveCoroutine != null)
                StopCoroutine(smoothMoveCoroutine);
        }

        IEnumerator SmoothMove(float target, float time)
        {
            //Debug.Log("Move to:" + target + " time:" + time);
            float v = 0.0f;

            float distance = Mathf.Abs(target - scrollbar.value);
            while (distance > 0.001f)
            {
                float newPosition = Mathf.SmoothDamp(scrollbar.value, target, ref v, time);
                scrollbar.value = newPosition;
                distance = Mathf.Abs(target - scrollbar.value);
                yield return null;
            }

            scrollbar.value = target;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StopMove();
        }
    }
}