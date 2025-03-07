using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest
    public float camZ; // The desired Z pos of the camera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (POI == null) return; // Return if there is no POI
        Vector3 destination = POI.transform.position;
        destination.z = camZ;
        this.transform.position = destination;
    }
}
