
namespace  GeometricPublicKeyCrypto
{
    public class Requestor : Actor
    {
        public double zetaRatio;
        public double PrivateSideLength;

        public Requestor()
        {
            this.ownPolygon = new Polygon();
            this.otherPartyPolygon = new Polygon();
        }

        public new void CalculateOwnPolygon () 
        {
            this.ownPolygon.SideLength = PrivateSideLength;
            this.ownPolygon.Orientation = PublicTheta;
            this.ownPolygon.CalculateRadiusBasedOnSideLegnthAndTheta();
            this.ownPolygon.CalculateNumberOfSidesBasedOnRadiusAndSideLength();
            this.ownPolygon.CalculateInteriorAngle();
        }

        public new void CalculateOtherPartyPolygon () 
        { 
            this.otherPartyPolygon.SideLength = zetaRatio * PrivateSideLength;
            this.otherPartyPolygon.Radius = this.ownPolygon.Radius;
            this.otherPartyPolygon.CalculateOriantationBasedOnTheRadiusAndSideLength();
            this.otherPartyPolygon.CalculateNumberOfSidesBasedOnRadiusAndSideLength();
            this.otherPartyPolygon.CalculateInteriorAngle();
        }
    }

}