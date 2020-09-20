using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public static class ExtensionMethods
    {
        public static JArray MapAll<TEntity>(this IEnumerable<TEntity> items, RestaurantDatabaseSettings ctx, bool customMap = false) where TEntity : RestaurantTable
        {
            JArray result = null;

            if (items != null)
            {
                result = new JArray();
            }

            foreach (var item in items)
            {
                result.Add(item.Map(ctx: ctx, customMap: customMap));
            }

            return result;
        }
    }
}
