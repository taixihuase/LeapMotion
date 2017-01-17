﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace Tool
{
    public static class EnumDescriptionTool
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumName)
        {
            string description;
            FieldInfo fieldInfo = enumName.GetType().GetField(enumName.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetDescriptAttr();
            if (attributes != null && attributes.Length > 0)
                description = attributes[0].Description;
            else
                throw new ArgumentException(string.Format("{0} 未能找到对应的枚举描述.", enumName));
            return description;
        }

        /// <summary>
        /// 获取枚举描述属性
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        private static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
        {
            return fieldInfo != null ? fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[] : null;
        }

        /// <summary>
        /// 通过描述获取枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static TEnum GetEnum<TEnum>(string description)
        {
            Type type = typeof(TEnum);
            foreach (FieldInfo field in type.GetFields())
            {
                DescriptionAttribute[] curDesc = field.GetDescriptAttr();
                if (curDesc != null && curDesc.Length > 0)
                {
                    if (curDesc[0].Description == description)
                        return (TEnum)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (TEnum)field.GetValue(null);
                }
            }
            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description));
        }
    }
}
