using System.Linq;
using UnityEngine;

namespace Case.Shared.GridLayout
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class GridLayout : MonoBehaviour
    {
        public Vector2Int count = new Vector2Int { x = 1, y = 1};
        public float maxScale = 1;

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;
            ForceLayout();
        }
#endif

        public void ForceLayout()
        {
            var childs = transform.GetComponentsInChildren<Transform>().ToList();
            childs.RemoveAt(0);

            float padding = 2f / (count.x - 1);

            var scaleByCount = Mathf.Min(maxScale / count.x, maxScale / count.y);

            int totalCount = count.x * count.y;
            for (int i = 0; i < totalCount; ++i) 
            {
                if (childs.Count <= i) break;
                var child = childs[i];

                int column = i / count.x;
                int row = i % count.x;

                child.localScale = new Vector3(scaleByCount, scaleByCount, 1);
                child.localPosition = new Vector3(padding * row, -padding * column, 0);
            }
        }

        public void ForceLayoutColumn(int sColumn)
        {
            var childs = transform.GetComponentsInChildren<Transform>().ToList();
            childs.RemoveAt(0);

            float padding = 2f / (count.x - 1);

            var scaleByCount = Mathf.Min(maxScale / count.x, maxScale / count.y);

            int totalCount = count.x * count.y;
            for (int i = 0; i < totalCount; ++i)
            {
                if (childs.Count <= i) break;
                var child = childs[i];

                int column = i / count.x;
                int row = i % count.x;

                if (sColumn == row) 
                {
                    child.localScale = new Vector3(scaleByCount, scaleByCount, 1);
                    child.localPosition = new Vector3(padding * row, -padding * column, 0);                
                }
            }
        }
    }
}
