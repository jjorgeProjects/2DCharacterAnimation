using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 offset;
    public float smoothing = 5f;
    public Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Offset between camera position and player position
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = target.position + offset;

        //Include LERP
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);

    }
}
