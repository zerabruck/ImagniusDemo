using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private Transform playerPosition;
    [SerializeField]private float distanceLookAhead;
    [SerializeField]private float cameraSpeed;
    [SerializeField]private float lookDown;

    private float lookAhead;


    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    private void Update() {
        // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
        transform.position = new Vector3(playerPosition.position.x + lookAhead, playerPosition.position.y + lookDown, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, distanceLookAhead * playerPosition.localScale.x, Time.deltaTime * cameraSpeed);


    }
    public void MoveToNewRoom(Transform _postion){
        currentPosX = _postion.position.x;
    }

   
}
