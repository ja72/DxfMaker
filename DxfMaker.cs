﻿using System;
using static System.Math;
using Microsoft.VisualBasic;

namespace ACA.Core.Planar
{
    /// <summary>
    /// Class DxfMaker to create AutoCAD DXF files from C#.
    /// </summary>
    /// <remarks>Orignally writen for VS2003 in VB.NET</remarks>
    public class DxfMaker
    {
        // ================================================================
        // Save countless hours of CAD design time by automating the design
        // process.  Output DXF files on the fly by using this source code.
        // Create a simple GUI to gather data to generate the geometry,
        // then export it instantly to a ".dxf" file which is a format
        // supported by most CAD software and even some design software.

        // This DXF generating source code was created by David S. Tufts,
        // you can contact me at kdtufts@juno.com.  The variables set
        // up in the DXF header are my personal preferences. The
        // variables can be changed by observing the settings of any
        // DXF file which was saved with your desired preferences.  Also,
        // any additional geometry can be added to this code in the form of
        // a function by observing the DXF output of any CAD software that
        // supports DXF files.  Good luck and have fun...
        // ================================================================

        private string DXF_BodyText;
        private string DXF_BlockBody;
        private int BlockIndex;
        public float DimScale = 1.0f;

        public string Layer;

        const double rad = Math.PI/180;

        public DxfMaker()
        {
            BlockIndex=0;
            DXF_BodyText="";
            DXF_BlockBody="";
            Layer="0";
        }

        // Build the header, this header is set with my personal preferences
        private string DXF_Header()
        {
            string DXF_HeaderRet = default;
            var HS = new string[20];
            HS[0]="  0|SECTION|  2|HEADER|  9";
            HS[1]="$ACADVER|  1|AC1009|  9";
            HS[2]="$INSBASE| 10|0.0| 20|0.0| 30|0.0|  9";
            HS[3]="$EXTMIN| 10|  0| 20|  0| 30|  0|  9";
            HS[4]="$EXTMAX| 10|368| 20|326| 30|0.0|  9";
            HS[5]="$LIMMIN| 10|0.0| 20|0.0|  9";
            HS[6]="$LIMMAX| 10|100.0| 20|100.0|  9";
            HS[7]="$ORTHOMODE| 70|     1|  9";
            HS[8]="$DIMSCALE| 40|"+DimScale+"|  9";
            HS[9]="$DIMSTYLE|  2|STANDARD|  9";
            HS[10]="$FILLETRAD| 40|0.0|  9";
            HS[11]="$PSLTSCALE| 70|     1|  0";
            HS[12]="ENDSEC|  0";
            HS[13]="SECTION|  2|TABLES|  0";
            HS[14]="TABLE|  2|VPORT| 70|     2|  0|VPORT|  2|*ACTIVE| 70|     0| 10|0.0| 20|0.0| 11|1.0| 21|1.0| 12|50.0| 22|50.0| 13|0.0| 23|0.0| 14|1.0| 24|1.0| 15|0.0| 25|0.0| 16|0.0| 26|0.0| 36|1.0| 17|0.0| 27|0.0| 37|0.0| 40|100.0| 41|1.55| 42|50.0| 43|0.0| 44|0.0| 50|0.0| 51|0.0| 71|     0| 72|   100| 73|     1| 74|     1| 75|     0| 76|     0| 77|     0| 78|     0|  0|ENDTAB|  0";
            HS[15]="TABLE|  2|LTYPE| 70|     1|  0|LTYPE|  2|CONTINUOUS|  70|     0|  3|Solid Line| 72|    65| 73|     0| 40|0.0|  0|ENDTAB|  0";
            HS[16]="TABLE|  2|LAYER| 70|     3|  0|LAYER|  2|0| 70|     0| 62|     7|  6|CONTINUOUS|  0|LAYER|  2|"+Layer+"| 70|     0| 62|     7|  6|CONTINUOUS|  0|LAYER|  2|DEFPOINTS| 70|     0| 62|     7| 6|CONTINUOUS|  0|ENDTAB|  0";
            HS[17]="TABLE|  2|VIEW| 70|     0|  0|ENDTAB|  0";
            HS[18]="TABLE|  2|DIMSTYLE| 70|     1|  0|DIMSTYLE|  2|STANDARD| 70|     0|  3||  4||  5||  6||  7|| 40|1.0| 41|0.125| 42|0.04| 43|0.25| 44|0.125| 45|0.0| 46|0.0| 47|0.0| 48|0.0|140|0.125|141|0.06|142|0.0|143|25.4|144|1.0|145|0.0|146|1.0|147|0.09| 71|     0| 72|     0| 73|     1| 74|     1| 75|     0| 76|     0| 77|     0| 78|     0|170|     0|171|     2|172|     0|173|     0|174|     0|175|     0|176|     0|177|     0|178|     0|  0|ENDTAB|  0";
            HS[19]="ENDSEC|  0|";
            DXF_HeaderRet=string.Join("|", HS);
            return DXF_HeaderRet;
        }


        // The block header, body, and footer are used to append the
        // header with any dimensional information added in the body.
        private string DXF_BlockHeader()
        {
            string DXF_BlockHeaderRet = default;
            DXF_BlockHeaderRet="SECTION|  2|BLOCKS|  0|";
            return DXF_BlockHeaderRet;
        }

        private void DXF_BuildBlockBody()
        {
            DXF_BlockBody=DXF_BlockBody+"BLOCK|  8|0|  2|*D"+BlockIndex+"|70|     1| 10|0.0| 20|0.0| 30|0.0|  3|*D"+BlockIndex+"|  1||0|ENDBLK|  8|0|0|";
            BlockIndex=BlockIndex+1;
        }

        private string DXF_BlockFooter()
        {
            string DXF_BlockFooterRet = default;
            DXF_BlockFooterRet="ENDSEC|  0|";
            return DXF_BlockFooterRet;
        }

        // The body header, and footer will always remain the same
        private string DXF_BodyHeader()
        {
            string DXF_BodyHeaderRet = default;
            DXF_BodyHeaderRet="SECTION|  2|ENTITIES|  0|";
            return DXF_BodyHeaderRet;
        }

        private string DXF_BodyFooter()
        {
            string DXF_BodyFooterRet = default;
            DXF_BodyFooterRet="ENDSEC|  0|";
            return DXF_BodyFooterRet;
        }

        private string DXF_Footer()
        {
            string DXF_FooterRet = default;
            DXF_FooterRet="EOF";
            return DXF_FooterRet;
        }

        public void DXF_Save(string fpath)
        {
            string strDXF_Output;
            string[] varDXF;
            try
            {
                // Build a full text string
                strDXF_Output=DXF_Header()+DXF_BlockHeader()+DXF_BlockBody+DXF_BlockFooter()+DXF_BodyHeader()+DXF_BodyText+DXF_BodyFooter()+DXF_Footer();
                // split the text string at "|" and output to specified file
                varDXF=strDXF_Output.Split('|');
                var fio = new System.IO.StreamWriter(fpath);
                for (int i = 0, loopTo = varDXF.Length-1; i<=loopTo; i++)
                    fio.WriteLine(varDXF[i]);
                fio.Close();
                // Reminder of where the file was saved to
                Interaction.MsgBox("DXF file was saved to:"+Constants.vbCrLf+fpath);
            }
            catch (System.IO.IOException ex)
            {
                Interaction.MsgBox($"Error during DXF Save ({ex.Message})");
            }

        }

        // ====================================================
        // All geometry is appended to the text: "DXF_BodyText"
        // ====================================================

        public void DXF_Line(float x1, float y1, float Z1, float x2, float y2, float Z2)
        {
            DXF_BodyText+="LINE|8|"+Layer+"| 10|"+x1+"| 20|"+y1+"| 30|"+Z1+"| 11|"+x2+"| 21|"+y2+"| 31|"+Z2+"|0|";

        }
        public void DXF_Circle(float x, float y, float Z, float Radius)
        {
            DXF_BodyText+="CIRCLE|8|"+Layer+"| 10|"+x+"| 20|"+y+"| 30|"+Z+"| 40|"+Radius+"| 39|  0|0|";

        }
        public void DXF_Arc(float x, float y, float Z, float Radius, float StartAngle, float EndAngle)
        {
            // "|62|1|" after Layer sets the to color (Red)
            DXF_BodyText+="ARC|8|"+Layer+"| 10|"+x+"| 20|"+y+"| 30|"+Z+"| 40|"+Radius+"| 50|"+StartAngle+"| 51|"+EndAngle+"|0|";

        }
        public void DXF_Text(float x, float y, float Z, float Height, string iText)
        {
            DXF_BodyText+="TEXT|8|"+Layer+"| 10|"+x+"| 20|"+y+"| 30|"+Z+"| 40|"+DimScale*Height+"|1|"+iText+"| 50|  0|0|";

        }
        public void DXF_Dimension(float x1, float y1, float x2, float y2, float PX1, float PY1, float PX2, float PY2, float iAng = 0f, string iText = "None")
        {
            // I know for sure that this works in AutoCAD.
            var strDim = new string[7];
            strDim[0]=$"DIMENSION|  8|{Layer}|  6|CONTINUOUS|  2|*D{BlockIndex}";
            strDim[1]=$" 10|{PX1}| 20|{PY1}| 30|0.0";
            strDim[2]=$" 11|{PX2}| 21|{PY2}| 31|0.0";
            strDim[3]=Interaction.IIf(iText=="None", " 70|     0", " 70|     0|  1|"+iText).ToString();
            strDim[4]=$" 13|{x1}| 23|{y1}| 33|0.0";
            strDim[5]=$" 14|{x2}| 24|{y2}| 34|0.0{Interaction.IIf(iAng==0f, "", "| 50|"+iAng)}";
            strDim[6]="1001|ACAD|1000|DSTYLE|1002|{|1070|   287|1070|     3|1070|    40|1040|"+DimScale+"|1070|   271|1070|     3|1070|   272|1070|     3|1070|   279|1070|     0|1002|}|  0|";
            DXF_BodyText=DXF_BodyText+string.Join("|", strDim);
            // All dimensions need to be referenced in the header information
            DXF_BuildBlockBody();
        }
        public void DXF_Rectangle(float x1, float y1, float Z1, float x2, float y2, float Z2)
        {
            var strRectangle = new string[6];
            strRectangle[0]="POLYLINE|  5|48|  8|"+Layer+"|66|1| 10|0| 20|0| 30|0| 70|1|0";
            strRectangle[1]="VERTEX|5|50|8|"+Layer+"| 10|"+x1+"| 20|"+y1+"| 30|"+Z1+"|  0";
            strRectangle[2]="VERTEX|5|51|8|"+Layer+"| 10|"+x2+"| 20|"+y1+"| 30|"+Z2+"|  0";
            strRectangle[3]="VERTEX|5|52|8|"+Layer+"| 10|"+x2+"| 20|"+y2+"| 30|"+Z2+"|  0";
            strRectangle[4]="VERTEX|5|53|8|"+Layer+"| 10|"+x1+"| 20|"+y2+"| 30|"+Z1+"|  0";
            strRectangle[5]="SEQEND|     8|"+Layer+"|0|";
            DXF_BodyText+=string.Join("|", strRectangle);
        }
        private void DXF_Border(float x1, float y1, float Z1, float x2, float y2, float Z2)
        {
            var strBorder = new string[6];
            strBorder[0]="POLYLINE|  8|"+Layer+"| 40|1| 41|1| 66|1| 70|1|0";
            strBorder[1]="VERTEX|  8|"+Layer+"| 10|"+x1+"| 20|"+y1+"| 30|"+Z1+"|  0";
            strBorder[2]="VERTEX|  8|"+Layer+"| 10|"+x2+"| 20|"+y1+"| 30|"+Z2+"|  0";
            strBorder[3]="VERTEX|  8|"+Layer+"| 10|"+x2+"| 20|"+y2+"| 30|"+Z2+"|  0";
            strBorder[4]="VERTEX|  8|"+Layer+"| 10|"+x1+"| 20|"+y2+"| 30|"+Z1+"|  0";
            strBorder[5]="SEQEND|  8|"+Layer+"|0|";
            DXF_BodyText+=string.Join("|", strBorder);
        }
        public void DXF_ShowText(float x, float y, float eAng, float eRad, object eText)
        {
            float eX;
            float eY;
            float iRadians;
            iRadians=(float)(eAng*rad);
            // Find the angle at which to draw the arrow head and leader
            eX=(float)((double)x-(double)eRad*Cos((double)iRadians));
            eY=(float)((double)y-(double)eRad*Sin((double)iRadians));
            // Draw an arrow head
            DXF_ArrowHead((float)((double)iRadians+(180d*rad)), x, y);
            // Draw the leader lines
            DXF_Line(x, y, 0f, eX, eY, 0f);
            DXF_Line(eX, eY, 0f, eX+2f, eY, 0f);
            // Place the text
            DXF_Text((float)((double)eX+2.5d), (float)((double)eY-0.75d), 0f, 1.5f, eText.ToString());
        }
        public void DXF_ArrowHead(float iRadians, float sngX, float sngY)
        {
            var x = new float[2];
            var y = new float[2];
            // The number "3" is the length of the arrow head.
            // Adding or subtracting 170 degrees from the angle
            // gives us a 10 degree angle on the arrow head.
            // Finds the first side of the arrow head
            x[0]=(float)((double)sngX+3d*Math.Sin((double)iRadians+(170d*rad)));
            y[0]=(float)((double)sngY+3d*Math.Cos((double)iRadians+(170d*rad)));
            // Finds the second side of the arrow head
            x[1]=(float)((double)sngX+3d*Math.Sin((double)iRadians-(170d*rad)));
            y[1]=(float)((double)sngY+3d*Math.Cos((double)iRadians-(170d*rad)));
            // Draw the first side of the arrow head
            DXF_Line(sngX, sngY, 0f, x[0], y[0], 0f); // /
            // Draw the second side of the arrow head
            DXF_Line(sngX, sngY, 0f, x[1], y[1], 0f); // \
            // Draw the bottom side of the arrow head
            DXF_Line(x[0], y[0], 0f, x[1], y[1], 0f); // _
        }

        public void DXF_Polyline(double[] p)
        {
            int N = (int)Round(p.Length/2d);
            string[] nodes = new string[N+1+1];
            nodes[0]="POLYLINE| 8|"+Layer+"| 66| 1|0";
            int i;
            var loopTo = N-1;
            for (i=0; i<=loopTo; i++)
                nodes[i+1]="VERTEX| 8|"+Layer+"| 10| "+p[2*i]+"| 20| "+p[2*i+1]+"|0";
            nodes[N+1]="SEQEND| 8|"+Layer+"|0|";
            DXF_BodyText+=string.Join("|", nodes);
        }

        public void DXF_Point(double x, double y)
        {
            DXF_BodyText+="POINT| 8|"+Layer+"| 10|"+x+"| 20|"+y+"|0|";
        }

        public void DXF_Note(double x, double y, string note, double txt_height = 0.125d)
        {
            string[] lines;
            lines=note.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0, loopTo = lines.Length-1; i<=loopTo; i++)
            {
                DXF_Text((float)x, (float)y, 0f, (float)txt_height, lines[i]);
                y=y-2d*txt_height;
            }
        }

    }
}