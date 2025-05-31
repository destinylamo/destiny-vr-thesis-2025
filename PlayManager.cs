using UnityEngine;
using UnityEngine.UI; // For Button UI

public class PlayManager : MonoBehaviour
{
    // References to the play scripts
    public PiritaPlay piritaScript;  
    public SpartaXPlay spartaXScript;  

    // References to UI elements
    public Button playButton;  
    public Button restartButton;  
    public Button backButton;  

    // UI for the play selection menu
    public GameObject playSelectionMenu;  
    public GameObject playControlsMenu; 

    void Start()
    {
        // Ensure both scripts are disabled at the start
        piritaScript.enabled = false;
        spartaXScript.enabled = false;

        // Set up button listeners
        playButton.onClick.AddListener(OnPlayButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        // Hide the play control menu initially
        playControlsMenu.SetActive(false);
    }

    // Function to select a play based on the button clicked in the menu
    public void SelectPlay(string playName)
    {
        // Disable both scripts initially to ensure no play is active
        piritaScript.enabled = false;
        spartaXScript.enabled = false;

        // Hide the play selection menu and show the play controls menu
        playSelectionMenu.SetActive(false);
        playControlsMenu.SetActive(true);

        // Activate the appropriate script based on the selected play
        if (playName == "Pirita")
        {
            piritaScript.enabled = true;  // Enable the Pirita play script
        }
        else if (playName == "SpartaX")
        {
            spartaXScript.enabled = true;  // Enable the Sparta X play script
        }
    }

    // Play button clicked - Start the play
    void OnPlayButtonClicked()
    {
        // Depending on which script is active, it will start the play
        if (piritaScript.enabled)
        {
            piritaScript.StartPlay();  // Start the Pirita play
        }
        else if (spartaXScript.enabled)
        {
            spartaXScript.StartPlay();  // Start the Sparta X play
        }
    }

    // Restart button clicked - Restart the play
    void OnRestartButtonClicked()
    {
        // Reset the ball and players to their starting positions
        if (piritaScript.enabled)
        {
            piritaScript.ResetPlay();  // Reset the Pirita play
        }
        else if (spartaXScript.enabled)
        {
            spartaXScript.ResetPlay();  // Reset the Sparta X play
        }
    }

    // Back button clicked - Go back to the play selection menu
    void OnBackButtonClicked()
    {
        // Disable both scripts and show the play selection menu
        piritaScript.enabled = false;
        spartaXScript.enabled = false;
        playControlsMenu.SetActive(false);
        playSelectionMenu.SetActive(true);
    }
}
