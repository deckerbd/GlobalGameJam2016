using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WitchController : MonoBehaviour {

	public float movementModifier = 2.5f;
	public float climbModifier = 2.5f;

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

	public GameObject beanVine;

	public bool climbing;

	private int totalBeanCount, totalCornCount;

	public string spawnType;
	public int currentMaxSeeds;

	public AudioClip[] mousePickup;
	public AudioClip[] mouseUse;
	public AudioClip[] mouseProgress;
	public AudioClip[] mouseReset;

	public AudioClip[] cornPickup;
	public AudioClip[] cornRefuse;
	public AudioClip[] cornUse;
	public AudioClip[] cornDie;

	public AudioClip[] beanPickup;
	public AudioClip[] beanUse;
	public AudioClip[] beanRefuse;

	// Use this for initialization
	void Start () {
		beanSeeds = new Queue ();
		cornSeeds = new Queue ();
		seedFollowers = new ArrayList ();

		seedImage = GameObject.Find ("SelectedSeed").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		ClimbCheck ();
		SeedSelectionManager ();
		if (climbing) {
			ClimbManager ();
		} else {
			InputManager ();
			JumpManager ();
		}
	}

	void ClimbManager(){
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Input.GetAxis ("Horizontal") * movementModifier, Input.GetAxis("Vertical") * climbModifier);
		}
	}

	void InputManager(){
		//NON-CLIMBING MOVEMENT
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

		if(Input.GetButtonDown("Reset")){
			ResetLevel();
		}

		//SEED USE
		if (!inSpawn) {
			if (Input.GetButtonDown ("Seed Action")) {
				PlantSeed ();
			}
		}

		//SEED PLUCK
		if (inSpawn) {
			if (Input.GetButtonDown ("Seed Action")) {

				ArrayList seedArray = new ArrayList();

				if (spawnType == "Bean") {
					foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper> ()) {
						if (sh.bean) {
							seedArray.Add (sh.gameObject);
						}
					}
					this.GetComponent<AudioSource> ().clip = beanPickup [Random.Range (0, beanPickup.Length)];
					this.GetComponent<AudioSource> ().Play ();
				} else if (spawnType == "Corn") {
					foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper>()) {
						if (sh.corn) {
							seedArray.Add (sh.gameObject);
						}
					}
					this.GetComponent<AudioSource> ().clip = cornPickup [Random.Range (0, cornPickup.Length)];
					this.GetComponent<AudioSource> ().Play ();
				}

				if (seedArray.Count < currentMaxSeeds) {
					AddFollower (spawnType);
				}else {
					RemoveFollower (spawnType);
					AddFollower (spawnType);
				}
			}
		}


	}

	void SeedSelectionManager(){
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
	}

	void PlantSeed(){
		if (seedTypes [seedIndex] == "Bean") {
			if (CanRemoveFollower ("Bean")) {
				Vector3 newPos;

				GameObject bgToGrow = null;

				if (this.transform.localScale.x > 0) {
					newPos = (this.transform.position + new Vector3 (1f, 0, 0));
				} else {
					newPos = (this.transform.position + new Vector3 (-1f, 0, 0));
				}

				BoxCollider2D[] boxes = GameObject.FindObjectsOfType<BoxCollider2D> ();
				ArrayList possibleBGs = new ArrayList ();

				foreach (BoxCollider2D b in boxes) {
					if (b.bounds.Contains (newPos) && b.gameObject.tag == "BackgroundPlatform") {
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

				if (possibleBGs.Count == 0) {
					return;
				}

				//Turn off bean

				GameObject originalBean = (GameObject)beanSeeds.Dequeue ();
				seedFollowers.Remove (originalBean);

				ArrayList tempArray = new ArrayList ();

				foreach (object obj in seedFollowers) {
					if (obj != null && ((GameObject) obj).GetComponent<SpriteRenderer>().enabled == true) {
						tempArray.Add (obj);
					}
				}

				seedFollowers = tempArray;

				for (int i = 0; i < seedFollowers.Count; i++) {
					GameObject beanGobj = seedFollowers[i] as GameObject;
					beanGobj.GetComponent<SeedFollow> ().positionInQueue = i;
				}

				originalBean.GetComponent<Renderer> ().enabled = false;
				originalBean.GetComponent<SeedFollow> ().enabled = false;

				if (possibleBGs.Count > 0) {
					GameObject vine = (GameObject)Instantiate (beanVine, newPos, this.transform.rotation);
					vine.transform.parent = originalBean.transform;
					vine.GetComponent<VineGrow> ().GrowVine (bgToGrow);
					this.GetComponent<AudioSource> ().clip = beanUse [Random.Range (0, beanUse.Length)];
					this.GetComponent<AudioSource> ().Play ();
				}
			}
		}
		if (seedTypes [seedIndex] == "Corn") {
			if (CanRemoveFollower ("Corn")) {
				
				Vector3 newPos;

				if (this.transform.localScale.x > 0) {
					newPos = (this.transform.position + new Vector3 (1f, 0, 0));
				} else {
					newPos = (this.transform.position + new Vector3 (-1f, 0, 0));
				}

				foreach (moundScript ms in GameObject.FindObjectsOfType<moundScript>()) {

					Debug.Log (ms.name);

					if (ms.GetComponent<BoxCollider2D> ().bounds.Contains (newPos)) {
						Debug.Log ("I did it");
						GameObject originalCorn = (GameObject)cornSeeds.Dequeue ();
						seedFollowers.Remove (originalCorn);

						ArrayList tempArray = new ArrayList ();

						foreach (object obj in seedFollowers) {
							if (obj != null && ((GameObject) obj).GetComponent<SpriteRenderer>().enabled == true) {
								tempArray.Add (obj);
							}
						}

						seedFollowers = tempArray;

						for (int i = 0; i < seedFollowers.Count; i++) {
							GameObject seedGobj = seedFollowers[i] as GameObject;
							seedGobj.GetComponent<SeedFollow> ().positionInQueue = i;
						}

						GameObject stalk = (GameObject)Instantiate (ms.cornstalk, ms.gameObject.transform.position, this.transform.rotation);
						stalk.transform.parent = originalCorn.transform;
						this.GetComponent<AudioSource> ().clip = cornUse [Random.Range (0, cornUse.Length)];
						this.GetComponent<AudioSource> ().Play ();
						originalCorn.GetComponent<Renderer> ().enabled = false;
						originalCorn.GetComponent<SeedFollow> ().enabled = false;
					}
				}

			}
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
			beanToAdd.GetComponent<SeedHelper> ().seedNumber = totalBeanCount + 1;
			totalBeanCount++;
			beanToAdd.GetComponent<SeedFollow>().positionInQueue = seedFollowers.Count;
			beanSeeds.Enqueue(beanToAdd);
			seedFollowers.Add (beanToAdd);
		}
		if (followerType == "Corn") {
			GameObject cornToAdd = Instantiate (cornPrefab, this.transform.position, this.transform.rotation) as GameObject;
			cornToAdd.GetComponent<SeedHelper> ().seedNumber = totalCornCount + 1;
			totalCornCount++;
			cornToAdd.GetComponent<SeedFollow> ().positionInQueue = seedFollowers.Count;
			cornSeeds.Enqueue (cornToAdd);
			seedFollowers.Add (cornToAdd);
		}
	}

	public void RemoveFollower(string followerType){
		if (followerType == "Bean") {
			GameObject deadBean = null;

			ArrayList possibleBeans = new ArrayList ();

			foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper>()) {
				if (sh.bean) {
					possibleBeans.Add (sh);
				}
			}

			int lowestBeanVal = 1000;

			foreach (Object obj in possibleBeans) {
				SeedHelper sh = (obj as SeedHelper);
				if (sh.seedNumber < lowestBeanVal) {
					deadBean = sh.gameObject;
					lowestBeanVal = sh.seedNumber;
				}
			}

			if (beanSeeds.Contains (deadBean)) {
				beanSeeds.Dequeue ();
			}

			seedFollowers.Remove (deadBean);
			ArrayList tempArray = new ArrayList ();

			foreach (object obj in seedFollowers) {
				if (obj != null) {
					tempArray.Add (obj);
				}
			}

			seedFollowers = tempArray;

			GameObject.Destroy (deadBean);

			for (int i = 0; i < seedFollowers.Count; i++) {
				GameObject beanGobj = seedFollowers[i] as GameObject;
				beanGobj.GetComponent<SeedFollow> ().positionInQueue = i;
			}

			Debug.Log (beanSeeds.Count);

		}

		if (followerType == "Corn") {
			GameObject deadCorn = null;

			ArrayList possibleCorns = new ArrayList ();

			foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper>()) {
				if (sh.corn) {
					possibleCorns.Add (sh);
				}
			}

			int lowestcornVal = 1000;

			foreach (Object obj in possibleCorns) {
				SeedHelper sh = (obj as SeedHelper);
				if (sh.seedNumber < lowestcornVal) {
					deadCorn = sh.gameObject;
					lowestcornVal = sh.seedNumber;
				}
			}

			if (cornSeeds.Contains (deadCorn)) {
				cornSeeds.Dequeue ();
			}

			seedFollowers.Remove (deadCorn);
			ArrayList tempArray = new ArrayList ();

			foreach (object obj in seedFollowers) {
				if (obj != null) {
					tempArray.Add (obj);
				}
			}

			seedFollowers = tempArray;

			GameObject.Destroy (deadCorn);

			for (int i = 0; i < seedFollowers.Count; i++) {
				GameObject cornGobj = seedFollowers[i] as GameObject;
				cornGobj.GetComponent<SeedFollow> ().positionInQueue = i;
			}

			Debug.Log (cornSeeds.Count);
		}
	}

	public bool CanRemoveFollower(string followerType){
		if (followerType == "Bean") {

			ArrayList allBeans = new ArrayList ();
			foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper>()) {
				if (sh.bean) {
					allBeans.Add (sh);
				}
			}
			if (allBeans.Count > 0) {
				return true;
			} else {
				return false;
			}
		} else if (followerType == "Corn") {

			ArrayList allCorn = new ArrayList ();
			foreach (SeedHelper sh in GameObject.FindObjectsOfType<SeedHelper>()) {
				if (sh.corn) {
					allCorn.Add (sh);
				}
			}
			if (allCorn.Count > 0) {
				return true;
			} else {
				return false;
			}
		} else
			return false;
	}

	void ClimbCheck(){
		foreach (Climbable climb in GameObject.FindObjectsOfType<Climbable>()) {
			GameObject gobj = climb.gameObject;

			if (gobj.GetComponent<BoxCollider2D> ().bounds.Contains (this.transform.position)) {
				foreach (GameObject gobj2D in GameObject.FindGameObjectsWithTag("PassthroughPlatform")) {
					gobj2D.GetComponent<BoxCollider2D> ().enabled = false;				
				}
				climbing = true;
				return;
			}
		}
		foreach (GameObject gobj2D in GameObject.FindGameObjectsWithTag("PassthroughPlatform")) {
			gobj2D.GetComponent<BoxCollider2D> ().enabled = true;
		}
		climbing = false;
	}

	public void ResetLevel(){
		StartCoroutine ("ResetLevelWait");
	}

	IEnumerator ResetLevelWait(){
		this.GetComponent<AudioSource> ().clip = mouseReset [Random.Range (0, mouseReset.Length)];
		this.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (this.GetComponent<AudioSource> ().clip.length);
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
