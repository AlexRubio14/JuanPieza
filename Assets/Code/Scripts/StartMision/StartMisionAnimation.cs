using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMisionAnimation : MonoBehaviour
{
    public enum AnimationState { MOVE_CAMERA, START_TRANSITION, END_TRANSITION, WAIT }
    private AnimationState animationState;

    [Header("Positions")]
    [SerializeField] private List<GameObject> playerPositions;
    [SerializeField] private Vector3 cameraPosition;

    [Header("Camera")]
    [SerializeField] private float cameraZLastPosition;
    [SerializeField] private float startAnimationPlayer;
    [SerializeField] private float stopCamera;
    [SerializeField] private float speed;
    private float initZPosition;
    private Camera animationCamera;
    private bool animationActivated;
    private float t;

    [Header("Transition")]
    [SerializeField] private Image transitionImage;
    [SerializeField] private float transitionDuration = 1f;

    [Header("Canvas")]
    [SerializeField] private QuestBoardObject board;

    private void Start()
    {
        animationCamera = Camera.main;
        animationState = AnimationState.START_TRANSITION;
        animationActivated = true;
        transitionImage.rectTransform.localScale = Vector3.one;
    }

    private void Update()
    {
        switch (animationState)
        {
            case AnimationState.MOVE_CAMERA:
                MoveCamera();
                break;
            case AnimationState.START_TRANSITION:
                TransitionCamera();
                break;
            case AnimationState.END_TRANSITION:
                TransitionCamera();
                break;
        }
    }

    private void MoveCamera()
    {
        t += Time.deltaTime * speed;
        float newZ = Mathf.Lerp(initZPosition, cameraZLastPosition, t);
        animationCamera.transform.position = new Vector3(animationCamera.transform.position.x, animationCamera.transform.position.y, newZ);

        if (t >= startAnimationPlayer && animationActivated)
            ActivePlayerAnimation();
        if (t >= stopCamera)
        {
            animationState = AnimationState.END_TRANSITION;
            t = 0;
        }
    }

    private void TransitionCamera()
    {
        t += Time.deltaTime;
        float progresStart = Mathf.Clamp01(t / transitionDuration);

        if(animationState == AnimationState.START_TRANSITION)
            transitionImage.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, progresStart);
        else
            transitionImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progresStart);

        if (t >= 1)
        {
            t = 0;

            if (animationState == AnimationState.START_TRANSITION)
                animationState = AnimationState.WAIT;
            else
                SceneManager.LoadScene("Battle");
        }
    }

    private void ActivePlayerAnimation()
    {
        foreach (PlayerController playerController in PlayersManager.instance.ingamePlayers)
        {
            playerController.animator.SetTrigger("Celebrate");
        }
        animationActivated = false;
    }

    public void StartAnimation()
    {
        board.GetQuestCanvas().SetActive(false);
        int i = 0;
        foreach ((PlayerInput, SinglePlayerController) player in PlayersManager.instance.players)
            player.Item1.SwitchCurrentActionMap("Dialogue");

        foreach (PlayerController playerController in PlayersManager.instance.ingamePlayers)
        {
            playerController.gameObject.transform.position = playerPositions[i].transform.position;
            playerController.gameObject.transform.rotation = Quaternion.identity;
            playerController.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            i++;
        }

        animationCamera.transform.rotation = Quaternion.identity;
        animationCamera.transform.position = cameraPosition;
        initZPosition = cameraPosition.z;

        animationState = AnimationState.MOVE_CAMERA;
    }
}
