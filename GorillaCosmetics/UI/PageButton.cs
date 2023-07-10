namespace GorillaCosmetics.UI
{
	/// <remarks>
	/// The original WardrobeFunctionButton is left alone, it handles all of the the visual aspects.
	/// This isn't an issue since the WardrobeFunctionButtons are disabled, and the cosmetics visuals are hidden.
	/// However, in the future, this may need to be updated to do everything on its own.
	/// </remarks>
	public class PageButton : GorillaPressableButton
	{
		public bool Forward { get; set; } = true;

		public override void ButtonActivation()
		{
			base.ButtonActivation();
			
			if (Forward)
			{
				Plugin.SelectionManager.NextPage();
			} else
			{
				Plugin.SelectionManager.PreviousPage();
			}
		}
	}
}
