namespace TechnicalTest.Core.Models
{
    public class GridValue
    {
        public GridValue(string gridValue)
        {
            //modified the check for gridValue lenght as it was not letting to pass columns 10 to 12
            if (string.IsNullOrEmpty(gridValue) || (gridValue.Length < 2 && gridValue.Length > 3)) return;

            //added checks for valid rows and columns
            string row = gridValue.Substring(0, 1).ToUpper();
            if (!"ABCDEF".Contains(row))
            {
                
                return;
            }

            int column = int.Parse(gridValue.Substring(1));
            if (column < 1 || column > 12)
            {
                
                return;
            }
            Row = row;
            Column = column;
           
        }


        public GridValue(int row, int column)
        {
            //added checks for valid rows and columns
            if (row < 1 || row > 6)  // 1 = 'A', 2 = 'B', ..., 6 = 'F'
            {
                return;
            }

            if (column < 1 || column > 12)
            {
                return;
            }
            var numericValueOfCharacter = (char)64 + row;
            Row = ((char)numericValueOfCharacter).ToString();
            Column = column;
        }

        public string? Row { get; set; }

        public int Column { get; set; }

        public int GetNumericRow() => Row != null ? char.ToUpper(char.Parse(Row)) - 64 : 0;
    }
}
