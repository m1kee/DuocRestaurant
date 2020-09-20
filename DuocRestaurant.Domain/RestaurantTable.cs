using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain
{
    public class RestaurantTable
    {
        public virtual JObject Map(RestaurantDatabaseSettings ctx, bool customMap = false)
        {
            JObject result = null;

            PropertyInfo[] objProperties = this.GetType().GetProperties();

            if (objProperties.Count() == 0)
            {
                throw new Exception("There are no mappeable properties.");
            }

            foreach (PropertyInfo pi in objProperties)
            {
                if (pi.PropertyType.IsPrimitive ||
                    pi.PropertyType.FullName.Contains("System.Int") ||
                    pi.PropertyType.FullName.Contains("System.DateTime") ||
                    pi.PropertyType.FullName.Contains("System.Decimal") ||
                    pi.PropertyType.FullName.Contains("System.Float") ||
                    pi.PropertyType.FullName.Contains("System.Double") ||
                    pi.PropertyType.FullName.Contains("System.String") ||
                    pi.PropertyType.FullName.Contains("System.Boolean") ||
                    pi.PropertyType.FullName.Contains("System.Byte") ||
                    pi.PropertyType.FullName.Contains("System.Guid"))
                {
                    if (pi.CanWrite)
                    {
                        var valueToAdd = pi.GetValue(this);

                        if (valueToAdd != null)
                        {
                            if (result == null)
                            {
                                result = new JObject();
                            }

                            if (pi.PropertyType.FullName.Contains("System.DateTime"))
                            {
                                var currentDate = DateTime.SpecifyKind((DateTime)pi.GetValue(this), DateTimeKind.Local);
                                result.Add(pi.Name, new JValue(currentDate));
                            }
                            else
                            {
                                result.Add(pi.Name, new JValue(valueToAdd));
                            }
                        }

                    }
                }
            }

            return result;
        }
    }
}
