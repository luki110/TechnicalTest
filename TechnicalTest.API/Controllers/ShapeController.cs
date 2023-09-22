using Microsoft.AspNetCore.Mvc;
using TechnicalTest.API.DTOs;
using TechnicalTest.Core;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Core.Models;


namespace TechnicalTest.API.Controllers
{
    /// <summary>
    /// Shape Controller which is responsible for calculating coordinates and grid value.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShapeController : ControllerBase
    {
        private readonly IShapeFactory _shapeFactory;

        /// <summary>
        /// Constructor of the Shape Controller.
        /// </summary>
        /// <param name="shapeFactory"></param>
        public ShapeController(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;
        }

        /// <summary>
        /// Calculates the Coordinates of a shape given the Grid Value.
        /// </summary>
        /// <param name="calculateCoordinatesRequest"></param>   
        /// <returns>A Coordinates response with a list of coordinates.</returns>
        /// <response code="200">Returns the Coordinates response model.</response>
        /// <response code="400">If an error occurred while calculating the Coordinates.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Shape))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateCoordinates")]
        [HttpPost]
        public IActionResult CalculateCoordinates([FromBody]CalculateCoordinatesDTO calculateCoordinatesRequest)
        {
            //  Get the ShapeEnum and if it is default (ShapeEnum.None) or not triangle, return BadRequest as only Triangle is implemented yet.
            ShapeEnum shapeType = (ShapeEnum)calculateCoordinatesRequest.ShapeType;
            if (shapeType != ShapeEnum.Triangle)
            {
                return BadRequest("Only Triangle shape is currently supported.");
            }
           
            //  Call the Calculate function in the shape factory.
            Grid grid = new Grid(calculateCoordinatesRequest.Grid.Size);
            GridValue gridValue = new GridValue(calculateCoordinatesRequest.GridValue);
            Shape? shape = _shapeFactory.CalculateCoordinates(shapeType, grid, gridValue);
            
            //  Return BadRequest with error message if the calculate result is null
            if(shape == null)
            {
                return BadRequest("Failed to calculate coordinates.");
            }

            //  Create ResponseModel with Coordinates and return as OK with responseModel.
            CalculateCoordinatesResponseDTO responseModel = new CalculateCoordinatesResponseDTO();
            foreach (var coordinate in shape.Coordinates) 
            {
                (responseModel.Coordinates ??= new List<CalculateCoordinatesResponseDTO.Coordinate>()).Add(new(coordinate.X, coordinate.Y));
            }
             
            
            return Ok(responseModel);
        }

        /// <summary>
        /// Calculates the Grid Value of a shape given the Coordinates.
        /// </summary>
        /// <remarks>
        /// A Triangle Shape must have 3 vertices, in this order: Top Left Vertex, Outer Vertex, Bottom Right Vertex.
        /// </remarks>
        /// <param name="gridValueRequest"></param>   
        /// <returns>A Grid Value response with a Row and a Column.</returns>
        /// <response code="200">Returns the Grid Value response model.</response>
        /// <response code="400">If an error occurred while calculating the Grid Value.</response>   
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GridValue))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("CalculateGridValue")]
        [HttpPost]
        public IActionResult CalculateGridValue([FromBody]CalculateGridValueDTO gridValueRequest)
        {
            //  Get the ShapeEnum and if it is default (ShapeEnum.None) or not triangle, return BadRequest as only Triangle is implemented yet.
            ShapeEnum shapeType = (ShapeEnum)gridValueRequest.ShapeType;
            if (shapeType != ShapeEnum.Triangle)
            {
                return BadRequest("Only Triangle shape is currently supported.");
            }
            
            //  Create new Shape with coordinates based on the parameters from the DTO.
            Grid grid = new Grid(gridValueRequest.Grid.Size);
            List<Core.Models.Coordinate> coordinates = new List<Core.Models.Coordinate>();
            foreach(var vertex in gridValueRequest.Vertices)
            {
                coordinates.Add(new(vertex.x, vertex.y));
            }
            
            Shape? shape = new Shape(coordinates);

            // Call the function in the shape factory to calculate grid value.
            GridValue? gridValue = _shapeFactory.CalculateGridValue(shapeType, grid, shape);
            
            // If the GridValue result is null then return BadRequest with an error message.
            if(gridValue == null || gridValue.Row == null)
            {
                return BadRequest("Failed to find the triangle with given coordinates.");
            }

            //Generate a ResponseModel based on the result and return it in Ok();
            
            CalculateGridValueResponseDTO responseModel = new CalculateGridValueResponseDTO(gridValue.Row, gridValue.Column);
            
            
            return Ok(responseModel);
        }
    }
}
