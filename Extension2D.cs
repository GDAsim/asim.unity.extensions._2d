using UnityEngine;

namespace asim.unity.extensions._2d
{
    /*
     * Enums
     */
    public enum OriginAnchor
    {
        TOPLEFT, TOP, TOPRIGHT,
        LEFT, MIDDLE, RIGHT,
        BOTTOMLEFT, BOTTOM, BOTTOMRIGHT
    }


    public static class Extension2D
    {
        //Positioning
        static Vector2 OriginPos = new Vector2(0, 0);
        static Vector2 OriginScale = new Vector2(1, 1);
        static float Rotation_RAD = 0;
        public static void TranslateOrigin(Vector2 translateAmt)
        {
            Vector2 newOrigin = new Vector2(OriginPos.x + translateAmt.x, OriginPos.y + translateAmt.y);
            //Vector2 rotatedvector = rotateWithRespectToOrigin(new Vector2(neworigin.x, neworigin.y));
            OriginPos.x = newOrigin.x;
            OriginPos.y = newOrigin.y;
        }
        public static void SetOriginScale(Vector2 newScale)
        {
            OriginScale = newScale;
        }
        public static void Rotate(float radians)
        {
            Rotation_RAD += radians;
        }
        static Vector2 RotatePointOnOrigin(Vector2 pointToRotate)
        {
            //1. Move Entire coordinates by -OriginPos to make the origin (0,0)
            pointToRotate -= OriginPos;
            //2. Apply the Standard 2D Rotation Matrix to obtain the rotated point 
            pointToRotate = new Vector2(Mathf.Cos(Rotation_RAD) * pointToRotate.x - Mathf.Sin(Rotation_RAD) * pointToRotate.y,
                Mathf.Sin(Rotation_RAD) * pointToRotate.x + Mathf.Cos(Rotation_RAD) * pointToRotate.y);
            //3. Add back OriginPos
            Vector2 rotatedPoint = pointToRotate + OriginPos;

            return rotatedPoint;
        }


        //Drawings
        static Texture2D DefaultTexture = new Texture2D(4, 4);
        public static void DrawDot(Vector2 position, Vector2 size, Color32 dotColor, Color32 borderColor, float thickness = 0)
        {
            position *= OriginScale;
            position += OriginPos;
            position = RotatePointOnOrigin(position);

            Rect rect = new Rect(position, size);
            GUI.DrawTexture(rect, DefaultTexture, ScaleMode.ScaleToFit, false, 0, dotColor, 0, size.x);
            if (thickness > 0) GUI.DrawTexture(rect, DefaultTexture, ScaleMode.ScaleToFit, false, 0, borderColor, thickness, size.x);
        }
        public static void DrawRect(Vector2 position, Vector2 size, Color32 rectColor, Color32 borderColor, bool IsCenterOrigin = false, float thickness = 0)
        {
            position *= OriginScale;
            position += OriginPos;
            position = RotatePointOnOrigin(position);

            Rect rect;
            if (IsCenterOrigin) rect = new Rect(position - size / 2f, size);
            else rect = new Rect(position, size);

            GUI.DrawTexture(rect, DefaultTexture, ScaleMode.StretchToFill, false, 0, rectColor, 0, 0);
            if (thickness > 0) GUI.DrawTexture(rect, DefaultTexture, ScaleMode.StretchToFill, false, 0, borderColor, thickness, 0);
        }

















        static bool HasBGDrawned = false;
        public static void SetBGColor(Color32 bgColor)
        {
            if (!HasBGDrawned)
            {
                GL.Clear(false, true, bgColor, 0);
                HasBGDrawned = true;
            }
        }
    }

}
