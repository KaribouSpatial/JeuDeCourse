using UnityEngine;
using System.Collections;

public class EndScreenManager : MonoBehaviour
{
    public GUIText _endingText;
    private string[] cars;

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
	}

    void Start()
    {
        cars = RaceManager.CarPositionString.Split(';');
        Debug.Log(cars[0].Split(',')[0]);
        this._endingText.guiText.text = cars[0].Split(',')[0] == "Joueur 1"
            ? "Félicitations! Vous avez gagné la course!"
            : "Meilleure chance la prochaine fois";

        this.ScoreTable();
    }

    // Update is called once per frame
	public void RePlay () 
	{
        Application.LoadLevel("mountainCourse");
	}

    void ScoreTable() {
        float win = Screen.width * 0.6f;
        float w1 = win * 0.35f; 
        float w2 = win * 0.15f;
        foreach (string car in this.cars)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(car.Split(',')[1], GUILayout.Width(w1));
            GUILayout.Label(car.Split(',')[0], GUILayout.Width(w2));
            GUILayout.EndHorizontal();
        }
    } 
}
