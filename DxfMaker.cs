using System;
using static System.Math;
using Microsoft.VisualBasic;
using System.Text;

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
        const double rad = Math.PI/180;

        readonly StringBuilder _body;
        readonly StringBuilder _blockBody;
        int _blockIndex;

        public string BodyText { get => _body.ToString(); }
        public string BlockBody { get => _blockBody.ToString(); }
        public float DimScale { get; set; } = 1.0f;
        public string Layer { get; set; } = "0";

        public DxfMaker()
        {
            _blockIndex=0;
            _body = new StringBuilder();
            _blockBody = new StringBuilder();
        }

        // Build the header, this header is set with my personal preferences
        private string Header()
        {            
            var HS = new string[20];
            HS[0]= $"  0|SECTION|  2|HEADER|  9";
            HS[1]= $"$ACADVER|  1|AC1009|  9";
            HS[2]= $"$INSBASE| 10|0.0| 20|0.0| 30|0.0|  9";
            HS[3]= $"$EXTMIN| 10|  0| 20|  0| 30|  0|  9";
            HS[4]= $"$EXTMAX| 10|368| 20|326| 30|0.0|  9";
            HS[5]= $"$LIMMIN| 10|0.0| 20|0.0|  9";
            HS[6]= $"$LIMMAX| 10|100.0| 20|100.0|  9";
            HS[7]= $"$ORTHOMODE| 70|     1|  9";
            HS[8]= $"$DIMSCALE| 40|{DimScale}|  9";
            HS[9]= $"$DIMSTYLE|  2|STANDARD|  9";
            HS[10]=$"$FILLETRAD| 40|0.0|  9";
            HS[11]=$"$PSLTSCALE| 70|     1|  0";
            HS[12]=$"ENDSEC|  0";
            HS[13]=$"SECTION|  2|TABLES|  0";
            HS[14]=$"TABLE|  2|VPORT| 70|     2|  0|VPORT|  2|*ACTIVE| 70|     0| 10|0.0| 20|0.0| 11|1.0| 21|1.0| 12|50.0| 22|50.0| 13|0.0| 23|0.0| 14|1.0| 24|1.0| 15|0.0| 25|0.0| 16|0.0| 26|0.0| 36|1.0| 17|0.0| 27|0.0| 37|0.0| 40|100.0| 41|1.55| 42|50.0| 43|0.0| 44|0.0| 50|0.0| 51|0.0| 71|     0| 72|   100| 73|     1| 74|     1| 75|     0| 76|     0| 77|     0| 78|     0|  0|ENDTAB|  0";
            HS[15]=$"TABLE|  2|LTYPE| 70|     1|  0|LTYPE|  2|CONTINUOUS|  70|     0|  3|Solid Line| 72|    65| 73|     0| 40|0.0|  0|ENDTAB|  0";
            HS[16]=$"TABLE|  2|LAYER| 70|     3|  0|LAYER|  2|0| 70|     0| 62|     7|  6|CONTINUOUS|  0|LAYER|  2|{Layer}| 70|     0| 62|     7|  6|CONTINUOUS|  0|LAYER|  2|DEFPOINTS| 70|     0| 62|     7| 6|CONTINUOUS|  0|ENDTAB|  0";
            HS[17]=$"TABLE|  2|VIEW| 70|     0|  0|ENDTAB|  0";
            HS[18]=$"TABLE|  2|DIMSTYLE| 70|     1|  0|DIMSTYLE|  2|STANDARD| 70|     0|  3||  4||  5||  6||  7|| 40|1.0| 41|0.125| 42|0.04| 43|0.25| 44|0.125| 45|0.0| 46|0.0| 47|0.0| 48|0.0|140|0.125|141|0.06|142|0.0|143|25.4|144|1.0|145|0.0|146|1.0|147|0.09| 71|     0| 72|     0| 73|     1| 74|     1| 75|     0| 76|     0| 77|     0| 78|     0|170|     0|171|     2|172|     0|173|     0|174|     0|175|     0|176|     0|177|     0|178|     0|  0|ENDTAB|  0";
            HS[19]=$"ENDSEC|  0|";

            return string.Join("|", HS);
        }


        // The block header, body, and footer are used to append the
        // header with any dimensional information added in the body.
        private string BlockHeader()
        {
            return "SECTION|  2|BLOCKS|  0|";
        }

        private void BuildBlockBody()
        {
            _blockBody.AppendLine($"BLOCK|  8|0|  2|*D{_blockIndex}|70|     1| 10|0.0| 20|0.0| 30|0.0|  3|*D{_blockIndex}|  1||0|ENDBLK|  8|0|0|");
            _blockIndex=_blockIndex+1;
        }

        private string BlockFooter()
        {
            return "ENDSEC|  0|";
        }

        // The body header, and footer will always remain the same
        private string BodyHeader()
        {
            return "SECTION|  2|ENTITIES|  0|";
        }

        private string BodyFooter()
        {
            return "ENDSEC|  0|";
        }

        private string Footer()
        {
            return "EOF";
        }
        public void Clear()
        {
            _body.Clear();
            _blockBody.Clear();
        }
        public void Save(string fpath)
        {
            string strOutput;
            string[] varDXF;
            try
            {
                // Build a full text string
                strOutput=Header()+BlockHeader()+BlockBody+BlockFooter()+BodyHeader()+BodyText+BodyFooter()+Footer();
                // split the text string at "|" and output to specified file
                varDXF=strOutput.Split('|');
                var fio = new System.IO.StreamWriter(fpath);
                for (int i = 0; i < varDXF.Length; i++)
                {
                    fio.WriteLine(varDXF[i]);
                }

                fio.Close();
                // Reminder of where the file was saved to
                Interaction.MsgBox($"DXF file was saved to:{Constants.vbCrLf}{fpath}");
            }
            catch (System.IO.IOException ex)
            {
                Interaction.MsgBox($"Error during DXF Save ({ex.Message})");
            }

        }

        // ====================================================
        // All geometry is appended to the text: "BodyText"
        // ====================================================

        public void Line(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            _body.AppendLine($"LINE|8|{Layer}| 10|{x1}| 20|{y1}| 30|{z1}| 11|{x2}| 21|{y2}| 31|{z2}|0|");
        }
        public void Circle(float x, float y, float z, float radius)
        {
            _body.AppendLine($"CIRCLE|8|{Layer}| 10|{x}| 20|{y}| 30|{z}| 40|{radius}| 39|  0|0|");
        }
        public void Arc(float x, float y, float z, float radius, float startAngle, float endAngle)
        {
            // "|62|1|" after Layer sets the to color (Red)
            _body.AppendLine($"ARC|8|{Layer}| 10|{x}| 20|{y}| 30|{z}| 40|{radius}| 50|{startAngle}| 51|{endAngle}|0|");
        }
        public void Text(float x, float y, float z, float height, string text)
        {
            _body.AppendLine($"TEXT|8|{Layer}| 10|{x}| 20|{y}| 30|{z}| 40|{DimScale*height}|1|{text}| 50|  0|0|");
        }
        public void Dimension(float x1, float y1, float x2, float y2, float px1, float py1, float px2, float py2, float angle = 0f, string text = "None")
        {
            // I know for sure that this works in AutoCAD.
            var strDim = new string[7];
            strDim[0]=$"DIMENSION|  8|{Layer}|  6|CONTINUOUS|  2|*D{_blockIndex}";
            strDim[1]=$" 10|{px1}| 20|{py1}| 30|0.0";
            strDim[2]=$" 11|{px2}| 21|{py2}| 31|0.0";
            strDim[3]=Interaction.IIf(text=="None", " 70|     0", $" 70|     0|  1|{text}").ToString();
            strDim[4]=$" 13|{x1}| 23|{y1}| 33|0.0";
            strDim[5]=$" 14|{x2}| 24|{y2}| 34|0.0{Interaction.IIf(angle==0f, "", $"| 50|{angle}")}";
            strDim[6]="1001|ACAD|1000|DSTYLE|1002|{|1070|   287|1070|     3|1070|    40|1040|{DimScale}|1070|   271|1070|     3|1070|   272|1070|     3|1070|   279|1070|     0|1002|}|  0|";
            _body.AppendLine(string.Join("|", strDim));
            // All dimensions need to be referenced in the header information
            BuildBlockBody();
        }
        public void Rectangle(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            var strRectangle = new string[6];
            strRectangle[0]=$"POLYLINE|  5|48|  8|{Layer}|66|1| 10|0| 20|0| 30|0| 70|1|0";
            strRectangle[1]=$"VERTEX|5|50|8|{Layer}| 10|{x1}| 20|{y1}| 30|{z1}|  0";
            strRectangle[2]=$"VERTEX|5|51|8|{Layer}| 10|{x2}| 20|{y1}| 30|{z2}|  0";
            strRectangle[3]=$"VERTEX|5|52|8|{Layer}| 10|{x2}| 20|{y2}| 30|{z2}|  0";
            strRectangle[4]=$"VERTEX|5|53|8|{Layer}| 10|{x1}| 20|{y2}| 30|{z1}|  0";
            strRectangle[5]=$"SEQEND|     8|{Layer}|0|";
            _body.AppendLine(string.Join("|", strRectangle));
        }
        public void Border(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            var strBorder = new string[6];
            strBorder[0]=$"POLYLINE|  8|{Layer}| 40|1| 41|1| 66|1| 70|1|0";
            strBorder[1]=$"VERTEX|  8|{Layer}| 10|{x1}| 20|{y1}| 30|{z1}|  0";
            strBorder[2]=$"VERTEX|  8|{Layer}| 10|{x2}| 20|{y1}| 30|{z2}|  0";
            strBorder[3]=$"VERTEX|  8|{Layer}| 10|{x2}| 20|{y2}| 30|{z2}|  0";
            strBorder[4]=$"VERTEX|  8|{Layer}| 10|{x1}| 20|{y2}| 30|{z1}|  0";
            strBorder[5]=$"SEQEND|  8|{Layer}|0|";
            _body.AppendLine(string.Join("|", strBorder));
        }
        public void ShowText(float x, float y, float angle, float radius, object text)
        {
            float eX;
            float eY;
            float iRadians;
            iRadians=(float)(angle*rad);
            // Find the angle at which to draw the arrow head and leader
            eX=(float)(x-radius*Cos(iRadians));
            eY=(float)(y-radius*Sin(iRadians));
            // Draw an arrow head
            ArrowHead((float)((double)iRadians+(180d*rad)), x, y);
            // Draw the leader lines
            Line(x, y, 0f, eX, eY, 0f);
            Line(eX, eY, 0f, eX+2f, eY, 0f);
            // Place the text
            Text((float)(eX+2.5), (float)(eY-0.75), 0f, 1.5f, text.ToString());
        }
        public void ArrowHead(float iRadians, float sngX, float sngY)
        {
            var x = new float[2];
            var y = new float[2];
            // The number "3" is the length of the arrow head.
            // Adding or subtracting 170 degrees from the angle
            // gives us a 10 degree angle on the arrow head.
            // Finds the first side of the arrow head
            x[0]=(float)(sngX+3*Math.Sin(iRadians+(170d*rad)));
            y[0]=(float)(sngY+3*Math.Cos(iRadians+(170d*rad)));
            // Finds the second side of the arrow head
            x[1]=(float)(sngX+3*Math.Sin(iRadians-(170d*rad)));
            y[1]=(float)(sngY+3*Math.Cos(iRadians-(170d*rad)));
            // Draw the first side of the arrow head
            Line(sngX, sngY, 0f, x[0], y[0], 0f); // /
            // Draw the second side of the arrow head
            Line(sngX, sngY, 0f, x[1], y[1], 0f); // \
            // Draw the bottom side of the arrow head
            Line(x[0], y[0], 0f, x[1], y[1], 0f); // _
        }

        public void Polyline(double[,] points)
        {
            int N = points.Length;
            string[] nodes = new string[N+1+1];
            nodes[0]=$"POLYLINE| 8|{Layer}| 66| 1|0";
            int i;
            var loopTo = N-1;
            for (i=0; i<=loopTo; i++)
                nodes[i+1]=$"VERTEX| 8|{Layer}| 10| {points[i,0]}| 20| {points[i,1]}|0";
            nodes[N+1]=$"SEQEND| 8|{Layer}|0|";
            _body.AppendLine(string.Join("|", nodes));
        }

        public void Point(double x, double y)
        {
            _body.AppendLine($"POINT| 8|{Layer}| 10|{x}| 20|{y}|0|");
        }

        public void Note(double x, double y, string note, double textHeight = 0.125d)
        {
            string[] lines;
            lines=note.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0, loopTo = lines.Length-1; i<=loopTo; i++)
            {
                Text((float)x, (float)y, 0f, (float)textHeight, lines[i]);
                y=y-2d*textHeight;
            }
        }

    }
}