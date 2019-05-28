using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public List<Transform> LaserAreaCorners;
    
    [Range(0, 10)] public float MinLaserInterval = 0.5f;
    [Range(0, 10)] public float MaxLaserInterval = 5f;
    [Range(0, 10)] public float MinLaserFireTime = 0.3f;
    [Range(0, 10)] public float MaxLaserFireTime = 3f;
    [Range(0.001f, 5f)] public float MinSpeed = 0.1f;
    [Range(0.001f, 5f)] public float MaxSpeed = 0.1f;

    [Range(0f, 2f)]public float ProjectorTime = 1; 

    public GameObject Beam;
    public GameObject Projector;

    private Rect _affectArea;
    private Coroutine _laserCoroutine;
    private Vector3 _targetPos;
    private Coroutine _moveCoroutine;
    private Vector3 _direction;
    private Animation _projectorAnimation;


    private void Awake()
    {
        var leftTop = LaserAreaCorners[0].transform.position;
        var rightBottom = LaserAreaCorners[1].transform.position;
        _affectArea = new Rect(
            leftTop.x, 
            leftTop.z, 
            rightBottom.x - leftTop.x, 
            rightBottom.z - leftTop.z);
        
        _projectorAnimation = Projector.GetComponent<Animation>();
    }

    private void OnEnable()
    {
        _laserCoroutine = StartCoroutine(LaserCoroutine());
    }

    private void OnDisable()
    {
         StopAllCoroutines();
    }

    private IEnumerator LaserCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(MinLaserInterval, MaxLaserInterval));
            StartCoroutine(FireCoroutine());

            yield return new WaitForSeconds(Random.Range(MinLaserFireTime, MaxLaserFireTime) + ProjectorTime);
            Stop();
        }
    }

    private IEnumerator FireCoroutine()
    {
        transform.position = GetRandomPosition();
        Projector.SetActive(true);
        _projectorAnimation.clip.frameRate = 60 * ProjectorTime;
        _projectorAnimation.Play(PlayMode.StopAll);
        yield return new WaitForSeconds(ProjectorTime);
        _projectorAnimation.Stop();

        Projector.SetActive(false);

        Beam.SetActive(true);
        _targetPos = GetRandomPosition();
        _direction = Vector3.Normalize(_targetPos - transform.position);
        _moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.position = transform.position + _direction * Random.Range(MinSpeed, MaxSpeed) * Time.fixedDeltaTime;
        }
    }

    private void Stop()
    {
        Beam.SetActive(false);
        StopCoroutine(_moveCoroutine);
    }
    
    private Vector3 GetRandomPosition()
    {
        var pos = new Vector3(
            _affectArea.position.x + Random.Range(0, _affectArea.width), 
            0, 
            _affectArea.position.y+ Random.Range(0, _affectArea.height));
        return pos;
    }
}
