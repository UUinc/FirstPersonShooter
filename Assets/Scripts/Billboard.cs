using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;

    private void Update()
    {
        if(cam == null)
            cam = FindFirstObjectByType<Camera>();
        
        if (cam == null)
            return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
