using UnityEngine;
using System.Collections;

public class CameraMenu : MonoBehaviour
{
    public float speed = 10;
    public float horizontalMovement = 100;
    public float verticalMovement = 100;
    public float zoomAmount = 100;
    public float maxAngleX = 310;
    public float minAngleX = 70;
    public float rayLenght = 1.5f;
    public bool debugDraw = false;
    public bool debugNoRaycast = false;
    public float zoomLevel = 40;
    public bool isIsleClicked = false;

    float distFromMin;
    float distFromMax;
    Vector3 direction;
    Vector3 origin;



    public CameraMenu()
    {


    }

    void Update()
    {
        Move();
        /* if (isIsleClicked)
         {
             Vector3 coordinates = cs.CoordinatesToZoom;
             AutoZoom(coordinates);
         }*/
    }

    void Move()
    {
        direction = new Vector3(0, 0, 0);
        origin = transform.position;

        direction.x = Input.GetAxis("Horizontal") * horizontalMovement;

        direction.z = Input.GetAxis("Vertical") * verticalMovement;

        var rotation = transform.rotation;
        transform.Rotate(new Vector3(-rotation.eulerAngles.x, 0, 0));

        if (!debugNoRaycast && direction.x != 0 && direction.x > 0 && Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.right)), rayLenght))
        {
            direction.x = 0;

            if (debugDraw)
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right), Color.red);

        }
        else if (debugDraw)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right));
        }


        if (!debugNoRaycast && direction.x != 0 && direction.x < 0 && Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.left)), rayLenght))
        {
            direction.x = 0;

            if (debugDraw)
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left), Color.red);

        }
        else if (debugDraw)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left));
        }

        if (!debugNoRaycast && direction.z != 0 && direction.z > 0 && Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.forward)), rayLenght))
        {
            direction.z = 0;
            if (debugDraw)
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);

        }
        else if (debugDraw)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
        }

        if (!debugNoRaycast && direction.z != 0 && direction.z < 0 && Physics.Raycast(new Ray(transform.position, transform.TransformDirection(Vector3.back)), rayLenght))
        {
            direction.z = 0;

            if (debugDraw)
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back), Color.red);

        }
        else if (debugDraw)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back));
        }

        Vector3 movement = transform.TransformDirection(direction);

        /*movement.y = Input.GetAxis("Mouse ScrollWheel") * zoomAmount * -1;

        if (movement.y == 0)
            movement.y = Input.GetAxis("Zoom") * zoomAmount * -1;


        if (!debugNoRaycast && movement.y != 0 && movement.y < 0 && Physics.Raycast(new Ray(transform.position, Vector3.down), rayLenght))
        {
            movement.y = 0;
        }

        if (!debugNoRaycast && movement.y != 0 && movement.y > 0 && Physics.Raycast(new Ray(transform.position, Vector3.up), rayLenght))
        {
            movement.y = 0;
        }*/



        Vector3 destination = origin + movement;

        transform.rotation = rotation;

        transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * speed);

        //  if (transform.position.y != origin.y)
        //    zoomLevel = zoomLevel + ((transform.position.y - origin.y) * 1.15f);

    }

  /*  public void AutoZoom(Vector3 coordinates)
    {
        zoomLevel = zoomLevel + ((transform.position.y - origin.y) * 1.15f);

    }


    public bool IsIsleClicked
    {
        set { isIsleClicked = value; }
    }
    */
}
