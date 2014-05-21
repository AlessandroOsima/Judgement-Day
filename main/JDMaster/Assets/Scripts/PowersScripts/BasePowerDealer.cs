using UnityEngine;
using System.Collections;
using System;

#if !UNITY_WINRT || (UNITY_WINRT && UNITY_EDITOR)
    using System.Reflection;
#endif

#if UNITY_WINRT && !UNITY_EDITOR
    using System.Reflection;
#endif


public abstract class PowerEffect
{
	protected BasePowerDealer owner;
	
	public String effectName
	{
		get
		{
			return owner.gameObject.name;
		}
	}
	
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
	public float cameraZoomScaleFactor = 1.0f;
	public bool enableUse = true;
	float distance = 0;
	CameraControl cameraControl;
	//Private vars
	private bool ready = true;
	private int souls;
	private Vector3 startPosition;
	private Vector3 Location; //for the preview of the power
	private float previousParticlePosY = 0;
	private Ray ray;
    private GameObject gameCamera;
	
	// Use this for initialization
	void Start()
	{
        gameCamera = GameObject.Find("RTSCameraSoundtrack");
		startPosition = transform.position; //saves the start position
		//pscore = GameObject.Find("_Global").GetComponent<Player_Score>(); //initializing the script
		//stat = GameObject.Find("Human").GetComponent<Status>();
	}
	
	void OnEnable() //The Power is activated by the power manager, initialize stuff here, do NOT use any find* function (Slooow !!)
	{
		if(cameraControl == null)
			cameraControl = Camera.main.GetComponent<CameraControl>();
		
		if(MouseCursorManager.mouseManager != null)
			MouseCursorManager.mouseManager.ShowCursor = false;
		
		//set the correct god ray/particle effect position before the ray is visible
		var zoomLevel = cameraControl.zoomLevel;
		distance = ((zoomLevel) * cameraZoomScaleFactor);
		Location.x = ray.origin.x + (ray.direction.x * distance);
		Location.y = 0;
		Location.z = ray.origin.z + (ray.direction.z * distance);
		transform.position = Location;
		
		godRay.SetActive(true);
		particleEffect.SetActive(true);
		ready = true;
	}
	
	void OnDisable() //The Power is deactivated by the power manager, deinitialize stuff here, do NOT use any find* function
	{
		godRay.SetActive(false);
		particleEffect.SetActive(false);
		
		if(MouseCursorManager.mouseManager != null)
			MouseCursorManager.mouseManager.ShowCursor = false;
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
//#if !UNITY_WINRT && UNITY_EDITOR
		string effectName = this.gameObject.name + "Effect";
		
		//Type type = Type.GetType(effectName);
#if UNITY_WINRT && !UNITY_EDITOR
        //PowerEffect effect = (PowerEffect)typeof(BasePowerDealer).GetTypeInfo().Assembly.CreateInstance(effectName);
        //PowerEffect effect = (PowerEffect)Activator.CreateInstance(typeof(BasePowerDealer).GetTypeInfo().Assembly.ToString(), effectName);

        PowerEffect powerEffect = getEffectFromName(effectName);
        //PowerEffect effect = (PowerEffect)Activator.CreateInstance(typeof(FireEffect));
#else
        PowerEffect powerEffect = (PowerEffect)Assembly.GetAssembly(this.GetType()).CreateInstance(effectName);
#endif

        powerEffect.initialize(this);

        if (!powerEffect.OnTriggerEnterOverride(other, this)) 
		{
			if (other.tag == GlobalManager.npcsTag) 
			{
				
				PersonStatus person = other.GetComponent<PersonStatus> ();
				
				if (person.UnitStatus != PersonStatus.Status.Dead && person.IsAValidTarget) 
				{
                    person.ActivePower = powerEffect;
				}
			}
		}
//#endif
 }

    PowerEffect getEffectFromName(string effectName)
    {
        switch (effectName)
        {
            case "RageEffect" :
            {
                return (PowerEffect) new RageEffect();
            }
            
            case "FireEffect" :
            {
                return (PowerEffect) new FireEffect();
            }

            case "ZombieEffect":
            {
                return (PowerEffect)new ZombieEffect();
            }

            case "CalmEffect":
            {
                return (PowerEffect)new CalmEffect();
            }

            case "LightingEffect" :
            {
                return (PowerEffect)new LightingEffect();
            }

            default: return null;
        }
    }
    
    void OnGUI()
    {
        //souls = GlobalManager.globalManager.souls;
        if (ready)
        {
            //PREVIEW
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            var zoomLevel = cameraControl.zoomLevel;
            distance = ((zoomLevel) * cameraZoomScaleFactor);

            Location.x = ray.origin.x + (ray.direction.x * distance);
            Location.y = 0;
            Location.z = ray.origin.z + (ray.direction.z * distance);
            transform.position = Location;

            particleEffect.transform.position = new Vector3(particleEffect.transform.position.x, 17.5f, particleEffect.transform.position.z);

            //Debug.Log(InputMapping.GetAction(Actions.Use));
            if ((InputMapping.GetAction(Actions.Use) > 0) && enableUse)
            {
                Debug.Log(InputMapping.GetAction(Actions.Use));
                Vector3 clickPosition = InputMapping.GetActionPosition(Actions.Use);
                if (clickPosition != Vector3.zero)
                {
                    ray = Camera.main.ScreenPointToRay(clickPosition);
                    RaycastHit hit;
                    int layerMask = 1 << 10;
                    layerMask = ~layerMask;
                    if (Physics.Raycast(ray, out hit, 1000, layerMask))
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 100f);
                        Debug.Log("hit ray " + hit.transform.tag);
                    }
                    else
                        Debug.Log("NO");
                    Location.x = hit.point.x;
                    Location.y = 0;
                    Location.z = hit.point.z;
                    transform.position = Location;
                    particleEffect.transform.position = new Vector3(particleEffect.transform.position.x, 17.5f, particleEffect.transform.position.z);

                }
                ready = false;
                previousParticlePosY = particleEffect.transform.position.y;
            }
        }
        else
        {

            particleEffect.transform.position = particleEffect.transform.position + Vector3.down * speed * Time.deltaTime;
            transform.position = transform.position;
            if (particleEffect.transform.position.y <= -10f)
            {
                ready = true;
                particleEffect.transform.position = new Vector3(Location.x, previousParticlePosY, Location.z);
                GlobalManager.globalManager.decrementSouls(price);
            }
        }

    }
}