using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public abstract class PowerEffect
{
    protected BasePowerDealer owner;
    public void initialize(BasePowerDealer owner)
    {
        this.owner = owner;
    }
    public abstract bool OnTriggerEnterOverride(Collider other, Power owner);
    public abstract void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator);
    public abstract void runNavigatorUpdate(UnitNavigationController navigator);
    public abstract void deliverOnCollisionEffect(Collider other, PersonStatus status);
}

public class BasePowerDealer : Power
{
    //Public vars
    public float speed = 25f;
    public int Fear = 70;
    public int Highness = 0;
    public GameObject godRay;
    public GameObject particleEffect;
    public PowerEffect powerEffect;
    public float distance = 40;
    //Private vars
    private bool ready = true;
    private int souls;
    private Vector3 startPosition;
    private Vector3 Location; //for the preview of the power
    private float previousParticlePosY = 0;
    private Ray ray;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position; //saves the start position
        //pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
        //stat = GameObject.Find("Human").GetComponent<Status>();
    }

    void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
    {
        MouseCursorManager.mouseManager.ShowCursor = false;
        godRay.SetActive(true);
        particleEffect.SetActive(true);
        ready = true;
    }

    void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
    {
        godRay.SetActive(false);
        particleEffect.SetActive(false);
        MouseCursorManager.mouseManager.ShowCursor = true;
    }

    public override void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator)
    {
    }

    public override void runNavigatorUpdate(UnitNavigationController navigator)
    {
    }


    //COLLISION
    public override void OnTriggerEnter(Collider other)
    {
        string effectName = this.gameObject.name + "Effect";

        //Type type = Type.GetType(effectName);
        PowerEffect effect = (PowerEffect)Assembly.GetExecutingAssembly().CreateInstance(effectName);
        effect.initialize(this);



        if (!effect.OnTriggerEnterOverride(other, this))
        {

            if (other.tag == GlobalManager.npcsTag)
            {
                PersonStatus person = other.GetComponent<PersonStatus>();

                if (person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget)
                {
                    audio.Play();
                    person.ActivePower = effect;
                }
            }
        }
    }


    void OnGUI()
    {
        //souls = GlobalManager.globalManager.souls;

        if (ready)
        {
            //PREVIEW
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Location.x = ray.origin.x + (ray.direction.x * 40);
            Location.y = 0;
            Location.z = ray.origin.z + (ray.direction.z * 40);
            transform.position = Location;
            particleEffect.transform.position = new Vector3(particleEffect.transform.position.x, 17.5f, particleEffect.transform.position.z);

            if (Input.GetMouseButton(0))
            {
                Debug.Log("HIT for :" + this.name);
                ready = false;
                previousParticlePosY = particleEffect.transform.position.y;
            }
        }
        else
        {

            particleEffect.transform.position = particleEffect.transform.position + Vector3.down * speed * Time.deltaTime;

            if (particleEffect.transform.position.y <= 0)
            {
                ready = true;
                particleEffect.transform.position = new Vector3(Location.x, previousParticlePosY, Location.z);
                GlobalManager.globalManager.decrementSouls(price);
            }
        }

    }
}
