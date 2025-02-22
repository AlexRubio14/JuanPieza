using UnityEngine;
using static RaftController;

public class controllerPirateBoarding : MonoBehaviour
{
    public bool isBoarding = false;

    private Vector3 posToJump;
    [SerializeField] private float jumpHeigt;
    [SerializeField] private Rigidbody rb;

    public enum PirateState { IDLE, PARABOLA, BOARDING, DEAD }

    public PirateState currentState { get; private set; } = PirateState.IDLE;


    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JumpIntoPlayerShip()
    {

        
    }

    public void ChangeState(PirateState newState)
    {

        switch (currentState)
        {
            case PirateState.IDLE:
                break;
            case PirateState.PARABOLA:
                rb.useGravity = false;
                break;
            case PirateState.BOARDING:
                break;
            case PirateState.DEAD:
                break;
            default:
                break;
        }

        switch (newState)
        {
            case PirateState.IDLE:
                break;
            case PirateState.PARABOLA:
                rb.useGravity = true;
                rb.isKinematic = false;
                JumpIntoPlayerShip();
                break;
            case PirateState.BOARDING:
                break;
            case PirateState.DEAD:
                rb.isKinematic = true;
                break;
            default:
                break;
        }

        currentState = newState;
    }

    private void ResetPirate()
    {
        //foreach (controllerPirateBoarding controller in pirates)
        //{
        //    controller.transform.SetParent(managerPirateBoarding.Instance.piratesHolder, true);
        //    controller.transform.position = managerPirateBoarding.Instance.piratesHolder.transform.position;
        //}
    }

    public void SetPosToJump(Vector3 _posToJump)
    {
        posToJump = _posToJump;
    }
}
