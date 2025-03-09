using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest
    public float camZ; // The desired Z pos of the camera
    private Vector3 originalPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        camZ = this.transform.position.z;
        originalPos = transform.position;
        print(originalPos.x + " " + originalPos.y);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (POI == null) return; // Return if there is no POI
        Vector3 destination = POI.transform.position;
        destination.z = camZ;
        this.transform.position = destination;
        Rigidbody2D poiRigid = POI.GetComponent<Rigidbody2D>();
        if (((poiRigid != null) && poiRigid.IsSleeping()) || poiRigid.linearVelocity.x < 0.1f)
        {
            poiRigid.linearDamping = 10;
            POI = null;
            transform.position = originalPos;
        }
    }
}
