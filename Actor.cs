namespace GeometricPublicKeyCrypto 
{
    public class Actor : IActor
    
    {        
        protected Polygon ownPolygon;
        protected Polygon otherPartyPolygon;
        public double PublicTheta {get;set;}            
        public double deltaOrientation {get;set;} //must be >0 (f5) δ = θB ​− θA 
        double zetaRatio {get;set;} //the ration between side lengths defined as z= SideLengthB /SideLegthA
        public Actor() {
            ownPolygon = new Polygon();
            otherPartyPolygon = new Polygon();
        }

        public void CalculateOwnPolygon()
        {
            throw new NotImplementedException();
        }

        public void CalculateOtherPartyPolygon()
        {
            throw new NotImplementedException();
        }

        /*public string GetOwnVertexAt(int index)
        {
            ownPolygon.CalculateVertex(index);
        }

        public string GetOtherPartyVertexAt(int index)
        {
            otherPartyPolygon.CalculateVertex(index);
        }
        */
    }
}