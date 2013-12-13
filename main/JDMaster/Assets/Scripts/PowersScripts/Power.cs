using UnityEngine;
using System.Collections;

//All the states of a power, used by PowerManager (you can still use it to force disable a power).
public enum PowerState
{
	ForceDisabled, //A power is permanently disabled, even if there are enough souls to use it
	Disabled, //A power is disabled, because there are not enough souls to use it
	Enabled, //A power is ready to be used
	Active //A power is being used by the player
}

/* Power is the base class for every Power to be used in the level, it defines some common properties between all the powers.
 * public string name; this is the name in the atlas without ".png" and "_over"
 * public int price; the amount of souls needed to use this power
 * public float scale; the scale of the power collider -> Not used right now
 * public PowerState powerState; The state of the power
 * To define a usable a power create a script that derives from this class and assign it to the powers array in the Powers Manager script in _Global
 * assign a price and a name (it must be in the powers atlas used by the GUILevel script to render the power bar)
 * If you want to force disable a power at runtime set it's powerState variable to PowerState.ForceDisabled and call refreshPowerState (defined in PowerManager), 
 * to reenable a power set it's state to PowerState.Disabled and call refreshPowerState again
 * A power script must be disabled in the editor, PowerManager will enable it when it has been activated from the power bar and will also disable it when it gets deactivated.
 * Implement the OnEnable() and OnDisable() functions to do initialization/deinizialization and to enable/disable all the renderers used by the power (to avoid wasting draw calls).
 * For a working implementation take a look at PowerRain.
 */
public abstract class Power : MonoBehaviour 
{
	public string name; 
	public int price; 
	//public float scale; 
	public PowerState powerState = PowerState.Disabled;
	public abstract void deliverPowerEffects(PersonStatus status, AnimationScript animator, UnitNavigationController navigator);
	public abstract void OnTriggerEnter(Collider other);
}
