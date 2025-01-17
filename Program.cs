using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        // Input values
        //Console.WriteLine("Enter B seed:");
        double B = 200; //int.Parse(Console.ReadLine());
        //Console.WriteLine("Enter D seed:");
        double D = 50; //int.Parse(Console.ReadLine());

        //V2
        double k = CalcB(B, D);
        CalcD(D,k);

        //Console.WriteLine($"Coordinates of vertex {vertexIndex}: X = {x:F6}, Y = {y:F6}"); //The output precision is limited to 6 decimal places to balance accuracy with performance on lower-powered systems.

    }
   
    static double CalcB(double B,double D) {
        double Bx = B; 
        double By = D;
        double sides = B * D; // 0 < k < infinite

        double r = NormalizeRadius(B*D);
        double interiorAngle = ((sides - 2 ) * 180 ) / sides;  //(interiorAngle + exteriorAngle) = 180
        double angularDistance = 360/sides;
        double exteriorAngle = angularDistance; //only for regular polygons 
        double area = Math.PI * r * r; //Pi * r^2 
        double Circunference = 2 * Math.PI * r;
        double sideLength = 2 * r * Math.Sin( Math.PI / sides);
        double k = sideLength/r;
        (double Bx1, double By1) = CalculateVertex(sides, 0, 0, r);
        // Bx1 += Bx;
        // By1 += By;

        Console.WriteLine($"interiorAngle: {interiorAngle}, exteriorAngle: {exteriorAngle}, area: {area}, circunference: {Circunference}");
        Console.WriteLine($"B: Seed: {B} \n\ts={sides}, r={r}, sides={sideLength}, k= {k}, angular distance={angularDistance}, interior angle= {interiorAngle}");
        int vertexIndex = GetOppositeVertexNumber(0, (int) sides); 
        (double Bx2, double By2) = CalculateVertex(sides, 1, 0, r);
        Console.WriteLine($"\t\t\tVertex {0}: X1 = {Bx1} HEX: {ConvertDoubleToHex(Bx1)}, Y1 = {By1} HEX: {ConvertDoubleToHex(By1)}");
        Console.WriteLine($"\t\t\tVertex {vertexIndex}: X2 = {Bx2} HEX: {ConvertDoubleToHex(Bx2)}, Y2 = {By2} HEX: {ConvertDoubleToHex(By2)}");
        return k;
    }

    static void CalcD (double D,double k){
        
        //B CALCULATIONS
        double Dy = D;
        double r = NormalizeRadius(D); // 0 < r < 2 so the polygon is inscribed in the circle.
		double sideLength = r * k;
        double angularDistance = 2 * Math.PI * r;
        double sides = 360/angularDistance;
        double interiorAngle = ((sides - 2 ) * 180 ) / sides;
        (double Dx1, double Dy1) = CalculateVertex(sides, 0, 0, r);
        // Dx1 += Dx;
        // Dy1 += Dy;
        Console.WriteLine($"D: Seed: {D}\n\ts={sides}, r={r}, sides={sideLength}, k= {k}, angular distance={angularDistance}, interior angle= {interiorAngle}");
         int vertexIndex = GetOppositeVertexNumber(0, (int) sides); 
        (double Dx2, double Dy2) = CalculateVertex(sides, 1, 0, r);
        Console.WriteLine($"\t\t\tVertex {0}: X1 = {Dx1} HEX: {ConvertDoubleToHex(Dx1)}, Y1 = {Dy1} HEX: {ConvertDoubleToHex(Dy1)}");
        Console.WriteLine($"\t\t\tVertex {vertexIndex}: X2 = {Dx2} HEX: {ConvertDoubleToHex(Dx2)}, Y2 = {Dy2} HEX: {ConvertDoubleToHex(Dy2)}");
    }

    static (double, double) CalculateVertex(double sides, int vertexIndex, double thetaDegrees, double radius)
    {
        // Precompute constants for efficiency
        double thetaRadians = thetaDegrees * Math.PI / 180.0; // Convert rotation angle to radians
        double angleIncrement = 2 * Math.PI / sides;         // Angle between vertices in radians

        // Calculate the angle for the desired vertex
        double vertexAngle = vertexIndex * angleIncrement + thetaRadians;

        // Use efficient trigonometric calculations
        double x = radius * Math.Cos(vertexAngle);
        double y = radius * Math.Sin(vertexAngle);

        return (x, y);
    }

     static string EncodeDoubleToBase64(double value)
    {
        // Convert the double to its binary representation as an 8-byte array
        byte[] bytes = BitConverter.GetBytes(value);

        // Encode the byte array to Base64 manually (optimized approach)
        const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        char[] base64 = new char[12]; // Base64 encoding of 8 bytes will always be 12 characters

        int index = 0;
        for (int i = 0; i < 8; i += 3)
        {
            int chunk = bytes[i] | (i + 1 < 8 ? bytes[i + 1] << 8 : 0) | (i + 2 < 8 ? bytes[i + 2] << 16 : 0);
            base64[index++] = base64Chars[(chunk >> 18) & 63];
            base64[index++] = base64Chars[(chunk >> 12) & 63];
            base64[index++] = i + 1 < 8 ? base64Chars[(chunk >> 6) & 63] : '=';
            base64[index++] = i + 2 < 8 ? base64Chars[chunk & 63] : '=';
        }
        return new string(base64);
    }
     static string ConvertDoubleToHex(double value)
    {
        // Convert the double to its binary representation as an 8-byte array
        byte[] bytes = BitConverter.GetBytes(value);

        // Build the hexadecimal string manually (optimized for speed)
        char[] hexChars = new char[16];
        const string hexLookup = "0123456789ABCDEF";

        for (int i = 0; i < bytes.Length; i++)
        {
            hexChars[i * 2] = hexLookup[bytes[i] >> 4];       // High nibble
            hexChars[i * 2 + 1] = hexLookup[bytes[i] & 0x0F]; // Low nibble
        }
        return new string(hexChars);
    }

    static int  GetOppositeVertexNumber(int n, int s) { //for odd number of vertex the result is fractional (as its between 2 vertex) so its set to the closest (in order) by truncating the result; 
        // Calculate the opposite vertex using modular arithmetic
        int opposite = (n + s / 2) % s;
        return opposite;
    }

    // Method to normalize a value from range [0, Double.MaxValue] to [0, 2]
    static double NormalizeRadius(double value)
    {
        if (value < 0) value = 0; // Ensure the value is non-negative
        return (value / double.MaxValue) * 2;
    }

    static void Rotate(double sides, int vertexIndex, double thetaDegrees, double radius){
        double rotation = 0;
        while(rotation < 360){
            Console.WriteLine($"\tRotation: {rotation}");
            for(int i=0; i<sides; i++){
                Console.WriteLine($"\t\tVertex: {i}");
                vertexIndex = i;
                (double x1, double y1) = CalculateVertex(sides, vertexIndex, thetaDegrees, radius);
                vertexIndex = GetOppositeVertexNumber(vertexIndex, (int) sides);
                (double x2, double y2) = CalculateVertex(sides, vertexIndex, thetaDegrees, radius);
            }
            rotation += thetaDegrees;
        }
    }
}
