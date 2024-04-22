# DxfMaker

A C# class to generate a basic DXF file that can include any of the following objects

## Usage

Create an instance of `DxfMaker` to use

### Notable Methods

 - `Point(x,y)`
 - `Line(x1, y1, z1, x2, y2, z2)`
 - `Circle(x, y, z, radius)`
 - `Arc(x, y, z, radius, startAngle, endAngle)`
 - `Text(x, y, z, height, text)`
 - `Dimension(x1, y1, x2, y2, px1, py1, px2, py2, angle = 0, text = "None")`
 - `Rectangle(x1, y1, z1, x2, y2, z2)`
 - `Polyline(double[,] points)`
 - `Note(x, y, note, textHeight = 0.125)`
 - `Border(x1, y1, z1, x2, y2, z2)`


 ### Example Code

 ```C#
 DxfMaker dxf = new DxfMaker();

float[,] points = new float[,]
{
    { 0, 10 },
    { 1, 12 },
    { 2, 17 },
    { 3, 14 },
    { 4, 9  }
};
int N = points.GetLength(0);
for (int i = 1; i < N; i++)
{
    dxf.DXF_Line(
        points[i - 1, 0], points[i - 1, 1], 0,
        points[i, 0], points[i, 1], 0);
}

dxf.DXF_Save("curve.dxf");
```

### Notes

Originally this code was written for VB.NET and Visual Studio 2003. Please excuse any odd code, as technology has changed quite a bit in the last 20 years.