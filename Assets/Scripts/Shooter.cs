using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _speed = 2;

    private Camera _camera;
    private RectTransform _targetTransform;
    private bool _canShoot = true;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _targetTransform = _target.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _canShoot)
        {
            _target.SetActive(true);

            Vector3 directon = Input.mousePosition;

            directon.x = directon.x - _camera.pixelWidth / 2;
            directon.y = directon.y - _camera.pixelHeight / 2;

            _targetTransform.anchoredPosition = directon;
        }
        else if (Input.GetMouseButtonUp(0) && _canShoot)
        {
            GameObject gameObject = Instantiate(_bulletPrefab);

            gameObject.transform.position = _camera.transform.position;

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

            Vector3 directon = _targetTransform.transform.position;

            directon.z = -directon.z;
            directon.y -= _camera.transform.position.y;

            rigidbody.AddForce(directon * _speed, ForceMode.Impulse);

            _target.SetActive(false);
        }        
    }

    public void StopShooting()
    {
        _canShoot = false;
    }

    public void ResumeShooting()
    {
        _canShoot = true;
    }
}
