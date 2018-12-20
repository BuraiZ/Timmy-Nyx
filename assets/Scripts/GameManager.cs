using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NyxForm {
    normal,
    cube,
    sphere,
    spider,
    shadow,
    Max
}

public class GameManager : MonoBehaviour {
    public Timmy timmy;
    public NyxStamina nyxContainer;
    public GameObject pauseMenu;
    private Nyx[] nyxs;
    private CameraFollow camera;
    private bool isPlayingTimmy;
    private NyxForm selectedNyxForm;
    private LevelManager levelManager;
    public bool paused = false;

    public GameObject blackScreen;
    public GameObject loseMessage;

    [SerializeField]
    private float blackScreenStartTimer = 1;

    [SerializeField]
    private float losePopupTimer = 3;

    [SerializeField]
    private float popupDurationBeforeMainMenu = 2;

    // Use this for initialization
    void Start () {
        levelManager = FindObjectOfType<LevelManager>();
        camera = FindObjectOfType<CameraFollow>();

        nyxs = new Nyx[(int)NyxForm.Max];
        nyxs[(int)NyxForm.normal] = nyxContainer.GetComponentInChildren<NormalNyx>();
        nyxs[(int)NyxForm.cube] = nyxContainer.GetComponentInChildren<CubeNyx>();
        nyxs[(int)NyxForm.sphere] = nyxContainer.GetComponentInChildren<SphereNyx>();
        nyxs[(int)NyxForm.spider] = nyxContainer.GetComponentInChildren<SpiderNyx>();
        nyxs[(int)NyxForm.shadow] = nyxContainer.GetComponentInChildren<ShadowNyx>();
        selectedNyxForm = 0;
        
        for (int i = 0; i < nyxs.Length; i++) {
            nyxs[i].gameObject.SetActive(false);
        }
        nyxContainer.gameObject.SetActive(false);

        isPlayingTimmy = true;
        timmy.IsSelected(true);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("CharacterChange") && timmy._Grounded) {
            CharacterChange();
        }
        CheckGlobalKeys();

    }

    public void CharacterChange() {
        if (isPlayingTimmy) {
            isPlayingTimmy = false;
            timmy.IsSelected(false);
            nyxContainer.gameObject.SetActive(true);
            timmy.GetComponent<Rigidbody>().isKinematic = true;
            timmy.GetComponent<Timmy>().StopHorizontalMovement();
            Vector3 timmyPosition = timmy.GetComponent<Transform>().position;
            nyxContainer.GetComponent<Transform>().position = new Vector3(timmyPosition.x - 2, timmyPosition.y, timmyPosition.z);
            nyxs[(int)selectedNyxForm].IsActive(true);
            SetNyxSelected(true);
            camera.SwitchFocusTo(nyxs[(int)selectedNyxForm].gameObject);
        } else {
            nyxs[(int)selectedNyxForm].Unpossession();

            // drop item when changing back to Timmy
            Transform pickup = nyxContainer.GetComponentInChildren<NormalNyx>().holdPos;
            if (pickup.childCount > 0) {
                pickup.GetComponentInChildren<Item>().Drop();
            }

            isPlayingTimmy = true;
            timmy.GetComponent<Rigidbody>().isKinematic = false;
            timmy.IsSelected(true);
            SetNyxSelected(false);
            camera.SwitchFocusTo(timmy.gameObject);
            nyxContainer.gameObject.SetActive(false);
            nyxs[(int)selectedNyxForm].UpdateParentPosition();
        }
    }

    public void ChangeNyxFormTo(NyxForm form) {
        selectedNyxForm = form;
        camera.SwitchFocusTo(nyxs[(int)selectedNyxForm].gameObject);
        nyxs[(int)form].IsActive(true);
    }

    public void ShadowMode()
    {
        if(selectedNyxForm != NyxForm.shadow)
        {
            ChangeNyxFormTo(NyxForm.shadow);

        }else if(selectedNyxForm == NyxForm.shadow)
        {
            ChangeNyxFormTo(0);
        }
    }

    private void SetNyxSelected(bool isSelected) {
        foreach (Nyx form in nyxs) {
            form.IsSelected(isSelected);
        }
    }

    public NyxForm GetNyxSelected()
    {
        return selectedNyxForm;
    }

    public bool IsPlayingTimmy() {
        return isPlayingTimmy;
    }

    private void CheckGlobalKeys()
    {
        if (Input.GetButtonDown("Pause")) {
            HandlePause();
        }
    }

    public void HandlePause() {
        if (paused) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        } else {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        paused = !paused;
    }

    public void StartActivateCoroutine(GameObject obj, float timerInSeconds) {
        StartCoroutine(Activate(obj, timerInSeconds));
    }

    public void TriggerLoseCondition() {
        StartCoroutine(DisplayLose());
    }

    IEnumerator DisplayLose() {
        yield return new WaitForSeconds(blackScreenStartTimer);
        blackScreen.SetActive(true);
        yield return new WaitForSeconds(losePopupTimer);
        loseMessage.SetActive(true);
        yield return new WaitForSeconds(popupDurationBeforeMainMenu);
        levelManager.LoadMenuScene();
    }

    IEnumerator Activate(GameObject obj, float timerInSeconds) {
        yield return new WaitForSeconds(timerInSeconds);
        obj.SetActive(true);
    }
}
