using System.Numerics;
using System.Text;

namespace GeometricPublicKeyCrypto
{
    /// <summary>
    /// A simple representation of a regular polygon inscribed in a circle.
    /// Includes methods for rotating and scaling the polygon.
    /// </summary>
    public class Polygon
    {
        public double Radius; // 0 < radius < double.MaxValue
        double Apothem = 0; // 0 < apothem < radius;
        public double SideLength {get;set;} // 0 < sideLength.  Note that for sides > 5, 0 < sideLength < r
        public double Sides {get;set;} // 2 < sides < infinity. Ideally we should set a MAX_SIDES constant that couples with the MAX_RESOLUTION of the double type (the min delta between two fractional numbers)
        public double Orientation {get;set;} //Polygon orientation from the x axis
        Vector2 Center {get;set;}
        double InteriorAngle = 0; //interior angle  60 <= beta < 180
// -- Constructors --
        /// <summary>
        /// Initialize Polygon's properties to default values (triangle) and set center to the origin (0,0)
        /// </summary>
        public  Polygon (){
            this.Center = new Vector2(0,0);
            this.Sides = 3;
            this.Orientation = 0;
            this.Radius = 1;
        }        
        /// <summary>
        /// Construct a regular polygon inscribed in a circle of given radius,
        /// with a specified center, number of sides, and orientation.
        /// </summary>
        public Polygon(Vector2 center, float radius, int sides, float orientation = 0f)
        {
            if(radius <=0){
                throw new ArgumentException("Radius must be greater than 0");
            }
            if(sides <=2){
                throw new ArgumentException("Number of sides must be greater than 2");
            }
            this.Center = center;
            this.Radius = radius;
            this.Sides = sides;
            this.Orientation = orientation;
            this.CalculateInteriorAngle();
            this.CalculateSideLengthBasedOnRadiusAndNumberOfSides();
        }

                /// <summary>
        /// 
        /// </summary>
        public void CalculateSideLengthBasedOnRadiusAndOrientation()
        {
            this.SideLength = 2 * this.Radius * Math.Sin(this.Orientation);
            if(this.SideLength <= 0)
            {
                this.SideLength *= -1; //As the Sin of theta can result in a negative value, invert sign 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateSideLengthBasedOnRadiusAndNumberOfSides()
        {
            this.SideLength = 2 * this.Radius * Math.Sin(Math.PI / this.Sides);  //(f1)   l = 2 * r * sin⁡ ⁣(π / n).
            if(this.SideLength <= 0)
            {
                this.SideLength *= -1; //As the Sin of theta can result in a negative value, invert sign  
            }
        }
        /// <summary>
        /// 
        /// </summary>
        void CalculateOrientationBasedOnTheNumberOfSides()
        {
            this.Orientation = Math.Sin(Math.PI / this.Sides);
        }
        
        /// <summary>
        /// Calculate the number of sides of the polyhon based on the polygon's side lenght and radius
        /// </summary>
        public void CalculateNumberOfSidesBasedOnRadiusAndSideLength()
        { 
            if(this.Radius <= 0 || this.SideLength <=0 ) 
            {
                throw new Exception("Impossible to calculate number of sides. Radius or side length out of bounds.");
            }
            this.Sides = ( 2 * Math.PI ) / Math.Asin( this.SideLength / (2 * this.Radius)); // (f2) n = 2π / ArcSin( l / 2r)
            if(this.Sides <= 0)
            {
                throw new Exception("An error occurred while calculating the number of sides. The result was a negative number.");
            }
        }

        /// <summary>
        /// Calculates the interior angule based on the number of sides
        /// </summary>
        /// <returns></returns>
        public void CalculateInteriorAngle()
        {
            if(this.Sides > 2 ){
                this.InteriorAngle = (2 * Math.PI) / this.Sides; //(f6) β = (2 * π)/ n
            }
            else {
                this.InteriorAngle = 180 - (2 * this.Orientation);
            }      
            if(this.InteriorAngle < 60 || this.InteriorAngle > 180 ) //(f5) β = 180 - (2 * θ)
            {
                throw new Exception("Error while calculating the interior angle. Angle out of bounds.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void RalculateNumberOfSidesBasedOnSideLegnthAndRadius()
        {
            if(this.SideLength <=0 || this.Radius<=0){
                throw new Exception("Parameter out of bounds");
            }
            this.Sides = (2 * Math.PI) / (Math.Asin( this.SideLength/(2 * this.Radius))); //(f10) n = ( 2 * π ) / ArcSin( l / (2 * r))
            if(this.Sides <= 0)
            {
                throw new Exception("An error occurred while calculating the number of sides. The result was a negative number.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void CalculateRadiusBasedOnSideLegnthAndTheta()
        {
            if(this.SideLength <=0 || this.Orientation <=0 ){
                throw new Exception("Parameter out of bounds");
            }
            this.Radius = this.SideLength / (2 * Math.PI * Math.Sin(this.Orientation)); // (f11) r = l / [ 2 * π * Sin(θ) ]
            if(this.Radius <= 0)
            {
                this.Radius *= -1; //as the Sin of theta can result in a negative number, invert the sign
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CalculateOriantationBasedOnTheRadiusAndSideLength()
        {
            if(this.SideLength <=0 || this.Radius <=0 ){
                throw new Exception("Parameter out of bounds");
            }
            this.Orientation = Math.Asin(this.SideLength / (2 * this.Radius)); //is it (2 * Math.Pi * this.Radius)?
            if(this.Orientation <= 0)
            {
                this.Orientation *= -1;
            }
        }


        /// <summary>
        /// Move's the passed vertex (based in the origin) to the new coordinates based on the center of the polygon's inscribed circle.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns>VectorD2</returns>
        public Vector2 TranslateVertex(Vector2 coordinate, Vector2 displacement)
        {
            coordinate += displacement;
            return coordinate;
        }
      

        public Vector2 CalculateVertex(int vertexIndex)
        {
            Vector2 vertex = new Vector2(0,0);
            // Precompute constants for efficiency
            double rotationRadians = this.Orientation * Math.PI / 180.0; // Convert rotation angle (polygon Orientation) to radians
            double angleIncrement = 2 * Math.PI / this.Sides; // Angle between vertices in radians (interior angle)

            // Calculate the angle for the desired vertex
            double vertexAngle = vertexIndex * angleIncrement + rotationRadians;

            // Use trigonometric calculations and translate vertext coordinates
            vertex.X = (float)((double) this.Radius * Math.Cos(vertexAngle));
            vertex.Y = (float)((double)  this.Radius * Math.Sin(vertexAngle));

            //translate (move) coordinate accounting for polygons' center
            vertex += this.Center;
            return vertex;
        }
        /// <summary>
        /// Returns the index of the opposite vertex based on the passed vertexIndex parameter. If the number of Sides of the polygon is odd, the oposit vertex is calculated to closest next opposite vertex ( using modular arithmetic).
        /// The vertex index is assumed to start from the vertex which 'y' coordinate is the greatest. This method is equivalent to locate the vertex that completes the symmetry axis of the polygon from the passed vertex (for a polygon with an even number of Sides).
        /// </summary>
        /// <param name="vertexIndex"></param>
        /// <returns></returns>
        public int  GetOppositeVertexNumber(int vertexIndex) {   
            int opposite = (int) ((vertexIndex + this.Sides / 2) % this.Sides); //Note that for odd numbers the result is fractional (as the opposite its between 2 vertex) so the result is set to the closest (in order) by truncating the result to an int. 
            return opposite;
        }

        /// <summary>
        /// Method to normalize a value from range [0, Double.MaxValue] to [0, 2]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double NormalizeRadius(double value)
        {
            if (value <= 0){
                throw new ArgumentException("Radius must be greater than 0.");
            } 
            return (value / double.MaxValue) * 2;
        }

        /// <summary>
        /// Returns a formatted string containing the Polygon's properties and their values.
        /// </summary>
        /// <returns>A formatted string representation of the Polygon's properties.</returns>
        public string GetPolygonInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------- Polygon Information -------------------");
            sb.AppendLine($"{"Center:",-20} ({Center.X}, {Center.Y})");
            sb.AppendLine($"{"Radius:",-20} {Radius:F4}");
            sb.AppendLine($"{"Apothem:",-20} {Apothem:F4}");
            sb.AppendLine($"{"Side Length:",-20} {SideLength:F4}");
            sb.AppendLine($"{"Number of Sides:",-20} {Sides:F0}");
            sb.AppendLine($"{"Orientation:",-20} {Orientation:F4} degrees");
            //sb.AppendLine($"{"Interior Angle:",-20} {InteriorAngle:F4} radians");
            sb.AppendLine("-----------------------------------------------------------");
            return sb.ToString();
        }

    }
}
