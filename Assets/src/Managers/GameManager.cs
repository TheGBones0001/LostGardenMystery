using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int totalBotoes;
    private int botoesAtivados = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        var buttons = FindObjectsByType<TargetButton>(FindObjectsSortMode.None);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].acceptedGroup == BoxGroup.Finish)
                totalBotoes++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    public void ExecuteAction(TargetButton button)
    {
        switch (button.acceptedGroup)
        {
            case BoxGroup.Finish:
                TileManager.Instance.ActivateTileAt(button.transform.position);
                botoesAtivados++;
                FinishLevel();
                break;
            case BoxGroup.Trap:
            case BoxGroup.Bridge:
                TileManager.Instance.ActivateGroup(button.acceptedGroup);
                break;
        }
    }

    public void CancelAction(TargetButton button)
    {
        switch (button.acceptedGroup)
        {
            case BoxGroup.Finish:
                TileManager.Instance.DeactivateTileAt(button.transform.position);
                botoesAtivados--;
                break;
            case BoxGroup.Trap:
            case BoxGroup.Bridge:
                TileManager.Instance.DeactivateGroup(button.acceptedGroup);
                break;
        }
    }

    public void Restart()
    {
        TileManager.Instance.DeactivateAllGroups();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FinishLevel()
    {
        if (botoesAtivados >= totalBotoes)
        {
            Debug.Log("Todos os botões ativados! Vitória!");
            SceneManager.LoadScene("VictoryScene");
        }
    }
}
