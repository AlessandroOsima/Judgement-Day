using System.Collections.Generic;
using UnityEngine;

public class CameraControlMenu : MonoBehaviour
{
    public float speed = 90;
    public float horizontalMovement = 100;
    public float rayLenght = 1.5f;
    public List<Vector3> positions;

    float distFromMin;
    float distFromMax;

    static TweenPosition tweenPos;
    private int isolaSelezionata = 0;
    int numeroIsole = 0;

    Transform target;
    float smooth = 5;
    Vector3 direction;
    Vector3 origin;
    Vector3 movement;
    Vector3 destination;

    void Start()
    {
        tweenPos = camera.GetComponent<TweenPosition>();
        tweenPos.ignoreTimeScale = true;
        tweenPos.duration = 2f;
        tweenPos.method = UITweener.Method.EaseInOut;
        numeroIsole = positions.Count;

    }

    void Update()
    {
        System.Threading.Thread.Sleep(200);
        // KeyEventArgs.SuppressKeyPress = false;

        direction = new Vector3(0, 0, 0);
        origin = transform.position;
        tweenPos.from = origin;

        direction.x = Input.GetAxis("Horizontal") * horizontalMovement;

        movement = transform.TransformDirection(direction);

        destination = origin + movement;


        if (destination.x > origin.x && isolaSelezionata < numeroIsole - 1)     //metto il  -1 perchè comincio a contare da 0
        {
            isolaSelezionata++;
            tweenPos.to = positions[isolaSelezionata];
            tweenPos.PlayForward();
        }
        else if (destination.x < origin.x && isolaSelezionata > 0)
        {
            isolaSelezionata--;
            tweenPos.to = positions[isolaSelezionata];
            tweenPos.PlayForward();

        }
        // KeyEventArgs.SuppressKeyPress = true;
    }

    public static TweenPosition cameraTweenPosition
    {
        get
        {
            return tweenPos;
        }
    }

}
