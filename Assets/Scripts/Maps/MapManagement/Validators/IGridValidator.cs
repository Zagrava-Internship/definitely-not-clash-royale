namespace Maps.MapManagement.Validators
{
    public interface IGridValidator
    {
        /// <summary>
        ///  Validates if the given grid position is valid for a unit or structure.
        ///  The Logic is based on the implementation of the interface.
        /// </summary>
        bool IsValid(int x, int y);
    }
}