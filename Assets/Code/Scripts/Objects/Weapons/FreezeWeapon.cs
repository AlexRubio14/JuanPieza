using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FreezeWeapon : MonoBehaviour
{
    [Header("Freeze")]
    [SerializeField] private float freezeMaxTime;
    private float freezeCurrentTime;
    private bool freeze;
    private Weapon weapon;

    [Header("Ice")]
    [SerializeField] private GameObject iceCube;
    [SerializeField] private Vector3 initPosition;
    [SerializeField] private float maxScale;
    [SerializeField] private float scaleTime;
    [SerializeField] private AudioClip breakIce;
    [SerializeField] private GameObject breakIceParticles;
    private ParticleSystem iceParticles;
    private GameObject iceCubeWeapon;

    [Header("Material")]
    [SerializeField] private GameObject fixedWeapon;
    [SerializeField] private GameObject brokenWeapon;
    [SerializeField] private Color freezeColor;
    private Color brokenColor;
    private Material breakMaterial;
    private Material fixedMaterial;

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        fixedMaterial = new Material(fixedWeapon.GetComponentInChildren<MeshRenderer>().material);
        breakMaterial = new Material(brokenWeapon.GetComponent<MeshRenderer>().material);

        foreach(MeshRenderer meshes in fixedWeapon.GetComponentsInChildren<MeshRenderer>())
            if(meshes.materials.Length == 1)
                meshes.material = fixedMaterial;

        brokenWeapon.GetComponent<MeshRenderer>().material = breakMaterial;
        brokenColor = breakMaterial.color;
    }

    private void Update()
    {
        Freeze();
    }

    private void Freeze()
    {
        if (!freeze)
            return;

        ChangeWeaponColor();

        if (freezeCurrentTime > freezeMaxTime && !weapon.GetFreeze())
        {
            weapon.SetFreeze(true);
            weapon.UnMountPlayer();
            GenerateIceCube();
        }
    }

    private void ChangeWeaponColor()
    {
        freezeCurrentTime += Time.deltaTime;
        float t = freezeCurrentTime / freezeMaxTime;
        Color fixedColorLerp = Color.Lerp(Color.white, freezeColor, t);
        Color brokenColorLerp = Color.Lerp(brokenColor, freezeColor, t);
        fixedMaterial.color = fixedColorLerp;
        breakMaterial.color = brokenColorLerp;
    }

    public void BreakIce()
    {
        if (!freeze)
            return;
        weapon.SetFreeze(false);
        freezeCurrentTime = 0;
        if(iceCubeWeapon != null)
            Destroy(iceCubeWeapon);
        fixedMaterial.color = Color.white;
        breakMaterial.color = brokenColor;
        weapon.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        AudioManager.instance.Play2dOneShotSound(breakIce, "Objects");
        
        Instantiate(breakIceParticles, transform.position + Vector3.up, Quaternion.identity);
    }

    private IEnumerator ScaleIce()
    {
        float currentTime = 0f;

        while (currentTime < scaleTime)
        {
            if(iceCubeWeapon != null)
                iceCubeWeapon.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(maxScale, maxScale, maxScale), currentTime / scaleTime);
            
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private void GenerateIceCube()
    {
        iceCubeWeapon = Instantiate(iceCube);
        iceCubeWeapon.transform.SetParent(this.transform, true);
        iceCubeWeapon.transform.localScale = Vector3.zero;
        iceCubeWeapon.transform.localPosition = initPosition;
        iceCubeWeapon.transform.localRotation = Quaternion.identity;
        StartCoroutine(ScaleIce());
    }

    public void SetFreeze(bool _freeze)
    { 
        freeze = _freeze;
    }
}
