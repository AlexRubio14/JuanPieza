using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ShipEnemy : Ship
{
    [Header("Velocity")]
    [SerializeField] private float velocity;
    private bool isArriving;
    private float t;
    private float startZ;

    public override void Start()
    {
        base.Start();

        t = 0;
        startZ = transform.position.z;
    }

    protected override void Update()
    {
        base.Update();

        if (isArriving)
            Arrive();
    }

    private void Arrive()
    {
        t += Time.deltaTime * velocity;
        float newZ = Mathf.Lerp(startZ, 0, t);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);

        if (t >= 1)
        {
            foreach (var enemies in GetComponent<EnemieManager>().GetEnemyList())
            {
                Camera.main.GetComponent<CameraController>().AddPlayer(enemies.gameObject);
                enemies.GetComponent<NavMeshAgent>().enabled = true;
            }

            isArriving = false;
        }
    }

    public void SetIsArriving(bool state)
    {
        isArriving = state;
    }
}
