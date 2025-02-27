using System.Numerics;

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

        public string GetOwnVertexEncodedAt(int index)
        {
            Vector2 vertex = ownPolygon.CalculateVertex(index);
            String encodedVertex = Helpers.EncodeDoubleToBase64(vertex.X) + Helpers.EncodeDoubleToBase64(vertex.Y);
            return encodedVertex;
        }

        public string GetOtherPartyEncodedVertexAt(int index)
        {
            Vector2 vertex = otherPartyPolygon.CalculateVertex(index);
            String encodedVertex = Helpers.EncodeDoubleToBase64(vertex.X) + Helpers.EncodeDoubleToBase64(vertex.Y);
            return encodedVertex;
        }

        public string GetPolygonInfo()
        {
            String info = "Own Polygon: \n" +  ownPolygon.GetPolygonInfo() + "\n" +
                          "Other Party Polygon: \n" + otherPartyPolygon.GetPolygonInfo();
            return info;
        }        
    }
}