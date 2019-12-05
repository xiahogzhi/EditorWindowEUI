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


        protected override void OnInitialize()
        {
        }

        protected override void OnLayoutBuild()
        {
            {
                Button btn = EuiCore.CreateElement<Button>();
                btn.Text = "测试按钮";
                btn.Size = new Vector2(60, 20);
                btn.OnClickEvt += () => { Debug.Log("点击"); };

                Label lab = EuiCore.CreateElement<Label>();
                lab.Text = " 测试";
                lab.AnchordPosition = new Vector2(100, 100);
                lab.Size = new Vector2(60, 20);


                Toggle toggle = EuiCore.CreateElement<Toggle>();
                toggle.Text = "测试";
                toggle.AnchordPosition = new Vector2(0, 100);
                toggle.Size = new Vector2(60, 20);


                InputField ifd = EuiCore.CreateElement<InputField>();
                ifd.Text = "测试";
//                ifd.SetAnchor(AnchorType.LeftTop);
                ifd.AnchordPosition = new Vector2(50, 0);
//                ifd.Pivot = new Vector2(0,0);
                ifd.Size = new Vector2(100, 20);


                Image im = EuiCore.CreateElement<Image>();
                im.MainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/demo.png");
                //设置裁剪
//                im.SetActiveClip(true);

                //设置父对象
                ifd.SetParent(im);


                //性能测试
//                for (int i = 0; i < 500; i++)
//                {
//                    ifd = EuiCore.CreateElement<InputField>();
//                    ifd.Text = "测试";
//                    ifd.SetAnchor(AnchorType.LeftTop);
//                    ifd.AnchordPosition = new Vector2((i % 10 * 100) + 50, (i / 10 * 20) + 10);
////                ifd.Pivot = new Vector2(0,0);
//                    ifd.Size = new Vector2(100, 20);
//                }
            }
        }
    }
}