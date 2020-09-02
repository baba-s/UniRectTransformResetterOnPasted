using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	/// <summary>
	/// UI オブジェクトの貼り付け時に RectTransform の Pos Z と Scale の値をリセットするエディタ拡張
	/// </summary>
	[InitializeOnLoad]
	internal static class RectTransformResetterOnPasted
	{
		//================================================================================
		// 変数(static)
		//================================================================================
		private static bool m_isPasting; // 貼り付け中の場合 true

		//================================================================================
		// 関数(static)
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		static RectTransformResetterOnPasted()
		{
			// 貼り付けコマンドを検知するためにコールバックを登録
			// hierarchyChanged だと Event.current を検知できない
			EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
			//EditorApplication.hierarchyChanged += OnChanged;
		}

		/// <summary>
		/// Hierarchy の項目を描画する時に呼び出されます
		/// </summary>
		private static void HierarchyWindowItemOnGui( int instanceID, Rect selectionRect )
		{
			// 貼り付けコマンド以外のイベントは無視
			var current     = Event.current;
			var commandName = current.commandName;

			if ( string.IsNullOrWhiteSpace( commandName ) ) return;
			if ( commandName != "Paste" ) return;

			var type = current.type;

			if ( type != EventType.ExecuteCommand && type != EventType.ValidateCommand ) return;

			// commandName が Paste のイベントは1フレームの間に何回も来るため
			// すでに独自の貼り付け処理を実行している場合はここから先には進めない
			if ( m_isPasting ) return;
			m_isPasting = true;

			EditorApplication.delayCall += ResetRectTransform;
		}

		/// <summary>
		/// RectTransform の Pos Z と Scale の値をリセットします
		/// </summary>
		private static void ResetRectTransform()
		{
			var rectTransforms = Selection.transforms
					.OfType<RectTransform>()
					.ToArray()
				;

			foreach ( var rectTransform in rectTransforms )
			{
				var canvas = rectTransform.GetComponentInParent<Canvas>();

				if ( canvas == null ) continue;

				var rootCanvas = canvas.rootCanvas;

				if ( rootCanvas == null ) continue;

				var anchoredPosition3D      = rectTransform.anchoredPosition3D;
				var rootCanvasRectTransform = rootCanvas.GetComponent<RectTransform>();
				var tolerance               = 0.00001f;

				if ( tolerance < Math.Abs( anchoredPosition3D.z - rootCanvasRectTransform.anchoredPosition3D.z ) ) continue;

				if ( rectTransform.localScale != rootCanvasRectTransform.localScale ) continue;

				anchoredPosition3D.z             = 0;
				rectTransform.anchoredPosition3D = anchoredPosition3D;
				rectTransform.localScale         = Vector3.one;
			}

			m_isPasting = false;
		}
	}
}