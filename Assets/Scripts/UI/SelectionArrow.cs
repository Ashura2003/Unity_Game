using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    private RectTransform rt; // Reference to the RectTransform component
    [SerializeField]private RectTransform[] options;
    [SerializeField]private AudioClip changeSound;
    [SerializeField]private AudioClip selectSound;
    private int currentOption = 0;
    private bool hasMoved = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>(); // Get the RectTransform component
    }

    private void Update()
    {
        // Move the arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(currentOption - 1);
            hasMoved = true; // The user has moved the selection
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(currentOption + 1);
            hasMoved = true; // The user has moved the selection
        }

        // Select the option, but only if the user has moved the selection
        if (hasMoved && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            Interact();
        }
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(selectSound);

        // Do something with the selected option
        options[currentOption].GetComponent<Button>().onClick.Invoke();
    }

    private void ChangePosition(int option)
    {
        currentOption = option;

        if(option != 0)
            SoundManager.instance.PlaySound(changeSound);

        if (currentOption < 0)
            currentOption = options.Length - 1;
        else if(currentOption > options.Length)
            currentOption = 0;

        // Change the position of the arrow
        rt.position = new Vector3(rt.position.x, options[currentOption].position.y, rt.position.z);
    }
}
