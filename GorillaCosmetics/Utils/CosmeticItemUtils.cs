using GorillaNetworking;
using System.Linq;

namespace GorillaCosmetics.Utils
{
	public static class CosmeticItemUtils
	{

		public static bool ContainsHat(CosmeticsController.CosmeticItem cosmeticItem)
		{
			return Plugin.SelectionManager.CurrentHat != null &&
				(cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Hat ||
				(cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Set && cosmeticItem.bundledItems.Any(id => CosmeticsController.instance.GetItemFromDict(id).itemCategory == CosmeticsController.CosmeticCategory.Set)));
		}
	}
}