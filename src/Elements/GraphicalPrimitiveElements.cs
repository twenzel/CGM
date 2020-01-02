using codessentials.CGM.Commands;

namespace codessentials.CGM.Elements
{
    public static class GraphicalPrimitiveElements
    {
        public static Command CreateCommand(int elementId, int elementClass, CGMFile container)
        {
            return ((GraphicalPrimitiveElement)elementId) switch
            {
                GraphicalPrimitiveElement.POLYLINE => new Polyline(container),
                GraphicalPrimitiveElement.DISJOINT_POLYLINE => new DisjointPolyline(container),
                GraphicalPrimitiveElement.POLYMARKER => new PolyMarker(container),
                GraphicalPrimitiveElement.TEXT => new Text(container),
                GraphicalPrimitiveElement.RESTRICTED_TEXT => new RestrictedText(container),
                GraphicalPrimitiveElement.APPEND_TEXT => new AppendText(container),
                GraphicalPrimitiveElement.POLYGON => new PolygonElement(container),
                GraphicalPrimitiveElement.POLYGON_SET => new PolygonSet(container),
                GraphicalPrimitiveElement.CELL_ARRAY => new CellArray(container),
                GraphicalPrimitiveElement.GENERALIZED_DRAWING_PRIMITIVE => new GeneralizedDrawingPrimitive(container),
                GraphicalPrimitiveElement.RECTANGLE => new RectangleElement(container),
                GraphicalPrimitiveElement.CIRCLE => new CircleElement(container),
                GraphicalPrimitiveElement.CIRCULAR_ARC_3_POINT => new CircularArc3Point(container),
                GraphicalPrimitiveElement.CIRCULAR_ARC_3_POINT_CLOSE => new CircularArc3PointClose(container),
                GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE => new CircularArcCentre(container),
                GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE_CLOSE => new CircularArcCentreClose(container),
                GraphicalPrimitiveElement.ELLIPSE => new EllipseElement(container),
                GraphicalPrimitiveElement.ELLIPTICAL_ARC => new EllipticalArc(container),
                GraphicalPrimitiveElement.ELLIPTICAL_ARC_CLOSE => new EllipticalArcClose(container),
                GraphicalPrimitiveElement.CIRCULAR_ARC_CENTRE_REVERSED => new CircularArcCentreReversed(container),
                GraphicalPrimitiveElement.CONNECTING_EDGE => new ConnectingEdge(container),
                GraphicalPrimitiveElement.HYPERBOLIC_ARC => new HyperbolicArc(container),
                GraphicalPrimitiveElement.PARABOLIC_ARC => new ParabolicArc(container),
                GraphicalPrimitiveElement.NON_UNIFORM_B_SPLINE => new NonUniformBSpline(container),
                GraphicalPrimitiveElement.NON_UNIFORM_RATIONAL_B_SPLINE => new NonUniformRationalBSpline(container),
                GraphicalPrimitiveElement.POLYBEZIER => new PolyBezier(container),
                GraphicalPrimitiveElement.POLYSYMBOL => new PolySymbol(container),
                GraphicalPrimitiveElement.BITONAL_TILE => new BitonalTile(container),
                GraphicalPrimitiveElement.TILE => new Tile(container),
                _ => new UnknownCommand(elementId, elementClass, container),
            };
        }
    }
}
