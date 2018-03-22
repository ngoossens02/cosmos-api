namespace Cosmos.API.Models
{
    /// <summary>
    /// Person Entity
    /// </summary>
    public class Person
    {        
        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Company
        /// </summary>
        public string Company { get; set; }
        
        /// <summary>
        /// Age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Salary
        /// </summary>
        public double Salary { get; set; }

        /// <summary>
        /// Is Vested
        /// </summary>
        public bool IsVested { get; set; }
    }
}