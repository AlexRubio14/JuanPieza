using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioMixer mixer;

    [Space, Header("2D"), SerializeField]
    private int total2DAS;
    [SerializeField]
    GameObject actions2dASObj;
    private AudioSource[] actions2dAS;

    [Space, Header("3D"), SerializeField]
    private int total3DAS;
    [SerializeField]
    GameObject actions3dASObj;
    private AudioSource[] actions3dAS;

    [SerializeField] public AudioClip seagullClip;
    [SerializeField] public AudioClip musicClip;
    [HideInInspector] public AudioSource seagullAs;
    [HideInInspector] public AudioSource musicAs;
    [HideInInspector] public AudioSource radioAs;
    [HideInInspector] public AudioSource danceAs;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        actions2dAS = new AudioSource[total2DAS];
        actions3dAS = new AudioSource[total3DAS];

        AudioMixerGroup mixerGroup = mixer.FindMatchingGroups("Master")[0];
        for (int i = 0; i < total2DAS; i++)
        {
            actions2dAS[i] = actions2dASObj.AddComponent<AudioSource>();
            actions2dAS[i].playOnAwake = false;
            actions2dAS[i].outputAudioMixerGroup = mixerGroup;
        }

        musicAs = actions2dASObj.AddComponent<AudioSource>();
        musicAs.playOnAwake = false;
        musicAs.outputAudioMixerGroup = mixer.FindMatchingGroups("Game Theme")[0];


        GameObject radioObject = new GameObject("RadioAS");
        radioObject.transform.SetParent(transform, false);
        radioAs = radioObject.AddComponent<AudioSource>();
        radioAs.playOnAwake = false;
        radioAs.outputAudioMixerGroup = mixer.FindMatchingGroups("Radio")[0];

        danceAs = actions2dASObj.AddComponent<AudioSource>();
        danceAs.playOnAwake = false;
        danceAs.loop = true;
        danceAs.outputAudioMixerGroup = mixer.FindMatchingGroups("DanceMusic")[0];

        for (int i = 0; i < total3DAS; i++)
        {
            actions3dAS[i] = actions3dASObj.AddComponent<AudioSource>();
            actions3dAS[i].playOnAwake = false;
            actions3dAS[i].outputAudioMixerGroup = mixerGroup;
        }
    }

    public void StopAllAudio(AudioSource[] exeptions)
    {

        foreach (AudioSource item in actions2dAS)
        {
            bool used = false;

            foreach (AudioSource exeption in exeptions)
            {
                if (item == exeption)
                {
                    used = true;
                }
            }

            if (!used)
            {
                item.Stop();
            }
        }
    }
    public AudioSource GetUnused2dAS() 
    {
        foreach (AudioSource item in actions2dAS)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }
        return null;
    }

    public AudioSource GetUnused3dAS()
    {
        foreach (AudioSource item in actions3dAS)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }
        return null;
    }

    public void Play2dOneShotSound(AudioClip _clip, string mixerGroup, float _volume = 1, float _minPitch = 0.75f, float _maxPitch = 1.25f)
    {
        AudioSource _as = GetUnused2dAS();
        PlayOneShotSound(_as, _clip, mixerGroup, _minPitch, _maxPitch, _volume);
    }

    public void Play3dOneShotSound(AudioClip _clip, string mixerGroup, float _radius, Vector2 _pos, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1)
    {
        AudioSource _as = GetUnused3dAS();
        _as.minDistance = _radius;
        _as.maxDistance = _radius * 5;
        _as.gameObject.transform.position = new Vector3(_pos.x, _pos.y, -10);
        _as.spatialBlend = 1;
        PlayOneShotSound(_as, _clip, mixerGroup, _minPitch, _maxPitch, _volume);
    }

    private void PlayOneShotSound(AudioSource _as, AudioClip _clip, string mixerGroup, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1)
    {
        if (_as != null)
        {
            _as.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroup)[0]; 
            _as.loop = false; 
            _as.pitch = Random.Range(_minPitch, _maxPitch); 
            _as.volume = _volume; 
            _as.PlayOneShot(_clip); 
        }
    }
    
    public void PlayOneShotRandomSound2d(AudioClip[] _clips, string mixerGroup, float _volume = 1, float _minPitch = 0.75f, float _maxPitch = 1.25f) 
    {
        Play2dOneShotSound(_clips[Random.Range(0, _clips.Length)], mixerGroup, _volume, _minPitch, _maxPitch);
    }
    public void PlayOneShotRandomSound3d(AudioClip[] _clips, string mixerGroup, float _radius, Vector2 _pos, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1)
    {
        Play3dOneShotSound(_clips[Random.Range(0, _clips.Length)], mixerGroup, _radius, _pos, _minPitch, _maxPitch, _volume);
    }
   
    public AudioSource Play2dLoop(AudioClip _clip, string mixerGroup, float _volume = 0.7f, float _minPitch = 0.75f, float _maxPitch = 1.25f) 
    {
        AudioSource _as = GetUnused2dAS();
        
        PlayLoopSound(_as, _clip, mixerGroup, _minPitch, _maxPitch, _volume);

        return _as;
    }
    public AudioSource Play3dLoop(AudioClip _clip, string mixerGroup, float _radius, Vector2 _pos, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 0.4f) 
    {
        AudioSource _as = GetUnused3dAS();
        _as.minDistance = _radius;
        _as.maxDistance = _radius * 5;
        _as.gameObject.transform.position = new Vector3(_pos.x, _pos.y, -10);
        PlayLoopSound(_as, _clip, mixerGroup, _minPitch, _maxPitch, _volume);
        return _as;
    }

    public void PlayLoopSound(AudioSource _as, AudioClip _clip, string mixerGroup, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 0.4f)
    {   
        if (_as != null)
        {
            _as.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroup)[0];
            _as.loop = true;
            _as.pitch = Random.Range(_minPitch, _maxPitch);
            _as.volume = _volume;
            _as.clip = _clip;
            _as.Play();
        }
    }

    public void StopLoopSound(AudioSource _as) 
    {
        if (_as == null)
            return;

        _as.loop = false;
        _as.clip = null;
        _as.Stop();
    }

    public IEnumerator FadeOutSFXLoop(AudioSource source, float  fadeSpeed = 0.05f)
    {
        if(source != null && source.gameObject)
        {
            yield return new WaitUntil(() => (source.volume -= fadeSpeed) <= 0);
            StopLoopSound(source);
        }
        else
            yield return null;
    }

    public AudioMixer GetMixer()
    {
        return mixer;
    }

    public void ApplyRadioFilter(AudioSource _as)
    {
        if (!_as)
            return;

        if(_as.TryGetComponent(out AudioLowPassFilter _lowFilter))
            _lowFilter.enabled = false;
        if (_as.TryGetComponent(out AudioReverbFilter _reverbFilter))
            _reverbFilter.enabled = false;


        AudioHighPassFilter highFilter = _as.GetComponent<AudioHighPassFilter>();
        if (!highFilter)
            highFilter = _as.AddComponent<AudioHighPassFilter>();

        highFilter.cutoffFrequency = 500f;
        highFilter.highpassResonanceQ = 1f;


        AudioDistortionFilter distortionFilter = _as.GetComponent<AudioDistortionFilter>();
        if (!distortionFilter)
            distortionFilter = _as.AddComponent<AudioDistortionFilter>();

        distortionFilter.distortionLevel = 0.1f;
    }

    public void ApplyUnderwaterFilter(AudioSource _as)
    {
        if (!_as)
            return;

        
        AudioDistortionFilter distortionFilter = _as.GetComponent<AudioDistortionFilter>();
        if (!distortionFilter)
            distortionFilter = _as.AddComponent<AudioDistortionFilter>();

        distortionFilter.distortionLevel = 0.15f;


        AudioHighPassFilter highFilter = _as.GetComponent<AudioHighPassFilter>();
        if (!highFilter)
            highFilter = _as.AddComponent<AudioHighPassFilter>();

        highFilter.cutoffFrequency = 550f;
        highFilter.highpassResonanceQ = 1f;


        AudioLowPassFilter lowFilter = _as.GetComponent<AudioLowPassFilter>();
        if (!lowFilter)
            lowFilter = _as.AddComponent<AudioLowPassFilter>();
        else
            lowFilter.enabled = true;
        
        lowFilter.cutoffFrequency = 500f;
        lowFilter.lowpassResonanceQ = 1.5f;


        AudioReverbFilter reverbFilter = _as.GetComponent<AudioReverbFilter>();
        if (!reverbFilter)
            reverbFilter = _as.AddComponent<AudioReverbFilter>();
        else
            reverbFilter.enabled = true;

        reverbFilter.reverbPreset = AudioReverbPreset.Underwater;

    }
}
