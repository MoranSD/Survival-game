using GameItems;

namespace InventorySystem
{
    internal class GameItemInventoryData
    {
        public BaseGameItemData baseData;
        public object unicData;

        public GameItemInventoryData(BaseGameItemData baseData, object unicData)
        {
            this.baseData = baseData;
            this.unicData = unicData;
        }
    }
}
