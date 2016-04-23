using UnityEngine;
using System.Collections;

public class NetworkController {
   
	public NetworkController()
    {

    }
    public string SendRequest(string url)
    {
        var www = new WWW(url);
        while (!www.isDone) ;
            

        if (!string.IsNullOrEmpty(www.error))
        {
            //Logger.Log("[Network] SendRequest ERROR:" + www.error);
            
            return www.error;
            
        }

        //Logger.Log("[Network] input data:{0}", www.text);
        
        return www.text;
    }
}
