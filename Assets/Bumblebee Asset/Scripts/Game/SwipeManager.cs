using UnityEngine;

namespace Bumblebee_Asset.Scripts.Game
{
    public class SwipeManager : MonoBehaviour
    {
        public static bool IsTap, IsSwipeLeft, IsSwipeRight, IsSwipeUp, IsSwipeDown;
        private bool _isDraging;
        private Vector2 _startTouch, _swipeDelta;

        private void Update()
        {
            IsTap = IsSwipeDown = IsSwipeUp = IsSwipeLeft = IsSwipeRight = false;

            #region Standalone Inputs

            if (Input.GetMouseButtonDown(0))
            {
                IsTap = true;
                _isDraging = true;
                _startTouch = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDraging = false;
                Reset();
            }

            #endregion

            #region Mobile Input

            if (Input.touches.Length > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    IsTap = true;
                    _isDraging = true;
                    _startTouch = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    _isDraging = false;
                    Reset();
                }
            }

            #endregion

            //Calculate the distance
            _swipeDelta = Vector2.zero;
            if (_isDraging)
            {
                if (Input.GetMouseButton(0))
                {
                    _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
                }
            }

            //Did we cross the distance?
            if (_swipeDelta.magnitude > 100)
            {
                //Which direction?
                float x = _swipeDelta.x;
                float y = _swipeDelta.y;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    //Left or Right
                    if (x < 0)
                        IsSwipeLeft = true;
                    else
                        IsSwipeRight = true;
                }
                else
                {
                    //Up or Down
                    if (y < 0)
                        IsSwipeDown = true;
                    else
                        IsSwipeUp = true;
                }

                Reset();
            }
        }

        private void Reset()
        {
            _startTouch = _swipeDelta = Vector2.zero;
            _isDraging = false;
        }
    }
}