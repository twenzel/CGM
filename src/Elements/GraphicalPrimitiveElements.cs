using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{    
    public static class GraphicalPrimitiveElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            switch ((GraphicalPrimitiveElement)elementId)
            {
                case GraphicalPrimitiveElement.POLYLINE: 
                    return new Polyline(container);
                case GraphicalPrimitiveElement.DISJOINT_POLYLINE:
                    return new DisjointPolyline(container);
                case GraphicalPrimitiveElement.POLYMARKER: 
                    return new PolyMarker(container);
                case GraphicalPrimitiveElement.TEXT: 
                    return new Text(container);
                case GraphicalPrimitiveElement.RESTRICTED_TEXT: 
                    return new RestrictedText(container);
                case GraphicalPrimitiveElement.APPEND_TEXT: 
                    return new AppendText(container);
                case GraphicalPrimitiveElement.POLYGON: 
                    return new PolygonElement(container);
                case GraphicalPrimitiveElement.POLYGON_SET: 
                    return new PolygonSet(container);
                case GraphicalPrimitiveElement.CELL_ARRAY: 
                    return new CellArray(container);
                case GraphicalPrimitiveElement.GENERALIZED_DRAWING_PRIMITIVE: 
                    return new GeneralizedDrawingPrimitive(container);
                case GraphicalPrimitiveElement.RECTANGLE:
                    return new RectangleElement(container);
                case GraphicalPrimitiveElement.CIRCLE: 
                    return new CircleElement(container);
                case GraphicalPrimitiveElement.CIRCULAR_ARC_3_POINT: 
                    return new CircularArc3Point(container);
                case GraphicalPrimitiveElement.CIRCULAR_ARC_3_POINT_CLOSE: 
                    return new CircularArc3PointClose(container);
                case GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE: 
                    return new CircularArcCentre(container);
                case GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE_CLOSE: 
                    return new CircularArcCentreClose(container);
                case GraphicalPrimitiveElement.ELLIPSE:
                    return new EllipseElement(container);
                case GraphicalPrimitiveElement.ELLIPTICAL_ARC: 
                    return new EllipticalArc(container);
                case GraphicalPrimitiveElement.ELLIPTICAL_ARC_CLOSE: 
                    return new EllipticalArcClose(container);
                case GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE_REVERSED:
                    return new CircularArcCentreReversed(container);
                case GraphicalPrimitiveElement.CONNECTING_EDGE:
                    return new ConnectingEdge(container);
                case GraphicalPrimitiveElement.HYPERBOLIC_ARC:
                    return new HyperbolicArc(container);
                case GraphicalPrimitiveElement.PARABOLIC_ARC:
                    return new ParabolicArc(container);
                case GraphicalPrimitiveElement.NON_UNIFORM_B_SPLINE:
                    return new NonUniformBSpline(container);
                case GraphicalPrimitiveElement.NON_UNIFORM_RATIONAL_B_SPLINE: 
                    return new NonUniformRationalBSpline(container);
                case GraphicalPrimitiveElement.POLYBEZIER: 
                    return new PolyBezier(container);
                case GraphicalPrimitiveElement.POLYSYMBOL: 
                    return new PolySymbol(container);
                case GraphicalPrimitiveElement.BITONAL_TILE:
                    return new BitonalTile(container);
                case GraphicalPrimitiveElement.TILE: 
                    return new Tile(container);
                default:
                    return new UnknownCommand(elementId, elementClass, container);
            }        
        }
    }
}
