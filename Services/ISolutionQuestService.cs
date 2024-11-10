namespace EverybodyCodes.Services
{
    public interface ISolutionQuestService
    {
        /// <summary>
        /// Execute this quest's first part
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        string PartOne(bool example);

        /// <summary>
        /// Execute this quest's second part
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        string PartTwo(bool example);

        /// <summary>
        /// Execute this quest's third part
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        string PartThree(bool example);
    }
}