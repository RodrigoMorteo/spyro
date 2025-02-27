using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
namespace GeometricPublicKeyCrypto 
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input values
            //Console.WriteLine("Alison's Polygon Side lenght (l):");
            double aliceSideLength = 20; //double.Parse(Console.ReadLine());
            //Console.WriteLine("Polyogn orientation angle (theta):");
            double aliceTheta = 50; //double.Parse(Console.ReadLine());
            //Console.WriteLine("Bob's Polygon number of sides(n):");
            //double theta = 50; //double.Parse(Console.ReadLine());
            //Console.WriteLine("Inscribed circle radius (r):");
            double bobRadius = 10; //double.Parse(Console.ReadLine());
            int bobNumberOfSides = 5; 

            Requestor Alice = new Requestor();
            Provider Bob = new Provider();
            
            //1. Alice chooses side lenght and orientation theta 
            Alice.PrivateSideLength = aliceSideLength; //private value
            Alice.PublicTheta = aliceTheta; //public polygon orientation

            //2. Alice shares polygon orientation (theta) as public information
            Bob.PublicTheta = Alice.PublicTheta;

            //3. Bob chooses number of sides and radius parameters to create it's own polygon and inferes Alices side length by imposing the selected radius to theta 
            Bob.PrivateNumberOfSides = bobNumberOfSides; //private value
            Bob.PrivateRadius = bobRadius; // private value
            //Bob calculates it's own polygon having all the information needed: orientation, number of sides and radius.
            Bob.CalculateOwnPolygon();
            //Bob calculates Alice's polygon by setting the same radius and knowing the orientation by applying the formula(f4) and (f2)
            Bob.CalculateOtherPartyPolygon();
            
            //4. Bob calculates zeta and shares it with Alice
            Bob.CalculateZetaRatio();
            Alice.zetaRatio = Bob.zetaRatio;

            //5. Alice, now knowing z, it's own polygon orientation (theta) and privately her own polygon's side lenght, calculates the two polygons
            Alice.CalculateOwnPolygon();
            Alice.CalculateOtherPartyPolygon();

            //6. Both now have a common secret and can start sharing information securely
            //For this example both parties will agree to select Bob's Polygon's 3rd vertex coordinates as Key
            

        }
    }
}