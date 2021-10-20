using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLists : MonoBehaviour
{

    public static List<string> activeGames = new List<string>();
    public static List<string> recentGames = new List<string>();
    public static List<string> friends = new List<string>();
    public static List<string> pendinginvites = new List<string>();
    public static List<string> pendingrequests = new List<string>();

    public static List<List<string>> activeGameMoves = new List<List<string>>();
    public static List<List<string>> recentGameMoves = new List<List<string>>();

    public static List<string> deniedrequests = new List<string>();

    public static List<string> notificationTokens = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
