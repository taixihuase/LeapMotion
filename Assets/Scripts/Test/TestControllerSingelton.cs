using UnityEngine;
using Controller;

public class TestControllerSingelton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TestCtrl.Instance.Test = 1;
        TestMonoCtrl.Instance.Test = 2;
        TestSingleton.Instance.Test = 3;
        TestMonoSingleton.Instance.Test = 4;
        Debug.Log(TestCtrl.Instance.Test);
        Debug.Log(TestMonoCtrl.Instance.Test);
        Debug.Log(TestSingleton.Instance.Test);
        Debug.Log(TestMonoSingleton.Instance.Test);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
