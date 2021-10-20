using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRefresh : MonoBehaviour, IEndDragHandler
{
    // Start is called before the first frame update

    private float refresh;
    void Start()
    {
        transform.gameObject.GetComponent<ScrollRect>().onValueChanged.AddListener(scrollRectCallBack);

        refresh = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void scrollRectCallBack(Vector2 value){

  refresh = value.y;

    }

    public void OnEndDrag(PointerEventData data){

        if(refresh == 1f){

            if(!SQLite.SQL.startLoading){
		        GameObject.Find("Loading-Games").transform.SetSiblingIndex(3);

		        SQLite.SQL.startLoading = true;
		        StartCoroutine(SQLite.SQL.loadingSpinner());
		    }

            if(this.gameObject.name == "Friends"){
                SQLite.SQL.updateCurrentFriends();
            }else{
                FirebaseListeners.FBL.returnHomeRecentGamesUpdate();
            }
        }

        refresh = 0f;
    }
}
