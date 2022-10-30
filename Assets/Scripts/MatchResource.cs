using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Resources;

public class MatchResource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool OneToTwo(Resource r1, Resource r2){
        return Eval(r1, r2, true, true);
    }

    public static bool OneToTwoNoRelic(Resource r1, Resource r2){
        return Eval(r1, r2, false, true);
    }

    public static bool OneToTwoLowValue(Resource r1, Resource r2){
        return Eval(r1, r2, false, false);
    }

    private static bool Eval(Resource r1, Resource r2, bool wildRelic, bool allowHighValueCost){
        if(r1 == r2){
            return true;
        }else if(r2 == Resource.Human && (r1 == Resource.Cultist && allowHighValueCost || r1 == Resource.Prisoner)){
            return true;
        }else if(r1 == Resource.Relic && wildRelic){
            return true;
        }
        return false;
    }
}
