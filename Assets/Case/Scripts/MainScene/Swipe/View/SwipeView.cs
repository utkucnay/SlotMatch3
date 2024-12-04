using System;
using UnityEngine;

namespace Case.MainScene.Swipe
{
    [RequireComponent(typeof(Collider2D))]
    public class SwipeView : MonoBehaviour
    {
        public event Action<SwipeDirection> OnSwipeEvent;

        private bool isDragging = false;
        private Vector3 enterMousePosition;

        private void OnMouseDown()
        {
            isDragging = true;
            enterMousePosition = Input.mousePosition;
        }

        private void OnMouseUp()
        {
            isDragging = false;
            enterMousePosition = Vector3.zero;
        }

        private void OnMouseExit()
        {
            if (isDragging)
            {
                var exitMousePositon = Input.mousePosition;
                SwipeDirection swipeDirection;

                var deltaX = exitMousePositon.x - enterMousePosition.x;
                var deltaY = exitMousePositon.y - enterMousePosition.y;

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY))
                {
                    if (deltaX > 0)
                    {
                        swipeDirection = SwipeDirection.Right;
                    }
                    else
                    {
                        swipeDirection = SwipeDirection.Left;
                    }
                }
                else
                {
                    if (deltaY > 0)
                    {
                        swipeDirection = SwipeDirection.Up;
                    }
                    else
                    {
                        swipeDirection = SwipeDirection.Down;
                    }
                }

                OnSwipe(swipeDirection);
            }

            isDragging = false;
        }

        private void OnSwipe(SwipeDirection swipeDirection)
        {
            OnSwipeEvent?.Invoke(swipeDirection);
        }
    }
}
