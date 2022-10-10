using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRate : PickUp
{
    [SerializeField] int _value;

    AudioSource _myAudioSource;
    ParticleSystem _myParticleSystem;
    Collider _myCollider;
    MeshRenderer _myMeshRenderer;

    void Awake()
    {
        _myAudioSource = GetComponent<AudioSource>();
        _myParticleSystem = GetComponent<ParticleSystem>();
        _myCollider = GetComponent<Collider>();
        _myMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;

        _currentDistance += _speed * Time.deltaTime;

        if (_currentDistance > _maxDistance)
        {
            FireRateFactory.Instance.ReturnFireRate(this);
        }
    }

    public override void Pick(Player player)
    {
        player.SetFireRate(_value);
        //OnPickUp();
    }

    void OnEnable()
    {
        _myCollider.enabled = true;
        _myMeshRenderer.enabled = true;
    }

    void OnDisable()
    {
        _currentDistance = 0;
    }

    public static void TurnOn(FireRate r)
    {
        r.gameObject.SetActive(true);
    }

    public static void TurnOff(FireRate r)
    {
        r.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        OnDestroy();
    }

    void OnDestroy()
    {
        _myAudioSource.Play();
        _myParticleSystem.Play();
        _myCollider.enabled = false;
        _myMeshRenderer.enabled = false;

        StartCoroutine("WaitReturn");
    }

    IEnumerator WaitReturn()
    {
        yield return new WaitForSeconds(1f);
        FireRateFactory.Instance.ReturnFireRate(this);
    }
}