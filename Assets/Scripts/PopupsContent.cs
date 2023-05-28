using UnityEngine;
using EasyUI.Dialogs;

public class PopupsContent : MonoBehaviour {

	void Start ( ) {

		DialogUI.Instance
		.SetTitle ( "Message 1" )
		.SetMessage ( "blabla" )
		.SetButtonColor ( DialogButtonColor.Black )
		.OnClose ( ( ) => Debug.Log ( "Closed 1" ) )
		.Show ( );


		DialogUI.Instance
		.SetTitle ( "Message 2" )
		.SetMessage ( "Yeah" )
		.SetButtonColor ( DialogButtonColor.Black )
		.SetButtonText ( "ok" )
		.OnClose ( ( ) => Debug.Log ( "Closed 2" ) )
		.Show ( );


		DialogUI.Instance
		.SetTitle ( "Message 3" )
		.SetMessage ( "Bye!" )
		.SetFadeInDuration ( 1f )
		.SetButtonColor ( DialogButtonColor.Black )
		.OnClose ( ( ) => Debug.Log ( "Closed 3" ) )
		.Show ( );

	}

}
