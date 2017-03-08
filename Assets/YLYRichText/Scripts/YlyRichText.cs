/*
Copyright (C) 2016 yly(cantry100@163.com) - All Rights Reserved
YLY富文本
author：雨Lu尧
email：cantry100@163.com
blog：http://www.hiwrz.com
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class YlyRichText : MaskableGraphic, ILayoutElement, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	private enum CharType{
		Normal = 0, //普通字符(normal character)
		UnderWire = 1, //下划线(underline character)
		DeleteWire = 2, //删除线(deleteline character)
	};
	private enum WireType{
		UnderWire = 0, //下划线(underline)
		DeleteWire = 1, //删除线(deleteline)
	};

	//超链接点击回调函数，函数参数为字符串类型，请查看YlyDelegateUtil.cs
	//(hyperLink click callback function, function parameter as a string type, please see YlyDelegateUtil.cs)
	public YlyDelegateUtil.StringDelegate onLinkClick;
	//静态表情路径，可根据实际情况修改(static emote asset path, can be modified according to actual condition)
	public static string emotePngPathFormat = "Assets/YlyRichText/Atlas/i{0}.png";
	//动态表情路径，可根据实际情况修改(dynamic emote asset path, can be modified according to actual condition)
	public static string animEmotePrefabPathFormat = "Assets/YlyRichText/Prefabs/i{0}.prefab";

	[TextArea(3, 10)][SerializeField, Tooltip("文本内容")]
	string m_Text = ""; //文本内容
	[SerializeField, Tooltip("字体")]
	static Font m_Font;
	[SerializeField, Tooltip("字体大小")]
	int m_FontSize = 28; //字体大小
	[SerializeField, Tooltip("行高")]
	float m_LineHeght = 40; //行高
	[SerializeField, Tooltip("行之间间隔")]
	float m_LineSpacing = 0f; //行之间间隔
	[SerializeField, Tooltip("字符之间的横向间隔")]
	float m_OffCharX = 8; //字符之间的横向间隔;
	[SerializeField, Tooltip("最大字符数")]
	int m_MaxChars = 1000; //最大字符数
	[SerializeField, Tooltip("是否自定义宽度自动换行")]
	bool m_IsCustomWidthToNewLine = false; //是否自定义宽度自动换行
	[SerializeField, Tooltip("自定义宽度自动换行")]
	float m_CustomWidthToNewLine = 100f; //自定义宽度自动换行
	[SerializeField, Tooltip("是否需要描边，注意：描边会比原来多4倍顶点，unity限制每个mesh最多65000个顶点，所以，能尽量不用就不用吧")]
	bool m_IsNeedOutLine = false; //是否需要描边，注意：unity限制每个mesh最多65000个顶点，所以，能尽量不用就不用吧
	[SerializeField, Tooltip("描边颜色")]
	Color32 m_OutLineColor = Color.black; //描边颜色
	[SerializeField, Tooltip("颜色")]
	Color32 m_TColor = Color.black; //描边颜色
	[SerializeField, Tooltip("是否自适应宽高")]
	bool m_IsAutoAdaptiveWidthHeight = false; //是否自适应宽高
	[SerializeField, Tooltip("横向Wrap模式")]
	HorizontalWrapMode m_HorizontalOverflow = HorizontalWrapMode.Wrap;
	[SerializeField, Tooltip("纵向Wrap模式")]
	VerticalWrapMode m_VerticalOverflow = VerticalWrapMode.Truncate;
	[SerializeField, Tooltip("对齐方式")]
	TextAnchor m_Alignment = TextAnchor.UpperLeft;
	[SerializeField, Tooltip("是否启用rich text模式")]
	bool m_EnableRichText = true;

	YlyRichTextParser m_TxtParser = null;
	List<LineData> m_Lines = new List<LineData>();
	Dictionary<string, AssetData> m_AssetDataDict = new Dictionary<string, AssetData>();
	float m_PreferredHeight = 0f;
	float m_PreferredWidth = 0f;
	int[] m_StrMask = new int[0];
	Dictionary<int, string> m_ParamDict = new Dictionary<int, string>();
	Dictionary<string, int> m_PreLoadAssetDic = new Dictionary<string, int>();
	string m_ParsedText = "";
	string m_RefHeightGoName = "YlyRichTextRefHeightGo";
	bool m_IsFontTextureDirty = false;
	bool m_IsPopulateMeshDone = false;
	bool m_IsDrag = false;

	YlyRichTextParser txtParser {
		get
		{
			if(m_TxtParser == null){
				m_TxtParser = new YlyRichTextParser();
			}
			return m_TxtParser;
		}
	}

	public Font font {
		get
		{
			return m_Font;
		}
		set
		{
			m_Font = value;

			UpdateLines();
		}
	}

	public Color color {
		get
		{
			return m_TColor;
		}
		set
		{
			m_TColor = value;

			ParseText();

			UpdateLines();
		}
	}

	public string text //设置文本内容
	{
		get
		{
			return m_Text;
		}
		set
		{
			m_Text = value;

			ParseText();

			UpdateLines();
		}
	}

	public int fontSize //设置字体大小
	{
		get
		{
			return m_FontSize;
		}
		set
		{
			m_FontSize = value;
			if(m_FontSize <= 0){
				m_FontSize = 1;
			}

			UpdateLines();
		}
	}

	public float lineHeght //设置行高
	{
		get
		{
			return m_LineHeght;
		}
		set
		{
			m_LineHeght = value;

			UpdateLines();
		}
	}

	public float lineSpacing //设置行间距
	{
		get
		{
			return m_LineSpacing;
		}
		set
		{
			m_LineSpacing = value;

			UpdateLines();
		}
	}

	public float offCharX //设置字符之间的横向间隔
	{
		get
		{
			return m_OffCharX;
		}
		set
		{
			m_OffCharX = value;

			UpdateLines();
		}
	}

	public int maxChars //设置最大字符数
	{
		get
		{
			return m_MaxChars;
		}
		set
		{
			m_MaxChars = value;
			if(m_MaxChars < 0){
				m_MaxChars = 0;
			}

			ParseText();

			UpdateLines();
		}
	}
		
	public bool isAutoAdaptiveWidthHeight //设置是否自适应宽高
	{
		get
		{
			return m_IsAutoAdaptiveWidthHeight;
		}
		set
		{
			m_IsAutoAdaptiveWidthHeight = value;

			UpdateLines();
		}
	}

	public bool isCustomWidthToNewLine //设置是否自定义宽度自动换行
	{
		get
		{
			return m_IsCustomWidthToNewLine;
		}
		set
		{
			m_IsCustomWidthToNewLine = value;

			UpdateLines();
		}
	}

	public float customWidthToNewLine //设置自定义宽度自动换行
	{
		get
		{
			return m_CustomWidthToNewLine;
		}
		set
		{
			m_CustomWidthToNewLine = value;

			UpdateLines();
		}
	}

	public bool isNeedOutLine //设置是否需要描边
	{
		get
		{
			return m_IsNeedOutLine;
		}
		set
		{
			m_IsNeedOutLine = value;

			UpdateLines();
		}
	}

	public Color32 outLineColor //设置描边颜色
	{
		get
		{
			return m_OutLineColor;
		}
		set
		{
			m_OutLineColor = value;

			if(!m_IsNeedOutLine){
				return;
			}

			UpdateLines();
		}
	}

	public HorizontalWrapMode horizontalOverflow //设置横向wrap模式
	{
		get
		{
			return m_HorizontalOverflow;
		}
		set
		{
			m_HorizontalOverflow = value;

			UpdateLines();
		}
	}

	public VerticalWrapMode verticalOverflow //设置纵向wrap模式
	{
		get
		{
			return m_VerticalOverflow;
		}
		set
		{
			m_VerticalOverflow = value;

			UpdateLines();
		}
	}

	public bool enableRichText //设置是否启用rich text模式
	{
		get
		{
			return m_EnableRichText;
		}
		set
		{
			m_EnableRichText = value;

			ParseText();

			UpdateLines();
		}
	}

	public TextAnchor alignment //设置对齐方式
	{
		get
		{
			return m_Alignment;
		}
		set
		{
			m_Alignment = value;

			UpdateLines();
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (m_Font != null && m_Font.material != null && m_Font.material.mainTexture != null)
				return m_Font.material.mainTexture;

			if (m_Material != null)
				return m_Material.mainTexture;

			return base.mainTexture;
		}
	}

	public virtual float flexibleHeight { get { return -1; } }

	public virtual float flexibleWidth { get { return -1; } }

	public virtual int layoutPriority { get { return 0; } }

	public virtual float minHeight{ get { return 0; } }

	public virtual float minWidth{ get { return 0; } }

	public virtual float preferredHeight
	{
		get
		{
			return m_PreferredHeight;
		}
	}
	public virtual float preferredWidth
	{
		get
		{
			return m_PreferredWidth;
		}
	}
	public virtual void CalculateLayoutInputHorizontal() {}
	public virtual void CalculateLayoutInputVertical() {}

	//普通字符数据
	class CharData
	{
		public CharType cType = CharType.Normal; //字符类型
		public UIVertex[] vertices = new UIVertex[4];
	}

	//图片、prefab等资源数据
	class AssetData
	{
		public string assetPath;
		public int aType; //资源类型：10000静态表情   10100动态表情   10200路径表示的图片
		public UnityEngine.Object obj; //资源对象
		public float x = 0f;
		public float y = 0f;
		public float width = 0f;
		public float height = 0f;
	}

	//线数据
	class WireData
	{
		public WireType wireType = WireType.UnderWire; //线类型
		public float beginX = 0f;
		public float endX = 0f;
		public Color32 uColor = Color.blue;
	}

	//超链接数据
	class LinkData
	{
		public Rect rect = new Rect(0, 0, 0, 0); //包围框;
		public string arg; //参数;
	}

	//行数据
	class LineData
	{
		public float x = 0f;
		public float y = 0f;
		public float z = 0f;
		public float width = 0f;
		public float height = 0f;
		public float charMaxHeight = 0f;
		public float charMinY = 0f;
		public float charMaxY = 0f;
		public YlyRichText ylyRichText;
		public List<CharData> charDatas = new List<CharData>();
		public List<AssetData> assetDatas = new List<AssetData>();
		public List<LinkData> linkDatas = new List<LinkData>();

		public LineData(){
			
		}

		public void AddCharData(char cstr, CharacterInfo ci, float charBeginX, float charEndX, Color32 topColor, Color32 bottomColor){
			int ymin = ci.minY;
			int ymax = ci.maxY;

			if(ymin < charMinY){
				charMinY = ymin;
			}
			if(ymax > charMaxY){
				charMaxY = ymax;
			}
			float charHeight = charMaxY - charMinY;
			if(charHeight > charMaxHeight){
				charMaxHeight = charHeight;
			}
			if(charMaxHeight > height){
				height = charMaxHeight;
			}

			if (cstr == '\n') {
				return;
			}

			UIVertex uiVertex0 = new UIVertex ();
			uiVertex0.position = new Vector3 (charBeginX, ymax, z);
			uiVertex0.color = topColor;
			uiVertex0.uv0 = ci.uvTopLeft;

			UIVertex uiVertex1 = new UIVertex ();
			uiVertex1.position = new Vector3 (charEndX, ymax, z);
			uiVertex1.color = topColor;
			uiVertex1.uv0 = ci.uvTopRight;

			UIVertex uiVertex2 = new UIVertex ();
			uiVertex2.position = new Vector3 (charEndX, ymin, z);
			uiVertex2.color = bottomColor;
			uiVertex2.uv0 = ci.uvBottomRight;

			UIVertex uiVertex3 = new UIVertex ();
			uiVertex3.position = new Vector3 (charBeginX, ymin, z);
			uiVertex3.color = bottomColor;
			uiVertex3.uv0 = ci.uvBottomLeft;

			CharData charData = new CharData ();
			charData.vertices [0] = uiVertex0;
			charData.vertices [1] = uiVertex1;
			charData.vertices [2] = uiVertex2;
			charData.vertices [3] = uiVertex3;
			charDatas.Add (charData);
		}

		public bool AddWireData(WireData wireData){
			if(wireData == null || wireData.beginX > wireData.endX){
				return false;
			}

			CharacterInfo ci;
			ylyRichText.font.GetCharacterInfo('_', out ci, ylyRichText.fontSize, FontStyle.Normal);

			int ymin = ci.minY;
			int ymax = ci.maxY;

			//xy左下角归零，避免出现位置误差--begin
			ymax = ymax - ymin;
			ymin = 0;
			//xy左下角归零，避免出现位置误差--end

			ylyRichText.font.GetCharacterInfo('▇', out ci, ylyRichText.fontSize, FontStyle.Normal);

			//截取字符▇贴图中间木有抗锯齿和变淡效果的一小块作为下划线的贴图uv
			float charUVWQua = (ci.uvTopRight.x - ci.uvTopLeft.x) / 4;
			float charUVHQua = (ci.uvTopLeft.y - ci.uvBottomLeft.y) / 4;

			UIVertex uiVertex0 = new UIVertex ();
			uiVertex0.position = new Vector3 (wireData.beginX, ymax, z);
			uiVertex0.color = wireData.uColor;
			uiVertex0.uv0 = new Vector2(ci.uvTopLeft.x + charUVWQua, ci.uvTopLeft.y - charUVHQua);

			UIVertex uiVertex1 = new UIVertex ();
			uiVertex1.position = new Vector3 (wireData.endX, ymax, z);
			uiVertex1.color = wireData.uColor;
			uiVertex1.uv0 = new Vector2(ci.uvTopRight.x - charUVWQua, ci.uvTopRight.y - charUVHQua);

			UIVertex uiVertex2 = new UIVertex ();
			uiVertex2.position = new Vector3 (wireData.endX, ymin, z);
			uiVertex2.color = wireData.uColor;
			uiVertex2.uv0 = new Vector2(ci.uvBottomRight.x - charUVWQua, ci.uvBottomRight.y + charUVHQua);

			UIVertex uiVertex3 = new UIVertex ();
			uiVertex3.position = new Vector3 (wireData.beginX, ymin, z);
			uiVertex3.color = wireData.uColor;
			uiVertex3.uv0 = new Vector2(ci.uvBottomLeft.x + charUVWQua, ci.uvBottomLeft.y + charUVHQua);

			CharData charData = new CharData();
			if (wireData.wireType == WireType.UnderWire) {
				//下划线
				charData.cType = CharType.UnderWire;
			} else if(wireData.wireType == WireType.DeleteWire){
				//删除线
				charData.cType = CharType.DeleteWire;
			}
			charData.vertices [0] = uiVertex0;
			charData.vertices [1] = uiVertex1;
			charData.vertices [2] = uiVertex2;
			charData.vertices [3] = uiVertex3;
			charDatas.Add(charData);
			return true;
		}

		public void AddAssetData(AssetData assetData, float emoteX){
			if(assetData == null){
				return;
			}

			assetData.x = emoteX;

			if (assetData.height > height) {
				height = assetData.height;
			}
			assetDatas.Add (assetData);
		}

		public void AddLinkData(LinkData linkData, float endX){
			if(linkData == null){
				return;
			}

			linkData.rect.xMax = endX;
			linkDatas.Add(linkData);
		}

		public void OnFinish(){
			float rectWidth = ylyRichText.rectTransform.rect.width;
			float rectHeight = ylyRichText.rectTransform.rect.height;
			int count = charDatas.Count;
			float lineOffsetX = x;
			float lineOffsetY = 0f;
			float lineCenter = y - height / 2;
			float lineBottom = y - height;
			float halfCharMaxHeight = charMaxHeight / 2;
			float charMinYDis = Mathf.Abs(charMinY);
			for(int i = 0; i < count; i++){
				if (charDatas [i].cType == CharType.UnderWire) { //下划线
					lineOffsetY = lineBottom;
				} else if(charDatas [i].cType == CharType.DeleteWire){ //删除线
					lineOffsetY = lineBottom + halfCharMaxHeight;
				} else { //普通字符
					lineOffsetY = lineBottom + charMinYDis;
				}

				charDatas [i].vertices [0].position.x = lineOffsetX + charDatas [i].vertices [0].position.x;
				charDatas [i].vertices [1].position.x = lineOffsetX + charDatas [i].vertices [1].position.x;
				charDatas [i].vertices [2].position.x = lineOffsetX + charDatas [i].vertices [2].position.x;
				charDatas [i].vertices [3].position.x = lineOffsetX + charDatas [i].vertices [3].position.x;

				charDatas[i].vertices[0].position.y = lineOffsetY + charDatas[i].vertices[0].position.y;
				charDatas[i].vertices[1].position.y = lineOffsetY + charDatas[i].vertices[1].position.y;
				charDatas[i].vertices[2].position.y = lineOffsetY + charDatas[i].vertices[2].position.y;
				charDatas[i].vertices[3].position.y = lineOffsetY + charDatas[i].vertices[3].position.y;
			}

			lineOffsetY = lineBottom;
			count = assetDatas.Count;
			for (int i = 0; i < count; i++) {
				assetDatas [i].x = lineOffsetX + assetDatas [i].x + assetDatas [i].width / 2;
				assetDatas [i].y = lineOffsetY + assetDatas [i].height / 2;
			}

			lineOffsetY = lineBottom;
			count = linkDatas.Count;
			for (int i = 0; i < count; i++) {
				linkDatas[i].rect.xMin = lineOffsetX + linkDatas[i].rect.xMin;
				linkDatas[i].rect.xMax = lineOffsetX + linkDatas[i].rect.xMax;
				linkDatas[i].rect.yMin = lineOffsetY;
				linkDatas [i].rect.yMax = lineOffsetY + charMaxHeight;
			}
		}

		public void OnPopulateCharMesh(ref VertexHelper vh, ref int j){
			int count = charDatas.Count;
			bool needOutLineTmp = false; //是否需要描边
			//Vector3[] outLineOffsetPos = new Vector3[]{new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0), new Vector3(1, -1, 0), new Vector3(0, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0)};
			Vector3[] outLineOffsetPos = new Vector3[]{new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0), new Vector3(-1, 1, 0)};
			for(int i=0;i<count;i++){
				needOutLineTmp = false;
				if (charDatas [i].cType == CharType.Normal) { //下划线
					needOutLineTmp = true;
				}

				if(ylyRichText.isNeedOutLine && needOutLineTmp){
					for(int k=0;k<4;k++){
						vh.AddVert(charDatas[i].vertices[0].position + outLineOffsetPos[k], ylyRichText.outLineColor, charDatas[i].vertices[0].uv0);
						vh.AddVert(charDatas[i].vertices[1].position + outLineOffsetPos[k], ylyRichText.outLineColor, charDatas[i].vertices[1].uv0);
						vh.AddVert(charDatas[i].vertices[2].position + outLineOffsetPos[k], ylyRichText.outLineColor, charDatas[i].vertices[2].uv0);
						vh.AddVert(charDatas[i].vertices[3].position + outLineOffsetPos[k], ylyRichText.outLineColor, charDatas[i].vertices[3].uv0);
						vh.AddTriangle(4 * j + 0, 4 * j + 1, 4 * j + 2);
						vh.AddTriangle(4 * j + 0, 4 * j + 2, 4 * j + 3);
						j++;
					}
				}

				vh.AddVert(charDatas[i].vertices[0].position, charDatas[i].vertices[0].color, charDatas[i].vertices[0].uv0);
				vh.AddVert(charDatas[i].vertices[1].position, charDatas[i].vertices[1].color, charDatas[i].vertices[1].uv0);
				vh.AddVert(charDatas[i].vertices[2].position, charDatas[i].vertices[2].color, charDatas[i].vertices[2].uv0);
				vh.AddVert(charDatas[i].vertices[3].position, charDatas[i].vertices[3].color, charDatas[i].vertices[3].uv0);
				vh.AddTriangle(4 * j + 0, 4 * j + 1, 4 * j + 2);
				vh.AddTriangle(4 * j + 0, 4 * j + 2, 4 * j + 3);
				j++;
			}
		}
	}

	protected YlyRichText()
	{
		useLegacyMeshGeneration = false;
	}

	protected override void Start()
	{
		base.Start();

		if(m_Font == null){
			m_Font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		}

		//Debug.Log ("==========================rich text start");
		Font.textureRebuilt += FontTextureRebuilt;

		text = m_Text;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if(onLinkClick == null || eventData == null){
			return;
		}
		if(m_IsDrag){
			return;
		}

		GameObject uiCameraGo = GameObject.FindGameObjectWithTag("UICamera");
		if(uiCameraGo == null){
			return;
		}
		Camera uiCamera = uiCameraGo.GetComponent<Camera>();
		if(uiCamera == null){
			return;
		}
		int count;
		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, uiCamera, out localPos);
		for(int i = 0; i < m_Lines.Count; i++){
			count = m_Lines[i].linkDatas.Count;
			for (int j = 0; j < count; j++) {
				//Debug.Log ("==========================OnPointerClick rect x="+m_Lines[i].linkDatas[j].rect.x+" y="+m_Lines[i].linkDatas[j].rect.y+" w="+m_Lines[i].linkDatas[j].rect.size.x+" h="+m_Lines[i].linkDatas[j].rect.size.y);
				if(m_Lines[i].linkDatas[j].rect.Contains(localPos)){
					onLinkClick(m_Lines[i].linkDatas[j].arg);
					return;
				}
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		m_IsDrag = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		m_IsDrag = false;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();

		//Debug.Log ("==============================OnPopulateMesh");
		int j = 0;

		for (int i = 0; i < m_Lines.Count; i++) {
			m_Lines[i].OnPopulateCharMesh(ref vh, ref j);
		}

		m_IsPopulateMeshDone = true;
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		#if UNITY_EDITOR
		if(!Application.isPlaying){
			EditorApplication.update += Update;
		}
		#endif
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		#if UNITY_EDITOR
		if(!Application.isPlaying){
			EditorApplication.update -= Update;
		}
		#endif
	}

	protected override void OnRectTransformDimensionsChange()
	{
		UpdateLines();
		base.OnRectTransformDimensionsChange();
	}

	void UpdateLines()
	{
		m_Lines.Clear();
		m_PreferredWidth = 0;
		m_PreferredHeight = 0;

		if(m_Font == null){
			return;
		}

		float rectWidth = rectTransform.rect.width;
		float rectHeight = rectTransform.rect.height;

		float lineBeginX = -rectTransform.pivot.x * rectWidth;
		float lineBeginY = (1 - rectTransform.pivot.y) * rectHeight;

		float totalWidth = 0f;
		float totalHeight = 0f;
		float customWidthToNewLine = rectWidth;
		if(m_IsCustomWidthToNewLine){
			customWidthToNewLine = m_CustomWidthToNewLine;
		}
		CharacterInfo ci;

		int strLen = m_ParsedText.Length;

		m_Font.RequestCharactersInTexture("▇_", m_FontSize, FontStyle.Normal); //加上▇_字符，用于下划线

		float charX = 0f;
		float charW = 0f;
		int lastBlockType = -1;
		Color sclr = m_TColor, eclr = m_TColor;
		float advanceX = 0f;
		float offcharX = 0f;
		FontStyle fontStyle = FontStyle.Normal;
		int fontSize = m_FontSize;
		bool isSpaceOrTabChar = false;
		WireData underWireData = null;
		AssetData assetData = null;
		LinkData linkData = null;
		WireData deleteWireData = null;
		LineData linedata = null;

		if(strLen > 0){
			//放入第一行空数据;
			linedata = new LineData();
			linedata.ylyRichText = this;
			linedata.height = m_LineHeght;
		}
		for (int i = 0; i < strLen; i++)
		{
			int blockType = m_StrMask[i] / YlyRichTextParser.RICHTEXT_MULL_BASE * YlyRichTextParser.RICHTEXT_MULL_BASE;
			int blockLen = m_StrMask[i] - blockType;

			if (blockType == YlyRichTextParser.RICHTEXT_COLOR_BASE) {//字体颜色
				if (m_ParamDict.ContainsKey (i)) {
					string strParam = m_ParamDict [i];

					if (blockLen == YlyRichTextParser.FIXED_LEN_COLOR_BLOCK) {//文字变色:<color=00ff00ff>此后的文字全部为绿色，直到再次使用<color=ffff00ff>此后的文字变成黄色，以此类推;
						float r = Convert.ToInt16 (strParam.Substring (0, 2), 16) / 255.0f; //R
						float g = Convert.ToInt16 (strParam.Substring (2, 2), 16) / 255.0f; //G
						float b = Convert.ToInt16 (strParam.Substring (4, 2), 16) / 255.0f; //B
						float a = Convert.ToInt16 (strParam.Substring (6, 2), 16) / 255.0f; //A
						sclr = eclr = new Color (r, g, b, a);
					} else if (blockLen == YlyRichTextParser.FIXED_LEN_COLOR_MORPH_BLOCK) {//渐变，由颜色一变到颜色二;
						float r = Convert.ToInt16 (strParam.Substring (0, 2), 16) / 255.0f; //R
						float g = Convert.ToInt16 (strParam.Substring (2, 2), 16) / 255.0f; //G
						float b = Convert.ToInt16 (strParam.Substring (4, 2), 16) / 255.0f; //B
						float a = Convert.ToInt16 (strParam.Substring (6, 2), 16) / 255.0f; //A
						sclr = new Color (r, g, b, a);
						float r1 = Convert.ToInt16 (strParam.Substring (8, 2), 16) / 255.0f; //R
						float g1 = Convert.ToInt16 (strParam.Substring (10, 2), 16) / 255.0f; //G
						float b1 = Convert.ToInt16 (strParam.Substring (12, 2), 16) / 255.0f; //B
						float a1 = Convert.ToInt16 (strParam.Substring (14, 2), 16) / 255.0f; //A
						eclr = new Color (r1, g1, b1, a1);
					}

					if (underWireData != null) {
						underWireData.endX = charX - m_OffCharX;
						if (linedata.AddWireData (underWireData)) {
							underWireData = new WireData();
							underWireData.beginX = charX - m_OffCharX;
						} else {
							underWireData = new WireData();
							underWireData.beginX = charX;
						}
						underWireData.uColor = sclr;
					}

					i += blockLen - 1; //由于下次循环会++i，所以在这里需要先 - 1
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_COLOR_END) {//颜色结束块;
				sclr = eclr = m_TColor;
				if (underWireData != null) {
					underWireData.endX = charX - m_OffCharX;
					if (linedata.AddWireData (underWireData)) {
						underWireData = new WireData();
						underWireData.beginX = charX - m_OffCharX;
					} else {
						underWireData = new WireData();
						underWireData.beginX = charX;
					}
					underWireData.uColor = sclr;
				}
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_UNDER_LINE_BASE) {//下划线起始块;
				underWireData = new WireData();
				underWireData.beginX = charX;
				underWireData.uColor = sclr;
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_UNDER_LINE_END) {//下划线结束块;
				if (underWireData != null) {
					underWireData.endX = charX - m_OffCharX;
					linedata.AddWireData (underWireData);
					underWireData = null;
					i += blockLen - 1;
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_DELETE_LINE_BASE) {//删除线起始块;
				deleteWireData = new WireData();
				deleteWireData.wireType = WireType.DeleteWire;
				deleteWireData.beginX = charX;
				deleteWireData.uColor = m_TColor;
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_DELETE_LINE_END) {//删除线结束块;
				if (deleteWireData != null) {
					deleteWireData.endX = charX - m_OffCharX;
					linedata.AddWireData (deleteWireData);
					deleteWireData = null;
					i += blockLen - 1;
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_LINK_BASE) {//超链接起始块;
				if (m_ParamDict.ContainsKey (i)) {
					string strParam = m_ParamDict [i];

					//sclr = eclr = Color.blue; //指定为蓝色，且有下划线;
					//underWireData = new WireData();
					//underWireData.beginX = charX;
					//underWireData.uColor = sclr;

					linkData = new LinkData();
					linkData.arg = strParam;
					linkData.rect.xMin = charX;

					i += blockLen - 1;
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_LINK_END) {//超链接结束块;
				if(linkData != null){
					//if (underWireData != null) {
					//	underWireData.endX = charX - m_OffCharX;
					//	linedata.AddWireData (underWireData);
					//	underWireData = null;
					//}
					linedata.AddLinkData(linkData, charX - m_OffCharX);
					linkData = null;
					sclr = eclr = m_TColor;
					i += blockLen - 1;
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_BOLD_BASE) {//加粗起始块;
				if (fontStyle == FontStyle.Italic) {
					fontStyle = FontStyle.BoldAndItalic;
				} else {
					fontStyle = FontStyle.Bold;
				}
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_BOLD_END) {//加粗结束块;
				if (fontStyle == FontStyle.BoldAndItalic || fontStyle == FontStyle.Italic) {
					fontStyle = FontStyle.Italic;
				} else {
					fontStyle = FontStyle.Normal;
				}
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_ITALIC_BASE) {//斜体起始块;
				if (fontStyle == FontStyle.Bold) {
					fontStyle = FontStyle.BoldAndItalic;
				} else {
					fontStyle = FontStyle.Italic;
				}
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_ITALIC_END) {//斜体结束块;
				if (fontStyle == FontStyle.BoldAndItalic || fontStyle == FontStyle.Bold) {
					fontStyle = FontStyle.Bold;
				} else {
					fontStyle = FontStyle.Normal;
				}
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			} else if (blockType == YlyRichTextParser.RICHTEXT_SIZE_BASE) {//字体大小起始块;
				int fontSizeTmp = -1;
				if (m_ParamDict.ContainsKey (i) && int.TryParse (m_ParamDict [i], out fontSizeTmp)) {
					fontSize = fontSizeTmp;
					i += blockLen - 1;
					lastBlockType = blockType;
					continue;
				}
			} else if (blockType == YlyRichTextParser.RICHTEXT_SIZE_END) {//字体大小结束块;
				fontSize = m_FontSize;
				i += blockLen - 1;
				lastBlockType = blockType;
				continue;
			}

			m_Font.RequestCharactersInTexture(m_ParsedText[i].ToString(), fontSize, fontStyle);
			m_Font.GetCharacterInfo(m_ParsedText[i], out ci, fontSize, fontStyle);

			//由于表情、图片等资源和普通字符需要判断换行，特殊处理
			if (blockType == YlyRichTextParser.RICHTEXT_EMOTE_BASE) {//表情;
				int eidx = -1;
				bool isAsset = false;
				if (m_ParamDict.ContainsKey (i) && int.TryParse (m_ParamDict [i], out eidx)) {
					if (YlyRichTextParser.GetEmoteType (eidx) == YlyRichTextParser.RICHTEXT_ANIM_EMOTE_BASE) {
						//动态表情;
						string assetPath = string.Format (animEmotePrefabPathFormat, m_ParamDict [i]);
						if (m_AssetDataDict.ContainsKey (assetPath)) {
							assetData = new AssetData ();
							assetData.assetPath = assetPath;
							assetData.aType = YlyRichTextParser.RICHTEXT_ANIM_EMOTE_BASE;
							assetData.obj = m_AssetDataDict [assetPath].obj;
							assetData.width = m_AssetDataDict [assetPath].width;
							assetData.height = m_AssetDataDict [assetPath].height;
							charW = assetData.width;
							isAsset = true;
						}
					} else {
						//静态表情;
						string assetPath = string.Format (emotePngPathFormat, m_ParamDict [i]);
						if (m_AssetDataDict.ContainsKey (assetPath)) {
							assetData = new AssetData ();
							assetData.assetPath = assetPath;
							assetData.aType = YlyRichTextParser.RICHTEXT_EMOTE_BASE;
							assetData.obj = m_AssetDataDict [assetPath].obj;
							assetData.width = m_AssetDataDict [assetPath].width;
							assetData.height = m_AssetDataDict [assetPath].height;
							charW = assetData.width;
							isAsset = true;
						}
					}
				}
				if(!isAsset){
					charW = ci.advance;
				}
			} else if(blockType == YlyRichTextParser.RICHTEXT_RES_PATH_BASE){//图片
				if (m_ParamDict.ContainsKey (i) && m_AssetDataDict.ContainsKey (m_ParamDict [i])) {
					string assetPath = m_ParamDict [i];
					assetData = new AssetData ();
					assetData.assetPath = assetPath;
					assetData.aType = YlyRichTextParser.RICHTEXT_RES_PATH_BASE;
					assetData.obj = m_AssetDataDict [assetPath].obj;
					assetData.width = m_AssetDataDict [assetPath].width;
					assetData.height = m_AssetDataDict [assetPath].height;
					charW = assetData.width;
				} else {
					charW = ci.advance;
				}
			} else { //普通字符
				//空格和tab
				if (m_ParsedText [i] == ' ' || m_ParsedText [i] == '\t') {
					isSpaceOrTabChar = true;
				} else {
					isSpaceOrTabChar = false;
				}
				charW = ci.advance;
			}

			advanceX = charW + m_OffCharX;
			offcharX = m_OffCharX;
			totalWidth += advanceX;

			if ((m_HorizontalOverflow == HorizontalWrapMode.Wrap && (totalWidth - m_OffCharX) >= customWidthToNewLine) || m_ParsedText [i] == '\n') {//满行，或有换行符;
				linedata.width = Mathf.Max(totalWidth - advanceX - m_OffCharX, 0);

				if (underWireData != null) {
					Color32 uColor = underWireData.uColor;
					underWireData.endX = charX - m_OffCharX;
					linedata.AddWireData (underWireData);
					underWireData = new WireData();
					underWireData.beginX = 0f;
					underWireData.uColor = uColor;
				}
				if (deleteWireData != null) {
					Color32 uColor = deleteWireData.uColor;
					deleteWireData.endX = charX - m_OffCharX;
					linedata.AddWireData (deleteWireData);
					deleteWireData = new WireData();
					deleteWireData.wireType = WireType.DeleteWire;
					deleteWireData.beginX = 0f;
					deleteWireData.uColor = uColor;
				}

				if(linkData != null){
					string strParam = linkData.arg;
					linedata.AddLinkData(linkData, charX - m_OffCharX);
					linkData = new LinkData();
					linkData.arg = strParam;
					linkData.rect.xMin = 0f;
				}

				totalHeight = totalHeight + linedata.height;
				if (m_VerticalOverflow == VerticalWrapMode.Truncate && totalHeight > rectHeight) {
					break;
				}
				totalHeight = totalHeight + m_LineSpacing;

				if(linedata.width > m_PreferredWidth){
					m_PreferredWidth = linedata.width;
				}

				charX = 0f;
				if (m_ParsedText [i] == '\n') {
					//不满行，但有换行符;
					totalWidth = 0f;
					offcharX = 0f;
					charW = 0f;
				} else {
					//满行
					totalWidth = advanceX;
				}
				m_Lines.Add (linedata);
				if (i < strLen - 1) {
					linedata = new LineData ();
					linedata.ylyRichText = this;
					linedata.height = m_LineHeght;
				}
			}

			if (assetData != null) {
				linedata.AddAssetData(assetData, charX);
				assetData = null;
				i += blockLen - 1;
				lastBlockType = blockType;
			} else {
				if(!isSpaceOrTabChar){
					linedata.AddCharData(m_ParsedText[i], ci, charX + ci.minX, charX + ci.maxX, sclr, eclr);
				}
				lastBlockType = 0;
			}

			charX += charW + offcharX;
		}

		m_PreferredHeight = Mathf.Max(totalHeight - m_LineSpacing, 0);

		float lineX = 0f;
		float lineY = 0f;
		switch (m_Alignment) {
		case TextAnchor.UpperLeft:
		case TextAnchor.UpperCenter:
		case TextAnchor.UpperRight: 
			lineY = lineBeginY;
			break;
		case TextAnchor.MiddleLeft: 
		case TextAnchor.MiddleCenter: 
		case TextAnchor.MiddleRight: 
			lineY = Mathf.Min(lineBeginY, lineBeginY - (rectHeight - m_PreferredHeight) / 2);
			break;
		case TextAnchor.LowerLeft: 
		case TextAnchor.LowerCenter: 
		case TextAnchor.LowerRight: 
			lineY = Mathf.Min(lineBeginY, lineBeginY - (rectHeight - m_PreferredHeight));
			break;
		}
		for (int i = 0; i < m_Lines.Count; i++) {
			switch (m_Alignment) {
			case TextAnchor.UpperLeft: 
			case TextAnchor.MiddleLeft: 
			case TextAnchor.LowerLeft: 
				lineX = lineBeginX;
				break;
			case TextAnchor.UpperCenter: 
			case TextAnchor.MiddleCenter: 
			case TextAnchor.LowerCenter: 
				lineX = lineBeginX + (rectWidth - m_Lines [i].width) / 2;
				break;
			case TextAnchor.UpperRight: 
			case TextAnchor.MiddleRight: 
			case TextAnchor.LowerRight: 
				lineX = lineBeginX + (rectWidth - m_Lines [i].width);
				break;
			}
			m_Lines [i].x = lineX;
			m_Lines [i].y = lineY;
			m_Lines[i].OnFinish();
			lineY = lineY - m_Lines [i].height - m_LineSpacing;
		}

		SetVerticesDirty();
	}

	void FontTextureRebuilt(Font changedFont)
	{
		if(m_Font == null || m_Font != changedFont){
			return;
		}
		m_IsFontTextureDirty = true;
	}

	//预加载表情、图片等资源并缓存
	void PreLoadAsset()
	{
		m_AssetDataDict.Clear();
		float eWidth = 0f;
		float eHeight = 0f;

		//参考高度节点，宽度按等比例进行缩放
		RectTransform refHeightGoRT = (RectTransform)transform.Find (m_RefHeightGoName);

		List<string> assetKeyList = new List<string>(m_PreLoadAssetDic.Keys);
		for (int i = 0; i < assetKeyList.Count; i++)
		{
			AssetData ad = null;
			if (m_PreLoadAssetDic [assetKeyList [i]] == YlyRichTextParser.RICHTEXT_ANIM_EMOTE_BASE) {
				UnityEngine.GameObject assetObj = LoadAsset (assetKeyList [i], typeof(UnityEngine.GameObject)) as UnityEngine.GameObject;
				if(assetObj == null){
					continue;
				}
				eWidth = assetObj.GetComponent<RectTransform> ().sizeDelta.x;
				eHeight = assetObj.GetComponent<RectTransform> ().sizeDelta.y;
				ad = new AssetData ();
				ad.obj = assetObj;
			} else if(m_PreLoadAssetDic [assetKeyList [i]] == YlyRichTextParser.RICHTEXT_EMOTE_BASE || m_PreLoadAssetDic [assetKeyList [i]] == YlyRichTextParser.RICHTEXT_RES_PATH_BASE){
				Sprite assetObj = LoadAsset(assetKeyList [i], typeof(UnityEngine.Sprite)) as UnityEngine.Sprite;
				if(assetObj == null){
					continue;
				}
				eWidth = assetObj.rect.width;
				eHeight = assetObj.rect.height;
				ad = new AssetData ();
				ad.obj = assetObj;
			}
			if(ad != null && !m_AssetDataDict.ContainsKey(assetKeyList [i])){
				ad.assetPath = assetKeyList [i];
				ad.width = eWidth;
				ad.height = eHeight;
				if(refHeightGoRT != null){
					ad.height = refHeightGoRT.sizeDelta.y;
					ad.width = (ad.height / eHeight) * eWidth;
				}
				m_AssetDataDict.Add (assetKeyList [i], ad);
			}
		}
	}

	void ParseText()
	{
		if (string.IsNullOrEmpty(m_Text)) {
			m_ParsedText = "";
			return;
		}

		string tmpText = m_Text;
		if (tmpText.Length > m_MaxChars)
		{
			tmpText = tmpText.Substring (0, m_MaxChars);
		}

		m_ParsedText = txtParser.Parse(tmpText, out m_StrMask, out m_ParamDict, out m_PreLoadAssetDic, m_EnableRichText);
		//Debug.Log(txtParser.GetLog());
		//Debug.Log ("==============================m_ParsedText="+m_ParsedText);

		PreLoadAsset();
	}

	void ClearAssetGo()
	{
		YlyAssetIdentify[] oldAssetIdentifyCs = gameObject.GetComponentsInChildren<YlyAssetIdentify>();
		for(int i=0;i<oldAssetIdentifyCs.Length;i++){
			if(oldAssetIdentifyCs[i].gameObject != gameObject){
				#if UNITY_EDITOR
				if(Application.isPlaying){
					GameObject.Destroy(oldAssetIdentifyCs[i].gameObject);
				}else{
					GameObject.DestroyImmediate(oldAssetIdentifyCs[i].gameObject);
				}
				#else
				GameObject.Destroy(oldAssetIdentifyCs[i].gameObject);
				#endif
			}
		}
	}

	UnityEngine.Object LoadAsset(string assetPath, System.Type assetType)
	{
		UnityEngine.Object assetObj = null;
		#if UNITY_EDITOR
		assetObj = AssetDatabase.LoadAssetAtPath (assetPath, assetType);
		#else
		//一般项目都有自己的资源加载管理器，对于AssetBundle的加载请自行处理哈，这里就不作赘述了
		//(general project has its own resource loading manager, for AssetBundle loading please make your own processing, here is no explanation)
		#endif
		if(assetObj == null){
			Debug.Log ("===============YlyRichText.LoadAsset asset not exit!!! 资源不存在！！！：" + assetPath);
		}
		return assetObj;
	}

	// Update is called once per frame
	void Update()
	{
		if(m_IsPopulateMeshDone){ //刷新表情、图片等资源go
			m_IsPopulateMeshDone = false;

			ClearAssetGo();

			int lineEmoteCount = 0;
			GameObject go = null;
			for (int i = 0; i < m_Lines.Count; i++) {
				lineEmoteCount = m_Lines[i].assetDatas.Count;
				for(int j = 0; j < lineEmoteCount; j++){
					if (m_Lines[i].assetDatas[j].aType == YlyRichTextParser.RICHTEXT_ANIM_EMOTE_BASE) {
						//动态表情;
						go = GameObject.Instantiate(m_Lines[i].assetDatas[j].obj) as UnityEngine.GameObject;
					} else if(m_Lines[i].assetDatas[j].aType == YlyRichTextParser.RICHTEXT_EMOTE_BASE || m_Lines[i].assetDatas[j].aType == YlyRichTextParser.RICHTEXT_RES_PATH_BASE){
						//静态表情、图片;
						go = new GameObject();
						go.AddComponent<RectTransform>();
						Image img = go.AddComponent<Image>();
						img.sprite = m_Lines[i].assetDatas[j].obj as Sprite;
					}
					if(go != null){
						go.AddComponent<YlyAssetIdentify>();
						go.transform.SetParent(transform);
						RectTransform rtt = go.GetComponent<RectTransform>();

						rtt.pivot = new Vector2(0.5f, 0.5f);
						rtt.anchorMax = new Vector2(0, 1);
						rtt.anchorMin = new Vector2(0, 1);
						rtt.localScale = new Vector3(1, 1, 1);
						rtt.localPosition = new Vector3(m_Lines[i].assetDatas[j].x, m_Lines[i].assetDatas[j].y, m_Lines[i].z);
						rtt.sizeDelta = new Vector2(m_Lines[i].assetDatas[j].width, m_Lines[i].assetDatas[j].height);
					}
				}
			}
			if(m_IsAutoAdaptiveWidthHeight && (m_HorizontalOverflow == HorizontalWrapMode.Overflow || m_VerticalOverflow == VerticalWrapMode.Overflow)){
				if(m_HorizontalOverflow == HorizontalWrapMode.Overflow && m_VerticalOverflow == VerticalWrapMode.Overflow){
					rectTransform.sizeDelta = new Vector2(m_PreferredWidth, m_PreferredHeight);
				}
				if(m_HorizontalOverflow == HorizontalWrapMode.Overflow && m_VerticalOverflow == VerticalWrapMode.Truncate){
					rectTransform.sizeDelta = new Vector2(m_PreferredWidth, rectTransform.sizeDelta.y);
				}
				if(m_HorizontalOverflow == HorizontalWrapMode.Wrap && m_VerticalOverflow == VerticalWrapMode.Overflow){
					rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, m_PreferredHeight);
				}
			}
			if(m_IsFontTextureDirty){
				//解决动态字体破碎问题(solve dynamic font broken problem)
				m_IsFontTextureDirty = false;
				UpdateLines();
			}
		}
	}

	void OnDestroy()
	{
		Font.textureRebuilt -= FontTextureRebuilt;
		ClearAssetGo();
		m_Lines.Clear();
		m_AssetDataDict.Clear();

		onLinkClick = null;
	}
}