namespace Assets.Scripts.Grid
{
   public class Grid<TGridObject> where TGridObject : class
   {
      private readonly int gridSize;
      private TGridObject[,] gridArray;

      public TGridObject Value { get; set; }

      private int cellSize;

      public Grid(int size, int cellSize)
      {
         this.gridSize = size;
         this.cellSize = cellSize;
         gridArray = new TGridObject[size, size];
      }

      public int GetGridSize()
      {
         return gridSize;
      }

      public TGridObject GetElementAtGridPosition(int x, int y)
      {
         if(x < gridSize && y < gridSize && x >= 0 && y >= 0) {
            return gridArray[x, y];
         } else {
            return default;
         }
      }

      public void SetElementAtGridPosition(int x, int y, TGridObject value)
      {
         if(x < gridSize && y < gridSize) {
            gridArray[x, y] = value;
         }
      }
   }
}
