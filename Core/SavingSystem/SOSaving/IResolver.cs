namespace ATBS.Core.SavingSystem 
{
    /// <summary>
    /// Scriptable object resolver interface for SOResolver
    /// </summary>
    public interface IResolver
    {
        public void Load(DataContainer data);
        public DataContainer Save();
    }
}