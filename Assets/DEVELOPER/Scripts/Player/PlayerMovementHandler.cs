using EssentialManagers.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace DEVELOPER.Scripts.Player
{
    public class PlayerMovementHandler : MonoBehaviour
    {
        [Header("Movement Settings")] public float moveSpeed = 5f;
        public DynamicJoystick joystick;

        private NavMeshAgent agent;
        private Vector3 moveInput;
        private Vector3 moveDirection;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            joystick = CanvasManager.instance.GetDynamicJoystick();
        }

        void Update()
        {
            moveInput = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

            if (moveInput.magnitude > 0.1f)
            {
                moveDirection = moveInput.normalized * moveSpeed;
                agent.Move(moveDirection * Time.deltaTime);

                // Yönü joystick yönüne çevir
                transform.forward = moveInput.normalized;
            }
        }
    }
}