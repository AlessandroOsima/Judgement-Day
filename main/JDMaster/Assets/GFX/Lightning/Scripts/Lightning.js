#pragma strict
var color : Color = Color.white;
var width = 0.5;
var lineRenderer : LineRenderer;
var jump = 1.0;
var dis = 20;
var updateDistance = false;
var maxDistance = 100;
var sparks = false;
var spread = 0.001;
var spark : Transform;
var clampStart = true;
var clampEnd = false;
var runWhileOutOfSight = false;
private var oldPos : Vector3[];
private var a = true;
private var i = 0;
private var totalZ = 0.0;

function Start() {
	lineRenderer = (GetComponent(LineRenderer) as LineRenderer);
	lineRenderer.SetColors(color, color);
	lineRenderer.SetWidth(width,width);
	lineRenderer.SetVertexCount(dis);
}
function FixedUpdate(){
	oldPos = new Vector3[dis];
	transform.BroadcastMessage("DestroySpark", SendMessageOptions.DontRequireReceiver);
	while(totalZ < dis){
		var pos : Vector3;
		pos.x = Random.Range(-jump, jump) * Time.deltaTime;
		pos.y = Random.Range(-jump, jump) * Time.deltaTime;
		pos.z += totalZ;
		oldPos[i] += pos;
		lineRenderer.SetPosition(i, oldPos[i]);
		if(sparks == true){
			if(Random.Range(-1.0, spread) > 0.0){
				var sub : Transform = Instantiate(spark, transform.position, Random.rotation) as Transform;
				var script : SubLightning = sub.GetComponent(SubLightning) as SubLightning;
				sub.parent = transform;
				sub.transform.localPosition = oldPos[i];
				script.color = color;
//				sub.GetComponent("subLightning").color = color;
			}
		}
		totalZ += 1.0;
		i++;
	}
	if(clampStart == true)
		lineRenderer.SetPosition(0, Vector3.zero);
	if(clampEnd == true)
		lineRenderer.SetPosition(i - 1, Vector3(0,0,totalZ - 1.0));
	i = 0;
	totalZ = 0.0;
	if(updateDistance == true)
		UpdateDis();
}

function UpdateDis(){
	var hit : RaycastHit;
	var fwd = transform.TransformDirection (Vector3.forward);
	if (Physics.Raycast (transform.position, fwd, hit, maxDistance)){
		dis = Mathf.Round(hit.distance + 2);
		oldPos = new Vector3[dis];
		lineRenderer.SetVertexCount(dis);
	}
	else{
		dis = maxDistance;
		oldPos = new Vector3[dis];
		lineRenderer.SetVertexCount(dis);
	}
}
/*
function UpdateLights(){
	//destroy old lights
	for(var oldLight : Transform in lights){
		Destroy(oldLight.gameObject);
	}
	//make new lights
	lights = new Transform[Mathf.Round(dis / 3)];
	for(var i : int = 0; i < lights.length; i++){
		var newLight : Transform = Instantiate(ourLight, transform.position, transform.rotation);
		newLight.light.color = color;
		lights[i] = newLight;
	}
}
*/
//get to work the boss is looking
function OnBecameVisible() {
	enabled = true;
}
//why go though the hassel if we cant be seen.  Inless we want to that is.
function OnBecameInvisible () {
	if(runWhileOutOfSight == false)
		enabled = false;
}