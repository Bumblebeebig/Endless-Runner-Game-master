using System.Collections;
using Bumblebee_Asset.Scripts.Game;
using UnityEngine;

namespace Bumblebee_Asset.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _controller;
        private Vector3 _move;
        public float forwardSpeed;
        public float maxSpeed;

        private int _desiredLane = 1;//0:left, 1:middle, 2:right
        public float laneDistance = 2.5f;//The distance between tow lanes

        public bool isGrounded;
        public LayerMask groundLayer;
        public Transform groundCheck;

        public float gravity = -12f;
        public float jumpHeight = 2;
        private Vector3 _velocity;

        public Animator animator;
        private bool _isSliding;

        public float slideDuration = 1.5f;

        private bool _toggle;
        private static readonly int IsGameStarted = Animator.StringToHash("isGameStarted");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            Time.timeScale = 1.2f;
        }

        private void FixedUpdate()
        {
            if (!GameManager.IsGameStarted || GameManager.IsGameOver)
                return;

            //Increase Speed
            if (_toggle)
            {
                _toggle = false;
                if (forwardSpeed < maxSpeed)
                    forwardSpeed += 0.1f * Time.fixedDeltaTime;
            }
            else
            {
                _toggle = true;
                if (Time.timeScale < 2f)
                    Time.timeScale += 0.005f * Time.fixedDeltaTime;
            }
        }

        void Update()
        {
            if (!GameManager.IsGameStarted || GameManager.IsGameOver)
                return;

            animator.SetBool(IsGameStarted, true);
            _move.z = forwardSpeed;

            isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
            animator.SetBool(IsGrounded, isGrounded);
            if (isGrounded && _velocity.y < 0)
                _velocity.y = -1f;

            if (isGrounded)
            {
                if (SwipeManager.IsSwipeUp)
                    Jump();

                if (SwipeManager.IsSwipeDown && !_isSliding)
                    StartCoroutine(Slide());
            }
            else
            {
                _velocity.y += gravity * Time.deltaTime;
                if (SwipeManager.IsSwipeDown && !_isSliding)
                {
                    StartCoroutine(Slide());
                    _velocity.y = -10;
                }                

            }
            _controller.Move(_velocity * Time.deltaTime);

            //Gather the inputs on which lane we should be
            if (SwipeManager.IsSwipeRight)
            {
                _desiredLane++;
                if (_desiredLane == 3)
                    _desiredLane = 2;
            }
            if (SwipeManager.IsSwipeLeft)
            {
                _desiredLane--;
                if (_desiredLane == -1)
                    _desiredLane = 0;
            }

            //Calculate where we should be in the future
            var transform1 = transform;
            var position = transform1.position;
            Vector3 targetPosition = position.z * transform1.forward + position.y * transform1.up;
            if (_desiredLane == 0)
                targetPosition += Vector3.left * laneDistance;
            else if (_desiredLane == 2)
                targetPosition += Vector3.right * laneDistance;

            //transform.position = targetPosition;
            if (transform.position != targetPosition)
            {
                Vector3 diff = targetPosition - transform.position;
                Vector3 moveDir = diff.normalized * (30 * Time.deltaTime);
                _controller.Move(moveDir.sqrMagnitude < diff.magnitude ? moveDir : diff);
            }

            _controller.Move(_move * Time.deltaTime);
        }

        private void Jump()
        {   
            StopCoroutine(Slide());
            animator.SetBool("isSliding", false);
            animator.SetTrigger("jump");
            _controller.center = Vector3.zero;
            _controller.height = 2;
            _isSliding = false;
   
            _velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(hit.transform.CompareTag("Obstacle"))
            {
                GameManager.IsGameOver = true;
                FindObjectOfType<AudioManager>().PlaySound("GameOver");
            }
        }

        private IEnumerator Slide()
        {
            _isSliding = true;
            animator.SetBool("isSliding", true);
            yield return new WaitForSeconds(0.25f/ Time.timeScale);
            _controller.center = new Vector3(0, -0.5f, 0);
            _controller.height = 1;

            yield return new WaitForSeconds((slideDuration - 0.25f)/Time.timeScale);

            animator.SetBool("isSliding", false);

            _controller.center = Vector3.zero;
            _controller.height = 2;

            _isSliding = false;
        }
    }
}
