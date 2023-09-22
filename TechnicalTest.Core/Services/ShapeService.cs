using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;

namespace TechnicalTest.Core.Services
{
    public class ShapeService : IShapeService
    {
        /// <summary>
        /// Creates a Triangle shape based on the provided grid and grid value.
        /// </summary>
        /// <param name="grid">The grid in which the triangle resides.</param>
        /// <param name="gridValue">The grid value indicating the position of the triangle.</param>
        /// <returns>A Triangle shape, or null if inputs are invalid.</returns>
        public Shape ProcessTriangle(Grid grid, GridValue gridValue)
        {
            // Validate grid size
            if (grid.Size <= 0)
            {
                return null;
            }

            // Validate grid value
            int numericRow = gridValue.GetNumericRow();
            if (numericRow <= 0 || gridValue.Column <= 0)
            {
                return null;
            }

            int gridSize = grid.Size;
            int topLeftVertexX = ((gridValue.Column - 1) / 2) * gridSize;
            int topLeftVertexY = (numericRow - 1) * gridSize;

            Coordinate topLeft = new Coordinate(topLeftVertexX, topLeftVertexY);
            Coordinate bottomRight = new Coordinate(topLeftVertexX + gridSize, topLeftVertexY + gridSize);

            if (gridValue.Column % 2 == 1)  // Left triangle
            {
                Coordinate outerVertex = new Coordinate(topLeftVertexX, topLeftVertexY + gridSize);
                return new Shape(new List<Coordinate>
                {topLeft, outerVertex, bottomRight });
            }
            else  // Right triangle
            {
                Coordinate outerVertex = new Coordinate(topLeftVertexX + gridSize, topLeftVertexY);
                return new Shape(new List<Coordinate>
                {topLeft, outerVertex, bottomRight });
            }
        }



        /// <summary>
        /// Calculates the GridValue based on a given triangle shape in a grid.
        /// </summary>
        /// <param name="grid">The grid object representing the overall grid.</param>
        /// <param name="triangle">The triangle object representing the shape.</param>
        /// <returns>The GridValue corresponding to the position of the triangle within the grid.</returns>
        public GridValue ProcessGridValueFromTriangularShape(Grid grid, Triangle triangle)
        {
   
            if (grid.Size <= 0 || !IsValidTriangle(grid.Size, triangle.TopLeftVertex,  triangle.OuterVertex, triangle.BottomRightVertex))
            {
                return null;
            }

            // Determine which grid cell the top-left vertex of the triangle is in
            int topLeftX = triangle.TopLeftVertex.X;
            int topLeftY = triangle.TopLeftVertex.Y;

            // Calculate the row and initial column values based on the grid cell
            int calculatedRow = (topLeftY / grid.Size) + 1;
            int initialColumn = (topLeftX / grid.Size) * 2;

            // Adjust the column value based on the position of the outer vertex
            int adjustedColumn = triangle.OuterVertex.X > topLeftX ? initialColumn + 2 : initialColumn + 1;

            return new GridValue(calculatedRow, adjustedColumn);
        }

        /// <summary>
        /// Validates if a triangle is creatable based on specified grid size and vertex coordinates.
        /// </summary>
        /// <remarks>
        /// A valid triangle must meet the following conditions:
        /// - Two of its sides' lengths must be equal to the grid size.
        /// - The remaining side must be greater than zero and should not be equal to the grid size.
        /// </remarks>
        /// <param name="gridSize">The grid size to validate against.</param>
        /// <param name="vertexA">The first vertex of the triangle.</param>
        /// <param name="vertexB">The second vertex of the triangle.</param>
        /// <param name="vertexC">The third vertex of the triangle.</param>
        /// <returns>True if the triangle is valid based on the given grid size and vertex coordinates, otherwise false.</returns>
        public static bool IsValidTriangle(int gridSize, Coordinate vertexA, Coordinate vertexB, Coordinate vertexC)
        {
            // Calculate the lengths of the triangle sides
            double sideAB = Distance(vertexA, vertexB);
            double sideBC = Distance(vertexB, vertexC);
            double sideCA = Distance(vertexC, vertexA);

       
            // Check if any two sides' lengths are equal to gridSize, and the other side is greater than gridSize
            if ((IsEqual(sideAB, gridSize) && IsEqual(sideBC, gridSize) && !IsZero(sideCA) && !IsEqual(sideCA, gridSize)) ||
                (IsEqual(sideBC, gridSize) && IsEqual(sideCA, gridSize) && !IsZero(sideAB) && !IsEqual(sideAB, gridSize)) ||
                (IsEqual(sideCA, gridSize) && IsEqual(sideAB, gridSize) && !IsZero(sideBC) && !IsEqual(sideBC, gridSize)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculates the distance between two coordinates.
        /// </summary>
        /// <param name="point1">First coordinate.</param>
        /// <param name="point2">Second coordinate.</param>
        /// <returns>Distance between the two coordinates.</returns>
        private static double Distance(Coordinate point1, Coordinate point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        /// <summary>
        /// Compares two double values for practical equality.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <returns>True if values are equal, otherwise false.</returns>
        private static bool IsEqual(double a, double b)
        {
            
            return a == b;
        }

        /// <summary>
        /// Checks if a double value is zero.
        /// </summary>
        /// <param name="a">The value to check.</param>
        /// <returns>True if the value is practically zero, otherwise false.</returns>
        private static bool IsZero(double a)
        {
            return Math.Abs(a) == 0;
        }
    

    }

}
