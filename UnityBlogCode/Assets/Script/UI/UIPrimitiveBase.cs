using System;

namespace UnityEngine.UI.Extensions
{
    public enum ResolutionMode
    {
        None,
        PerSegment,
        PerLine,
    }

    /// <summary>
    /// ui扩展基类
    /// </summary>
    public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        protected static Material s_ETC1DefaultUI = null;

        /// <summary>
        /// 默认Sprite
        /// </summary>
        [SerializeField] private Sprite m_Sprite;

        public Sprite sprite
        {
            get => m_Sprite;
            set
            {
                if (!SetPropertyUtility.SetClass(ref m_Sprite, value)) return;
                GeneratedUVs();
                SetAllDirty();
            }
        }

        /// <summary>
        /// 覆盖的Sprite
        /// </summary>
        [NonSerialized] private Sprite m_OverrideSprite;

        public Sprite overrideSprite
        {
            get => activeSprite;
            set
            {
                if (!SetPropertyUtility.SetClass(ref m_OverrideSprite, value)) return;
                GeneratedUVs();
                SetAllDirty();
            }
        }

        /// <summary>
        /// 正在使用中的Sprite
        /// </summary>
        public Sprite activeSprite => m_OverrideSprite != null ? m_OverrideSprite : m_Sprite;


        /// <summary>
        /// alpha阈值
        /// </summary>
        internal float m_EventAlphaThreshold = 1;

        public float eventAlohaThreshold
        {
            get => m_EventAlphaThreshold;
            set => m_EventAlphaThreshold = value;
        }

        [SerializeField] private ResolutionMode m_improveResolution;

        public ResolutionMode ImproveResolution
        {
            get => m_improveResolution;
            set
            {
                m_improveResolution = value;
                SetAllDirty();
            }
        }

        [SerializeField] protected float m_Resolution;

        public float Resolution
        {
            get => m_Resolution;
            set
            {
                m_Resolution = value;
                SetAllDirty();
            }
        }

        [SerializeField] private bool m_useNativeSize;

        public bool UseNativeSize
        {
            get => m_useNativeSize;
            set
            {
                m_useNativeSize = value;
                SetAllDirty();
            }
        }

        protected UIPrimitiveBase()
        {
            useLegacyMeshGeneration = false;
        }

        public static Material defaultETC1GraphicMaterial
        {
            get
            {
                if (s_ETC1DefaultUI == null)
                {
                    s_ETC1DefaultUI = Canvas.GetETC1SupportedCanvasMaterial();
                }

                return s_ETC1DefaultUI;
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (activeSprite == null)
                {
                    if (material != null && material.mainTexture != null)
                    {
                        return material.mainTexture;
                    }

                    return s_WhiteTexture;
                }

                return activeSprite.texture;
            }
        }

        public bool hasBorder
        {
            get
            {
                if (activeSprite != null)
                {
                    var v = activeSprite.border;
                    return v.sqrMagnitude > 0.0f;
                }

                return false;
            }
        }

        public float pixelsPerUnit
        {
            get
            {
                float spritePixelsPerUnit = 100;
                if (activeSprite)
                {
                    spritePixelsPerUnit = activeSprite.pixelsPerUnit;
                }

                float referencePixelsPerUnit = 100;
                if (canvas)
                {
                    referencePixelsPerUnit = canvas.referencePixelsPerUnit;
                }

                return spritePixelsPerUnit / referencePixelsPerUnit;
            }
        }

        public override Material material
        {
            get
            {
                if (m_Material != null)
                {
                    return m_Material;
                }

                if (activeSprite && activeSprite.associatedAlphaSplitTexture != null)
                {
                    return defaultETC1GraphicMaterial;
                }

                return defaultMaterial;
            }
            set => base.material = value;
        }

        //vertex buffer object
        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];
            for (var i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }

            return vbo;
        }

        /// <summary>
        /// 生产uv
        /// </summary>
        protected virtual void GeneratedUVs()
        {
        }

        protected virtual void ResolutionToNativeSize(float distance)
        {
        }

        #region ILayoutElement Interface

        public virtual void CalculateLayoutInputHorizontal()
        {
        }

        public virtual void CalculateLayoutInputVertical()
        {
        }

        public virtual float minWidth => 0;

        public virtual float preferredWidth
        {
            get
            {
                // if()
            }
        }

        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            SetAllDirty();
        }
    }
}