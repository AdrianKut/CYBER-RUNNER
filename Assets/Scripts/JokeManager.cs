using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class JokeManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshJoke;

    public static List<string> jokes = new List<string>();
    void Start()
    {
        JokesInitialization();
        DisplayRandomJoke();
    }

    private void JokesInitialization()
    {
        jokes.Add("Where can I\nleave electro-waste?");
        jokes.Add("Did you know that\nI love marathons?");
        jokes.Add("I will always\n deliver hot rolls!");
        jokes.Add("Tag, you're it!");
        jokes.Add("It's seems I have lost \nfeeling in my legs");
        jokes.Add("When I was a child, \nI had to walk 10 km to school");
        jokes.Add("The girls used to run away from me\nbut now they have no chance.");
        jokes.Add("Do you want \nrace with me?");
        jokes.Add("I just go out \nto get the milk");
        jokes.Add("I'm member of \nRepublic of Gamers and You?");
    }

    private int randomJoke = 0;
    public void DisplayRandomJoke()
    {
        int currentJoke = randomJoke;
        do {
             randomJoke = Random.Range(0, jokes.Count);
        } while (currentJoke == randomJoke);

        textMeshJoke.text = jokes[randomJoke];
    }

}
