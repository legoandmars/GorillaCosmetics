using GorillaNetworking;
using System.Linq;

namespace GorillaCosmetics.Utils
{
	public static class CosmeticItemUtils
	{

		public static bool ContainsHat(CosmeticsController.CosmeticItem cosmeticItem)
		{
			return Plugin.SelectionManager.CurrentHat != null &&
				(cosmeticItem.itemSlot == "hat" ||
				(cosmeticItem.itemSlot == "set" && cosmeticItem.bundledItems.Any(id => CosmeticsController.instance.GetItemFromDict(id).itemSlot == "hat")));
		}
	}
}