# Sphere Controller: Physics-Based Interactive Pattern Recognition in Unity

Status: Done

### **Overview**

The **Sphere Controller Project** is a **physics-driven interaction system** in Unity designed to run on Android Phone that integrates **pattern matching, movement tracking, and physics-based gameplay mechanics**. The system enables a sphere to follow a predefined pattern of objects while ensuring **realistic physics-based movement, user interaction, and event-driven logic**.

This project is designed to:

1. Create a dynamic physics-based sphere movement system
2. Allow users to interact with patterns using gestures (swipes or touches)
3. Validate user input against predefined patterns
4. Implement event-driven gameplay mechanics using Unity’s event system
5. Enable tracking and saving of gameplay data for analysis

## **Pipeline Overview**

The core system consists of the following steps:

### **1. Board Initialization**

- Generate and manage **patterns of objects** (interactive elements).
- Store and track **user interactions** for validation.

### **2. Sphere Movement System**

- Move the sphere using **force-based physics interactions**.
- Interpolate movement along predefined waypoints.
- Control **speed, acceleration, and stopping points** dynamically.

### **3. User Interaction & Pattern Recognition**

- Capture user input (touch or swipe).
- Compare the **swiped pattern with the actual pattern**.
- Validate the user’s input and trigger appropriate **success or failure events**.

### **4. Event-Driven Gameplay**

- **EventManager** system handles **real-time event triggers**.
- Success and failure states are dynamically updated based on **user interaction with objects**.

### **5. Data Tracking & Storage**

- Logs gameplay interactions and pattern recognition results.
- Saves data in CSV format for post-game analysis.


## **Implementation Details**

### **1. Grid-Based World Generation**

The game world consists of a **grid-like structure** where objects (cubes) are instantiated at fixed positions. This allows for **pattern-based interaction** where the player must follow a specific sequence.

### **Grid Creation Code (`AddButtons.cs`)**

```jsx
using UnityEngine;

public class AddButtons : MonoBehaviour {

	[SerializeField] public GameObject cube; 
	[SerializeField] float xoffset = 0.5f;
	[SerializeField] float yoffset = 0.5f;

	int worldWidth  = 6;
	int worldHeight  = 3;
	int cubeNum = 0;

	void Awake () {
		CreateWorld ();
	}

	void CreateWorld () {
		for(int y = 0; y < worldWidth; y++) {
			for(int x = 0; x < worldHeight; x++) {
				GameObject c1 = Instantiate(cube, cube.transform.position + new Vector3(x * xoffset, -y * yoffset), cube.transform.rotation);
				cubeNum += 1;
				c1.name = "" + cubeNum;
			}
		}
	}
}

```

1. **Dynamically instantiates a grid of objects**
2. **Assigns unique names to objects for pattern recognition**

### **2. Pattern Recognition & Validation**

In this project, **patterns** are defined as a sequence of objects (cubes) that the player must interact with in the correct order. The **Board system** manages pattern tracking, comparison, and validation.

### **Pattern Matching Workflow**

1. **Predefined sequences of objects** (patterns) are created.
2. The player interacts with objects in a **swipe or touch-based manner**.
3. Each interaction is **logged and compared** to the expected pattern.
4. The system determines if the **user’s input matches the correct sequence**.
5. **Success or failure events** are triggered based on pattern correctness.

### **2.1 Managing Patterns (`Board.cs`)**

The `Board` class stores and manages pattern sequences while handling **pattern validation logic**.

### **Core Responsibilities of `Board.cs`:**

- Stores **pattern sequences** for recognition.
- Manages **user interactions** to compare them with stored sequences.
- Handles **event triggering** for correct or incorrect matches.

```jsx
using System;
using System.Collections.Generic;
using UnityEngine;

public class Board {
	List<Pattern> patterns = new List<Pattern>();  // Stores predefined patterns
	List<Obstacle> obstacles = new List<Obstacle>();  // Optional obstacles
	public List<string> swipedPatterns = new List<string>();  // User's input sequence
	public List<string> actualPatterns = new List<string>();  // Correct sequence
	private int matchingIndex = 0;

	public Board() {
		matchingIndex = 0;
		LoadLinkedList();
	}

	public void AddPattern(Pattern pattern) {
		patterns.Add(pattern);
		actualPatterns.Add(pattern.getName());
	}

	public void AddObstacle(Obstacle obstacle) {
		obstacles.Add(obstacle);
	}

	public void MatchPatterns(GameObject go, Vector3 pos) {
		int number = Int32.Parse(go.name);

		// Validate user input sequence
		if (number > 9) {
			if (matchingIndex == 0) {
				matchingIndex += 1;	
			}
		} else {
			matchingIndex = 0;
		}

		// Store interaction
		GameData.instance.LoadToTemp(ChunkToSave(go, pos));
		swipedPatterns.Add(go.name);
	}

	public bool PatternsMatch() {
		if (swipedPatterns.Count != 0 && actualPatterns.Count != 0) {
			return swipedPatterns.SequenceEqual(actualPatterns);
		}
		return false;
	}
}

```

1. **Tracks both correct and user-input patterns**
2. **Compares sequences and determines success/failure**
3. **Stores input for analytics and debugging**

### **2.2 Defining Patterns (`Pattern.cs`)**

A pattern is a collection of GameObjects (cubes) representing a **predefined movement sequence**.

```jsx
using System.Collections.Generic;
using UnityEngine;

public class Pattern {
    public List<GameObject> sequence { get; private set; }
    public string name { get; private set; }

    public Pattern(List<GameObject> sequence, string name) {
        this.sequence = sequence;
        this.name = name;
    }

    public string getName() {
        return this.name;
    }
}

```

1. **Encapsulates a sequence of objects**
2. **Provides a method to retrieve pattern names**

### **2.3 User Input Handling & Pattern Matching (`GameLogic.cs`)**

The `GameLogic` class processes **touch or swipe gestures** and checks if the player's actions match the expected pattern.

### **User Input Workflow**

1. **Detect user input** via touch or mouse interaction.
2. **Identify which object was touched/swiped.**
3. **Log and compare interactions** against the correct sequence.
4. **Trigger event-based feedback (success or failure).**

**Touch Logic & Event Triggering**

```jsx
using UnityEngine;
using System;
using System.Collections.Generic;

public class GameLogic {
	public static GameObject previousGO = null;

	public void TouchLogic(Board board) {
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
			pos.z = 0;

			// Detects collisions with game objects (cubes)
			Collider2D[] currentFrame = Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y), LayerMask.GetMask("Cube"));

			foreach (Collider2D c2 in currentFrame) {
				GameObject go = c2.gameObject;
				if (previousGO != go) {
					board.MatchPatterns(go, pos);
					previousGO = go;

					// Trigger pattern match event
					EventManager.TriggerEvent("matches");

					// Change color to indicate interaction
					go.GetComponent<SpriteRenderer>().material.color = Color.red;
					Debug.Log("Object touched: " + go.name);
				}
			}
		}
	}
}

```

1. **Processes touch inputs dynamically**
2. **Detects interactions with objects**
3. **Triggers events upon object selection**

### **2.4 Validating Patterns & Triggering Events (`Main.cs`)**

After the user has interacted with the pattern, we need to **check if the sequence is correct** and handle the **game response**accordingly.

### **Pattern Validation Logic**

```jsx
void Update() {
	if (rep <= totalRepetition) {
		if (Main.enableTouch) {
			waitText = "Start";
			gl.TouchLogic(GetBoard());
		} else {
			waitText = "Wait";
		}
	} else {
		Debug.Log("Game Over triggered");
		EventManager.TriggerEvent("gameover");
	}
}
```

**Event Handling: Correct vs Incorrect Patterns**

```jsx
void NextBoard() {
	StartCoroutine(ShowMessEnumerator("Correct Pattern!"));
	right = true;
	playerPoints += 5;
	ClearBoard();
	level += 1;
	InitBoard();	
}

public void ReloadLevel() {
	StartCoroutine(ShowMessEnumerator("Wrong Pattern"));
	right = false;
	ClearBoard();
	GetBoard().LoadLinkedList();
	SphereController.instance.SetBoard(GetBoard());
}
```

1. **Tracks player progress and level advancement**
2. **Handles correct vs incorrect interactions using event-driven logic**

## **Summary of Pattern Recognition System**

| **Component** | **Purpose** |
| --- | --- |
| **`Board.cs`** | Stores, tracks, and validates pattern sequences. |
| **`Pattern.cs`** | Defines patterns as a collection of objects. |
| **`GameLogic.cs`** | Captures user input, detects object interactions, and triggers events. |
| **`Main.cs`** | Handles pattern validation, level progression, and feedback. |

## **3. Sphere Movement System (Physics-Based)**

The **Sphere Movement System** is designed to move a sphere along a **predefined path of interactive objects** while ensuring **realistic physics-based movement**.

### **Key Features of the Movement System**

1. Uses **Unity’s Rigidbody physics** for natural rolling behavior.
2. Moves **between pattern-defined points** dynamically.
3. Controls **speed, acceleration, and stopping conditions**.
4. Handles **interpolation for smooth transitions**.

### **3.1 Sphere Movement Logic (`SphereController.cs`)**

The sphere follows a series of **waypoints (interPoints)** defined by the objects in the pattern. It moves between these points at a configurable speed.

### **Sphere Movement Code**

```jsx
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {
	
	public GameObject sphere;
	public static SphereController instance = null;
	public Board currentBoard = null;
	public List<Vector3> interPoints = new List<Vector3>();
	public float speed;

	void Awake () {
		if (instance == null) {
			instance = this;
		}else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {
		speed = (float) MainMenuButtons.speed;
		sphere = Instantiate(sphere, sphere.transform.position, sphere.transform.rotation);
		sphere.GetComponent<MeshRenderer>().material.color = Color.red;
	}

	public void SetBoard (Board board) {
		currentBoard = board;
		InterpolateDataPoints();
		ResetSphere();
	}

	void InterpolateDataPoints() {
		Vector3 temp = new Vector3();
		interPoints.Clear();
		int i = 0;

		// Generates a smooth path between interaction points
		foreach (GameObject go in currentBoard.allPatterns) {
			Vector3 current = go.transform.position;
			if (i != 0) {
				interPoints.Add(Vector3.Lerp(temp, current, 0.20f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.40f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.60f));
				interPoints.Add(Vector3.Lerp(temp, current, 0.80f));
			}
			temp = current;
			i++;
		}
		interPoints.Add(temp);
	}

	void ResetSphere() {
		StartCoroutine(MoveSphere());
	}

	private IEnumerator MoveSphere() {
		yield return new WaitForSeconds(2f);
		int i = 0;
		foreach (Vector3 points in interPoints) {
			if (i >= 10) {
				Main.enableTouch = true;  // Allows player input
			} else {
				Main.enableTouch = false;
			}
			sphere.transform.position = points;
			i++;
			yield return new WaitForSeconds(speed);
		}
	}
}

```

1. **Uses `Vector3.Lerp()` for smooth transitions**
2. **Interpolates movement between interaction points**
3. **Controls speed and physics-based motion**

### **3.2 Handling Sphere Physics (`Rigidbody` & Drag Control)**

To maintain **realistic motion**, the sphere's **Rigidbody component** is adjusted for:

- **Mass:** Ensures a smooth rolling motion.
- **Drag:** Prevents excessive acceleration.
- **Angular Drag:** Controls spin speed.

### **Recommended Rigidbody Settings**

| **Property** | **Value** | **Effect** |
| --- | --- | --- |
| `mass` | `1` | Standard weight for a rolling object. |
| `drag` | `0.5` | Prevents excessive acceleration. |
| `angularDrag` | `0.1` | Controls rotational speed. |
| `freezeRotation` | `false` | Allows natural rolling behavior. |

### **3.3 Future Expansions for Movement System**

🚀 **Enhancements include:**

1. **AI-controlled sphere navigation**
2. **Obstacle-based physics interactions**
3. **Gravity-based terrain effects (ramps, slopes, and jumps)**

## **4. Event-Driven Gameplay (Dynamic Game Flow Management)**

Unity’s **EventManager** system is used to handle real-time **gameplay events** such as:

- **Correct Pattern Matches**
- **Incorrect Pattern Selections**
- **Game Completion & Restart**

### **4.1 Event System (`EventManager.cs`)**

The **EventManager** allows different game components to communicate efficiently without direct dependencies.

### **EventManager Implementation**

```jsx
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	private Dictionary<string, UnityEvent> eventDictionary;
	private static EventManager eventManager;

	public static EventManager instance {
		get {
			if (!eventManager) {
				eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
				if (!eventManager) {
					Debug.LogError("No active EventManager in the scene.");
				} else {
					eventManager.Init();
				}
			}
			return eventManager;
		}
	}

	void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener) {
		if (eventManager == null) return;
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.Invoke();
		}
	}
}
```

1. **Modular event-based communication**
2. **Eliminates dependencies between game components**

### **4.2 Game Response to Events (`Main.cs`)**

The `Main` script listens for success and failure events, updating the game state accordingly.

### **Handling Game Progression**

```jsx
void Update() {
	if (rep <= totalRepetition) {
		if (Main.enableTouch) {
			waitText = "Start";
			gl.TouchLogic(GetBoard());
		} else {
			waitText = "Wait";
		}
	} else {
		Debug.Log("Game Over triggered");
		EventManager.TriggerEvent("gameover");
	}
}
```

**Handling Success & Failure**

```jsx
void NextBoard() {
	StartCoroutine(ShowMessEnumerator("Correct Pattern!"));
	right = true;
	playerPoints += 5;
	ClearBoard();
	level += 1;
	InitBoard();	
}

public void ReloadLevel() {
	StartCoroutine(ShowMessEnumerator("Wrong Pattern"));
	right = false;
	ClearBoard();
	GetBoard().LoadLinkedList();
	SphereController.instance.SetBoard(GetBoard());
}
```

1. **Listens for pattern match events**
2. **Handles game state updates dynamically**

## **5. Data Tracking & Storage (Gameplay Analytics)**

To analyze **player performance**, gameplay interactions are logged in CSV files.

### **5.1 Data Logging (`GameData.cs`)**

```jsx
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
    public static GameData instance = null;
    public static List<string> tempData = new List<string>();

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadToTemp(string data) {
        tempData.Add(data);
    }

    public void SaveToFile(string Location) {
        for (int i = 0; i < tempData.Count; i++) {
            File.AppendAllText(Location, tempData[i] + Environment.NewLine);
        }
        tempData.Clear();
    }
}
```

1. **Stores interactions for analysis**
2. **Supports debugging and future AI training**

## **Next Steps:**

🚀 **Expand with:**

- [ ]  **Multiplayer Mode & AI Opponents**
- [ ]  **Advanced Obstacle Avoidance System**
- [ ]  **Augmented Reality (AR) Integration**
