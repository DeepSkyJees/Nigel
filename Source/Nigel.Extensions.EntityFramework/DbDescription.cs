using System.Collections.Generic;

namespace Nigel.Extensions.EntityFramework
{
    /// <summary>
    /// Class DbDescription.
    /// </summary>
    public class DbDescription
    {
        /// <summary>
        /// 名字
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        /// <value>The column.</value>
        public List<DbDescription> Column { get; set; }
    }
}