using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VanHuy
{
    [System.Serializable]
    public class DataHole
    {
        public int indexFox;
        public float timeSpawnFox;
    }

    public class Hole : MonoBehaviour
    {
        public int indexFox;
        public float timeSpawnFox;
        public int indexTurnFox;
        public bool isEndSpawn;
        public LogicGame logicGame;
        public ParticleSystem vfxSmashImpact;
        public GameObject foxParent;
        public List<FoxController> listFox;
        public List<DataHole> listDataHoles;
        // Start is called before the first frame update
        void Start()
        {
            timeSpawnFox = listDataHoles[indexTurnFox].timeSpawnFox;
        }

        // Update is called once per frame
        void Update()
        {
            SetTimeSpanwFox();
        }
        public void RefreshFox()
        {
            
        }
        public void SpawnFox()
        {
            FoxController foxClone = Instantiate(listFox[indexFox], foxParent.transform);
            foxClone.hole = this; 
        }
        public void SetTimeSpanwFox()
        {
            if(isEndSpawn == false)
            {
                if (listDataHoles.Count > 0)
                {
                    timeSpawnFox -= Time.deltaTime;
                    if (timeSpawnFox <= 0)
                    {
                        indexFox = listDataHoles[indexTurnFox].indexFox;
                        SpawnFox();
                        indexTurnFox++;
                        if (indexTurnFox <= listDataHoles.Count - 1)
                        {
                            timeSpawnFox = listDataHoles[indexTurnFox].timeSpawnFox;
                        }
                        else
                        {
                            isEndSpawn = true;
                        }    
                    }
                }
            }    
        }
    }
}
