using UnityEngine;

public class AdjustCrosshair : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject topLine;
    [SerializeField] private GameObject rightLine;
    [SerializeField] private GameObject bottomLine;
    [SerializeField] private GameObject leftLine;

    private PlayerShoot playerShoot;
    private RectTransform top;
    private RectTransform right;
    private RectTransform bottom;
    private RectTransform left;

    [SerializeField] private float duration;

    public float offset;

    void Start()
    {
        playerShoot = player.GetComponent<PlayerShoot>();
        top = topLine.GetComponent<RectTransform>();
        right = rightLine.GetComponent<RectTransform>();
        bottom = bottomLine.GetComponent<RectTransform>();
        left = leftLine.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (playerShoot.shoot)
        {
            top.localPosition = Vector2.Lerp(top.localPosition, new Vector2(0f, offset), duration * Time.deltaTime);
            right.localPosition = Vector2.Lerp(right.localPosition, new Vector2(offset, 0f), duration * Time.deltaTime);
            bottom.localPosition = Vector2.Lerp(bottom.localPosition, new Vector2(0f, -offset), duration * Time.deltaTime);
            left.localPosition = Vector2.Lerp(left.localPosition, new Vector2(-offset, 0f), duration * Time.deltaTime);
        }
        else
        {
            top.localPosition = Vector2.Lerp(top.localPosition, Vector3.zero, duration / 2f * Time.deltaTime);
            right.localPosition = Vector2.Lerp(right.localPosition, Vector2.zero, duration / 2f * Time.deltaTime);
            bottom.localPosition = Vector2.Lerp(bottom.localPosition, Vector2.zero, duration / 2f * Time.deltaTime);
            left.localPosition = Vector2.Lerp(left.localPosition, Vector2.zero, duration / 2f * Time.deltaTime);
        }

    }
}