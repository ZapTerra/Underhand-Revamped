using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForesightCardKinematics : MonoBehaviour
{
    public float travelSpeed;
    public Animator animator;
    public Card card;
    public Image graphic;
    public int index;
    public bool canDiscard;
    public Vector3 targetMovement;
    public Vector3 startingPosition;
    public Vector3 discardPosition;
    public Vector3 keepEventPosition = new Vector3(-5f, 7.5f, 0f);
    public Vector3 discardEventPosition = new Vector3(5f, 7.5f, 0f);
    public float totalToTravel;
    private bool selected;
    private bool playingAnimation;
    private bool readyToExit;
    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        graphic.sprite = card.eventSprite;
        startingPosition = transform.position;
        targetMovement = FindObjectOfType<ForesightAnimationManager>().giveMePosition(index) - transform.position;
        totalToTravel = targetMovement.magnitude;
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        if(totalToTravel > 0){
            Vector3 movementThisFrame = targetMovement.normalized * Mathf.Clamp(Time.unscaledDeltaTime * travelSpeed, 0, totalToTravel);
            transform.position += movementThisFrame;
            totalToTravel -= movementThisFrame.magnitude;
        }else if (!readyToExit){
            readyToExit = true;
            discardPosition = new Vector3(-5f, 3.5f, 0f);
        }
    }

    public IEnumerator animateAway(float delay){
        yield return new WaitForSecondsRealtime(delay);
        startingPosition = transform.position;
        targetMovement = discardPosition - transform.position;
        totalToTravel = targetMovement.magnitude;
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if(canDiscard){
            if (isOn)
            {
                graphic.color = Color.gray;
                CardManager.foresightDiscardIndices.Add(index);
                discardPosition = new Vector3(5f, 3.5f, 0f);
            }
            else
            {
                graphic.color = Color.white;
                CardManager.foresightDiscardIndices.Remove(index);
                discardPosition = new Vector3(-5f, 3.5f, 0f);
            }
        }
    }
}
