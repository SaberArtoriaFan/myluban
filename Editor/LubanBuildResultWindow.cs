using UnityEngine;
using UnityEditor;

namespace Luban
{
    public class LubanBuildResultWindow : EditorWindow
    {
        OutPutRecord outPutRecord;
        public LubanBuildResultWindow()
        {
            position = new Rect(this.position.xMin, position.yMin,1200, 500);

        }

        public void Init(OutPutRecord outPutRecord)
        {
            this.outPutRecord =outPutRecord;
        }
        private void OnGUI()
        {
            var heigh = 20;
            if (outPutRecord == null) return;
            GUILayout.BeginVertical();

            //»æÖÆ±êÌâ
            GUILayout.Space(10);
            GUI.skin.label.fontSize = 24;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Luban Build Result");
            GUI.skin.label.fontSize = 14;
            GUILayout.Space(10);
            heigh += 24;
            //GUILayout.Label(System.DateTime.Now.);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            foreach (var outPutRecord in outPutRecord.list)
            {
                if (outPutRecord.Item1)
                    GUI.color = Color.red;
                else
                    GUI.color = Color.white;
                GUILayout.Label($" {outPutRecord.Item2}");
                GUILayout.Space(5);
                heigh += 10;
            }

            if (heigh > position.height)
            {
                //Debug.Log(heigh+""+position.height);
                position = new Rect(this.position.xMin, position.yMin, position.width, heigh);

            }
            GUI.color = Color.white;



            GUILayout.EndVertical();

        }
    }
}