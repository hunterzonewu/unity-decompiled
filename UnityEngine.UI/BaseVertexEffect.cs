using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Base class for effects that modify the the generated Vertex Buffers.</para>
	/// </summary>
	[Obsolete("Use BaseMeshEffect instead", true)]
	public abstract class BaseVertexEffect
	{
		[Obsolete("Use BaseMeshEffect.ModifyMeshes instead", true)]
		public abstract void ModifyVertices(List<UIVertex> vertices);
	}
}
