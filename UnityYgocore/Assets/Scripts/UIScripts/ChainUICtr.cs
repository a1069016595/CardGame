using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainUICtr : MonoBehaviour
{
    List<ChainNumUI> freeChainNumUIList;

    List<ChainNumUI> activeChainNumUIList;

    GameObject chainUI;

    DuelUIManager duelUIMgr;

    void Awake()
    {
        DuelEventSys.GetInstance.updateUI_chainUI += GetInstance_updateUI_chainUI;
        DuelEventSys.GetInstance.uiAnim_chainAnim += GetInstance_uiAnim_chainAnim;

        freeChainNumUIList = new List<ChainNumUI>();
        activeChainNumUIList = new List<ChainNumUI>();

        chainUI = Resources.Load<GameObject>("Prefebs/prefeb_ChainUI");
        for (int i = 0; i < 10; i++)
        {   
            ChainNumUI obj = Instantiate(chainUI).GetComponent<ChainNumUI>();
            obj.Hide();
            freeChainNumUIList.Add(obj);
        }
    }

    void GetInstance_uiAnim_chainAnim(normalDele dele)
    {
        activeChainNumUIList[activeChainNumUIList.Count - 1].ShowFlashAnim(dele);
    }

    void Start()
    {
        duelUIMgr = DuelUIManager.GetInstance();
    }

    void GetInstance_updateUI_chainUI(Chain chain)
    {
        for (int i = activeChainNumUIList.Count; i < chain.chainNum; i++)
        {
            LauchEffect e= chain.GetEffect(i);
            Vector3 pos = duelUIMgr.GetCardAreaPos(e.ownerCard);
            if(activeChainNumUIList.Count<i+1)
            {
                activeChainNumUIList.Add(freeChainNumUIList.GetRemoveFirstList<ChainNumUI>());
            }
            activeChainNumUIList[i].Init(i + 1, pos, gameObject);
        }
        for (int i = chain.chainNum; i < activeChainNumUIList.Count; i++)
        {
            ChainNumUI obj = activeChainNumUIList.GetRemoveList<ChainNumUI>(i);
            obj.Hide();
            freeChainNumUIList.Add(obj);
            i--;
        }
    }
}
