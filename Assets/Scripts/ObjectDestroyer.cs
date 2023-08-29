using System.Collections;
using UnityEngine;

public static class ObjectDestroyer
{
    public static void DestroyInTime(MonoBehaviour monoBehaviour, float scale, float time)
    {
        monoBehaviour.StartCoroutine(Destroy(monoBehaviour.gameObject, scale, time));
    }
    
    private static IEnumerator Destroy(GameObject gameObject, float scale, float time)
    {
        var to = new Vector3(scale, scale, scale);
        
        gameObject.LeanScale(to, time);
        
        yield return new WaitUntil(() => gameObject.transform.localScale == to);
        
        GameObject.Destroy(gameObject);
    } 
}