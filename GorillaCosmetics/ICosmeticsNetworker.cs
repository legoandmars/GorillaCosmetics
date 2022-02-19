using ExitGames.Client.Photon;
using Photon.Realtime;

namespace GorillaCosmetics
{
	public interface ICosmeticsNetworker
	{
		void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps);
	}
}