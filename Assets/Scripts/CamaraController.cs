using UnityEngine;

public class CameraZoomController : MonoBehaviour {
    public GameObject targetPrefab;
    public float initialDistance = 20f;
    public float zoomSensitivity = 5f;
    public float zoomMin = 5f;
    public float zoomMax = 50f;
    public float moveSpeed = 10f;
    public float rotationSpeed = 45f; // degrees per second

    private Transform target;
    private float currentDistance;
    private float currentAngle = 45f;

    void Start() {
        // 기준 오브젝트를 바닥 중심에 생성
        Vector3 groundCenter = new Vector3(0f, 0f, 0f);
        GameObject targetObj = Instantiate(targetPrefab, groundCenter, Quaternion.identity);
        target = targetObj.transform;

        currentDistance = initialDistance;
        UpdateCameraPosition();
    }

    void Update() {
        HandleZoom();
        HandleMove();
        HandleRotation();
    }

    // wheel
    void HandleZoom() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f) {
            currentDistance -= scroll * zoomSensitivity;
            currentDistance = Mathf.Clamp(currentDistance, zoomMin, zoomMax);
            UpdateCameraPosition();
        }
    }

    // wasd movement
    void HandleMove() {
        Vector3 forward = new Vector3(-Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, -Mathf.Sin(currentAngle * Mathf.Deg2Rad));
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) { dir += forward; }
        if (Input.GetKey(KeyCode.S)) { dir -= forward; }
        if (Input.GetKey(KeyCode.D)) { dir += right; }
        if (Input.GetKey(KeyCode.A)) { dir -= right; }

        if (dir != Vector3.zero) {
            Vector3 nextPos = target.position + dir.normalized * moveSpeed * Time.deltaTime;
            if (Physics.Raycast(nextPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Ground"))) { target.position = hit.point; }
            UpdateCameraPosition();
        }
    }

    // qe rotation
    void HandleRotation() {
        if (Input.GetKey(KeyCode.Q)) {
            currentAngle -= rotationSpeed * Time.deltaTime;
            UpdateCameraPosition();
        }
        if (Input.GetKey(KeyCode.E)) {
            currentAngle += rotationSpeed * Time.deltaTime;
            UpdateCameraPosition();
        }
    }

    // 카메라 위치 갱신
    void UpdateCameraPosition() {
        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 offsetDir = new Vector3(Mathf.Cos(angleRad), 1f, Mathf.Sin(angleRad)).normalized;
        Vector3 cameraPos = target.position + offsetDir * currentDistance;

        Camera.main.transform.position = cameraPos;
        Camera.main.transform.LookAt(target);
    }
}