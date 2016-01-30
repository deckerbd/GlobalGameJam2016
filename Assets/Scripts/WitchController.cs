using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WitchController : MonoBehaviour {

	public float movementModifier = 2.5f;

	public GameObject groundCheck;
	public float jumpModifier = 200f;

	public bool grounded = true;

	public Queue beanSeeds;
	public Queue cornSeeds;

	public GameObject beanPrefab;
	public GameObject cornPrefab;

	public ArrayList seedFollowers;

	public string[] seedTypes;
	public Sprite[] seedPictures;
	public Image seedImage;

	private int seedIndex;

	public bool inSpawn;

	// Use this for initialization
	void Start () {
		beanSeeds = new Queue ();
		cornSeeds = new Queue ();
		seedFollowers = new ArrayList ();

		seedImage = GameObject.Find ("SelectedSeed").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		InputManager ();
		JumpManager ();
	}

	void InputManager(){
		//MOVEMENT
		if (Input.GetAxis ("Horizontal") != 0) {
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * movementModifier, this.gameObject.GetComponent<Rigidbody2D>().velocity.y);
			if (this.gameObject.GetComponent<Rigidbody2D> ().velocity.x > 0) {
				if (this.gameObject.transform.localScale.x < 0) {
					this.gameObject.transform.localScale = new Vector3 (this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
				}
			} else if (this.gameObject.GetComponent<Rigidbody2D> ().velocity.x < 0){
				if (this.gameObject.transform.localScale.x > 0) {
					this.gameObject.transform.localScale = new Vector3 (this.gameObject.transform.localScale.x * -1, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
				}
			}
		}

		//SEED SELECTION
		if(Input.GetButtonDown("Cycle Left")){
			if(seedIndex - 1 >= seedTypes.Length){
				seedIndex = 0;
			}else if(seedIndex - 1 < 0){
				seedIndex = seedTypes.Length - 1;
			}else{
				seedIndex--;
			}
			seedImage.sprite = seedPictures[seedIndex];
		}else if(Input.GetButtonDown("Cycle Right")){
			if(seedIndex + 1 >= seedTypes.Length){
				seedIndex = 0;
			}else if(seedIndex + 1 < 0){
				seedIndex = seedTypes.Length - 1;
			}else{
				seedIndex++;
			}
			seedImage.sprite = seedPictures[seedIndex];
		}

		//SEED USE
		if (!inSpawn) {
			if (Input.GetButtonDown ("Seed Action")) {
				PlantSeed ();
			}
		}

	}

	void PlantSeed(){


		if (seedTypes [seedIndex] == "Bean") {
			GameObject bgToGrow = null;
			Vector3 newPos = (this.transform.position + new Vector3 (1f, 0, 0));

			BoxCollider2D[] boxes = GameObject.FindObjectsOfType<BoxCollider2D> ();
			ArrayList possibleBGs = new ArrayList();

			foreach (BoxCollider2D b in boxes) {
				if (b.bounds.Contains (newPos)){
					possibleBGs.Add (b.gameObject);
				}
			}

			int largestOrder = -10000;
			foreach (Object obj in possibleBGs) {
				GameObject gobj = obj as GameObject;
				if (gobj.GetComponent<SpriteRenderer> ().sortingOrder > largestOrder) {
					bgToGrow = gobj;
					largestOrder = gobj.GetComponent<SpriteRenderer> ().sortingOrder;
				}
			}

			Debug.Log (bgToGrow.name);

		}

	}

	void JumpManager(){
		JumpChecker ();
		if(grounded){
			if (Input.GetButtonDown ("Jump")) {
				this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
				this.gameObject.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * jumpModifier);
			}
		}
	}

	void JumpChecker(){
		grounded = Physics2D.Linecast (transform.position, groundCheck.transform.position, 1 << LayerMask.NameToLayer ("Walkable"));
	}

	public void AddFollower(string followerType){
		if (followerType == "Bean") {
			GameObject beanToAdd = Instantiate (beanPrefab, this.transform.position, this.transform.rotation) as GameObject;
			beanToAdd.GetComponent<SeedFollow>().positionInQueue = seedFollowers.Count;
			beanSeeds.Enqueue(beanToAdd);
			seedFollowers.Add (beanToAdd);
		}
		if (followerType == "Corn") {
			GameObject cornToAdd = Instantiate (cornPrefab, this.transform.position, this.transform.rotation) as GameObject;
			cornToAdd.GetComponent<SeedFollow> ().positionInQueue = beanSeeds.Count + cornSeeds.Count;
			cornSeeds.Enqueue (cornToAdd);
			seedFollowers.Add (cornToAdd);
		}
	}

	public void RemoveFollower(string followerType){
		if (followerType == "Bean") {
			GameObject deadBean = beanSeeds.Dequeue () as GameObject;
			seedFollowers.Remove (deadBean);
			GameObject.Destroy (deadBean);
			Debug.Log (beanSeeds.Count);
			for (int i = 0; i < seedFollowers.Count; i++) {
				GameObject beanGobj = seedFollowers[i] as GameObject;
				beanGobj.GetComponent<SeedFollow> ().positionInQueue = i;
			}
		}
		if (followerType == "Corn") {

			GameObject deadCorn = cornSeeds.Dequeue () as GameObject;
			seedFollowers.Remove (deadCorn);
			GameObject.Destroy (deadCorn);
			for (int i = 0; i < seedFollowers.Count; i++) {
				GameObject beanGobj = seedFollowers[i] as GameObject;
				beanGobj.GetComponent<SeedFollow> ().positionInQueue = i;
			}
		}
	}

}
