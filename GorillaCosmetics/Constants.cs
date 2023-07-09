using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaCosmetics
{
	public static class Constants
	{
		public static readonly Vector3 PreviewHatLocalPosition = new Vector3(0, -0.04f, 0.54f);
		public static readonly Quaternion PreviewHatLocalRotation = Quaternion.Euler(0, 90, 100);
		public static readonly Vector3 PreviewHatLocalScale = Vector3.one * 0.25f;

		public static readonly Vector3 PreviewOrbLocalPosition = new Vector3(0, 0, 0.29f);
		public static readonly Quaternion PreviewOrbRotation = Quaternion.identity;
		public static readonly Vector3 PreviewOrbLocalScale = Vector3.one * 0.45f;

		public static readonly Vector3 PreviewOrbHeadModelLocalPositionOffset = new Vector3(-0.6f, 0, 0f);

		public static readonly Vector3 EnableButtonLocalPositionOffset = new Vector3(0, 0.12f, 0f);
	}
}
