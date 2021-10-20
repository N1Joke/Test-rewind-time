using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMenager : MonoBehaviour
{
    [SerializeField] private Shooter _shooter;
    [SerializeField] private GameObject _rewindButton;
    [SerializeField] private float _delayToChekMovements = 1;

    private List<TimeBody> _timeBodies;
    private bool _gotShoot = false;
    private float _timeSinceStartRecording = 0;
    private bool _rewinding = false;
    private bool _recording = false;

    private void Awake()
    {
        _timeBodies = new List<TimeBody>();

        Button button = _rewindButton.GetComponent<Button>();
        button.onClick.AddListener(RewindRequired);

        TimeBody[] timeBodies = transform.GetComponentsInChildren<TimeBody>();

        foreach (TimeBody timeBody in timeBodies)
        {
            _timeBodies.Add(timeBody);
        }
    }

    private void FixedUpdate()
    {
        bool SomeCubeIsMoving = false;

        foreach (TimeBody timeBody in _timeBodies)
        {
            SomeCubeIsMoving = timeBody._moving;
            if (SomeCubeIsMoving)
                break;
        }

        if (!SomeCubeIsMoving && _gotShoot && !_rewinding)
            ShowButtonRewind();
        if (_recording)
            _timeSinceStartRecording += Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        foreach (TimeBody timeBody in _timeBodies)
        {
            timeBody.Shoot += StartRecord;
        }
    }

    private void OnDisable()
    {
        foreach (TimeBody timeBody in _timeBodies)
        {
            timeBody.Shoot -= StartRecord;
        }
    }

    private void ShowButtonRewind()
    {
        _recording = false;
        _shooter.StopShooting();

        if (!_rewinding)
        {
            _gotShoot = false;
            _rewindButton.SetActive(true);
            foreach (TimeBody timeBody in _timeBodies)
            {
                timeBody.StopRecording();
            }
        }
    }

    private void RewindRequired()
    {
        _rewinding = true;
        _rewindButton.SetActive(false);

        foreach (TimeBody timeBody in _timeBodies)
        {
            timeBody.StartRewind();
        }

        StartCoroutine(ResumeShooter());
    }

    private void StartRecord()
    {
        if (!_rewinding && !_recording)
        {
            _recording = true;

            StartCoroutine(DelayToChekMovements());

            foreach (TimeBody timeBody in _timeBodies)
            {
                timeBody.StartRecord();
            }
        }
    }

    private IEnumerator DelayToChekMovements()
    {
        yield return new WaitForSeconds(_delayToChekMovements);
        _gotShoot = true;
    }

    private IEnumerator ResumeShooter()
    {
        yield return new WaitForSeconds(_timeSinceStartRecording);
        _timeSinceStartRecording = 0;
        _shooter.ResumeShooting();
        _rewinding = false;
    }
}
