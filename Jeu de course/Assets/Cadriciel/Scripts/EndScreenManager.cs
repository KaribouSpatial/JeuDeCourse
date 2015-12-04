using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public UnityEngine.UI.Text _endingText;
    public GameObject playerScore;
    private string[] cars;

	void Awake()
	{
		Input.simulateMouseWithTouches = true;
	}

    void Start()
    {
        cars = RaceManager.CarPositionString.Split(';');
        this._endingText.text = cars[0].Split(',')[0] == "Joueur 1"
            ? "Félicitations! Vous avez gagné la course!"
            : "Meilleure chance la prochaine fois!";

        this.ScoreTable();
    }

    // Update is called once per frame
	public void RePlay () 
	{
        Application.LoadLevel("mountainCourse");
	}

    void ScoreTable() {
        foreach (string car in this.cars)
        {
            if (!string.IsNullOrEmpty(car))
            {
				var carName = car.Split(',')[0];
				var position = car.Split(',')[1];
                GameObject boardEntry = (GameObject) Instantiate(playerScore);
                boardEntry.transform.SetParent(GameObject.Find("PlayerEntries").transform, false);
                boardEntry.transform.FindChild("Position").GetComponent<Text>().text = position;
                boardEntry.transform.FindChild("Player").GetComponent<Text>().text = carName;
                if (position == "1")
                {
                    boardEntry.transform.FindChild("Time").GetComponent<Text>().text = RaceManager.Timer + "s";
                }
                else
                {
                    boardEntry.transform.FindChild("Time").GetComponent<Text>().text = "";
                }
            }
        }
    } 
}
