using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Base class for effects that modify the generated Mesh.</para>
	/// </summary>
	[ExecuteInEditMode]
	public abstract class BaseMeshEffect : UIBehaviour, IMeshModifier
	{
		[NonSerialized]
		private Graphic m_Graphic;

		protected Graphic graphic
		{
			get
			{
				if (this.m_Graphic == null)
				{
					this.m_Graphic = base.GetComponent<Graphic>();
				}
				return this.m_Graphic;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
		}

		/// <summary>
		///   <para>See MonoBehaviour.OnDisable.</para>
		/// </summary>
		protected override void OnDisable()
		{
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
			base.OnDidApplyAnimationProperties();
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
		}

		/// <summary>
		///   <para>See:IMeshModifier.</para>
		/// </summary>
		/// <param name="mesh"></param>
		public virtual void ModifyMesh(Mesh mesh)
		{
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				this.ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(mesh);
			}
		}

		public abstract void ModifyMesh(VertexHelper vh);
	}
}
