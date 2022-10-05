using Nigel.Extensions.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Nigel.Basic;

namespace HuaGui.BimModel.Data.MyMigration
{
    /// <summary>
    /// Class DbDescriptionHelper.
    /// </summary>
    internal class DbDescriptionHelper
    {
        /// <summary>
        /// The database descriptions
        /// </summary>
        private static List<DbDescription> _dbDescriptions;

        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>System.String.</returns>
        internal static string GetDescription(string tableName, string columnName = "")
        {
            //初始化信息，设为单例模式
            if (_dbDescriptions == null)
            {
                _dbDescriptions = GetDescription();
            }

            //根据条件取出描述信息并返回
            if (!tableName.IsNoneValue())
            {
                if (!columnName.IsNoneValue())
                {
                    return _dbDescriptions.FirstOrDefault(p => p.Name == tableName)?.Description;
                }

                return _dbDescriptions.FirstOrDefault(p => p.Name == tableName)?.Column.FirstOrDefault(p => p.Name == columnName)?.Description;
            }

            return string.Empty;
        }

        /// <summary>
        /// 初始化得到全部的类和字段的描述信息
        /// </summary>
        /// <returns>List&lt;DbDescription&gt;.</returns>
        internal static List<DbDescription> GetDescription()
        {
            var result = new List<DbDescription>();
            //加载dll
            var assembly = Assembly.Load("HuaGui.BimModel.Data");
            var types = assembly.GetTypes();
            //取到HuaGui.BimModel.Data.Models的所有对象的信息
            var modelTypes = types.Where(t => t.Namespace == "HuaGui.BimModel.Data.Models").ToList();
            foreach (var modelType in modelTypes)
            {
                DbDescription description = new DbDescription()
                {
                    Column = new List<DbDescription>()
                };
                //获取类的描述信息
                var attribute = modelType.GetCustomAttribute<DescriptionAttribute>();
                description.Name = modelType.Name.ToPlural();

                description.Description = attribute?.Description;
                //获取当前类的属性
                var fields = modelType.GetProperties();
                foreach (var fieldInfo in fields)
                {
                    //获取属性的描述信息
                    attribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();
                    description.Column.Add(new DbDescription
                    {
                        Name = fieldInfo.Name,
                        Description = attribute?.Description
                    });
                }
                result.Add(description);
            }

            return result;
        }
    }
}