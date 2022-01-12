using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellController : MonoBehaviour
{
    [SerializeField] List<GameObject> bulletHellObj = new List<GameObject>();
    [SerializeField] float startDelay;
    [SerializeField] float nextDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletPatternCoroutine());
    }

    IEnumerator BulletPatternCoroutine()
    {
        while(true)
        {
            for(int i = 0; i < bulletHellObj.Count; i++)
            {
                yield return new WaitForSeconds(startDelay);
                bulletHellObj[i].SetActive(true);
                yield return new WaitForSeconds(nextDelay);
                bulletHellObj[i].SetActive(false);
            }
        }
    }
}
