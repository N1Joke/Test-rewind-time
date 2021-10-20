using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class TimeBody : MonoBehaviour
{
    [SerializeField] private bool _isRewinding = false;
    [SerializeField] private bool _isRecording = false;

    private List<PointInTime> _pointsInTime;
    private Rigidbody _rigidbody;
    private Vector3 _lastPos;

    public event UnityAction Shoot;
    public bool _moving = false;

    private void Start()
    {
        _pointsInTime = new List<PointInTime>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {     
        if (_isRewinding)
            Rewind();
        if (_isRecording)
            Record();

        if (_lastPos == transform.position)
            _moving = false;
        else
            _moving = true;

        _lastPos = transform.position;
    }

    public void StartRecord()
    {
        _isRecording = true;
    }
    private void Record()
    {
        _pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    private void Rewind()
    {
        if (_pointsInTime.Count > 0)
        {
            PointInTime pointInTime = _pointsInTime[0];

            transform.position = pointInTime.Position;
            transform.rotation = pointInTime.Rotation;
            _pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        StopRecording();
        _isRewinding = true;
        _rigidbody.isKinematic = true;
    }

    public void StopRewind()
    {
        _isRewinding = false;
        _rigidbody.isKinematic = false;
    }

    public void StopRecording()
    {
        _isRecording = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            Shoot?.Invoke();
        }
    }
}
