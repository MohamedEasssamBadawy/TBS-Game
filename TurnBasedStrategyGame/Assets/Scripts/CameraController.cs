using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField] private CinemachineFollow cinemachineFollow;

    private Vector3 targetFollowOffset;

    private void Start() {
        targetFollowOffset = cinemachineFollow.FollowOffset;
    }

    private void Update() {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement() {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
        if (Input.GetKey(KeyCode.W)) {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputMoveDir.x = +1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputMoveDir.x = -1f;
        }

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation() {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom() {
        float zoomIncreaseAmount = 1f;
        targetFollowOffset.y = InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineFollow.FollowOffset =
            Vector3.Lerp(cinemachineFollow.FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
