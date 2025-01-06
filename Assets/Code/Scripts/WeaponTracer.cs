using UnityEngine;

public class WeaponTracer : MonoBehaviour
{

    [SerializeField]
    private LayerMask hitLayers;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform starterPos;

    [Space, SerializeField]
    private int maxSteps;
    [SerializeField]
    private float stepSize;

    private Weapon weapon;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (weapon.isBeingUsed)
            PredictTrajectory();
         
        lineRenderer.enabled = weapon.isBeingUsed;
    }


    public void PredictTrajectory()
    {
        Vector3 velocity = weapon.bulletForce / rb.mass * starterPos.forward;
        Vector3 position = starterPos.position;
        UpdateLineRenderer(1, (0, position));
        for (int i = 1; i <= maxSteps; i++)
        {
            velocity = CalculateNewVelocity(velocity, rb.linearDamping, Time.fixedDeltaTime);
            Vector3 nextPosition = position + velocity * Time.fixedDeltaTime;

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, 5f, hitLayers))
            {
                if(i != 0)
                    UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }

            position = nextPosition;

            UpdateLineRenderer(maxSteps, (i, position));
        }

        
    }

    private Vector3 CalculateNewVelocity(Vector3 _velocity, float _drag, float _increment)
    {
        _velocity += Physics.gravity * _increment;
        _velocity *= Mathf.Clamp01(1 - _drag * _increment);
        return _velocity;
    }

    private void UpdateLineRenderer(int _count, (int point, Vector3 pos) _pointPos)
    {
        if (_pointPos.point >= _count)
            return;

        lineRenderer.positionCount = _count;
        lineRenderer.SetPosition(_pointPos.point, _pointPos.pos);
    }

}
