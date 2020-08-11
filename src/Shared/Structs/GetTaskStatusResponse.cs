namespace Shared.Structs
{
    /// <summary>
    ///  Struct representing the return type for the GetTaskStatus API.
    /// </summary>
    public struct GetTaskStatusResponse
    {
        public bool completed;
        public string message;
    }
}