using EditorWindowEUI.UI;
using UnityEditor;
using UnityEngine;

namespace EditorWindowEUI
{
    public class TestWindow : BaseEditorWindowGUI
    {
        [MenuItem("Tools/测试窗口")]
        public static void Open()
        {
            GetWindow<TestWindow>().Show();
        }

        public static GUISkin TestSkin => AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Skin.guiskin");


        protected override void OnInitialize()
        {
        }


        protected override void OnLayoutBuild()
        {
            {
//                int index = 0;
//                foreach (GUIStyle VARIABLE in GUI.skin)
//                {
//                    Button b = EuiCore.CreateElement<Button>();
//                    b.Style = VARIABLE;
//                    b.SetAnchor(AnchorType.LeftTop);
//                    b.Pivot = new Vector2(0, 0);
//                    b.Size = new Vector2(30, 30);
//                    b.AnchoredPosition = new Vector2(index % 30 * 35, index / 30 * 35);
//                    b.OnClickEvt += () =>
//                    {
//                        Debug.Log(VARIABLE);
//                        Selection.activeObject = GUI.skin;
//                    };
//
//                    index++;
//                }
                TimeAxis ta = EuiCore.CreateElement<TimeAxis>();

                //return;
                VerticalScrollRect verticalScrollRect = EuiCore.CreateElement<VerticalScrollRect>();
                verticalScrollRect.AnchoredPosition = new Vector2(0, 0);
                verticalScrollRect.Size = new Vector2(400, 500);


                StackLayout sl = EuiCore.CreateElement<StackLayout>();
                verticalScrollRect.SetContent(sl);

                ta.SetParent(sl);

                Button btn = EuiCore.CreateElement<Button>();
                btn.Text = "测试按钮";
                btn.Size = new Vector2(60, 20);
                btn.OnClickEvt += () => { Debug.Log("点击"); };
                btn.SetParent(sl);

                btn = EuiCore.CreateElement<Button>();
                btn.Text = "测试按钮";
                btn.Size = new Vector2(60, 180);
                btn.OnClickEvt += () => { Debug.Log("点击"); };
                btn.SetParent(sl);


                Label lab = EuiCore.CreateElement<Label>();
                lab.Text = " 测试";
                lab.AnchoredPosition = new Vector2(100, 100);
                lab.Size = new Vector2(60, 20);
                lab.SetParent(sl);


                Toggle toggle = EuiCore.CreateElement<Toggle>();
                toggle.Text = "测试";
                toggle.AnchoredPosition = new Vector2(0, 100);
                toggle.Size = new Vector2(60, 20);
                toggle.SetParent(sl);
                InputField ifd = EuiCore.CreateElement<InputField>();
                ifd.SetParent(sl);


                ifd.Text = "测试";
//                ifd.SetAnchor(AnchorType.LeftTop);
                ifd.AnchoredPosition = new Vector2(0, 0);
//                ifd.Pivot = new Vector2(0,0);
                ifd.Size = new Vector2(100, 20);


                Image im = EuiCore.CreateElement<Image>();
                im.MainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/demo.aseprite");
//                im.AnchordPosition = new Vector2(-200, 0);
                //设置裁剪
                im.SetActiveClip(true);

                //设置父对象
                ifd.SetParent(im);

                VerticalScrollbar sr = EuiCore.CreateElement<VerticalScrollbar>();
                sr.SetAnchor(AnchorType.RightStretch);
                sr.Pivot = new Vector2(1, 0.5f);
                sr.OnValueChanged += (x) => { Debug.Log(x); };
                sr.SetSliderHeightRelative(0.5f);
                //sr.SetParent(im);
                sr.SetValue(1);
            }
        }
    }
}