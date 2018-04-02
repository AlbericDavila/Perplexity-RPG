using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	[SerializeField] AudioClip clip;
	[SerializeField] int layerFilter = 0;
	[SerializeField] float playerDistanceThreshold = 5f;
	[SerializeField] bool isOneTimeOnly = true;

	bool hasPlayed = false;
	AudioSource audioSource;
    GameObject player;

	void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.clip = clip;
        /*
		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = playerDistanceThreshold;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        */
        player = GameObject.FindWithTag("Player");
	}

    /*
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == layerFilter)
		{
			RequestPlayAudioClip();
		}
	}*/

    void Update()
    {
        // TODO make this Distance calculation less expensive
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDistanceThreshold)
        {
            RequestPlayAudioClip();
        }
    }

    void RequestPlayAudioClip()
	{
		if (isOneTimeOnly && hasPlayed)
		{
			return;
		}
		else if (audioSource.isPlaying == false)
		{
			audioSource.Play();
			hasPlayed = true;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(0, 255f, 0, .5f);
		Gizmos.DrawWireSphere(transform.position, playerDistanceThreshold);
	}
}