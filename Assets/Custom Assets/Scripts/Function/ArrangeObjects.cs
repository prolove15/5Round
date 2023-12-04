using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeObjects : MonoBehaviour
{

    public enum ArrangeRightDirection
    {
        LeftToRight, RightToLeft
    }

    public enum ArrangeUpDirection
    {
        UpToDown, DownToUp
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------------------------------ 
    public static List<Vector3> GetLocalArrangePoints(
        int pointsCount, int pointsCountPerRow, float rowInterval, float columnInterval,
        ArrangeRightDirection rightDir = ArrangeRightDirection.LeftToRight,
        ArrangeUpDirection upDir = ArrangeUpDirection.UpToDown, float upOffset = 0.01f)
    {
        List<Vector3> arrangePoints = new List<Vector3>();

        int rowCount = Mathf.CeilToInt((float)pointsCount / (float)pointsCountPerRow);

        for(int i = 0; i < rowCount; i++)
        {
            //
            int columnCount = pointsCountPerRow;
            if(i == rowCount - 1)
            {
                columnCount = (pointsCount % pointsCountPerRow) == 0 ? pointsCountPerRow
                    : (pointsCount % pointsCountPerRow);
            }

            //
            float zPos = ((rowCount - 1) / 2f - i) * rowInterval;

            for(int j = 0; j < columnCount; j++)
            {
                float xPos = (j - (columnCount - 1) / 2f) * columnInterval;

                arrangePoints.Add(new Vector3(xPos, 0f, zPos));
            }
        }

        if (rightDir == ArrangeRightDirection.RightToLeft)
        {
            for(int i = 0; i < arrangePoints.Count; i++)
            {
                arrangePoints[i] = new Vector3(-arrangePoints[i].x, arrangePoints[i].y, arrangePoints[i].z);
            }
        }

        if (upDir == ArrangeUpDirection.DownToUp)
        {
            for(int i = 0; i < arrangePoints.Count; i++)
            {
                arrangePoints[i] = new Vector3(arrangePoints[i].x, arrangePoints[i].y, -arrangePoints[i].z);
            }
        }

        return arrangePoints;
    }

    //------------------------------
    public static List<Vector3> GetArrangePoints(Vector3 centerPos,
        int pointsCount, int pointsCountPerRow, float rowInterval, float columnInterval,
        ArrangeRightDirection rightDir = ArrangeRightDirection.LeftToRight,
        ArrangeUpDirection upDir = ArrangeUpDirection.UpToDown)
    {
        List<Vector3> result = ArrangeObjects.GetLocalArrangePoints(pointsCount, pointsCountPerRow,
            rowInterval, columnInterval, rightDir, upDir);

        for (int i = 0; i < result.Count; i++)
        {
            result[i] += centerPos;
        }

        return result;
    }
}
