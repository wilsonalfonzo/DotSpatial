// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System.Collections;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.GeometriesGraph.Index;
using DotSpatial.Topology.Operation.Relate;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// Checks that a {GeometryGraph} representing an area
    /// (a <c>Polygon</c> or <c>MultiPolygon</c> )
    /// is consistent with the SFS semantics for area geometries.
    /// Checks include:
    /// Testing for rings which self-intersect (both properly and at nodes).
    /// Testing for duplicate rings.
    /// If an inconsistency if found the location of the problem is recorded.
    /// </summary>
    public class ConsistentAreaTester
    {
        private readonly GeometryGraph _geomGraph;
        private readonly LineIntersector _li = new RobustLineIntersector();
        private readonly RelateNodeGraph _nodeGraph = new RelateNodeGraph();

        // the intersection point found (if any)
        private Coordinate _invalidPoint;

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomGraph"></param>
        public ConsistentAreaTester(GeometryGraph geomGraph)
        {
            _geomGraph = geomGraph;
        }

        /// <summary>
        /// Returns the intersection point, or <c>null</c> if none was found.
        /// </summary>
        public virtual Coordinate InvalidPoint
        {
            get
            {
                return _invalidPoint;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsNodeConsistentArea
        {
            get
            {
                /*
                * To fully check validity, it is necessary to
                * compute ALL intersections, including self-intersections within a single edge.
                */
                SegmentIntersector intersector = _geomGraph.ComputeSelfNodes(_li, true);
                if (intersector.HasProperIntersection)
                {
                    _invalidPoint = intersector.ProperIntersectionPoint;
                    return false;
                }
                _nodeGraph.Build(_geomGraph);
                return IsNodeEdgeAreaLabelsConsistent;
            }
        }

        /// <summary>
        /// Check all nodes to see if their labels are consistent.
        /// If any are not, return false.
        /// </summary>
        private bool IsNodeEdgeAreaLabelsConsistent
        {
            get
            {
                for (IEnumerator nodeIt = _nodeGraph.GetNodeEnumerator(); nodeIt.MoveNext(); )
                {
                    RelateNode node = (RelateNode)nodeIt.Current;
                    if (!node.Edges.IsAreaLabelsConsistent)
                    {
                        _invalidPoint = (Coordinate)node.Coordinate.Clone();
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Checks for two duplicate rings in an area.
        /// Duplicate rings are rings that are topologically equal
        /// (that is, which have the same sequence of points up to point order).
        /// If the area is topologically consistent (determined by calling the
        /// <c>isNodeConsistentArea</c>,
        /// duplicate rings can be found by checking for EdgeBundles which contain more than one EdgeEnd.
        /// (This is because topologically consistent areas cannot have two rings sharing
        /// the same line segment, unless the rings are equal).
        /// The start point of one of the equal rings will be placed in invalidPoint.
        /// Returns <c>true</c> if this area Geometry is topologically consistent but has two duplicate rings.
        /// </summary>
        public virtual bool HasDuplicateRings
        {
            get
            {
                for (IEnumerator nodeIt = _nodeGraph.GetNodeEnumerator(); nodeIt.MoveNext(); )
                {
                    RelateNode node = (RelateNode)nodeIt.Current;
                    for (IEnumerator i = node.Edges.GetEnumerator(); i.MoveNext(); )
                    {
                        EdgeEndBundle eeb = (EdgeEndBundle)i.Current;
                        if (eeb.EdgeEnds.Count > 1)
                        {
                            _invalidPoint = eeb.Edge.GetCoordinate(0);
                            return true;
                        }
                    }
                }
                return false;
            }
        }
    }
}