
namespace GeometricPublicKeyCrypto{
    public class Provider: Actor
    {

        public double zetaRatio;
        public int PrivateNumberOfSides;
        public double PrivateRadius;
        
        /// <summary>
        /// 
        /// </summary>
        public new void CalculateOwnPolygon () 
        {
            this.ownPolygon.Radius = PrivateRadius;
            this.ownPolygon.Sides = PrivateNumberOfSides;
            this.ownPolygon.Orientation = this.PublicTheta;
            this.ownPolygon.CalculateSideLengthBasedOnRadiusAndOrientation();
            //this.ownPolygon.CalculateInteriorAngle();
        }

        /// <summary>
        /// 
        /// </summary>
        public new void CalculateOtherPartyPolygon() 
        {
            this.otherPartyPolygon.Radius = PrivateRadius;
            this.otherPartyPolygon.Orientation = this.PublicTheta;
            this.otherPartyPolygon.CalculateSideLengthBasedOnRadiusAndOrientation();
            this.otherPartyPolygon.CalculateNumberOfSidesBasedOnRadiusAndSideLength();            
            //this.otherPartyPolygon.CalculateInteriorAngle();
        }

        public double CalculateZetaRatio(){
            if(this.ownPolygon.SideLength <= 0 || this.otherPartyPolygon.SideLength <= 0)
            {
                throw new Exception("Cannot calculate the Zeta Radio. Parameter out of bounds");
            }
            this.zetaRatio = this.ownPolygon.SideLength / this.otherPartyPolygon.SideLength;
            return this.zetaRatio;
        }
    }
}