using System;
using UnityEngine.Serialization;
using UnityEngine.Sprites;

namespace UnityEngine.UI
{
	/// <summary>
	///   <para>Displays a Sprite for the UI System.</para>
	/// </summary>
	[AddComponentMenu("UI/Image", 11)]
	public class Image : MaskableGraphic, ICanvasRaycastFilter, ISerializationCallbackReceiver, ILayoutElement
	{
		/// <summary>
		///   <para>Image Type.</para>
		/// </summary>
		public enum Type
		{
			/// <summary>
			///   <para>Displayes the full Image.</para>
			/// </summary>
			Simple,
			/// <summary>
			///   <para>Displays the Image as a 9-sliced Image.</para>
			/// </summary>
			Sliced,
			/// <summary>
			///   <para>Displays the Image Tiling the central part of the Sprite.</para>
			/// </summary>
			Tiled,
			/// <summary>
			///   <para>Display portion of the Image.</para>
			/// </summary>
			Filled
		}

		/// <summary>
		///   <para>Fill method to be used by the Image.</para>
		/// </summary>
		public enum FillMethod
		{
			/// <summary>
			///   <para>The Image will be filled Horizontally.</para>
			/// </summary>
			Horizontal,
			/// <summary>
			///   <para>The Image will be filled Vertically.</para>
			/// </summary>
			Vertical,
			/// <summary>
			///   <para>The Image will be filled Radially with the radial center in one of the corners.</para>
			/// </summary>
			Radial90,
			/// <summary>
			///   <para>The Image will be filled Radially with the radial center in one of the edges.</para>
			/// </summary>
			Radial180,
			/// <summary>
			///   <para>The Image will be filled Radially with the radial center at the center.</para>
			/// </summary>
			Radial360
		}

		/// <summary>
		///   <para>Origin for the Image.FillMethod.Horizontal.</para>
		/// </summary>
		public enum OriginHorizontal
		{
			/// <summary>
			///   <para>Origin at the Left side.</para>
			/// </summary>
			Left,
			/// <summary>
			///   <para>Origin at the Right side.</para>
			/// </summary>
			Right
		}

		/// <summary>
		///   <para>Origin for the Image.FillMethod.Vertical.</para>
		/// </summary>
		public enum OriginVertical
		{
			/// <summary>
			///   <para>Origin at the Bottom edge.</para>
			/// </summary>
			Bottom,
			/// <summary>
			///   <para>Origin at the Top edge.</para>
			/// </summary>
			Top
		}

		/// <summary>
		///   <para>Origin for the Image.FillMethod.Radial90.</para>
		/// </summary>
		public enum Origin90
		{
			/// <summary>
			///   <para>Radial starting at the Bottom Left corner.</para>
			/// </summary>
			BottomLeft,
			/// <summary>
			///   <para>Radial starting at the Top Left corner.</para>
			/// </summary>
			TopLeft,
			/// <summary>
			///   <para>Radial starting at the Top Right corner.</para>
			/// </summary>
			TopRight,
			/// <summary>
			///   <para>Radial starting at the Bottom Right corner.</para>
			/// </summary>
			BottomRight
		}

		/// <summary>
		///   <para>Origin for the Image.FillMethod.Radial180.</para>
		/// </summary>
		public enum Origin180
		{
			/// <summary>
			///   <para>Center of the radial at the center of the Bottom edge.</para>
			/// </summary>
			Bottom,
			/// <summary>
			///   <para>Center of the radial at the center of the Left edge.</para>
			/// </summary>
			Left,
			/// <summary>
			///   <para>Center of the radial at the center of the Top edge.</para>
			/// </summary>
			Top,
			/// <summary>
			///   <para>Center of the radial at the center of the Right edge.</para>
			/// </summary>
			Right
		}

		/// <summary>
		///   <para>One of the points of the Arc for the Image.FillMethod.Radial360.</para>
		/// </summary>
		public enum Origin360
		{
			/// <summary>
			///   <para>Arc starting at the center of the Bottom edge.</para>
			/// </summary>
			Bottom,
			/// <summary>
			///   <para>Arc starting at the center of the Right edge.</para>
			/// </summary>
			Right,
			/// <summary>
			///   <para>Arc starting at the center of the Top edge.</para>
			/// </summary>
			Top,
			/// <summary>
			///   <para>Arc starting at the center of the Left edge.</para>
			/// </summary>
			Left
		}

		[FormerlySerializedAs("m_Frame"), SerializeField]
		private Sprite m_Sprite;

		[NonSerialized]
		private Sprite m_OverrideSprite;

		[SerializeField]
		private Image.Type m_Type;

		[SerializeField]
		private bool m_PreserveAspect;

		[SerializeField]
		private bool m_FillCenter = true;

		[SerializeField]
		private Image.FillMethod m_FillMethod = Image.FillMethod.Radial360;

		[Range(0f, 1f), SerializeField]
		private float m_FillAmount = 1f;

		[SerializeField]
		private bool m_FillClockwise = true;

		[SerializeField]
		private int m_FillOrigin;

		private float m_EventAlphaThreshold = 1f;

		private static readonly Vector2[] s_VertScratch = new Vector2[4];

		private static readonly Vector2[] s_UVScratch = new Vector2[4];

		private static readonly Vector3[] s_Xy = new Vector3[4];

		private static readonly Vector3[] s_Uv = new Vector3[4];

		/// <summary>
		///   <para>The sprite that is used to render this image.</para>
		/// </summary>
		public Sprite sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_Sprite, value))
				{
					this.SetAllDirty();
				}
			}
		}

		/// <summary>
		///   <para>Set an override sprite to be used for rendering.</para>
		/// </summary>
		public Sprite overrideSprite
		{
			get
			{
				return (!(this.m_OverrideSprite == null)) ? this.m_OverrideSprite : this.sprite;
			}
			set
			{
				if (SetPropertyUtility.SetClass<Sprite>(ref this.m_OverrideSprite, value))
				{
					this.SetAllDirty();
				}
			}
		}

		/// <summary>
		///   <para>How the Image is draw.</para>
		/// </summary>
		public Image.Type type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Image.Type>(ref this.m_Type, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>Whether this image should preserve its Sprite aspect ratio.</para>
		/// </summary>
		public bool preserveAspect
		{
			get
			{
				return this.m_PreserveAspect;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_PreserveAspect, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>Whether or not to render the center of a Tiled or Sliced image.</para>
		/// </summary>
		public bool fillCenter
		{
			get
			{
				return this.m_FillCenter;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_FillCenter, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>What type of fill method to use.</para>
		/// </summary>
		public Image.FillMethod fillMethod
		{
			get
			{
				return this.m_FillMethod;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<Image.FillMethod>(ref this.m_FillMethod, value))
				{
					this.SetVerticesDirty();
					this.m_FillOrigin = 0;
				}
			}
		}

		/// <summary>
		///   <para>Amount of the Image shown when the Image.type is set to Image.Type.Filled.</para>
		/// </summary>
		public float fillAmount
		{
			get
			{
				return this.m_FillAmount;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_FillAmount, Mathf.Clamp01(value)))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>Whether the Image should be filled clockwise (true) or counter-clockwise (false).</para>
		/// </summary>
		public bool fillClockwise
		{
			get
			{
				return this.m_FillClockwise;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<bool>(ref this.m_FillClockwise, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>Controls the origin point of the Fill process. Value means different things with each fill method.</para>
		/// </summary>
		public int fillOrigin
		{
			get
			{
				return this.m_FillOrigin;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_FillOrigin, value))
				{
					this.SetVerticesDirty();
				}
			}
		}

		/// <summary>
		///   <para>The alpha threshold specifying the minimum alpha a pixel must have for the event to be passed through.</para>
		/// </summary>
		public float eventAlphaThreshold
		{
			get
			{
				return this.m_EventAlphaThreshold;
			}
			set
			{
				this.m_EventAlphaThreshold = value;
			}
		}

		/// <summary>
		///   <para>The image's texture. (ReadOnly).</para>
		/// </summary>
		public override Texture mainTexture
		{
			get
			{
				if (!(this.overrideSprite == null))
				{
					return this.overrideSprite.texture;
				}
				if (this.material != null && this.material.mainTexture != null)
				{
					return this.material.mainTexture;
				}
				return Graphic.s_WhiteTexture;
			}
		}

		/// <summary>
		///   <para>True if the sprite used has borders.</para>
		/// </summary>
		public bool hasBorder
		{
			get
			{
				return this.overrideSprite != null && this.overrideSprite.border.sqrMagnitude > 0f;
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				float num = 100f;
				if (this.sprite)
				{
					num = this.sprite.pixelsPerUnit;
				}
				float num2 = 100f;
				if (base.canvas)
				{
					num2 = base.canvas.referencePixelsPerUnit;
				}
				return num / num2;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.minWidth.</para>
		/// </summary>
		public virtual float minWidth
		{
			get
			{
				return 0f;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.preferredWidth.</para>
		/// </summary>
		public virtual float preferredWidth
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
				{
					return DataUtility.GetMinSize(this.overrideSprite).x / this.pixelsPerUnit;
				}
				return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.flexibleWidth.</para>
		/// </summary>
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.minHeight.</para>
		/// </summary>
		public virtual float minHeight
		{
			get
			{
				return 0f;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.preferredHeight.</para>
		/// </summary>
		public virtual float preferredHeight
		{
			get
			{
				if (this.overrideSprite == null)
				{
					return 0f;
				}
				if (this.type == Image.Type.Sliced || this.type == Image.Type.Tiled)
				{
					return DataUtility.GetMinSize(this.overrideSprite).y / this.pixelsPerUnit;
				}
				return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.flexibleHeight.</para>
		/// </summary>
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.layoutPriority.</para>
		/// </summary>
		public virtual int layoutPriority
		{
			get
			{
				return 0;
			}
		}

		protected Image()
		{
			base.useLegacyMeshGeneration = false;
		}

		/// <summary>
		///   <para>Serialization Callback.</para>
		/// </summary>
		public virtual void OnBeforeSerialize()
		{
		}

		/// <summary>
		///   <para>Serialization Callback.</para>
		/// </summary>
		public virtual void OnAfterDeserialize()
		{
			if (this.m_FillOrigin < 0)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillMethod == Image.FillMethod.Horizontal && this.m_FillOrigin > 1)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillMethod == Image.FillMethod.Vertical && this.m_FillOrigin > 1)
			{
				this.m_FillOrigin = 0;
			}
			else if (this.m_FillOrigin > 3)
			{
				this.m_FillOrigin = 0;
			}
			this.m_FillAmount = Mathf.Clamp(this.m_FillAmount, 0f, 1f);
		}

		private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			Vector4 vector = (!(this.overrideSprite == null)) ? DataUtility.GetPadding(this.overrideSprite) : Vector4.zero;
			Vector2 vector2 = (!(this.overrideSprite == null)) ? new Vector2(this.overrideSprite.rect.width, this.overrideSprite.rect.height) : Vector2.zero;
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			int num = Mathf.RoundToInt(vector2.x);
			int num2 = Mathf.RoundToInt(vector2.y);
			Vector4 result = new Vector4(vector.x / (float)num, vector.y / (float)num2, ((float)num - vector.z) / (float)num, ((float)num2 - vector.w) / (float)num2);
			if (shouldPreserveAspect && vector2.sqrMagnitude > 0f)
			{
				float num3 = vector2.x / vector2.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > num4)
				{
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
				}
				else
				{
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
				}
			}
			result = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * result.x, pixelAdjustedRect.y + pixelAdjustedRect.height * result.y, pixelAdjustedRect.x + pixelAdjustedRect.width * result.z, pixelAdjustedRect.y + pixelAdjustedRect.height * result.w);
			return result;
		}

		/// <summary>
		///   <para>Adjusts the image size to make it pixel-perfect.</para>
		/// </summary>
		public override void SetNativeSize()
		{
			if (this.overrideSprite != null)
			{
				float x = this.overrideSprite.rect.width / this.pixelsPerUnit;
				float y = this.overrideSprite.rect.height / this.pixelsPerUnit;
				base.rectTransform.anchorMax = base.rectTransform.anchorMin;
				base.rectTransform.sizeDelta = new Vector2(x, y);
				this.SetAllDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (this.overrideSprite == null)
			{
				base.OnPopulateMesh(toFill);
				return;
			}
			switch (this.type)
			{
			case Image.Type.Simple:
				this.GenerateSimpleSprite(toFill, this.m_PreserveAspect);
				break;
			case Image.Type.Sliced:
				this.GenerateSlicedSprite(toFill);
				break;
			case Image.Type.Tiled:
				this.GenerateTiledSprite(toFill);
				break;
			case Image.Type.Filled:
				this.GenerateFilledSprite(toFill, this.m_PreserveAspect);
				break;
			}
		}

		private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
		{
			Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
			Vector4 vector = (!(this.overrideSprite != null)) ? Vector4.zero : DataUtility.GetOuterUV(this.overrideSprite);
			Color color = base.color;
			vh.Clear();
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, new Vector2(vector.x, vector.y));
			vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, new Vector2(vector.x, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, new Vector2(vector.z, vector.w));
			vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, new Vector2(vector.z, vector.y));
			vh.AddTriangle(0, 1, 2);
			vh.AddTriangle(2, 3, 0);
		}

		private void GenerateSlicedSprite(VertexHelper toFill)
		{
			if (!this.hasBorder)
			{
				this.GenerateSimpleSprite(toFill, false);
				return;
			}
			Vector4 vector;
			Vector4 vector2;
			Vector4 a;
			Vector4 a2;
			if (this.overrideSprite != null)
			{
				vector = DataUtility.GetOuterUV(this.overrideSprite);
				vector2 = DataUtility.GetInnerUV(this.overrideSprite);
				a = DataUtility.GetPadding(this.overrideSprite);
				a2 = this.overrideSprite.border;
			}
			else
			{
				vector = Vector4.zero;
				vector2 = Vector4.zero;
				a = Vector4.zero;
				a2 = Vector4.zero;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			a2 = this.GetAdjustedBorders(a2 / this.pixelsPerUnit, pixelAdjustedRect);
			a /= this.pixelsPerUnit;
			Image.s_VertScratch[0] = new Vector2(a.x, a.y);
			Image.s_VertScratch[3] = new Vector2(pixelAdjustedRect.width - a.z, pixelAdjustedRect.height - a.w);
			Image.s_VertScratch[1].x = a2.x;
			Image.s_VertScratch[1].y = a2.y;
			Image.s_VertScratch[2].x = pixelAdjustedRect.width - a2.z;
			Image.s_VertScratch[2].y = pixelAdjustedRect.height - a2.w;
			for (int i = 0; i < 4; i++)
			{
				Vector2[] expr_172_cp_0 = Image.s_VertScratch;
				int expr_172_cp_1 = i;
				expr_172_cp_0[expr_172_cp_1].x = expr_172_cp_0[expr_172_cp_1].x + pixelAdjustedRect.x;
				Vector2[] expr_191_cp_0 = Image.s_VertScratch;
				int expr_191_cp_1 = i;
				expr_191_cp_0[expr_191_cp_1].y = expr_191_cp_0[expr_191_cp_1].y + pixelAdjustedRect.y;
			}
			Image.s_UVScratch[0] = new Vector2(vector.x, vector.y);
			Image.s_UVScratch[1] = new Vector2(vector2.x, vector2.y);
			Image.s_UVScratch[2] = new Vector2(vector2.z, vector2.w);
			Image.s_UVScratch[3] = new Vector2(vector.z, vector.w);
			toFill.Clear();
			for (int j = 0; j < 3; j++)
			{
				int num = j + 1;
				for (int k = 0; k < 3; k++)
				{
					if (this.m_FillCenter || j != 1 || k != 1)
					{
						int num2 = k + 1;
						Image.AddQuad(toFill, new Vector2(Image.s_VertScratch[j].x, Image.s_VertScratch[k].y), new Vector2(Image.s_VertScratch[num].x, Image.s_VertScratch[num2].y), base.color, new Vector2(Image.s_UVScratch[j].x, Image.s_UVScratch[k].y), new Vector2(Image.s_UVScratch[num].x, Image.s_UVScratch[num2].y));
					}
				}
			}
		}

		private void GenerateTiledSprite(VertexHelper toFill)
		{
			Vector4 vector;
			Vector4 vector2;
			Vector4 a;
			Vector2 vector3;
			if (this.overrideSprite != null)
			{
				vector = DataUtility.GetOuterUV(this.overrideSprite);
				vector2 = DataUtility.GetInnerUV(this.overrideSprite);
				a = this.overrideSprite.border;
				vector3 = this.overrideSprite.rect.size;
			}
			else
			{
				vector = Vector4.zero;
				vector2 = Vector4.zero;
				a = Vector4.zero;
				vector3 = Vector2.one * 100f;
			}
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float num = (vector3.x - a.x - a.z) / this.pixelsPerUnit;
			float num2 = (vector3.y - a.y - a.w) / this.pixelsPerUnit;
			a = this.GetAdjustedBorders(a / this.pixelsPerUnit, pixelAdjustedRect);
			Vector2 uvMin = new Vector2(vector2.x, vector2.y);
			Vector2 vector4 = new Vector2(vector2.z, vector2.w);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = base.color;
			float x = a.x;
			float num3 = pixelAdjustedRect.width - a.z;
			float y = a.y;
			float num4 = pixelAdjustedRect.height - a.w;
			toFill.Clear();
			Vector2 uvMax = vector4;
			if (num == 0f)
			{
				num = num3 - x;
			}
			if (num2 == 0f)
			{
				num2 = num4 - y;
			}
			if (this.m_FillCenter)
			{
				for (float num5 = y; num5 < num4; num5 += num2)
				{
					float num6 = num5 + num2;
					if (num6 > num4)
					{
						uvMax.y = uvMin.y + (vector4.y - uvMin.y) * (num4 - num5) / (num6 - num5);
						num6 = num4;
					}
					uvMax.x = vector4.x;
					for (float num7 = x; num7 < num3; num7 += num)
					{
						float num8 = num7 + num;
						if (num8 > num3)
						{
							uvMax.x = uvMin.x + (vector4.x - uvMin.x) * (num3 - num7) / (num8 - num7);
							num8 = num3;
						}
						Image.AddQuad(toFill, new Vector2(num7, num5) + pixelAdjustedRect.position, new Vector2(num8, num6) + pixelAdjustedRect.position, base.color, uvMin, uvMax);
					}
				}
			}
			if (this.hasBorder)
			{
				uvMax = vector4;
				for (float num9 = y; num9 < num4; num9 += num2)
				{
					float num10 = num9 + num2;
					if (num10 > num4)
					{
						uvMax.y = uvMin.y + (vector4.y - uvMin.y) * (num4 - num9) / (num10 - num9);
						num10 = num4;
					}
					Image.AddQuad(toFill, new Vector2(0f, num9) + pixelAdjustedRect.position, new Vector2(x, num10) + pixelAdjustedRect.position, base.color, new Vector2(vector.x, uvMin.y), new Vector2(uvMin.x, uvMax.y));
					Image.AddQuad(toFill, new Vector2(num3, num9) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, num10) + pixelAdjustedRect.position, base.color, new Vector2(vector4.x, uvMin.y), new Vector2(vector.z, uvMax.y));
				}
				uvMax = vector4;
				for (float num11 = x; num11 < num3; num11 += num)
				{
					float num12 = num11 + num;
					if (num12 > num3)
					{
						uvMax.x = uvMin.x + (vector4.x - uvMin.x) * (num3 - num11) / (num12 - num11);
						num12 = num3;
					}
					Image.AddQuad(toFill, new Vector2(num11, 0f) + pixelAdjustedRect.position, new Vector2(num12, y) + pixelAdjustedRect.position, base.color, new Vector2(uvMin.x, vector.y), new Vector2(uvMax.x, uvMin.y));
					Image.AddQuad(toFill, new Vector2(num11, num4) + pixelAdjustedRect.position, new Vector2(num12, pixelAdjustedRect.height) + pixelAdjustedRect.position, base.color, new Vector2(uvMin.x, vector4.y), new Vector2(uvMax.x, vector.w));
				}
				Image.AddQuad(toFill, new Vector2(0f, 0f) + pixelAdjustedRect.position, new Vector2(x, y) + pixelAdjustedRect.position, base.color, new Vector2(vector.x, vector.y), new Vector2(uvMin.x, uvMin.y));
				Image.AddQuad(toFill, new Vector2(num3, 0f) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, y) + pixelAdjustedRect.position, base.color, new Vector2(vector4.x, vector.y), new Vector2(vector.z, uvMin.y));
				Image.AddQuad(toFill, new Vector2(0f, num4) + pixelAdjustedRect.position, new Vector2(x, pixelAdjustedRect.height) + pixelAdjustedRect.position, base.color, new Vector2(vector.x, vector4.y), new Vector2(uvMin.x, vector.w));
				Image.AddQuad(toFill, new Vector2(num3, num4) + pixelAdjustedRect.position, new Vector2(pixelAdjustedRect.width, pixelAdjustedRect.height) + pixelAdjustedRect.position, base.color, new Vector2(vector4.x, vector4.y), new Vector2(vector.z, vector.w));
			}
		}

		private static void AddQuad(VertexHelper vertexHelper, Vector3[] quadPositions, Color32 color, Vector3[] quadUVs)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			for (int i = 0; i < 4; i++)
			{
				vertexHelper.AddVert(quadPositions[i], color, quadUVs[i]);
			}
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		private static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0f), color, new Vector2(uvMin.x, uvMin.y));
			vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0f), color, new Vector2(uvMin.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0f), color, new Vector2(uvMax.x, uvMax.y));
			vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0f), color, new Vector2(uvMax.x, uvMin.y));
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
		{
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (rect.size[i] < num && num != 0f)
				{
					float num2 = rect.size[i] / num;
					int index;
					int expr_56 = index = i;
					float num3 = border[index];
					border[expr_56] = num3 * num2;
					int expr_75 = index = i + 2;
					num3 = border[index];
					border[expr_75] = num3 * num2;
				}
			}
			return border;
		}

		private void GenerateFilledSprite(VertexHelper toFill, bool preserveAspect)
		{
			toFill.Clear();
			if (this.m_FillAmount < 0.001f)
			{
				return;
			}
			Vector4 drawingDimensions = this.GetDrawingDimensions(preserveAspect);
			Vector4 vector = (!(this.overrideSprite != null)) ? Vector4.zero : DataUtility.GetOuterUV(this.overrideSprite);
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = base.color;
			float num = vector.x;
			float num2 = vector.y;
			float num3 = vector.z;
			float num4 = vector.w;
			if (this.m_FillMethod == Image.FillMethod.Horizontal || this.m_FillMethod == Image.FillMethod.Vertical)
			{
				if (this.fillMethod == Image.FillMethod.Horizontal)
				{
					float num5 = (num3 - num) * this.m_FillAmount;
					if (this.m_FillOrigin == 1)
					{
						drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
						num = num3 - num5;
					}
					else
					{
						drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.m_FillAmount;
						num3 = num + num5;
					}
				}
				else if (this.fillMethod == Image.FillMethod.Vertical)
				{
					float num6 = (num4 - num2) * this.m_FillAmount;
					if (this.m_FillOrigin == 1)
					{
						drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
						num2 = num4 - num6;
					}
					else
					{
						drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.m_FillAmount;
						num4 = num2 + num6;
					}
				}
			}
			Image.s_Xy[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
			Image.s_Xy[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
			Image.s_Xy[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
			Image.s_Xy[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
			Image.s_Uv[0] = new Vector2(num, num2);
			Image.s_Uv[1] = new Vector2(num, num4);
			Image.s_Uv[2] = new Vector2(num3, num4);
			Image.s_Uv[3] = new Vector2(num3, num2);
			if (this.m_FillAmount < 1f && this.m_FillMethod != Image.FillMethod.Horizontal && this.m_FillMethod != Image.FillMethod.Vertical)
			{
				if (this.fillMethod == Image.FillMethod.Radial90)
				{
					if (Image.RadialCut(Image.s_Xy, Image.s_Uv, this.m_FillAmount, this.m_FillClockwise, this.m_FillOrigin))
					{
						Image.AddQuad(toFill, Image.s_Xy, base.color, Image.s_Uv);
					}
				}
				else if (this.fillMethod == Image.FillMethod.Radial180)
				{
					for (int i = 0; i < 2; i++)
					{
						int num7 = (this.m_FillOrigin <= 1) ? 0 : 1;
						float t;
						float t2;
						float t3;
						float t4;
						if (this.m_FillOrigin == 0 || this.m_FillOrigin == 2)
						{
							t = 0f;
							t2 = 1f;
							if (i == num7)
							{
								t3 = 0f;
								t4 = 0.5f;
							}
							else
							{
								t3 = 0.5f;
								t4 = 1f;
							}
						}
						else
						{
							t3 = 0f;
							t4 = 1f;
							if (i == num7)
							{
								t = 0.5f;
								t2 = 1f;
							}
							else
							{
								t = 0f;
								t2 = 0.5f;
							}
						}
						Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t3);
						Image.s_Xy[1].x = Image.s_Xy[0].x;
						Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t4);
						Image.s_Xy[3].x = Image.s_Xy[2].x;
						Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
						Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t2);
						Image.s_Xy[2].y = Image.s_Xy[1].y;
						Image.s_Xy[3].y = Image.s_Xy[0].y;
						Image.s_Uv[0].x = Mathf.Lerp(num, num3, t3);
						Image.s_Uv[1].x = Image.s_Uv[0].x;
						Image.s_Uv[2].x = Mathf.Lerp(num, num3, t4);
						Image.s_Uv[3].x = Image.s_Uv[2].x;
						Image.s_Uv[0].y = Mathf.Lerp(num2, num4, t);
						Image.s_Uv[1].y = Mathf.Lerp(num2, num4, t2);
						Image.s_Uv[2].y = Image.s_Uv[1].y;
						Image.s_Uv[3].y = Image.s_Uv[0].y;
						float value = (!this.m_FillClockwise) ? (this.m_FillAmount * 2f - (float)(1 - i)) : (this.fillAmount * 2f - (float)i);
						if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(value), this.m_FillClockwise, (i + this.m_FillOrigin + 3) % 4))
						{
							Image.AddQuad(toFill, Image.s_Xy, base.color, Image.s_Uv);
						}
					}
				}
				else if (this.fillMethod == Image.FillMethod.Radial360)
				{
					for (int j = 0; j < 4; j++)
					{
						float t5;
						float t6;
						if (j < 2)
						{
							t5 = 0f;
							t6 = 0.5f;
						}
						else
						{
							t5 = 0.5f;
							t6 = 1f;
						}
						float t7;
						float t8;
						if (j == 0 || j == 3)
						{
							t7 = 0f;
							t8 = 0.5f;
						}
						else
						{
							t7 = 0.5f;
							t8 = 1f;
						}
						Image.s_Xy[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t5);
						Image.s_Xy[1].x = Image.s_Xy[0].x;
						Image.s_Xy[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, t6);
						Image.s_Xy[3].x = Image.s_Xy[2].x;
						Image.s_Xy[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t7);
						Image.s_Xy[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t8);
						Image.s_Xy[2].y = Image.s_Xy[1].y;
						Image.s_Xy[3].y = Image.s_Xy[0].y;
						Image.s_Uv[0].x = Mathf.Lerp(num, num3, t5);
						Image.s_Uv[1].x = Image.s_Uv[0].x;
						Image.s_Uv[2].x = Mathf.Lerp(num, num3, t6);
						Image.s_Uv[3].x = Image.s_Uv[2].x;
						Image.s_Uv[0].y = Mathf.Lerp(num2, num4, t7);
						Image.s_Uv[1].y = Mathf.Lerp(num2, num4, t8);
						Image.s_Uv[2].y = Image.s_Uv[1].y;
						Image.s_Uv[3].y = Image.s_Uv[0].y;
						float value2 = (!this.m_FillClockwise) ? (this.m_FillAmount * 4f - (float)(3 - (j + this.m_FillOrigin) % 4)) : (this.m_FillAmount * 4f - (float)((j + this.m_FillOrigin) % 4));
						if (Image.RadialCut(Image.s_Xy, Image.s_Uv, Mathf.Clamp01(value2), this.m_FillClockwise, (j + 2) % 4))
						{
							Image.AddQuad(toFill, Image.s_Xy, base.color, Image.s_Uv);
						}
					}
				}
			}
			else
			{
				Image.AddQuad(toFill, Image.s_Xy, base.color, Image.s_Uv);
			}
		}

		private static bool RadialCut(Vector3[] xy, Vector3[] uv, float fill, bool invert, int corner)
		{
			if (fill < 0.001f)
			{
				return false;
			}
			if ((corner & 1) == 1)
			{
				invert = !invert;
			}
			if (!invert && fill > 0.999f)
			{
				return true;
			}
			float num = Mathf.Clamp01(fill);
			if (invert)
			{
				num = 1f - num;
			}
			num *= 1.57079637f;
			float cos = Mathf.Cos(num);
			float sin = Mathf.Sin(num);
			Image.RadialCut(xy, cos, sin, invert, corner);
			Image.RadialCut(uv, cos, sin, invert, corner);
			return true;
		}

		private static void RadialCut(Vector3[] xy, float cos, float sin, bool invert, int corner)
		{
			int num = (corner + 1) % 4;
			int num2 = (corner + 2) % 4;
			int num3 = (corner + 3) % 4;
			if ((corner & 1) == 1)
			{
				if (sin > cos)
				{
					cos /= sin;
					sin = 1f;
					if (invert)
					{
						xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
						xy[num2].x = xy[num].x;
					}
				}
				else if (cos > sin)
				{
					sin /= cos;
					cos = 1f;
					if (!invert)
					{
						xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
						xy[num3].y = xy[num2].y;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}
				if (!invert)
				{
					xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
				}
				else
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
				}
			}
			else
			{
				if (cos > sin)
				{
					sin /= cos;
					cos = 1f;
					if (!invert)
					{
						xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
						xy[num2].y = xy[num].y;
					}
				}
				else if (sin > cos)
				{
					cos /= sin;
					sin = 1f;
					if (invert)
					{
						xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
						xy[num3].x = xy[num2].x;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}
				if (invert)
				{
					xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
				}
				else
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
				}
			}
		}

		/// <summary>
		///   <para>See ILayoutElement.CalculateLayoutInputHorizontal.</para>
		/// </summary>
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		/// <summary>
		///   <para>See ILayoutElement.CalculateLayoutInputVertical.</para>
		/// </summary>
		public virtual void CalculateLayoutInputVertical()
		{
		}

		/// <summary>
		///   <para>See:ICanvasRaycastFilter.</para>
		/// </summary>
		/// <param name="screenPoint"></param>
		/// <param name="eventCamera"></param>
		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (this.m_EventAlphaThreshold >= 1f)
			{
				return true;
			}
			Sprite overrideSprite = this.overrideSprite;
			if (overrideSprite == null)
			{
				return true;
			}
			Vector2 local;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out local);
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			local.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
			local.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
			local = this.MapCoordinate(local, pixelAdjustedRect);
			Rect textureRect = overrideSprite.textureRect;
			Vector2 vector = new Vector2(local.x / textureRect.width, local.y / textureRect.height);
			float u = Mathf.Lerp(textureRect.x, textureRect.xMax, vector.x) / (float)overrideSprite.texture.width;
			float v = Mathf.Lerp(textureRect.y, textureRect.yMax, vector.y) / (float)overrideSprite.texture.height;
			bool result;
			try
			{
				result = (overrideSprite.texture.GetPixelBilinear(u, v).a >= this.m_EventAlphaThreshold);
			}
			catch (UnityException ex)
			{
				Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
				result = true;
			}
			return result;
		}

		private Vector2 MapCoordinate(Vector2 local, Rect rect)
		{
			Rect rect2 = this.sprite.rect;
			if (this.type == Image.Type.Simple || this.type == Image.Type.Filled)
			{
				return new Vector2(local.x * rect2.width / rect.width, local.y * rect2.height / rect.height);
			}
			Vector4 border = this.sprite.border;
			Vector4 adjustedBorders = this.GetAdjustedBorders(border / this.pixelsPerUnit, rect);
			for (int i = 0; i < 2; i++)
			{
				if (local[i] > adjustedBorders[i])
				{
					if (rect.size[i] - local[i] <= adjustedBorders[i + 2])
					{
						int index;
						int expr_C7 = index = i;
						float num = local[index];
						local[expr_C7] = num - (rect.size[i] - rect2.size[i]);
					}
					else if (this.type == Image.Type.Sliced)
					{
						float t = Mathf.InverseLerp(adjustedBorders[i], rect.size[i] - adjustedBorders[i + 2], local[i]);
						local[i] = Mathf.Lerp(border[i], rect2.size[i] - border[i + 2], t);
					}
					else
					{
						int index;
						int expr_182 = index = i;
						float num = local[index];
						local[expr_182] = num - adjustedBorders[i];
						local[i] = Mathf.Repeat(local[i], rect2.size[i] - border[i] - border[i + 2]);
						int expr_1E0 = index = i;
						num = local[index];
						local[expr_1E0] = num + border[i];
					}
				}
			}
			return local;
		}
	}
}
