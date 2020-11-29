using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class CampaignMenu : MonoBehaviour
    {
        [SerializeField] private Sprite[] chooseLevelBtnSprites;

        [SerializeField] private Sprite closedLevelChooser;

        [SerializeField] private ChooserLevel prefab;

        [SerializeField] private HorizontalLayoutGroup[] horizontalLayoutGroups;

        [SerializeField] private GameObject scrollNext;

        [SerializeField] private GameObject scrollBack;

        [SerializeField] private int menuCapacity = 20;

        private int _totalLevels;

        private int _horizontalLayoutGroupCapacity;

        private void Start()
        {
            _horizontalLayoutGroupCapacity = menuCapacity / horizontalLayoutGroups.Length;
            if (UserDataManager.Instance.LastLevelCompleted < menuCapacity)
            {
                scrollNext.SetActive(true);
            }
            CreateBtns();
        }

        private void CreateBtns()
        {
            for (int i = UserDataManager.Instance.LastLevelCompleted; i <= UserDataManager.Instance.LastLevelCompleted + menuCapacity;)
            {
                foreach (var layoutGroup in horizontalLayoutGroups)
                {
                    for (int k = 1; k <= _horizontalLayoutGroupCapacity; k++)
                    {
                        ChooserLevel newButton = Instantiate(prefab, layoutGroup.transform, false);
                        newButton.GetLevel(i);
                        if (UserDataManager.Instance.LastLevelCompleted >= i)
                        {
                            newButton.GetSprite(
                                chooseLevelBtnSprites[Random.Range(0, chooseLevelBtnSprites.Length - 1)]);
                        }
                        else
                        {
                            newButton.GetSprite(closedLevelChooser);
                        }
                        i++;
                    }
                }
            }
        }
        
    }
}