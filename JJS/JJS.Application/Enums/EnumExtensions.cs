using System;
using System.Collections.Generic;
using System.Text;

namespace JJS.Application.Enums
{
    public static class EnumExtensions
    {
        public static bool IsSuitable(Type enumType, int value)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(nameof(enumType));
            }

            var entities = Enum.GetValues(enumType);
            int composite = 0;
            foreach (var entity in entities)
            {
                composite |= (int)entity;
            }

            return (composite | value) == composite;
        }
    }
}
